using System.Linq;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Delete a specific QueueItem from the Queue.
    /// </summary>
    public class DeleteQueueItem : Command
    {
        public int Id { get; set; }

        internal class Handler : CommandHandler<DeleteQueueItem, CommandResponse>
        {
            private readonly IDb _db;
            private readonly IPusher _pusher;

            public Handler(IDb db, IPusher pusher)
            {
                _db = db;
                _pusher = pusher;
            }

            public override CommandResponse Handle(DeleteQueueItem cmd)
            {
                var queueItem = _db
                    .Query<QueueItem>()
                    .Single(x => x.Id == cmd.Id);

                _db.Delete(queueItem);
                _db.Commit();

                _pusher.NotifyQueueItemsChanged();

                return CommandResponse.Success();
            }
        }
    };
}
