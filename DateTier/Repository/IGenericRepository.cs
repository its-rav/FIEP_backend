using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTier.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        EntityEntry<T> Insert(T obj);
        void Update(T obj);
        void InsertRange(List<T> obj);
        void UpdateRange(List<T> obj);
        IQueryable<T> FindAllByProperty(Func<T, bool> expression);
        T FindFirstByProperty(Func<T, bool> expression);
        int Commit();
    }
}
