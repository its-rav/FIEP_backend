
using DataTier.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTier.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected FIEPContext _context;
        protected DbSet<T> table = null;

        public GenericRepository(FIEPContext context)
        {
            this._context = context;
            table = _context.Set<T>();
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public IQueryable<T> FindAllByProperty(Func<T, bool> expression)
        {
            return table.Where(expression).AsQueryable();
        }

        public T FindFirstByProperty(Func<T, bool> expression)
        {
            return table.FirstOrDefault(expression);
        }

        public IQueryable<T> GetAll()
        {
            return table.Select(x => x);
        }
        public EntityEntry<T> Insert(T obj)
        {
            var result = table.Add(obj);
            return result;
        }

        public void InsertRange(List<T> obj)
        {
            table.AddRange(obj);
        }

        public void Update(T obj)
        {
            table.Update(obj);
        }

        public void UpdateRange(List<T> obj)
        {
            table.UpdateRange(obj);
        }


    }
}
