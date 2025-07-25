using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Data;
using Yonetim360.DataAccess.Repository.Abstract;

namespace Yonetim360.DataAccess.Repository.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context, DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }
        #region WriteRepository

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
             _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region ReadRepository
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter, int pageSize = 100, int pageNumber = 1)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, bool tracked = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            var query = CreateQuery(filter, include);

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(filter);
        }


        public IQueryable<T> CreateQuery(Expression<Func<T, bool>>? predicate = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }
        #endregion
    }
}
