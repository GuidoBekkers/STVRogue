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
        public Item() { }
        public Item(String id) { this.id = id; }

        public void use(Player player)
        {
            Logger.log($"Using {this.GetType().Name} {id}");
        }
    }

    public class HealingPotion : Item
    {
        public uint HPvalue;

        /* Create a healing potion with random HP-value */
        public HealingPotion(String id) : base(id)
        {
            HPvalue = (uint) RandomGenerator.rnd.Next(10) + 1;
        }

        public void use(Player player)
        {
            player.HP = (int) Math.Min(player.HPbase, player.HP + HPvalue);
            base.use(player);
        }
    }

    public class Crystal : Item
    {
        public Crystal(String id) : base(id) { }
        public void use(Player player)
        {
            player.accelerated = true;
            if (player.location is Bridge) player.dungeon.destroyConnectionsAtFromSide(player.location as Bridge);
            base.use(player);
        }
    }
}
