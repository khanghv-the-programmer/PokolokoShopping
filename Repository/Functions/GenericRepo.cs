using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.DataContext;
using Repository.Interfaces;

namespace Repository.Functions
{
    public class GenericRepo<T> : IGenereicInterface<T> where T : class
    {
        private DatabaseContext _context = null;
        protected DbSet<T> table = null;

        public GenericRepo()
        {
            _context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            table = _context.Set<T>();
        }
        public GenericRepo(DatabaseContext context)
        {
            _context = context;
            table = context.Set<T>();
        }

        public async Task Add(T obj)
        {
           await table.AddAsync(obj);    
        }

        public virtual Task Delete(object ID)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetBy(object ID)
        {
            throw new NotImplementedException();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public virtual Task Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
