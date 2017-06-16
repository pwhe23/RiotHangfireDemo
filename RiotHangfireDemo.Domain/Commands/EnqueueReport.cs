namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Create a random fake report that can be added to the Queue.
    /// </summary>
    public class EnqueueReport : Command
    {
        internal class Handler : CommandHandler<EnqueueReport, CommandResponse>
        {
            private readonly IQueue _queue;

            public Handler(IQueue queue)
            {
                _queue = queue;
            }

            public override CommandResponse Handle(EnqueueReport cmd)
            {
                var name = Faker.Name.FullName();

                _queue.Enqueue(new GenerateReportTask
                {
                    User = Faker.Internet.UserName(name),
                    Title = Faker.Company.BS(),
                });

                return CommandResponse.Success();
            }
        };
    };
}