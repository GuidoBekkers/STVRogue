using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STVRogue.GameLogic
{
    public class Creature
    {
        public String id;
        public String name;
        public int HP;
        public uint AttackRating = 1;
        public Node location;
        public Creature() { }
        public void Attack(Creature foe)
        {
            foe.HP = (int)Math.Max(0, foe.HP - AttackRating);
            String killMsg = foe.HP == 0 ? ", KILLING it" : "";
            Logger.log($"Creature {id} attacks {foe.id}{killMsg}.");
        }
    }


    public class Monster : Creature
    {
        public Pack pack;
        public Monster(String id)
        {
            this.id = id; name = "Orc";
        }
    }

    public class Player : Creature
    {
        public int HPmax = 100;
        public uint KillPoint = 0;
        List<Item> bag = new List<Item>();
        public Player() { id = "player"; }
    }
}
