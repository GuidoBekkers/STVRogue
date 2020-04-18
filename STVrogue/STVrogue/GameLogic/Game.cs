using System;
using System.Collections.Generic;
using STVrogue.GameControl;

namespace STVrogue.GameLogic
{
    /// <summary>
    /// Representing the configuration of a level.
    /// </summary>
    [Serializable()]
    public class GameConfiguration
    {
        public int numberOfRooms;
        public int maxRoomCapacity;
        public DungeonShapeType dungeonShape;
        public int initialNumberOfMonsters;
        public int initialNumberOfHealingPots;
        public int initialNumberOfRagePots;
        public DifficultyMode difficultyMode;
    }
    
    [Serializable()]
    public enum DifficultyMode {
        NEWBIEmode,
        NORMALmode,
        ELITEmode
    }
    
    /// <summary>
    /// This class implements the logic of STV-Rogue. It holds its entire game-state
    /// and provides an update-method which is invoked every turn to execute all
    /// things that should happen at that turn.
    ///
    /// Some other methods are deliberately exposed as public to make it easier for
    /// us to track that you indeed unit-test them.
    /// </summary>
    public class Game
    {

        Player player;
        Dungeon dungeon;
        private bool gameover = false;

        /// <summary>
        /// Ignore this variable. It is added for some debug purpose.
        /// </summary>
        public int z_;

        /* To count the number of passed turns. */
        int turnNumber = 0;

        public Game()
        {
            player = new Player("0", "Bagginssess");
        }
        
        /// <summary>
        /// Try to create an instance of Game satisfying the specified configuration.
        /// It should throw an exception if it does not manage to generate a dungeon
        /// satisfying the configuration.
        /// </summary>
        public Game(GameConfiguration conf)
        {
            throw new NotImplementedException();
        }

        public Player Player => player;

        public Dungeon Dungeon => dungeon;

        public int TurnNumber => turnNumber;

        public bool Gameover => gameover;

        /// <summary>
        /// Move the creature c from its current location to the given destination room.
        /// This should only be done if the room is a neighboring room and if moving c
        /// to the destination room would not exceed the room's capacity.
        /// </summary>
        public void Move(Creature c, Room destination)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Execute an attack by the attacker on the defender. This should only be done when
        /// the attacker is alive, and both attacker and defender are in the same room.
        /// </summary>
        public void Attack(Creature attacker, Creature defender)
        {
            
        }

        /// <summary>
        /// Activate the effect of using the given item (by the player). Note an effect,
        /// once activated, may last for several turns.
        /// </summary>
        public void UseItem(Item i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cause a creature to flee a combat. This will take the creature to a neighboring
        /// room. This should not breach the capacity of that room. Note that fleeing a
        /// combat is not always possible --see the Project Document.
        /// The method returns true if fleeing was successful, else false.
        /// </summary>
        public bool Flee(Creature c)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Perform a single turn-update on the game. In every turn, each creature
        /// is allowed to do one action. The player does and specified in the argument
        /// of this method. A monster can either do nothing, move, attack, or flee.
        /// See the Project Document that defines when these are possible.
        /// The order in which creatures execute their actions is random.
        /// </summary>
        public void Update(Command playerAction)
        {
            Console.WriteLine("** Turn " + TurnNumber + ": "  + Player.Name + " " + playerAction);
            if (playerAction.Name == CommandType.ATTACK)
            {
                Console.WriteLine("      Clang! Wooosh. WHACK!");
            }
            if (playerAction.Name == CommandType.FLEE)
            {
                Console.WriteLine("      We knew you are a coward.");
            }
            if (playerAction.Name == CommandType.DoNOTHING)
            {
                Console.WriteLine("      Lazy. Start working!");
            }
            turnNumber++;
        }
        
    }
}
