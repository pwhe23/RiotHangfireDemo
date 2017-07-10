namespace RiotHangfireDemo.Domain
{
    public class RequeueItems : Command
    {
        public int[] ItemIds { get; set; }

        internal class Handler : CommandHandler<RequeueItems, CommandResponse>
        {
            private readonly IQueue _queue;

            public Handler(IQueue queue)
            {
                _queue = queue;
            }

            public override CommandResponse Handle(RequeueItems cmd)
            {
                if (cmd.ItemIds == null || cmd.ItemIds.Length < 1)
                    return CommandResponse.Error("No items selected");

                foreach (var queueItemId in cmd.ItemIds)
                {
                    _queue.Requeue(queueItemId);
                }

                return CommandResponse.Success($"Requeued {cmd.ItemIds.Length} Task(s)");
            }
        };
    };
}
