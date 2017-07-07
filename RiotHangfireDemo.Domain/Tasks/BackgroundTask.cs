namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Background tasks which are serialized into QueueItems to be
    /// executed.
    /// </summary>
    public abstract class BackgroundTask : IRequest<TaskResult>
    {
        public int? UserId { get; set; }
        public virtual string Name => GetType().Name;
    };

    /// <summary>
    /// Standardize the response class used by ITasks to pass
    /// results back to the QueueItem.
    /// </summary>
    public class TaskResult
    {
        public string Log { get; set; }
    };
}
