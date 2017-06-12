using Microsoft.AspNet.SignalR;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class Pusher : IPusher
    {
        //REF: http://docs.hangfire.io/en/latest/background-processing/tracking-progress.html
        public void Push(string type, object data = null)
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