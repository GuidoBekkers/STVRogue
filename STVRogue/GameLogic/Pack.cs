using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STVRogue.GameLogic
{
    public class Pack
    {
        String id;
        public List<Monster> members;
        public Node location;

        public void Attack(Player p)
        {
            foreach (Monster m in members) m.Attack(p);
        }
    }
}
