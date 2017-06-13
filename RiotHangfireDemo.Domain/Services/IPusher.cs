namespace RiotHangfireDemo.Domain
{
    public interface IPusher
    {
        void NotifyQueueItemsChanged();
    };
}
