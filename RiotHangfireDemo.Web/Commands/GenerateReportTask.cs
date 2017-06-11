using System;
using System.Threading;
using MediatR;

namespace RiotHangfireDemo
{
    public class GenerateReportTask : ITask
    {
        public string User { get; set; }
        public string Title { get; set; }
        public string Name => $"{nameof(GenerateReportTask)}-{User}";

        internal class Handler : IRequestHandler<GenerateReportTask, TaskResult>
        {
            private readonly IRandomizer _randomizer;

            public Handler(IRandomizer randomizer)
            {
                _randomizer = randomizer;
            }

            public TaskResult Handle(GenerateReportTask cmd)
            {
                var timeout = TimeSpan.FromSeconds(_randomizer.Next(1, 30));
                Thread.Sleep(timeout);

                if (_randomizer.Next(1, 7) == 4)
                    throw new Exception($"ERROR generating report for {cmd.User}");

                return new TaskResult
                {
                    Log = $"Generated report '{cmd.Title}' for {cmd.User}",
                };
            }
        };
    };
}