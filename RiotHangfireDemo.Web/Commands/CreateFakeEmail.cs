using MediatR;

namespace RiotHangfireDemo
{
    public class CreateFakeEmail : IRequest<Unit>
    {
        internal class Handler : IRequestHandler<CreateFakeEmail, Unit>
        {
            private readonly IQueue _queue;

            public Handler(IQueue queue)
            {
                _queue = queue;
            }

            public Unit Handle(CreateFakeEmail message)
            {
                var name = Faker.Name.FullName();

                _queue.Enqueue(new SendEmail
                {
                    To = Faker.Internet.Email(name),
                    Subject = Faker.Company.BS(),
                    Body = $"Hello {name}," + Faker.Lorem.Paragraph(),
                });

                return Unit.Value;
            }
        }
    };
}