using System;
namespace STVrogue.GameLogic
{
    public class Item : GameEntity
    {
        public Item(String ID) : base(ID){ }
        
        public virtual void Use(Player player){ }
    }

    public class HealingPotion : Item
    {
        /* it can heal this many HP */
        int healValue;

        public HealingPotion(String ID, int heal) : base(ID)
        {
            // Check if the given heal value is above 0
            if (heal < 0)
                throw new ArgumentException();
            
            // Set the healValue to the given int
            this.healValue = heal;
        }
        
        public int HealValue1 => healValue;
        
        /// <summary>
        /// Use the healing potion, adding it's healValue to the players' HP
        /// </summary>
        /// <param name="player">The player object</param>
        public override void Use(Player player)
        {
            // Check if the player actually has this item in their bag
            if (player.Bag.Contains(this))
            {
                // Record the old hp
                int prevHP = player.Hp;
                
                // Heal the player
                player.Hp += this.healValue;
                
                // Remove this item from the players' bag
                player.Bag.Remove(this);
                
                // TODO: write to the console that this potion was used
            }
        }
    }

    public class RagePotion : Item
    {
        public RagePotion(String ID) : base(ID){ }
        
        /// <summary>
        /// Use the rage potion, enraging the player
        /// </summary>
        /// <param name="player">The player object</param>
        public override void Use(Player player)
        {
            // Check if the player actually has this item in their bag
            if (player.Bag.Contains(this))
            {
                // Enrage the player
                player.Enraged = true;
                
                // Remove this item from the players' bag
                player.Bag.Remove(this);
                
                // TODO: write to the console that this potion was used
            }
        }
    }

}
