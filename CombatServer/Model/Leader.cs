using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestionServer.Model
{
    class Leader
    {
        public string Name;
        public object Effect;

        public Leader(string name, object effect)
        {
            this.Name = name;
            this.Effect = effect;
        }
    }
}
