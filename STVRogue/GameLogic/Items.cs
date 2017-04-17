using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STVRogue.GameLogic
{
    public class Item
    {
        public String id;
        public void use(Player player)
        {
            Logger.log($"Using {this.GetType().Name} {id}");
        }
    }
}
