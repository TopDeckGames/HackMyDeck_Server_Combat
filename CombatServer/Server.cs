using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using GestionServer.Handlers;
using GestionServer.Model;

namespace GestionServer
{
    public class Server
    {
        private volatile TcpListener tcpListener;
        private Thread listenThread, cleanupThread;
        private volatile int port;
        private volatile List<ClientHandler> handlers = new List<ClientHandler>();
        private volatile bool active;
        public static Dictionary<User, DateTime> AvailableUsers { get; set; }

        /// <summary>
        /// Initialise et démarre le serveur TCP
        /// </summary>
        public Server()
        {
            Logger.log(typeof(Server), "Démarrage du serveur", Logger.LogType.Info);

            this.active = true;

            port = int.Parse(ConfigurationManager.AppSettings["port"]);

            if(Server.AvailableUsers == null)
            {
                Server.AvailableUsers = new Dictionary<User, DateTime>();
            }

            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.listenThread = new Thread(new ThreadStart(listenForClients));
            this.listenThread.Start();

            this.cleanupThread = new Thread(new ThreadStart(cleanUp));
            this.cleanupThread.Start();
        }

        /// <summary>
        /// Attend les connexions et pour chacune lance un nouveau thread
        /// </summary>
        private void listenForClients()
        {
            this.tcpListener.Start();

            while (this.active)
            {
                TcpClient client;

                try
                {
                    client = this.tcpListener.AcceptTcpClient();
                }
                catch
                {
                    continue;
                }
                ClientHandler clientHandler;

                lock (this.handlers)
                {
                    try
                    {
                        clientHandler = new ClientHandler(client);
                        this.handlers.Add(clientHandler);
                    }
                    catch (Exception e)
                    {
                        Logger.log(typeof(Server), e.Message, Logger.LogType.Fatal);
                        continue;
                    }

                    Thread clientThread = new Thread(new ThreadStart(clientHandler.handle));
                    clientThread.Start();
                }
            }
        }

        /// <summary>
        /// Enlève les handlers inactifs de la mémoire
        /// </summary>
        private void cleanUp()
        {
            while (this.active)
            {
                lock(Server.AvailableUsers)
                {
                    Dictionary<User, DateTime> temp = new Dictionary<User, DateTime>();
                    foreach(KeyValuePair<User, DateTime> k in Server.AvailableUsers)
                    {
                        k.Value.AddMinutes(10);
                        if(DateTime.Compare(k.Value, DateTime.Now) > 0)
                        {
                            temp.Add(k.Key, k.Value);
                        }
                    }
                    Server.AvailableUsers = temp;
                }

                lock (this.handlers)
                {
                    List<ClientHandler> temp = new List<ClientHandler>();
                    foreach (ClientHandler client in this.handlers)
                    {
                        if (client.isActive() && Server.AvailableUsers.ContainsKey(client.User))
                        {
                            temp.Add(client);
                        }
                    }
                    this.handlers = temp;
                }

                System.Threading.Thread.Sleep(300000);
            }
        }

        /// <summary>
        /// Arrête le serveur et stop tous les threads créés
        /// </summary>
        public void stop()
        {
            Logger.log(typeof(Server), "Arrêt du serveur", Logger.LogType.Info);

            this.active = false;

            lock (this.handlers)
            {
                //Arrêt des threads en cours
                foreach (ClientHandler client in this.handlers)
                {
                    client.Active = false;
                }
            }

            //Fermeture du socket
            this.tcpListener.Stop();
        }

        /// <summary>
        /// Affiche les informations du serveur
        /// </summary>
        public void info()
        {
            Logger.log(typeof(Server), this.handlers.Count + " clients connectés", Logger.LogType.Info);
        }

        /// <summary>
        /// Retourne le nombre de joueurs connectés
        /// </summary>
        /// <returns>Int</returns>
        public int getNbPlayers()
        {
            return this.handlers.Count;
        }
    }
}