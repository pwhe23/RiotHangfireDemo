using MediatR;

namespace RiotHangfireDemo.Domain
{
    public interface ITask : IRequest<TaskResult>, ICommand
    {
        string Name { get; }
    };

    public class TaskResult
    {
        public string Log { get; set; }
    };

    public interface IQueue
    {
        void Enqueue(ITask task);
    };
}
