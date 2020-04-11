using System;
using STVrogue.GameControl;
using STVrogue.GameLogic;

namespace STVrogue
{
    /// <summary>
    /// This is the Main of STV Rogue. It loops over, reading user command.
    /// </summary>
f    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game() ;
            Console.WriteLine(" _______ _________            _______  _______  _______           _______ ");
            Console.WriteLine("(  ____ \\\\__   __/|\\     /|  (  ____ )(  ___  )(  ____ \\|\\     /|(  ____ \\") ;
            Console.WriteLine("| (    \\/   ) (   | )   ( |  | (    )|| (   ) || (    \\/| )   ( || (    \\/");
            Console.WriteLine("| (_____    | |   | |   | |  | (____)|| |   | || |      | |   | || (__    ");
            Console.WriteLine("(_____  )   | |   ( (   ) )  |     __)| |   | || | ____ | |   | ||  __)   ");
            Console.WriteLine("      ) |   | |    \\ \\_/ /   | (\\ (   | |   | || | \\_  )| |   | || (      ");
            Console.WriteLine("/\\____) |   | |     \\   /    | ) \\ \\__| (___) || (___) || (___) || (____/\\") ;
            Console.WriteLine("\\_______)   )_(      \\_/     |/   \\__/(_______)(_______)(_______)(_______/");
            
            Console.WriteLine("Welcome stranger...");
            while (true)
            {
                Console.Write("Your action: Move(m) | Attack(a) | Use-item(u) | Flee(f) | Nothing(SPACE) | Quit(q)");
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
                    case 'q' : return;
                }
            }
            
        }

    
    }
}
