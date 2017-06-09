using MediatR;

namespace RiotHangfireDemo
{
    public interface ITask : IRequest<TaskResult>
    {
        string Name { get; }
    };

    public class TaskResult
    {
        public string Log { get; set; }
    };
}
