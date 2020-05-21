using System;

namespace STVrogue.Utils
{
    public static class RandomFactory
    {
        private const int Seed = 1234;
        private static readonly Random Random = new Random(Seed);

        public static ref readonly Random GetRandom()
        {
            return ref Random;
        }
    }
}