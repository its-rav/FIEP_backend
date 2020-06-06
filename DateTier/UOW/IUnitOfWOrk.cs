using DataTier.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTier.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>()
       where T : class;

        int Commit();

    }
}
