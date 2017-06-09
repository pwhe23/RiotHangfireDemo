using System.Linq;
using Hangfire;

namespace RiotHangfireDemo
{
    public class Queue
    {
        public void Enqueue(Task task)
        {
            BackgroundJob.Enqueue(() => Execute(task.Id));
        }

        public static void Execute(int taskId)
        {
            using (Ioc.BeginScope())
            {
                var db = Ioc.Get<DemoDb>();

                var task = db.Tasks.Single(x => x.Id == taskId);
                task.Completed = true;

                db.SaveChanges();
            }
        }
    };
}
