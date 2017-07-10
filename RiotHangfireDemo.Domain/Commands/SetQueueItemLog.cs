using System.Linq;

namespace RiotHangfireDemo.Domain
{
    public class SetQueueItemLog : Command
    {
        public string Log { get; set; }
        public int[] ItemIds { get; set; }

        internal class Handler : CommandHandler<SetQueueItemLog, CommandResponse>
        {
            private readonly IDb _db;
            private readonly IPusher _pusher;

            public Handler(IDb db, IPusher pusher)
            {
                _db = db;
                _pusher = pusher;
            }

            public override CommandResponse Handle(SetQueueItemLog cmd)
            {
                if (cmd.ItemIds == null || cmd.ItemIds.Length < 1)
                    return CommandResponse.Error("No items selected");

                var queueItems = _db
                    .Query<QueueItem>()
                    .Where(x => cmd.ItemIds.Contains(x.Id))
                    .ToArray();

                foreach (var queueItem in queueItems)
                {
                    queueItem.Log = cmd.Log;
                }

                _db.Commit();
                _pusher.NotifyQueueItemsChanged();

                return CommandResponse.Success();
            }
        };
    };
}
