using System;
using System.Threading;
using MediatR;

namespace RiotHangfireDemo
{
    public class GenerateReport : ITask
    {
        public string User { get; set; }
        public string Title { get; set; }
        public string Name => $"{nameof(GenerateReport)}-{User}";

        internal class Handler : IRequestHandler<GenerateReport, TaskResult>
        {
            private static readonly Random _random = new Random();

            public TaskResult Handle(GenerateReport cmd)
            {
                var timeout = TimeSpan.FromSeconds(_random.Next(1, 30));
                Thread.Sleep(timeout);

                if (_random.Next(1, 7) == 4)
                    throw new Exception($"ERROR generating report for {cmd.User}");

                return new TaskResult
                {
                    Log = $"Generated report '{cmd.Title}' for {cmd.User}",
                };
            }
        };
    };
}