﻿using System;
using System.Linq;

namespace RiotHangfireDemo.Domain
{
    public interface IDb : IDisposable
    {
        bool CreateDatabase();
        IQueryable<T> Query<T>() where T : class;
        T Add<T>(T entity) where T : class;
        T Delete<T>(T entity) where T : class;
        int SaveChanges();
    };
}
