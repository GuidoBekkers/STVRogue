using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STVRogue.GameLogic
{
    public class Game
    {
        public Player player;
        public Dungeon dungeon;

        /* This creates a player and a random dungeon of the given level and node-capacity
         * The player is positioned at the dungeon's starting-node.
         * The constructor also randomly seeds monster-packs and items into the dungeon. The total
         * number of monsters are as specified. Monster-packs should be seeded as such that
         * the nodes' capacity are not violated. Furthermore the seeding of the monsters
         * and items should meet the balance requirements stated in the Project Document.
         */
        public Game(uint level, uint nodeCapcityMultiplier, uint numberOfMonsters)
        {
            Logger.log($"Creating a game of level {level}, node capacity multiplier {nodeCapcityMultiplier}, and {numberOfMonsters} monsters.");
            player = new Player();
        }

        /*
         * A single update turn to the game. We specify where the player moves to. 
         * All items in the new positions are looted, thus moved to the player's bag.
         * After that, all monster-packs are moved for one turn as well. If after this
         * the player's node is a contested node, a fight will occur.
         * The method returns true if the player survive the update, and false if he does not.
         */
        public Boolean update(Node playerNextPosition)
        {
            Logger.log($"Player moves from {player.location.id} to {playerNextPosition.id}");
            return true;
        }
    }
}
