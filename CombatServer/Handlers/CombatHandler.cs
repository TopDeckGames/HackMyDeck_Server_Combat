using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;
using CombatServer.Model;

using LuaInterface;

namespace CombatServer.Handlers
{
    public class CombatHandler
    {
        private const int TURN_DURATION = 60;

        public Combat Combat { get; set; }
        public bool Active { get; set; }
        public DateTime Time { get; set; }
        private Timer clock;

        //Handler des clients
        private ClientHandler clientHandler1;
        private ClientHandler clientHandler2;

        //Mains des deux joueurs
        private List<Object> hand1;
        private List<Object> hand2;

        //Ressources des joueurs
        private int life1
        {
            get { return life1; }
            set
            {
                life1 = value;
                if (life1 <= 0)
                {
                    this.combatEnd();
                }
            }
        }
        private int life2
        {
            get { return life2; }
            set
            {
                life2 = value;
                if (life2 <= 0)
                {
                    this.combatEnd();
                }
            }
        }
        private int energy1 = 0;
        private int energy2 = 0;

        //Plateaux de jeu
        private Object[,] table1 = new Object[6, 1];
        private Object[,] table2 = new Object[6, 1];

        //Joueur débutant le combat
        private int beginner;

        //Tour courant
        private int turnNumber = 0;
        private bool turnActive = false;

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

            this.life1 = this.life2 = 30;
        }

        private void start()
        {
            //Si les deux joueurs sont bien connectés
            if (this.ClientHandler1 != null && this.ClientHandler2 != null)
            {
                //On initialise le combat
                Server.AvailableCombats.Remove(this.Combat);

                //On détermine le joueur qui commence
                Random rdm = new Random();
                this.beginner = rdm.Next(2);

                //Mélanger les decks

                //Début tour
                this.nextTurn();
            }
        }

        /// <summary>
        /// Prépare le prochain tour
        /// </summary>
        private void nextTurn()
        {
            //On bloque les requêtes

            //On incrémente le compteur de tours
            this.turnNumber++;

            //On déclanche les effets de début de tour sur toutes les cartes du plateau

            //Démarrage du tour de jeu
            this.turnActive = true;

            //Timer de fin de tour
            clock = new Timer(CombatHandler.TURN_DURATION * 1000);
            clock.Elapsed += new ElapsedEventHandler(endTurn);
            clock.Enabled = true;
        }

        /// <summary>
        /// Évènement déclanché à la fin d'un tour
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void endTurn(object source, ElapsedEventArgs e)
        {
            //Fin du tour de jeu
            this.turnActive = false;
            this.clock.Enabled = false;

            //On attend que toutes les actions du tour soient terminées
            while (true)
            {
                System.Threading.Thread.Sleep(500);
            }

            //On déclanche les effets de fin de tour sur toutes les cartes du plateau et on met à jours tous les effets

            //On envoi l'état du plateau aux clients pour être sur de la synchronisation des informations

            //On ouvre le tour du joueur suivant
            this.nextTurn();
        }

        /// <summary>
        /// Exécute les actions liées à la fin du combat
        /// </summary>
        private void combatEnd()
        {
            //Fin du tour de jeu
            this.turnActive = false;

            //On envoi les notifications de victoire / défaite

            //On enregistre le résultat du combat en base

            //On redirige les joueurs vers un serveur de gestion

            this.Active = false;
        }
    }
}

/*
Lua luaInterpret = new Lua();
luaInterpret["test"] = 1;
luaInterpret.DoString("test = test * 2");
double test = (double)luaInterpret["test"];
Console.WriteLine(test);
*/