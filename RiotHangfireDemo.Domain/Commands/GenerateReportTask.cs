using System;
using System.Threading;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// This is a fake task that simulates the generation
    /// of a slow report to be run in the background.
    /// </summary>
    public class GenerateReportTask : BackgroundTask
    {
        public string User { get; set; }
        public string Title { get; set; }
        public override string Name => $"{base.Name}-{User}";

        internal class Handler : CommandHandler<GenerateReportTask, TaskResult>
        {
            private readonly IRandomizer _randomizer;

            public Handler(IRandomizer randomizer)
            {
                _randomizer = randomizer;
            }

            public override TaskResult Handle(GenerateReportTask cmd)
            {
                var timeout = _randomizer.GetRandomTimeout();
                Thread.Sleep(timeout);

                if (_randomizer.IsRandomError())
                    throw new Exception($"ERROR generating report for {cmd.User}");

                return new TaskResult
                {
                    Log = $"Generated report '{cmd.Title}' for {cmd.User}",
                };
            }
        };
    };
}