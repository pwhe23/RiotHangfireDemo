using MediatR;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Background tasks which are serialized into QueueItems to be
    /// executed.
    /// </summary>
    public interface ITask : IRequest<TaskResult>, ICommand
    {
        string Name { get; }
    };

    /// <summary>
    /// Standardize the response class used by ITasks to pass
    /// results back to the QueueItem.
    /// </summary>
    public class TaskResult
    {
        public string Log { get; set; }
    };

    /// <summary>
    /// Enqueue tasks to be executed in the background.
    /// </summary>
    public interface IQueue
    {
        void Enqueue(ITask task);
    };
}
