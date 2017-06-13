using System;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// Randomize values for our simulated Task classes.
    /// </summary>
    public class Randomizer : IRandomizer
    {
        private static readonly Random _random = new Random();
        private readonly DemoConfig _config;

        public Randomizer(DemoConfig config)
        {
            _config = config;
        }

        public TimeSpan GetRandomTimeout()
        {
            return TimeSpan.FromSeconds(_random.Next(1, _config.RandomizerTimeoutMax));
        }

        public bool IsRandomError()
        {
            return _random.Next(1, _config.RandomizerErrorMax) == _config.RandomizerErrorMax - 1;
        }
    };
}