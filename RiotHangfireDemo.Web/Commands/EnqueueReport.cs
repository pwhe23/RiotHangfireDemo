using MediatR;

namespace RiotHangfireDemo
{
    public class EnqueueReport : IRequest<Unit>
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

                _queue.Enqueue(new GenerateReport
                {
                    User = Faker.Internet.UserName(name),
                    Title = Faker.Company.BS(),
                });

                return Unit.Value;
            }
        };
    };
}