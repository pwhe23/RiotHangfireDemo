using System;

namespace RiotHangfireDemo
{
    public interface IRandomizer
    {
        int Next(int min, int max);
    };

    public class Randomizer : IRandomizer
    {
        private static readonly Random _random = new Random();

        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }
    };
}