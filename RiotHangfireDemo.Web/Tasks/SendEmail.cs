using System;
using System.Threading;
using MediatR;

namespace RiotHangfireDemo
{
    public class SendEmail : ITask
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Name => $"{nameof(SendEmail)}-{To}";

        internal class Handler : IRequestHandler<SendEmail, TaskResult>
        {
            private readonly IRandomizer _randomizer;

            public Handler(IRandomizer randomizer)
            {
                _randomizer = randomizer;
            }

            public TaskResult Handle(SendEmail cmd)
            {
                var timeout = TimeSpan.FromSeconds(_randomizer.Next(1, 30));
                Thread.Sleep(timeout);

                if (_randomizer.Next(1, 7) == 4)
                    throw new Exception($"ERROR sending email to {cmd.To}");

                return new TaskResult
                {
                    Log = $"Sent email '{cmd.Subject}' to {cmd.To}",
                };
            }
        };
    };
}
