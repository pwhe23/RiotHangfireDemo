using System.Linq;
using MediatR;

namespace RiotHangfireDemo.Domain
{
    public class DeleteQueueItem : IRequest<Unit>, ICommand
    {
        public int Id { get; set; }

        internal class Handler : IRequestHandler<DeleteQueueItem, Unit>
        {
            private readonly IDb _db;
            private readonly IPusher _pusher;

            public Handler(IDb db, IPusher pusher)
            {
                _db = db;
                _pusher = pusher;
            }

            public Unit Handle(DeleteQueueItem cmd)
            {
                var queueItem = _db
                    .Query<QueueItem>()
                    .Single(x => x.Id == cmd.Id);

                _db.Delete(queueItem);
                _db.SaveChanges();

                _pusher.NotifyQueueItemsChanged();

                return Unit.Value;
            }
        }
    };
}
