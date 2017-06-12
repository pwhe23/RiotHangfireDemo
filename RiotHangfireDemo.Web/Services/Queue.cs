using System;
using System.Linq;
using Hangfire;
using Newtonsoft.Json;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class Queue : IQueue
    {
        private readonly IDb _db;
        private readonly ICommander _commander;
        private readonly IClock _clock;

        public Queue(IDb db, ICommander commander, IClock clock)
        {
            _db = db;
            _commander = commander;
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
                Type = task.GetType().Name,
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
                var result = (TaskResult)_commander.Execute(queueItem.Type, queueItem.Data);

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
