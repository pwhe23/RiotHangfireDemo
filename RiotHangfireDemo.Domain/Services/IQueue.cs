namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Enqueue tasks to be executed in the background.
    /// </summary>
    public interface IQueue
    {
        void Enqueue(BackgroundTask task);
    };
}
