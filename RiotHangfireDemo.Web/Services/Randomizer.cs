using System;

namespace RiotHangfireDemo
{
    public interface IRandomizer
    {
        TimeSpan GetRandomTimeout();
        bool IsRandomError();
    };

    public class Randomizer : IRandomizer
    {
        private static readonly Random _random = new Random();

        public TimeSpan GetRandomTimeout()
        {
            return TimeSpan.FromSeconds(_random.Next(1, 30));
        }

        public bool IsRandomError()
        {
            return _random.Next(1, 10) == 5;
        }
    };
}