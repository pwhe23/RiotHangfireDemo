namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Define Notifications to be send to the client.
    /// </summary>
    public interface IPusher
    {
        void NotifyQueueItemsChanged();
    };
}
