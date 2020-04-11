using System;
using System.Diagnostics;

namespace STVrogue.Utils
{
    public class Utils
    {
        /// <summary>
        /// Log the string. The current behavior is to simply print the string to
        /// the console. Change the method if you want a different behavior.
        /// </summary>
        public static void Log(String s)
        {
            Console.Out.WriteLine("** " + s);
            Debug.WriteLine("** " + s);
        }

        /// <summary>
        /// Return a pseudo-random generator, using the given seed.
        /// </summary>
        public static Random MkPseudoRandomGen(int seed)
        {
            return new Random(seed);
        }

    }
}
