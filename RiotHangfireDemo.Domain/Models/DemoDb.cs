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

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }

        public virtual T Add<T>(T entity) where T : class
        {
            var entry = Entry(entity);
            entry.State = EntityState.Added;
            return entity;
        }

        public virtual T Delete<T>(T entity) where T : class
        {
            var entry = Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Deleted;
            }
            return entity;
        }
    };
}