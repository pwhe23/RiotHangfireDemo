namespace RiotHangfireDemo.Domain
{
    public interface IPusher
    {
        void Push(string type, object data = null);
    };
}
