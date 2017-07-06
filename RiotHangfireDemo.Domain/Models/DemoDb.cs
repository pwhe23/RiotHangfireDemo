using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// The DemoDb context is used to communicate with our SQL Server database.
    /// The only thing we're storing right now are the QueueItems. Hangfire creates
    /// its own tables in a separate [Hangfire] schema.
    /// </summary>
    internal class DemoDb : DbContext, IDb
    {
        public DemoDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DemoDb, DemoDbMigrations>());
        }

        public DbSet<QueueItem> QueueItems { get; set; }
        public DbSet<User> Users { get; set; }

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

        public int Commit()
        {
            return SaveChanges();
        }
    };

    internal class DemoDbMigrations : DbMigrationsConfiguration<DemoDb>
    {
        public DemoDbMigrations()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            MigrationsAssembly = typeof(DemoDbMigrations).Assembly;
            MigrationsNamespace = typeof(DemoDbMigrations).Namespace;
        }

        protected override void Seed(DemoDb db)
        {
            var admin = new User
            {
                Email = "paul@tagovi.com",
                Password = PasswordHash.CreateHash("pw"),
                Role = User.ADMIN,
            };

            var user = new User
            {
                Email = "user@tagovi.com",
                Password = PasswordHash.CreateHash("pw"),
                Role = User.USER,
            };

            db.Users.AddOrUpdate(x => x.Email, new[]
            {
                admin,
                user,
            });

            db.SaveChanges();
        }
    };
}