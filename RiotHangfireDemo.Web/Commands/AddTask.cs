using MediatR;

namespace RiotHangfireDemo
{
    public class AddTask : IRequest<Unit>
    {
        internal class Handler : IRequestHandler<AddTask, Unit>
        {
            public Unit Handle(AddTask cmd)
            {
                return Unit.Value;
            }
        };
    };
}