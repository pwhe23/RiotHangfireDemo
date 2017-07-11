using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    //REF: http://docs.hangfire.io/en/latest/extensibility/using-job-filters.html
    public class DemoHangfireJobFilter : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private readonly IPusher _pusher;

        public DemoHangfireJobFilter(IPusher pusher)
        {
            _pusher = pusher;
        }

        public void OnCreating(CreatingContext filterContext)
        {
            _pusher.NotifyQueueItemsChanged();
        }

        public void OnCreated(CreatedContext filterContext)
        {
            _pusher.NotifyQueueItemsChanged();
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            _pusher.NotifyQueueItemsChanged();
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            _pusher.NotifyQueueItemsChanged();
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _pusher.NotifyQueueItemsChanged();
        }

        public void OnStateElection(ElectStateContext context)
        {
            _pusher.NotifyQueueItemsChanged();
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _pusher.NotifyQueueItemsChanged();
        }
    };
}