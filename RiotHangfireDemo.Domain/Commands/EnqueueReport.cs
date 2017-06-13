using MediatR;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Create a random fake report that can be added to the Queue.
    /// </summary>
    public class EnqueueReport : IRequest<Unit>, ICommand
    {
        internal class Handler : IRequestHandler<EnqueueReport, Unit>
        {
            private readonly IQueue _queue;

            public Handler(IQueue queue)
            {
                _queue = queue;
            }

            public Unit Handle(EnqueueReport cmd)
            {
                var name = Faker.Name.FullName();

                _queue.Enqueue(new GenerateReportTask
                {
                    User = Faker.Internet.UserName(name),
                    Title = Faker.Company.BS(),
                });

                return Unit.Value;
            }
        };
    };
}