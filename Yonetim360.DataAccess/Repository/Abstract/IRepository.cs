using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.DataAccess.Repository.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, bool tracked = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter, int pageSize = 100, int pageNumber = 1);
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
