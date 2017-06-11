using System;
using System.Linq;
using Hangfire;
using MediatR;
using Newtonsoft.Json;

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

    public interface IQueue
    {
        void Enqueue(ITask task);
    };

    public class Queue : IQueue
    {
        private readonly IDb _db;
        private readonly IMediator _mediator;
        private readonly IClock _clock;

        public Queue(IDb db, IMediator mediator, IClock clock)
        {
            _db = db;
            _mediator = mediator;
            _clock = clock;
        }

        public void Enqueue(ITask task)
        {
            if (task == null)
                return;

            var queuedTask = new QueueItem
            {
                Name = task.Name,
                Status = QueueItem.QUEUED,
                Created = _clock.Now(),
                Type = task.GetType().FullName,
                Data = JsonConvert.SerializeObject(task),
            };

            _db.Add(queuedTask);
            _db.SaveChanges();

            BackgroundJob.Enqueue(() => Execute(queuedTask.Id));
        }

        //REF: http://docs.hangfire.io/en/latest/best-practices.html#make-job-arguments-small-and-simple
        public void Execute(int queueItemId)
        {
            var queueItem = _db
                .Query<QueueItem>()
                .Single(x => x.Id == queueItemId);

            queueItem.Status = QueueItem.RUNNING;
            queueItem.Started = _clock.Now();
            _db.SaveChanges();

            try
            {
                var commandType = Type.GetType(queueItem.Type, true, false);
                var cmd = (ITask)JsonConvert.DeserializeObject(queueItem.Data, commandType);
                var result = (TaskResult)_mediator.Execute(cmd);

                queueItem.Status = QueueItem.COMPLETED;
                queueItem.Completed = _clock.Now();
                queueItem.Log = result.Log;
            }
            catch (Exception ex)
            {
                queueItem.Status = QueueItem.ERROR;
                queueItem.Completed = _clock.Now();
                queueItem.Log = ex.Message;
            }

            _db.SaveChanges();
        }
    };
}
