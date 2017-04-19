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
        public Dungeon dungeon;

        public void Attack(Player p)
        {
            foreach (Monster m in members) {
               m.Attack(p);
               if (p.HP == 0) break ;
            }
        }
        
        /* Move the pack to an adjacent node. */
        public void move1(Node u) {
             if (!location.neighbors.Contains(u)) throw new ArgumentException() ; 
             uint capacity = dungeon.M * (dungeon.level(u) + 1) ;
             if (u.packs.Count >= capacity) {
                 Logger.log($"Pack {id} is trying to move to an already full node {u.id}. Rejected.");
                 return ;
             }
             location = u ;
             u.packs.Add(this);
             Logger.log($"Pack {id} moves to an already full node {u.id}. Rejected.");
        
        }
        
        /* Move the pack one node further along a shortest path to u. */
        public void moveTowards(Node u) {
            List<Node> path = dungeon.shortestpath(location,u) ;
            move1(path[0]) ;
        }
    }
}
