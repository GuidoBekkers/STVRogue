using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using STVrogue.GameControl;
using static STVrogue.Utils.RandomFactory;

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
        public string playerName;
        public int playerBaseHp = 10;
        public int playerBaseAr = 1;

        public int GetDifficultyMultiplier(DifficultyMode difficulty)
        {
            int returnVal = 0;
            switch (difficulty)
            {
                case DifficultyMode.NEWBIEmode:
                {
                    returnVal = 3;
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
    public enum DifficultyMode
    {
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
        private HashSet<Monster> livingMonsters;

        /// <summary>
        /// Ignore this variable. It is added for some debug purpose.
        /// </summary>
        public int z_;

        /* To count the number of passed turns. */
        int turnNumber = 0;

        int healUsed; // Keeps track of the last turn a heal potion was used
        int rageUsed; // Keeps track of the last turn a rage potion was used

        //public Game()
        //{

        //}

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
            var difMult = conf.GetDifficultyMultiplier(difficultyMode);

            // Initialize the player
            player = new Player("playerId", conf.playerName, (conf.playerBaseHp * difMult),
                (conf.playerBaseAr * difMult));

            // Initialize the dungeon
            dungeon = new Dungeon(conf.dungeonShape, conf.numberOfRooms, conf.maxRoomCapacity);

            // Seed the monsters and items
            TrySeed();

            // Get the list of all monsters created in the dungeon and store it
            livingMonsters = new HashSet<Monster>();//dungeon.monsters;

            // Place the player in the starting room of the dungeon
            player.Location = dungeon.StartRoom;

            // Try seeding the monsters and items a couple of times, throw an exception is still not successful
            void TrySeed()
            {
                for (int i = 0; i < 10; i++)
                {
                    // Try seeding the monsters and items 10 times
                    if (dungeon.SeedMonstersAndItems(conf.initialNumberOfMonsters, conf.initialNumberOfHealingPots,
                        conf.initialNumberOfRagePots))
                    {
                        return;
                    }
                }
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
                throw new ArgumentException(
                    $"Destination ({destination.Id}) was not a neighbour to creature's location ({c.Location.Id})");
            }
        }

        /// <summary>
        /// Execute an attack by the attacker on the defender. This should only be done when
        /// the attacker is alive, and both attacker and defender are in the same room.
        /// </summary>
        public void Attack(Creature attacker, Creature defender)
        {
            // Check if the attacker is alive
            if (!attacker.Alive)
            {
                throw new ArgumentException($"Attacker: {attacker.Id} is not alive");
            }
            // Check if the attacker and defender are in the same room
            else if (attacker.Location != defender.Location)
            {
                throw new ArgumentException(
                    $"Attacker's ({attacker.Id}) location ({attacker.Location.Id}) does not match defender's ({defender.Id}) location ({defender.Location.Id})");
            }
            else
            {
                // Execute the attack
                attacker.Attack(defender);

                // Check if the defender died
                if (!defender.Alive)
                {
                    if (defender is Monster)
                    {
                        livingMonsters.Remove(defender as Monster);
                    }
                    else
                    {
                        gameover = true;
                    }
                }
            }
        }

        /// <summary>
        /// Activate the effect of using the given item (by the player). Note an effect,
        /// once activated, may last for several turns.
        /// </summary>
        public void UseItem(Item i)
        {
            // Use the item
            player.Use(i);

            if (i is HealingPotion)
            {
                // Store the turn in which the item was used
                healUsed = turnNumber;
            }
            else if (i is RagePotion)
            {
                // Store the turn in which the item was used
                rageUsed = turnNumber;

                // Check the difficulty 
                if (difficultyMode == DifficultyMode.ELITEmode)
                {
                    // Check if the player's location neighbours the exit room
                    if (player.Location.Neighbors.Contains(dungeon.ExitRoom))
                    {
                        player.EliteFlee = false;
                    }
                }
            }
        }

        /// <summary>
        /// Cause a creature to flee a combat. This will take the creature to a neighboring
        /// room. This should not breach the capacity of that room. Note that fleeing a
        /// combat is not always possible --see the Project Document.
        /// The method returns true if fleeing was successful, else false.
        /// </summary>
        public bool Flee(Creature c)
        {
            var possibleRooms = c.Location.Neighbors.ToList();
            var canFlee = true;

            // Conditions for a monster
            if (c is Monster)
            {
                if(c.Location != player.Location) return false;
                
                // A monster can't flee to a room if it would exceed max capacity
                possibleRooms.RemoveAll(r =>r.Capacity <= r.Monsters.Count);

            }
            // Conditions for Player
            else if (c is Player)
            {
                if(c.Location.Monsters.Count < 1) throw new ArgumentException($"Player: {c.Id} is not in combat");
                
                // In normal mode and elite mode: if player is enraged, player cannot flee
                if ((DifficultyMode == DifficultyMode.NORMALmode || DifficultyMode == DifficultyMode.ELITEmode) &&
                    (c as Player).Enraged) canFlee = false;

                // In Elite mode: if player.eliteFlee is false, player cannot flee
                if (DifficultyMode == DifficultyMode.ELITEmode && !(c as Player).EliteFlee) canFlee = false;

                // If heal potion is used at turn t, player can only flee at turn t+2 or later
                if (TurnNumber <= HealUsed + 1) canFlee = false;

                // Player cannot flee to exit room
                possibleRooms.RemoveAll(r => r.RoomType == RoomType.EXITroom);
                
                foreach (Room r in possibleRooms)
                {
                    // Player can always flee when next to start room
                    if (r.RoomType == RoomType.STARTroom) canFlee = true;
                }
                
            }

            // if there is less then 1 possible room creature cannot flee
            if (possibleRooms.Count < 1) canFlee = false;

            // If all conditions are met the creature flees to a random room in possibleRooms
            if (canFlee)
            {
                c.Move(possibleRooms[GetRandom().Next(possibleRooms.Count)]);
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
            Console.WriteLine($"** Turn {turnNumber.ToString()}: {player.Name} {playerAction}");

            // Handle the given player action
            HandlePlayerTurn(playerAction);

            // Handle the monsters' turns
            foreach (var m in livingMonsters)
            {
                HandleMonsterTurn(m);
            }
            
            // Check if the player should calm down
            if (turnNumber - rageUsed >= 5)
            {
                player.Enraged = false;
            }

            // Update the turn number
            turnNumber++;
        }

        /// <summary>
        /// Handles the player's turn, depending on the given action
        /// </summary>
        /// <param name="playerAction">The command given by the player</param>
        private void HandlePlayerTurn(Command playerAction)
        {
            // Handle the given player action
            switch (playerAction.Name)
            {
                // Handle the player doing nothing
                case (CommandType.DoNOTHING):
                {
                    Console.WriteLine("You chose to do nothing.");
                    break;
                }
                // Handle the player moving to the given location
                case (CommandType.MOVE):
                {
                    HandlePlayerTurnMove(playerAction.Args[1]);
                    break;
                }
                // Handle the player attacking the given enemy
                case (CommandType.ATTACK):
                {
                    HandlePlayerTurnAttack(playerAction.Args[1]);
                    break;
                }
                // Handle the player picking up all the items in the room
                case (CommandType.PICKUP):
                {
                    // Add all items in the room to the player's bag
                    foreach (var i in player.Location.Items)
                    {
                        player.Bag.Add(i);
                    }
                    // Remove all the items from the room
                    player.Location.Items.Clear();
                    break;
                }
                // Handle the player using the given item
                case (CommandType.USE):
                {
                    HandlePlayerTurnUse(playerAction.Args[1]);
                    break;
                }
                // Handle the player fleeing
                case (CommandType.FLEE):
                {
                    // Check if the player can flee, executing the action if possible
                    if (Flee(player))
                    {
                        Console.WriteLine("You have successfully fled from the fight");
                    }
                    else
                    {
                        Console.WriteLine("Your attempt to flee from combat was not successful");
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Handles the player's move command
        /// </summary>
        /// <param name="targetId">The id of the room the player wants to move to</param>
        private void HandlePlayerTurnMove(string targetId)
        {
            // Create a bool denoting if the room was found
            var found = false;
            // Find the room with the correct ID
            foreach (var r in player.Location.Neighbors)
            {
                // When found, move to that location
                if (r.Id == targetId)
                {
                    Move(player, r);
                    found = true;
                    Console.WriteLine($"You moved to room {targetId}");
                }
            }
            // if the destination was not found, throw an exception
            if (!found)
            {
                throw new ArgumentException($"The given room ({targetId}) was not accessible from the player's location ({player.Location.Id})");
            }
        }

        /// <summary>
        /// Handles the player's attack command
        /// </summary>
        /// <param name="targetId">The id of the monster the player wants to attack</param>
        private void HandlePlayerTurnAttack(string targetId)
        {
            // Create a bool denoting if the target was found
            var found = false;
            // Find the monster with the correct ID
            foreach (var m in player.Location.Monsters)
            {
                // When found, attack that monster
                if (m.Id == targetId)
                {
                    // Store the monster's old HP
                    int prevHp = m.Hp;
                            
                    // Attack the monster
                    Attack(player, m);
                    found = true;
                    Console.WriteLine($"You attacked monster {targetId}, dealing {(prevHp - m.Hp).ToString()} damage");
                }
            }
            // if the target was not found, throw an exception
            if (!found)
            {
                throw new ArgumentException($"The given monster ({targetId}) was not in the same location as the player ({player.Location.Id})");
            }
        }

        /// <summary>
        /// Handles the player's use command
        /// </summary>
        /// <param name="targetId">The id of the potion the player wants to use</param>
        private void HandlePlayerTurnUse(string targetId)
        {
            // Create a bool denoting if the item was found
            var found = false;
            // Find the item with the correct ID
            foreach (var item in player.Bag)
            {
                // When found, use that item
                if (item.Id == targetId)
                {
                    UseItem(item);
                    found = true;
                }
            }
            // if the target was not found, throw an exception
            if (!found)
            {
                throw new ArgumentException($"The given item ({targetId}) was not in the player's bag");
            }
        }

        /// <summary>
        /// Handles the turn for the given monster
        /// </summary>
        /// <param name="m">The monster whose turn should be handled</param>
        private void HandleMonsterTurn(Monster m)
        {
            // Decide how many actions the monster can choose from
            int action;
            // If he is in combat: do nothing, attack and flee
            if (m.Location == player.Location)
            {
                // Set the action to either 1, 2 or 3
                action = GetRandom().Next(1, 4);
            }
            // if he is not in combat: move or do nothing
            else
            {
                // Set the action to either 0 or 1
                action = GetRandom().Next(0, 2);
            }

            // Handle the chosen action
            switch (action)
            {
                case 0: // move
                {
                    // Define all possible destinations
                    var destinations = new List<Room>();

                    // Check if there are any rooms the monster can move to
                    foreach (var r in m.Location.Neighbors)
                    {
                        // Check if the room can still hold a monster
                        if (r.Monsters.Count < r.Capacity)
                        {
                            destinations.Add(r);
                        }
                    }

                    // Check if the monster can move to any room
                    if (destinations.Count > 0)
                    {
                        // move to a random room
                        Move(m, destinations[GetRandom().Next(0, destinations.Count)]);
                    }

                    // Else do nothing
                    break;
                }
                case 1: // do nothing
                {
                    break;
                }
                case 2: // attack
                {
                    // Store the player's old HP
                    int prevHp = player.Hp;
                    
                    // Handle the monster attacking the player
                    Attack(m, player);
                    
                    Console.WriteLine($"Monster {m.Id} attacked you and dealt {(prevHp - player.Hp).ToString()} damage");
                    break;
                }
                case 3: // flee
                {
                    // Handle the monster fleeing, it doesn't matter if the flee was successful because the monster does nothing if he can't flee.
                    Flee(m);
                    
                    Console.WriteLine($"Monster {m.Id} fled from the fight");
                    break;
                }
            }
        }
    }
}
