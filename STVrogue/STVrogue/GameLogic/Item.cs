using System;
namespace STVrogue.GameLogic
{
    public class Item : GameEntity
    {
        public Item(String id) : base(id){ }
        
        public virtual void Use(Player player){ }
    }

    public class HealingPotion : Item
    {
        private readonly int _healValue;
        /* it can heal this many HP */

        public HealingPotion(String id, int heal) : base(id)
        {
            // Check if the given heal value is above 0
            if (heal <= 0)
                throw new ArgumentException();
            
            // Set the healValue to the given int
            _healValue = heal;
        }

        public int HealValue => _healValue;

        /// <summary>
        /// Use the healing potion, adding it's healValue to the player's HP
        /// </summary>
        /// <param name="player">The player object</param>
        public override void Use(Player player)
        {
            // Check if the player actually has this item in their bag
            if (!player.Bag.Contains(this))
            {
                // TODO: write to the console that this potion is not in the players' possession
                return;
            }

            // Check if the player is already at max HP
            if (player.Hp == player.HpMax)
            {
                // TODO: write tot the console that the player is already at max HP
                return;
            }
            
            // Record the old hp
            var prevHp = player.Hp;
                
            // Heal the player
            player.Hp += this.HealValue;
                
            // Remove this item from the player's bag
            player.Bag.Remove(this);
                
            // TODO: write to the console that this potion was used
        }
    }

    public class RagePotion : Item
    {
        public RagePotion(String id) : base(id){ }
        
        /// <summary>
        /// Use the rage potion, enraging the player
        /// </summary>
        /// <param name="player">The player object</param>
        public override void Use(Player player)
        {
            // Check if the player actually has this item in their bag
            if (!player.Bag.Contains(this)) 
            {
                // TODO: write to the console that this potion is not in the players' possession
                return;
            }
            
            // Enrage the player
            player.Enraged = true;
                
            // Remove this item from the player's bag
            player.Bag.Remove(this);
                
            // TODO: write to the console that this potion was used
        }
    }
}
