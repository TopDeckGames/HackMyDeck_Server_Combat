using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestionServer.Model
{
    class Enhancement
    {
        public string Name;
        public string Description;
        public object Effect;
        public int Cost;
        public int Time;

        public Enhancement(string name, string description, object effect, int cost, int time)
        {
            this.Name = name;
            this.Description = description;
            this.Effect = effect;
            this.Cost = cost;
            this.Time = time;
        }

    }
}
