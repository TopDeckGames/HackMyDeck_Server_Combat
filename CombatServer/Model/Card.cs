using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestionServer.Model
{
    class Card
    {
        public enum TypeRarity {exemple=1}

        public string Titre { get; set; }
        public string Description { get; set; }
        public int CostInGame { get; set; }
        public int CostInStore { get; set; }
        public Boolean IsBuyable { get; set; }
        public string Type { get; set; }
        public TypeRarity Rarity { get; set; }

        public Card(string titre, string description, int costInGame, int costInStore, Boolean isBuyable, string type, TypeRarity rarity)
        {
            this.Titre = titre;
            this.Description = description;
            this.CostInGame = costInGame;
            this.CostInStore = costInStore;
            this.IsBuyable = isBuyable;
            this.Type = type;
            this.Rarity = rarity;
        }

    }
}
