using System.Linq;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Delete all of the QueueItem records from the Queue. This will not clear
    /// out the Jobs from Hangfire, they will all just fail.
    /// </summary>
    public class ClearQueue : Command
    {
        internal class Handler : CommandHandler<ClearQueue, CommandResponse>
        {
            private readonly IDb _db;
            private readonly IPusher _pusher;

            public Handler(IDb db, IPusher pusher)
            {
                _db = db;
                _pusher = pusher;
            }

            public override CommandResponse Handle(ClearQueue cmd)
            {
                var queueItems = _db
                    .Query<QueueItem>()
                    .ToList();

                queueItems.ForEach(x => _db.Delete(x));

                _db.Commit();
                _pusher.NotifyQueueItemsChanged();

                return CommandResponse.Success();
            }
        }
    };
}
