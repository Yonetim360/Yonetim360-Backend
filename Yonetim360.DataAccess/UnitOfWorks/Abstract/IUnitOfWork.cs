using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;

namespace Yonetim360.DataAccess.UnitOfWorks.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        void SetEntityState<T>(T entity, EntityState state) where T : class;
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
