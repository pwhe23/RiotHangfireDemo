using Microsoft.AspNet.SignalR;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// Push notifications to the clients via SignalR
    /// </summary>
    public class Pusher : IPusher
    {
        public void NotifyQueueItemsChanged()
        {
            Push("QueueItems.Changed");
        }

        //REF: http://docs.hangfire.io/en/latest/background-processing/tracking-progress.html
        private static void Push(string type, object data = null)
        {
            GlobalHost
                .ConnectionManager
                .GetHubContext<PushHub>()
                .Clients
                .All
                .Push(type, data);
        }
    };
}