using System;
using STVrogue.GameControl;
using STVrogue.GameLogic;

namespace STVrogue
{
    /// <summary>
    /// This is the Main of STV Rogue. It loops over, reading user command.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 1,
                initialNumberOfHealingPots = 1,
                initialNumberOfRagePots = 1,
                difficultyMode = DifficultyMode.NORMALmode
            };
            Game game = new Game(gameConfiguration);
            Console.WriteLine(" _______ _________            _______  _______  _______           _______ ");
            Console.WriteLine("(  ____ \\\\__   __/|\\     /|  (  ____ )(  ___  )(  ____ \\|\\     /|(  ____ \\") ;
            Console.WriteLine("| (    \\/   ) (   | )   ( |  | (    )|| (   ) || (    \\/| )   ( || (    \\/");
            Console.WriteLine("| (_____    | |   | |   | |  | (____)|| |   | || |      | |   | || (__    ");
            Console.WriteLine("(_____  )   | |   ( (   ) )  |     __)| |   | || | ____ | |   | ||  __)   ");
            Console.WriteLine("      ) |   | |    \\ \\_/ /   | (\\ (   | |   | || | \\_  )| |   | || (      ");
            Console.WriteLine("/\\____) |   | |     \\   /    | ) \\ \\__| (___) || (___) || (___) || (____/\\") ;
            Console.WriteLine("\\_______)   )_(      \\_/     |/   \\__/(_______)(_______)(_______)(_______/");
            
            Console.WriteLine("Welcome stranger...");
            bool gameover = false;
            while (!gameover)
            {
                Console.WriteLine("You are in a room. It is dark, and it feels dangerous...");
                Console.WriteLine("Your action: Move(m)   | Pick-item(p)  | Do-nothing(SPACE) | Quit(q)");
                Console.Write    ("             Attack(a) |    Flee(f)    | Use-item(u) ");
                var c = Console.ReadKey().KeyChar;
                Console.WriteLine("");
                switch (c)
                {
                    case 'm' : game.Update(new Command(CommandType.MOVE, ""));
                        break;
                    case 'a' : game.Update(new Command(CommandType.ATTACK, ""));
                        break;
                    case 'u' : game.Update(new Command(CommandType.USE, ""));
                        break;
                    case 'f' : game.Update(new Command(CommandType.FLEE, ""));
                        break;
                    case ' ' : game.Update(new Command(CommandType.DoNOTHING, ""));
                        break;
                    case 'q' : gameover = true ;
                        break;
                }
            }
            Console.WriteLine("** YOU WIN! Score:" + game.Player.Kp + ". Go ahead and brag it out.");
        }

    
    }
}
