using System;
using System.Linq;
using Hangfire;
using MediatR;
using Newtonsoft.Json;

namespace RiotHangfireDemo
{
    public interface IQueue
    {
        void Enqueue(ITask task);
        void Execute(QueueItem queueItem);
    };

    public class Queue : IQueue
    {
        private readonly IDb _db;
        private readonly IMediator _mediator;
        private readonly IPusher _pusher;
        private readonly ITime _time;

        public Queue(IDb db, IMediator mediator, IPusher pusher, ITime time)
        {
            _db = db;
            _mediator = mediator;
            _pusher = pusher;
            _time = time;
        }

        public void Enqueue(ITask task)
        {
            if (task == null)
                return;

            var queuedTask = new QueueItem
            {
                Name = task.Name,
                Status = QueueItem.QUEUED,
                Created = _time.Now(),
                Type = task.GetType().FullName,
                Data = JsonConvert.SerializeObject(task),
            };

            _db.Add(queuedTask);
            _db.SaveChanges();

            BackgroundJob.Enqueue(() => ExecuteQueueItem(queuedTask.Id));
        }

        //REF: http://docs.hangfire.io/en/latest/best-practices.html#make-job-arguments-small-and-simple
        public static void ExecuteQueueItem(int queueItemId)
        {
            using (Ioc.BeginScope())
            {
                var db = Ioc.Get<IDb>();
                var queue = Ioc.Get<IQueue>();

                var task = db
                    .Query<QueueItem>()
                    .Single(x => x.Id == queueItemId);

                queue.Execute(task);
            }
        }

        public void Execute(QueueItem queueItem)
        {
            queueItem.Status = QueueItem.RUNNING;
            queueItem.Started = _time.Now();
            _db.SaveChanges();

            try
            {
                var commandType = Type.GetType(queueItem.Type, true, false);
                var task = (ITask)JsonConvert.DeserializeObject(queueItem.Data, commandType);
                var result = (TaskResult)_mediator.Execute(task);

                queueItem.Status = QueueItem.COMPLETED;
                queueItem.Completed = _time.Now();
                queueItem.Log = result.Log;
            }
            catch (Exception ex)
            {
                queueItem.Status = QueueItem.ERROR;
                queueItem.Completed = _time.Now();
                queueItem.Log = ex.ToString();
            }

            _db.SaveChanges();

            _pusher.Push("Refresh");
        }
    };
}
