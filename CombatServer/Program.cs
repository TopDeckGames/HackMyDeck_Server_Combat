using System;
using GestionServer.Data;
using GestionServer.Manager;
using GestionServer.Model;

namespace GestionServer
{
    class MainClass
    {
        public static Server Server;
        public static Monitor Monitor;

        public static void Main(string[] args)
        {
            Server = new Server();
            Monitor = new Monitor();

            while (true)
            {
                Console.Write("> ");
                string cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "stop":
                    case "exit":
                        Server.stop();
                        Monitor.stop();
                        Environment.Exit(0);
                        break;
                    case "restart":
                        Server.stop();
                        Server = new Server();
                        break;
                    case "info":
                        Server.info();
                        break;
                    case "":
                        break;
                    default:
                        Logger.log(typeof(MainClass), "Commande inconnue : " + cmd, Logger.LogType.Warn);
                        break;
                }
            }
        }
    }
}