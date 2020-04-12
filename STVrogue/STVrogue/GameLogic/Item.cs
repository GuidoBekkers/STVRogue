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
        int healValue;

        public HealingPotion(String ID, int heal) : base(ID)
        {
            this.healValue = heal;
        }
        
        public int HealValue1 => healValue;

    }

    public class RagePotion : Item
    {
        public RagePotion(String ID) : base(ID){ }
        
    }

}
