using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestionServer.Model
{
    class Structure
    {
        public enum TypeBat { exemple=1 }
        public string Name;
        public TypeBat Type;
        public int Level;
        public string Description;
        public Boolean Locked;
        public int Effectif;

        public Structure(string name, TypeBat type, int level, string description, Boolean locked, int effectif)
        {
            this.Name = name;
            this.Type = type;
            this.Level = level;
            this.Description = description;
            this.Locked = locked;
            this.Effectif = effectif;
        }
    }
}
