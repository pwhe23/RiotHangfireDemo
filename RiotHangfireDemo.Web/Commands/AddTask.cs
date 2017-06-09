using System;
using MediatR;

namespace RiotHangfireDemo
{
    public class AddTask : IRequest<Unit>
    {
        internal class Handler : IRequestHandler<AddTask, Unit>
        {
            private readonly DemoDb _db;
            private readonly Queue _queue;

            public Handler(DemoDb db, Queue queue)
            {
                _db = db;
                _queue = queue;
            }

            public Unit Handle(AddTask cmd)
            {
                var task = new Task
                {
                    Name = Guid.NewGuid().ToString("N"),
                    Completed = false,
                };

                _db.Tasks.Add(task);
                _db.SaveChanges();

                _queue.Enqueue(task);

                return Unit.Value;
            }
        };
    };
}