using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace RiotHangfireDemo
{
    //REF: http://docs.hangfire.io/en/latest/extensibility/using-job-filters.html
    public class HangfireJobPusher : JobFilterAttribute, IServerFilter, IApplyStateFilter
    {
        private readonly IPusher _pusher;

        public HangfireJobPusher(IPusher pusher)
        {
            _pusher = pusher;
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            _pusher.Push("QueueItems.Changed");
        }

        public void OnPerformed(PerformedContext filterContext)
        {
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _pusher.Push("QueueItems.Changed");
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    };
}