namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Create a random fake email that can be added to the Queue.
    /// </summary>
    public class EnqueueEmail : Command
    {
        internal class Handler : CommandHandler<EnqueueEmail, CommandResponse>
        {
            private readonly IQueue _queue;

            public Handler(IQueue queue)
            {
                _queue = queue;
            }

            public override CommandResponse Handle(EnqueueEmail cmd)
            {
                var name = Faker.Name.FullName();

                _queue.Enqueue(new SendEmailTask
                {
                    To = Faker.Internet.Email(name),
                    Subject = Faker.Company.BS(),
                });

                return CommandResponse.Success();
            }
        };
    };
}