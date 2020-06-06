
using DataTier.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTier.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected FEIP_be_dbContext _context;
        protected DbSet<T> table = null;

        public GenericRepository(FEIP_be_dbContext context)
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
        public void Insert(T obj)
        {
            table.Add(obj);
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
