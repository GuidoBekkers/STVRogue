using System;
using System.Linq.Expressions;
using System.Text;
using STVrogue.GameControl;
using STVrogue.GameLogic;
using STVrogue.TestInfrastructure;
using STVrogue.Utils;
using static STVrogue.Utils.RandomFactory;

namespace STVrogue
{
    /// <summary>
    /// This is the Main of STV Rogue. It loops over, reading user command.
    /// </summary>
    class Program
    {
        // A struct denoting the player's possible actions this turn
        private struct PossibleActions
        {
            public bool CanQuit { get; set; }
            public bool CanDoNothing { get; set; }
            public bool CanMove { get; set; }
            public bool CanAttack { get; set; }
            public bool CanPickup { get; set; }
            public bool CanUse { get; set; }
            public bool CanFlee { get; set; }
        }

        private static PossibleActions _possibleActions;
        
        // The game configuration for this instance
        private static GameConfiguration _gameConfiguration = new GameConfiguration();

        // Returns a random dungeon shape
        private static DungeonShapeType GetRandomDungeonShape()
        {
            var array = DungeonShapeType.GetValues(typeof(DungeonShapeType));
            return (DungeonShapeType) array.GetValue(GetRandom().Next(array.Length));
        }
        
        // A string builder for efficient writing to the console
        private static StringBuilder sb = new StringBuilder();
        
        // The Game variable
        private static Game game;

        static void Main(string[] args)
        {
            PrettyPrintLogo();
            InitGame();
            _gameConfiguration = RandomGameConfig(_gameConfiguration);
            RandomFactory.Reset(); // Reset the random generator, for easier saving/loading
            game = CreateGame(_gameConfiguration);
            SaveHelper.RecordConfig(_gameConfiguration); // Record the game configuration
            bool gameover = false;
            while (!gameover && !game.Gameover)
            {
                // Check which actions the player can take
                CheckPossibleActions();

                sb.Clear();
                // Fill the stringbuilder with some fitting flavortext
                
                sb.Append($"** HP: ").Append(game.Player.Hp).Append("/").Append(game.Player.HpMax).Append(", location: ").Append(game.Player.Location.Id).Append("\n");
                sb.Append("You enter a cold and damp room ");
                switch (_gameConfiguration.difficultyMode)
                {
                    case DifficultyMode.NEWBIEmode:
                        sb.Append("and you see ");
                        break;
                    case DifficultyMode.NORMALmode:
                        sb.Append("and with the light from your torch you see ");
                        break;
                    case DifficultyMode.ELITEmode:
                        sb.Append("and in the darkness you can barely make out ");
                        break;
                }

                if (_possibleActions.CanMove) // canMove means no combat
                {
                    sb.Append("an empty room");
                    if (_possibleActions.CanPickup)
                        sb.Append($" with {game.Player.Location.Items.Count.ToString()} items laying on the ground");
                    sb.Append(".\n");
                }
                else
                {
                    sb.Append($"{game.Player.Location.Monsters.Count.ToString()} horrible monsters coming towards you");
                    if (_possibleActions.CanPickup)
                        sb.Append(
                            $" with {game.Player.Location.Items.Count.ToString()} items laying on the ground behind them");
                    sb.Append(".\n");
                }

                sb.Append("What do you do?").Append("\n");
                Console.WriteLine(sb.ToString());
                sb.Clear();

                // Print the options
                PrintPossibleActions();

                // Read the input
                var input = ReadValidInput();

                switch (input)
                {
                    case 'm': // Move
                        PrintMoveTargets();
                        game.Update(new Command(CommandType.MOVE, GetMoveTarget()));
                        break;
                    case 'a': // Attack
                        PrintAttackTargets();
                        game.Update(new Command(CommandType.ATTACK, GetAttackTarget()));
                        break;
                    case 'u': // Use
                        PrintUseTargets();
                        game.Update(new Command(CommandType.USE, GetUseTarget()));
                        break;
                    case 'p': // Pickup
                        game.Update(new Command(CommandType.PICKUP, ""));
                        break;
                    case 'f': // Flee
                        game.Update(new Command(CommandType.FLEE, ""));
                        break;
                    case ' ': // Do nothing
                        game.Update(new Command(CommandType.DoNOTHING, ""));
                        break;
                    case 's': // Save
                        SaveHelper.SaveToFile("save.txt");
                        break;
                    case 'l': // Load
                        LoadGame("save.txt");
                        break;
                    case 'q': // Quit
                        SaveHelper.Quit();
                        gameover = true;
                        break;
                }
            }

            sb.Append("\n");
            if (game.Gameover && game.Player.Location == game.Dungeon.ExitRoom)
            {
                sb.Append("You run through the door and are greeted with mountains of treasure in front of you").Append("\n");
                sb.Append("Next to the seemingly endless piles of gold and silver you spot some old, but beautiful and powerful, weapons and armor.").Append("\n");
                sb.Append("As you stuff your pockets with loot you see a small hatch leading outside").Append("\n");
                sb.Append(
                        "After you've left, loot in hand, standing in the fresh breeze, you hear that old familiar voice echo from the mountains again:")
                    .Append("\n");
                sb.Append($"\"Good job, {_gameConfiguration.playerName}. You have found the treasure of the ancients, may it serve you well\"").Append("\n");
                sb.Append($"Score: {game.Player.Kp.ToString()}");
            }
            else
            {
                sb.Append($"As you fall to the ground, you feel your life slip away until everything fades to black").Append("\n");
                sb.Append($"Your score: {game.Player.Kp.ToString()}");
            }
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }

        /// <summary>
        /// Find a valid item to use
        /// </summary>
        /// <returns>The id of the item the player wants to use</returns>
        private static string GetUseTarget()
        {
            while (true)
            {
                var target = Console.ReadLine();
                foreach (var i in game.Player.Bag)
                {
                    if (i.Id == target)
                    {
                        return target;
                    }
                }
            }
        }

        /// <summary>
        /// Print all items the player can use
        /// </summary>
        private static void PrintUseTargets()
        {
            sb.Append("\n| ");
            foreach (var i in game.Player.Bag)
            {
                if (i is RagePotion)
                    sb.Append("Rage Potion");
                else
                    sb.Append("Heal Potion");
                sb.Append(" (").Append(i.Id).Append(") | ");
            }
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }
        /// <summary>
        /// Find a valid attack target
        /// </summary>
        /// <returns>The id of the monster the player wants to attack</returns>
        private static string GetAttackTarget()
        {
            while (true)
            {
                var target = Console.ReadLine();
                foreach (var m in game.Player.Location.Monsters)
                {
                    if (m.Id == target)
                    {
                        return target;
                    }
                }
            }
        }

        /// <summary>
        /// Print all monsters the player can attack
        /// </summary>
        private static void PrintAttackTargets()
        {
            sb.Append("\n| ");
            foreach (var m in game.Player.Location.Monsters)
            {
                sb.Append("monster (").Append(m.Id).Append(") | ");
            }
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }

        /// <summary>
        /// Find a valid move target
        /// </summary>
        /// <returns>the id of the destination</returns>
        private static string GetMoveTarget()
        {
            while (true)
            {
                var target = Console.ReadLine();
                foreach (var r in game.Player.Location.Neighbors)
                {
                    if (r.Id == target)
                    {
                        return target;
                    }
                }
            }
        }

        /// <summary>
        /// Prints all rooms neighbouring the player's location
        /// </summary>
        private static void PrintMoveTargets()
        {
            sb.Append("\n| ");
            foreach (var r in game.Player.Location.Neighbors)
            {
                if (r.RoomType == RoomType.ORDINARYroom)
                {
                    sb.Append("room  (").Append(r.Id).Append(") | ");
                }
                else if (r.RoomType == RoomType.STARTroom)
                {
                    sb.Append("start (").Append(r.Id).Append(") | ");
                }
                else if (r.RoomType == RoomType.EXITroom)
                {
                    sb.Append("exit  (").Append(r.Id).Append(") | ");
                }
            }
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }
        
        /// <summary>
        /// Reads only a valid input key
        /// </summary>
        /// <returns>the valid input</returns>
        private static char ReadValidInput()
        {
            var c = Console.ReadKey().KeyChar;
            switch (c)
            {
                case 'm':
                    if (_possibleActions.CanMove) return c;
                    ReadValidInput();
                    break;
                case 'a':
                    if (_possibleActions.CanAttack) return c;
                    ReadValidInput();
                    break;
                case 'u':
                    if (_possibleActions.CanUse) return c;
                    ReadValidInput();
                    break;
                case 'f':
                    if (_possibleActions.CanFlee) return c;
                    ReadValidInput();
                    break;
                case 'p':
                    if (_possibleActions.CanPickup) return c;
                    ReadValidInput();
                    break;
                case ' ':
                    if (_possibleActions.CanDoNothing) return c;
                    ReadValidInput();
                    break;
                case 'q':
                    if (_possibleActions.CanQuit) return c;
                    ReadValidInput();
                    break;
                case 's':
                    return c;
                case 'l':
                    return c;
                default:
                    ReadValidInput();
                    break;
            }
            return ' '; // Useless return statement, needed to make the compiler happy
        }

        /// <summary>
        /// Print all actions the player can currently perform to the screen 
        /// </summary>
        private static void PrintPossibleActions()
        {
            int i = 0;
            bool enterPossible = true;
            if (_possibleActions.CanMove) // The player can move
            {
                sb.Append("| Move(m)           ");
                i++;
            }

            if (_possibleActions.CanPickup) // The player can pickup
            {
                sb.Append("| Pickup items(p)   ");
                i++;
            }

            if (_possibleActions.CanUse) // The player can use an item
            {
                sb.Append("| Use item(u)       ");
                i++;
            }

            if (_possibleActions.CanAttack) // The player can attack
            {
                sb.Append("| Attack(a)         ");
                i++;
            }

            if (i == 4)
            {
                sb.Append("\n");
                enterPossible = false;
            }

            if (_possibleActions.CanFlee) // The player can flee
            {
                sb.Append("| Flee(f)           ");
                i++;
            }

            if (i == 4 && enterPossible)
            {
                sb.Append("\n");
                enterPossible = false;
            }

            if (_possibleActions.CanDoNothing) // The player can do nothing
            {
                sb.Append("| Do Nothing(SPACE) ");
                i++;
            }

            if (i == 4 && enterPossible)
            {
                sb.Append("\n");
                enterPossible = false;
            }

            if (_possibleActions.CanQuit) // The player can quit
            {
                sb.Append("| Quit(q)           ");
                i++;
            }
            
            if (i == 4 && enterPossible)
            {
                sb.Append("\n");
                enterPossible = false;
            }
            
            sb.Append("| Save(s)           ");
            i++;
            
            if (i == 4 && enterPossible)
            {
                sb.Append("\n");
                enterPossible = false;
            }
            
            sb.Append("| Load(l)           ");
            
            
            sb.Append("|\n");
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }

        /// <summary>
        /// Check which actions the player can take and store them in the PossibleMoves struct
        /// </summary>
        private static void CheckPossibleActions()
        {
            // If there are monsters at the player's location, he is in combat
            // combat means attack and flee are possible, but no moving
            _possibleActions.CanAttack = game.Player.Location.Monsters.Count > 0;
            _possibleActions.CanFlee = game.Player.Location.Monsters.Count > 0;
            // The player can also only move if there are neighbouring rooms to move to
            _possibleActions.CanMove = game.Player.Location.Monsters.Count == 0 && game.Player.Location.Neighbors.Count > 0;
            
            // Check if there are items in the player's location
            // Items means pickup is possible
            _possibleActions.CanPickup = game.Player.Location.Items.Count > 0;
            
            // Check if the player has any items
            // Items mean use is possible
            _possibleActions.CanUse = game.Player.Bag.Count > 0;
            
            // The player can always do nothing and quit
            _possibleActions.CanDoNothing = true;
            _possibleActions.CanQuit = true;
        }

        /// <summary>
        /// Initializes the game with the correct GameConfiguration
        /// </summary>
        private static void InitGame()
        {
            /*
            sb.Append("").Append("\n");
            
            Console.Write(sb.ToString());
            sb.Clear();
             */
            // Set the mood
            sb.Append("After a long and windy road you finally reach your destination, an ancient cave.").Append("\n");
            sb.Append("Having heard of the many tales and legends of treasure awaiting you, you briskly pace onward.")
                .Append("\n");
            sb.Append("Suddenly, a deep, ethereal voice echos from the mountain:").Append("\n");

            // Get the player's name
            sb.Append("\"Welcome traveler, what is your name?\"").Append("\n");
            Console.Write(sb.ToString());
            sb.Clear();
            GetName();
            sb.Append(
                    $"\"I feel luck might be in your favor, {_gameConfiguration.playerName}. Those who went before you did not share that pleasure.\"")
                .Append("\n");
            sb.Append("As the voice fades, you move closer to the cave.").Append("\n");

            // Get the difficulty they want to play at
            sb.Append("You see three entrances before you:").Append("\n");
            sb.Append("(1) - One that looks quite clean and well lit.").Append("\n");
            sb.Append("(2) - One that looks rather dirty, but decently lit.").Append("\n");
            sb.Append("(3) - One that looks completely overgrown and pitch black.").Append("\n");
            sb.Append("Through which entrance will you try your luck? (1, 2, or 3)").Append("\n");
            Console.Write(sb.ToString());
            sb.Clear();
            GetDif();
            Console.Write(sb.ToString());
            sb.Clear();

            // Get the name of the player, handling null inputs
            void GetName()
            {
                while (true)
                {
                    var s = Console.ReadLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        _gameConfiguration.playerName = s;
                    }
                    else
                    {
                        Console.WriteLine("\"It would be unwise to ignore me, what is your name?\"");
                        continue;
                    }
                    break;
                }
            }

            // Get the chosen difficulty setting, handling null and false inputs
            void GetDif()
            {
                while (true)
                {
                    var s = Console.ReadLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        var i = int.Parse(s);
                        if (i == 1 || i == 2 | i == 3)
                        {
                            switch (i)
                            {
                                case 1: // easy mode
                                {
                                    sb.Append("You feel a wave of excitement and fear rush over you as you enter the clean and well lit entrance.")
                                        .Append("\n");
                                    sb.Append("Your adventure begins...").Append("\n").Append("\n").Append("\n");
                                    _gameConfiguration.difficultyMode = DifficultyMode.NEWBIEmode;
                                    break;
                                }
                                case 2: // normal mode
                                {
                                    sb.Append("You grab a torch off the wall as you approach the middle entrance and venture onwards into the cave.")
                                        .Append("\n");
                                    sb.Append("Your adventure begins...").Append("\n").Append("\n").Append("\n");
                                    _gameConfiguration.difficultyMode = DifficultyMode.NORMALmode;
                                    break;
                                }
                                case 3: // hard mode
                                {
                                    sb.Append("You wisely grab a torch from the well lit entrance before using your sword to hack away at the thick and thorny vines shielding this entrance.")
                                        .Append("\n");
                                    sb.Append("You venture on, clenching your sword in one hand and your torch in the other, hoping it's flickering flames will never fade.")
                                        .Append("\n");
                                    sb.Append("Your adventure begins...").Append("\n").Append("\n").Append("\n");
                                    _gameConfiguration.difficultyMode = DifficultyMode.ELITEmode;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("There are only three entrances, which one do you choose? (1, 2, or 3)");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Standing around will get you nowhere, which entrance do you choose? (1, 2, or 3)");
                        continue;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Try creating a game, retrying with different values if it fails
        /// </summary>
        /// <returns></returns>
        public static Game CreateGame(GameConfiguration gc)
        {
            Game g;
            try
            {
                g = new Game(gc);
            }
            catch (Exception e)
            {
                if (e.InnerException is ArgumentException)
                {
                    return CreateGame(RandomGameConfig(gc));
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return g;
        }

        /// <summary>
        /// Initialized the game configuration
        /// </summary>
        public static GameConfiguration RandomGameConfig(GameConfiguration gc)
        {
            gc.numberOfRooms = GetRandom().Next(5, 20);
            gc.maxRoomCapacity = GetRandom().Next(10, 15);
            gc.dungeonShape = GetRandomDungeonShape();
            gc.initialNumberOfMonsters = GetRandom().Next(3, 6);
            gc.initialNumberOfHealingPots = GetRandom().Next(1, 10);
            gc.initialNumberOfRagePots = GetRandom().Next(1, 10);

            return gc;
        }

        /// <summary>
        /// Pretty prints the logo
        /// </summary>
        private static void PrettyPrintLogo()
        {
            Console.WriteLine();
            Console.WriteLine(" _______ _________            _______  _______  _______           _______ ");
            Console.WriteLine("(  ____ \\\\__   __/|\\     /|  (  ____ )(  ___  )(  ____ \\|\\     /|(  ____ \\");
            Console.WriteLine("| (    \\/   ) (   | )   ( |  | (    )|| (   ) || (    \\/| )   ( || (    \\/");
            Console.WriteLine("| (_____    | |   | |   | |  | (____)|| |   | || |      | |   | || (__    ");
            Console.WriteLine("(_____  )   | |   ( (   ) )  |     __)| |   | || | ____ | |   | ||  __)   ");
            Console.WriteLine("      ) |   | |    \\ \\_/ /   | (\\ (   | |   | || | \\_  )| |   | || (      ");
            Console.WriteLine("/\\____) |   | |     \\   /    | ) \\ \\__| (___) || (___) || (___) || (____/\\");
            Console.WriteLine("\\_______)   )_(      \\_/     |/   \\__/(_______)(_______)(_______)(_______/");
            Console.WriteLine();
        }

        /// <summary>
        /// Loads the game from the given save file
        /// </summary>
        /// <param name="saveFile">the path to the save file</param>
        private static void LoadGame(string saveFile)
        {
            // Create the GamePlay simulator
            GamePlay sim = new GamePlay(saveFile);

            // Replace this instance of Game with the simulated one
            game = sim.GetLoadedGame();
        }
    }
}
