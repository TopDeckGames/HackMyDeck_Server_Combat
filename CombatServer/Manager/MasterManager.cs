using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;
using System.Threading;
using CombatServer.Helper;
using CombatServer.Model;

namespace CombatServer.Manager
{
    public class MasterManager
    {
        private TcpClient tcpClient;
        private RSACryptoServiceProvider rsaClient, rsaServer;
        public IPAddress MasterAddress { get; set; }
        public int MasterPort { get; set; }

        public MasterManager()
        {
            this.MasterPort = 3001;
        }

        private void connectToMaster()
        {
            try
            {
                //Ouverture de la connexion
                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(this.MasterAddress, this.MasterPort);

                //Récupèration de la clef de chiffrement
                StreamReader keyFile = new StreamReader(ConfigurationManager.AppSettings["server_private_key"]);
                string key = keyFile.ReadToEnd();
                keyFile.Close();

                NetworkStream stm = this.tcpClient.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] message = new byte[1024];

                try
                {
                    this.rsaClient = new RSACryptoServiceProvider(1024);
                    this.rsaServer = new RSACryptoServiceProvider(1024);
                    this.rsaClient.FromXmlString(key);

                    //Récupèration de la clef publique de l'autre pair
                    string serverKey = "";
                    bool flag = true;
                    int count = 0;
                    while (flag && count < 10) //Tant que la clef n'est pas valide et que l'on a reçu moins de 10 messages
                    {
                        count++;
                        int size = stm.Read(message, 0, 1024);
                        byte[] data = new byte[size];
                        for (int i = 0; i < size; i++)
                            data[i] = message[i];

                        serverKey += asen.GetString(rsaClient.Decrypt(data, false));
                        try
                        {
                            this.rsaServer.FromXmlString(serverKey);
                            flag = false;
                        }
                        catch (Exception)
                        {
                            flag = true;
                        }
                    }
                }
                catch (CryptographicException e)
                {
                    Logger.log(typeof(MasterManager), "L'encryption des données a échoué : " + e.Message, Logger.LogType.Error);
                    this.disconnectFromServer();
                }
            }
            catch (SocketException e)
            {
                Logger.log(typeof(MasterManager), "La connexion au serveur a échoué " + e.Message, Logger.LogType.Error);
                this.disconnectFromServer();
            }
            catch (IOException e)
            {
                Logger.log(typeof(MasterManager), "La lecture du fichier xml a échoué " + e.Message, Logger.LogType.Error);
                this.disconnectFromServer();
            }
        }

        /// <summary>
        /// Déconnexion du serveur courrant
        /// </summary>
        private void disconnectFromServer()
        {
            if (this.rsaClient != null)
                this.rsaClient.PersistKeyInCsp = false;
            if (this.rsaServer != null)
                this.rsaServer.PersistKeyInCsp = false;
            if (this.tcpClient != null)
                this.tcpClient.Close();
        }

        /// <summary>
        /// Envoi une chaine de charactères au serveur en la cryptant
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        private void sendToServer(string message)
        {
            ASCIIEncoding byteConverter = new ASCIIEncoding();

            List<byte[]> messages = ByteArray.SplitByteArray(byteConverter.GetBytes(message), 117);
            foreach (byte[] m in messages)
            {
                this.tcpClient.Client.Send(this.rsaServer.Encrypt(m, false));
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Attend une réponse du serveur distant
        /// </summary>
        /// <returns>Request</returns>
        private Request waitResponseFromServer()
        {
            NetworkStream clientStream = this.tcpClient.GetStream();
            int messageLength = int.Parse(ConfigurationManager.AppSettings["monitoring_message_length"]);
            byte[] message = new byte[messageLength];
            int bytesRead;
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            Request req = null;
            StringBuilder requests = new StringBuilder();

            while (req == null)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, messageLength);
                }
                catch (Exception e)
                {
                    Logger.log(typeof(MasterManager), "Erreur lors de l'execution du socket : " + e.Message, Logger.LogType.Error);
                    break;
                }

                if (bytesRead == 0)
                {
                    Logger.log(typeof(MasterManager), "Connexion interrompue", Logger.LogType.Info);
                    break;
                }

                //On récupère le message exact qui a été reçu
                byte[] data = new byte[bytesRead];
                for (int i = 0; i < bytesRead; i++)
                    data[i] = message[i];

                try
                {
                    //On décrypte la chaine charactère et on l'ajoute à la pile
                    string request = byteConverter.GetString(this.rsaClient.Decrypt(data, false));
                    requests.Append(request);
                    req = JsonSerializer.fromJson<Request>(requests.ToString());
                }
                catch (CryptographicException e)
                {
                    Logger.log(typeof(MasterManager), "Erreur lors du décryptage du message : " + e.Message, Logger.LogType.Error);
                }
                catch
                {

                }
            }

            return req;
        }
    }
}