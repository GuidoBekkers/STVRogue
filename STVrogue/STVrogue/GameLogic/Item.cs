using System;
namespace STVrogue.GameLogic
{
    public class Item : GameEntity
    {
        public Item(String id) : base(id){ }

        public virtual void Use(Player player)
        {
            Console.WriteLine("You throw the empty bottle on the ground.");
        }
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
            // Check if the player is already at max HP
            if (player.Hp == player.HpMax)
            {
                Console.WriteLine("When you start drinking the potion, you realise you are already more then healthy, so you decide to put the potion back in your bag");
                return;
            }
            
            // Record the old hp
            var prevHp = player.Hp;
                
            // Heal the player
            player.Hp += this.HealValue;
                
            Console.WriteLine($"You drank a healing potion and recovered {(player.Hp - prevHp).ToString()} HP!");
            base.Use(player);
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
            // Enrage the player
            player.Enraged = true;
                
            Console.WriteLine("You drank a rage potion and feel enraged for 5 turns!");
            base.Use(player);
        }
    }
}
