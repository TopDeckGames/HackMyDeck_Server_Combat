using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CombatServer.Model;

namespace CombatServer.Handlers
{
    public class CombatHandler
    {
        public Combat Combat { get; set; }
        public bool Active { get; set; }
        public DateTime Time { get; set; }
        private ClientHandler clientHandler1;
        private ClientHandler clientHandler2;

        public ClientHandler ClientHandler1
        {
            get { return clientHandler1; }
            set 
            { 
                clientHandler1 = value;
                this.start();
            }
        }
        
        public ClientHandler ClientHandler2
        {
            get { return clientHandler2; }
            set 
            { 
                clientHandler2 = value;
                this.start();
            }
        }

        public CombatHandler()
        {
            this.Active = true;
            this.Time = DateTime.Now;
        }

        private void start()
        {
            //Si les deux joueurs sont bien connectés
            if(this.ClientHandler1 != null && this.ClientHandler2 != null)
            {
                //On initialise le combat
                Server.AvailableCombats.Remove(this.Combat);

                //On set la vie des joueurs
                int life1 = 30;
                int life2 = 30;

                //On détermine le joueur qui commence
                Random rdm = new Random();
                rdm.Next(2);
            }
        }
    }
}
