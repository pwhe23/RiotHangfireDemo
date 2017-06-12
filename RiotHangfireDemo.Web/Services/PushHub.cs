using Microsoft.AspNet.SignalR;

namespace RiotHangfireDemo.Web
{
    public class PushHub : Hub
    {
        public void Push(string type, object data = null)
        {
            Clients.All.Push(type, data);
        }
    };
}