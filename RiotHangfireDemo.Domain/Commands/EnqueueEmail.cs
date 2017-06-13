using MediatR;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Create a random fake email that can be added to the Queue.
    /// </summary>
    public class EnqueueEmail : IRequest<Unit>, ICommand
    {
        internal class Handler : IRequestHandler<EnqueueEmail, Unit>
        {
            private readonly IQueue _queue;

            public Handler(IQueue queue)
            {
                _queue = queue;
            }

            public Unit Handle(EnqueueEmail cmd)
            {
                var name = Faker.Name.FullName();

                _queue.Enqueue(new SendEmailTask
                {
                    To = Faker.Internet.Email(name),
                    Subject = Faker.Company.BS(),
                });

                return Unit.Value;
            }
        };
    };
}