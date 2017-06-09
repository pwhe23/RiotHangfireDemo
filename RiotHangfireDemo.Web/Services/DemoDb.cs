using System.Data.Entity;

namespace RiotHangfireDemo
{
    public class DemoDb : DbContext
    {
        public DemoDb()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DemoDb>());
        }

        public DbSet<QueueItem> QueueItems { get; set; }
    };
}