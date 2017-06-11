using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace RiotHangfireDemo
{
    //REF: http://docs.hangfire.io/en/latest/extensibility/using-job-filters.html
    public class HangfireLogPushFilter : JobFilterAttribute, IApplyStateFilter
    {
        private readonly IPusher _pusher;

        public HangfireLogPushFilter(IPusher pusher)
        {
            _pusher = pusher;
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