using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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
        private readonly ITime _time;

        public Queue(IDb db, IMediator mediator, ITime time)
        {
            _db = db;
            _mediator = mediator;
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
                var result = (TaskResult)ExecuteCommandViaMediator(task);

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
        }

        private object ExecuteCommandViaMediator(object request)
        {
            if (request == null)
                return null;

            try
            {
                var requestInterface = request.GetType().GetInterface("IRequest`1");
                var send = _mediator.GetType().GetMethod("Send").MakeGenericMethod(requestInterface.GetGenericArguments());
                return send.Invoke(_mediator, new[] { request });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Queue.Send ERROR on request type " + request.GetType(), ex);
            }

            return null;
        }
    };
}
