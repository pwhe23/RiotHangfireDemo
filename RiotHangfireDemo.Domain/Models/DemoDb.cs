using System.Data.Entity;
using System.Linq;

namespace RiotHangfireDemo.Domain
{
    internal class DemoDb : DbContext, IDb
    {
        public DemoDb()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DemoDb>());
        }

        public DbSet<QueueItem> QueueItems { get; set; }

        public bool CreateDatabase()
        {
            return Database.CreateIfNotExists();
        }

        public virtual T Add<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Added;
            return entity;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }
    };
}