using System;
using System.Diagnostics;
using System.Text;
using STVrogue.GameControl;
using STVrogue.GameLogic;
using static STVrogue.Utils.RandomFactory;

namespace STVrogue
{
    /// <summary>
    /// This is the Main of STV Rogue. It loops over, reading user command.
    /// </summary>
    class Program
    {
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
            InitGameConfig();
            game = new Game(_gameConfiguration);
            bool gameover = false;
            while (!gameover || !game.Gameover)
            {
                Console.WriteLine("You are in a room. It is dark, and it feels dangerous...");
                Console.WriteLine("Your action: Move(m)   | Pick-item(p)  | Do-nothing(SPACE) | Quit(q)");
                Console.Write("             Attack(a) |    Flee(f)    | Use-item(u) ");
                var c = Console.ReadKey().KeyChar;
                Console.WriteLine("");
                switch (c)
                {
                    case 'm':
                        game.Update(new Command(CommandType.MOVE, ""));
                        break;
                    case 'a':
                        game.Update(new Command(CommandType.ATTACK, ""));
                        break;
                    case 'u':
                        game.Update(new Command(CommandType.USE, ""));
                        break;
                    case 'f':
                        game.Update(new Command(CommandType.FLEE, ""));
                        break;
                    case ' ':
                        game.Update(new Command(CommandType.DoNOTHING, ""));
                        break;
                    case 'q':
                        gameover = true;
                        break;
                }
            }

            Console.WriteLine("** YOU WIN! Score:" + game.Player.Kp + ". Go ahead and brag it out.");
        }

        private static void CheckPossibleActions()
        {
            
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
        /// Initialized the game configuration
        /// </summary>
        private static void InitGameConfig()
        {
            _gameConfiguration.numberOfRooms = GetRandom().Next(5, 15);
            _gameConfiguration.maxRoomCapacity = 10;
            _gameConfiguration.dungeonShape = GetRandomDungeonShape();
            _gameConfiguration.initialNumberOfMonsters = GetRandom().Next(15, 30);
            _gameConfiguration.initialNumberOfHealingPots = GetRandom().Next(1, 10);
            _gameConfiguration.initialNumberOfRagePots = GetRandom().Next(1, 10);
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
    }
}
