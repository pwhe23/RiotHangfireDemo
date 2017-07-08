namespace RiotHangfireDemo.Domain
{
    public class RequeueTasks : Command
    {
        public int[] TaskIds { get; set; }

        internal class Handler : CommandHandler<RequeueTasks, CommandResponse>
        {
            private readonly IQueue _queue;

            public Handler(IQueue queue)
            {
                _queue = queue;
            }

            public override CommandResponse Handle(RequeueTasks cmd)
            {
                foreach (var taskId in cmd.TaskIds)
                {
                    _queue.Requeue(taskId);
                }

                return CommandResponse.Success($"Requeued {cmd.TaskIds.Length} Task(s)");
            }
        }
    };
}
