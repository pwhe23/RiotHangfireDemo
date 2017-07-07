using System;
using System.Linq;
using Hangfire;
using Newtonsoft.Json;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// Queue can Enqueue tasks to be executed by Hangfire.
    /// </summary>
    public class Queue : IQueue
    {
        private readonly IClock _clock;
        private readonly ICommander _commander;
        private readonly IDb _db;
        private readonly UserContext _userContext;

        public Queue(IClock clock, ICommander commander, IDb db, UserContext userContext)
        {
            _clock = clock;
            _commander = commander;
            _db = db;
            _userContext = userContext;
        }

        public void Enqueue(BackgroundTask task)
        {
            if (task == null)
                return;

            if (task.UserId == null)
            {
                task.UserId = _userContext.UserId;
            }

            var queuedTask = new QueueItem
            {
                Name = task.Name,
                Status = QueueItem.QUEUED,
                Created = _clock.Now(),
                Type = task.GetType().Name,
                Data = JsonConvert.SerializeObject(task),
            };

            _db.Add(queuedTask);
            _db.Commit();

            BackgroundJob.Enqueue(() => Execute(queuedTask.Id));
        }

        //REF: http://docs.hangfire.io/en/latest/best-practices.html#make-job-arguments-small-and-simple
        /// <summary>
        /// Called on a background thread by Hangfire.
        /// </summary>
        public void Execute(int queueItemId)
        {
            var queueItem = _db
                .Query<QueueItem>()
                .Single(x => x.Id == queueItemId);

            queueItem.Status = QueueItem.RUNNING;
            queueItem.Started = _clock.Now();
            _db.Commit();

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

            _db.Commit();
        }
    };
}
