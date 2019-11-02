using System;

namespace UsefullAlgorithms.MarkovChains
{
    public class SimpleRandom : IRandom
    {
        private readonly Random _random;

        public SimpleRandom()
        {
            _random = new Random();
        }

        public double Next()
        {
            return _random.NextDouble();
        }
    }
}
