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

        /* This creates a player and a dungeon of the given level. The player is positioned at the
         * dungeon's starting-node.
         * The constructor also seeds monster-packs and items into the dungeon.
         */
        public Game(uint level)
        {
            Logger.log($"Creating a game of level {level}");
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
