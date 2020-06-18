using System;

namespace STVrogue.Utils
{
    public static class RandomFactory
    {
        private const int Seed = 1234;
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