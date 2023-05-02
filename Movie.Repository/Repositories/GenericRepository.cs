using Microsoft.EntityFrameworkCore;
using Movie.Core.Repositories;
using Movie.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context; // Veritabanı ile işlemler yapacağımız için _context
        private readonly DbSet<T> _dbSet; // Tablolarla işlem yapacağımız için _dbSet

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // Memory'e entity eklendi.
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity!=null) // id'ye sahip data var ise;
            {
                _context.Entry(entity).State = EntityState.Detached; // Entity memory'de takip edilmesin. Memory'de takip edilmesin, çünkü Update metodu ile zaten memory'e takip edileceğini bildiriyorum.
            }
            return entity;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity); // _context.Entry(entity).State = EntityState.Deleted;
        }

        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified; // _context.Update(entity);
            return entity;
            // Update metodu ile memory'e takip edileceğini bildiriyorum.
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}

/*
 
Task<IQueryable<T>> GetAllAsync();   

public IQueryable<T> GetAllAsync()
{
    return _dbSet.AsNoTracking().AsQueryable(); // datayı memory'e alma ve anlık olarak izleme
}

*/
