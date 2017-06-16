using System;
using System.Threading;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// This is a fake task that simulates the sending
    /// of an email to be run in the background.
    /// </summary>
    public class SendEmailTask : BackgroundTask
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public override string Name => $"{base.Name}-{To}";

        internal class Handler : CommandHandler<SendEmailTask, TaskResult>
        {
            private readonly IRandomizer _randomizer;

            public Handler(IRandomizer randomizer)
            {
                _randomizer = randomizer;
            }

            public override TaskResult Handle(SendEmailTask cmd)
            {
                var timeout = _randomizer.GetRandomTimeout();
                Thread.Sleep(timeout);

                if (_randomizer.IsRandomError())
                    throw new Exception($"ERROR sending email to {cmd.To}");

                return new TaskResult
                {
                    Log = $"Sent email '{cmd.Subject}' to {cmd.To}",
                };
            }
        };
    };
}
