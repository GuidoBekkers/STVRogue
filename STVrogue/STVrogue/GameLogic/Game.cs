using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public int playerBaseHp = 10;
        public int playerBaseAr = 1;

        public int GetDifficultyMultiplier(DifficultyMode difficulty)
        {
            int returnVal = 0;
            switch (difficulty)
            {
                case DifficultyMode.NEWBIEmode:
                {
                    returnVal =  3;
                    break;
                }
                case DifficultyMode.NORMALmode:
                {
                    returnVal = 2;
                    break;
                }
                case DifficultyMode.ELITEmode:
                {
                    returnVal = 1;
                    break;
                }
            }

            return returnVal;
        }
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
        DifficultyMode difficultyMode;
        bool gameover = false;
        
        /// <summary>
        /// Ignore this variable. It is added for some debug purpose.
        /// </summary>
        public int z_;

        /* To count the number of passed turns. */
        int turnNumber = 0;

        int healUsed; // Keeps track of the last turn a heal potion was used
        int rageUsed; // Keeps track of the last turn a rage potion was used
        
        public Game()
        {
            //player = new Player("0", "Bagginssess");
        }
        
        /// <summary>
        /// Try to create an instance of Game satisfying the specified configuration.
        /// It should throw an exception if it does not manage to generate a dungeon
        /// satisfying the configuration.
        /// </summary>
        public Game(GameConfiguration conf)
        {
            // Set the difficulty mode
            difficultyMode = conf.difficultyMode;
            
            // Get the difficulty HP and AR multiplier
            int difMult = conf.GetDifficultyMultiplier(difficultyMode);
            
            // Initialize the player
            player = new Player("playerId", "player", (conf.playerBaseHp * difMult), (conf.playerBaseAr * difMult));
            
            // Initialize the dungeon
            dungeon = new Dungeon(conf.dungeonShape, conf.numberOfRooms, conf.maxRoomCapacity);
            
            // Seed the monsters and items
            if (!dungeon.SeedMonstersAndItems(conf.initialNumberOfMonsters, conf.initialNumberOfHealingPots,
                conf.initialNumberOfRagePots))
            {
                // Throw and argument exception if this seeding fails
                throw new ArgumentException("Could not seed the dungeon with the given parameters");
            }
        }

        public Player Player => player;

        public Dungeon Dungeon => dungeon;

        public int TurnNumber
        {
            get => turnNumber;
            set => turnNumber = value;
        }

        public int HealUsed
        {
            get => healUsed;
            set => healUsed = value;
        }
        
        public int RageUsed
        {
            get => rageUsed;
            set => rageUsed = value;
        }

        public bool Gameover => gameover;
        
        public DifficultyMode DifficultyMode => difficultyMode;

        /// <summary>
        /// Move the creature c from its current location to the given destination room.
        /// This should only be done if the room is a neighboring room and if moving c
        /// to the destination room would not exceed the room's capacity.
        /// </summary>
        public void Move(Creature c, Room destination)
        {
            if (c.Location.Neighbors.Contains(destination))
            {
                c.Move(destination);
            }
            else
            {
                throw new ArgumentException($"Destination ({destination.Id}) was not a neighbour to creature's location ({c.Location.Id})");
            }
        }

        /// <summary>
        /// Execute an attack by the attacker on the defender. This should only be done when
        /// the attacker is alive, and both attacker and defender are in the same room.
        /// </summary>
        public void Attack(Creature attacker, Creature defender)
        {
            if (!attacker.Alive)
            {
                throw new ArgumentException($"Attacker: {attacker.Id} is not alive");
            }
            else if (attacker.Location != defender.Location)
            {
                throw new ArgumentException($"Attacker's ({attacker.Id}) location ({attacker.Location.Id}) does not match defender's ({defender.Id}) location ({defender.Location.Id})");
            }
            else
            {
                attacker.Attack(defender);
            }
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
            List<Room> possibleRooms = c.Location.ReachableRooms();
            bool canFlee = true;
            Random random = new Random();
            
            // Conditions for a monster
            if (c is Monster)
            {
                foreach (Room r in possibleRooms)
                {
                    // A monster can't flee to a room if it would exceed max capacity
                    if (r.Capacity <= r.Monsters.Count) possibleRooms.Remove(r);
                }
            }
            // Conditions for Player
            else if (c is Player)
            {
                // In normal mode and elite mode: if player is enraged, player cannot flee
                if ((DifficultyMode == DifficultyMode.NORMALmode || DifficultyMode == DifficultyMode.ELITEmode) && (c as Player).Enraged) canFlee = false;
                
                // In Elite mode: if player.eliteFlee is false, player cannot flee
                if (DifficultyMode == DifficultyMode.ELITEmode && !(c as Player).EliteFlee) canFlee = false;
                
                // If heal potion is used at turn t, player can only flee at turn t+2 or later
                if (TurnNumber <= HealUsed + 1) canFlee = false;
                
                foreach (Room r in possibleRooms)
                {
                    // Player cannot flee to exit room
                    if (r.RoomType == RoomType.EXITroom) possibleRooms.Remove(r);
                    // Player can always flee when next to start room
                    else if (r.RoomType == RoomType.STARTroom) canFlee = true;
                }
            }
            // if there is less then 1 possible room creature cannot flee
            if (possibleRooms.Count < 1) canFlee = false;
            
            // If all conditions are met the creature flees to a random room in possibleRooms
            if (canFlee)
            {
                c.Move(possibleRooms[random.Next(possibleRooms.Count)]);
                return true;
            }
            else return false;
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
            
            // Handle the given action
            switch (playerAction.Name)
            {
                case (CommandType.ATTACK):
                {
                    Console.WriteLine("      Clang! Wooosh. WHACK!");
                    break;
                }
                case (CommandType.FLEE):
                {
                    Console.WriteLine("      We knew you are a coward.");
                    break;
                }
                case (CommandType.DoNOTHING):
                {
                    Console.WriteLine("      Lazy. Start working!");
                    break;
                }
            }
            
            // Update the turn number
            turnNumber++;
        }
        
    }
}
