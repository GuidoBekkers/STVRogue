using System;

namespace STVrogue.Utils
{
    public static class RandomFactory
    {
        public static int Seed = 12345;
        private static Random _random = new Random(Seed);

        /// <summary>
        /// Random generator
        /// </summary>
        /// <returns>A reference to the global random generator</returns>
        public static ref readonly Random GetRandom()
        {
            return ref _random;
        }
        
        /// <summary>
        /// Resets the global random generator
        /// </summary>
        public static void Reset()
        {
            _random = new Random(Seed);
        }
    }
}