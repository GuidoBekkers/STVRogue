using System;
namespace STVrogue.GameLogic
{
    public class Item : GameEntity
    {
        public Item(String ID) : base(ID){ }
        
    }

    public class HealingPotion : Item
    {
        /* it can heal this many HP */
        int HPvalue;

        public HealingPotion(String ID, int heal) : base(ID)
        {
            this.HPvalue = heal;
        }
        
        public int HPvalue1 => HPvalue;

    }

    public class RagePotion : Item
    {
        public RagePotion(String ID) : base(ID){ }
        
    }

}
