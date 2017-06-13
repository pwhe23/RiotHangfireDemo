using System.Linq;
using MediatR;

namespace RiotHangfireDemo.Domain
{
    public class ClearQueue : IRequest<Unit>, ICommand
    {
        internal class Handler : IRequestHandler<ClearQueue, Unit>
        {
            private readonly IDb _db;
            private readonly IPusher _pusher;

            public Handler(IDb db, IPusher pusher)
            {
                _db = db;
                _pusher = pusher;
            }

            public Unit Handle(ClearQueue cmd)
            {
                var queueItems = _db
                    .Query<QueueItem>()
                    .ToList();

                queueItems.ForEach(x => _db.Delete(x));

                _db.SaveChanges();
                _pusher.NotifyQueueItemsChanged();

                return Unit.Value;
            }
        }
    };
}
