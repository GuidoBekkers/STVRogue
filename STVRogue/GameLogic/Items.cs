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
	public Boolean used = false ;
        public Item() { }
        public Item(String id) { this.id = id; }

        public void use(Player player)
        {
            if (used) {
                Logger.log($"{Player.id} is trying to use an expired item: {this.GetType().Name} {id}. Rejected.");
                return ;
            }
            Logger.log($"{Player.id} uses {this.GetType().Name} {id}");
            used = true ;
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

        new public void use(Player player)
        {
            base.use(player);
            player.HP = (int) Math.Min(player.HPbase, player.HP + HPvalue);   
        }
    }

    public class Crystal : Item
    {
        public Crystal(String id) : base(id) { }
        new public void use(Player player)
        {
            base.use(player);
            player.accelerated = true;
            if (player.location is Bridge) player.dungeon.disconnect(player.location as Bridge);    
        }
    }
}
