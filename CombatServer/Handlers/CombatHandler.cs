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

        //Handler des clients
        private ClientHandler clientHandler1;
        private ClientHandler clientHandler2;

        //Mains des deux joueurs
        private List<Object> hand1;
        private List<Object> hand2;

        //Plateaux de jeu
        private Object[,] table1 = new Object[6,1];
        private Object[,] table2 = new Object[6,1];

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
            this.hand1 = new List<object>();
            this.hand2 = new List<object>();
        }

        private void start()
        {
            //Si les deux joueurs sont bien connectés
            if(this.ClientHandler1 != null && this.ClientHandler2 != null)
            {
                //On initialise le combat
                Server.AvailableCombats.Remove(this.Combat);

                //On set les ressources des joueurs
                int life1 = 30;
                int life2 = 30;
                int energy1 = 0;
                int energy2 = 0;

                //On détermine le joueur qui commence
                Random rdm = new Random();
                int currentPlayer = rdm.Next(2);

                //Mélanger les decks

                //Début tour
                //Timer turn
            }
        }
    }
}
