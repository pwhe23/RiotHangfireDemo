using System;
using System.Linq;

namespace RiotHangfireDemo.Domain
{
    public interface IDb : IDisposable
    {
        bool CreateDatabase();
        T Add<T>(T entity) where T : class;
        IQueryable<T> Query<T>() where T : class;
        int SaveChanges();
    };
}
