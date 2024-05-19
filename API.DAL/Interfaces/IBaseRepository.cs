using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.DAL.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task Create(T entity, CancellationToken cancellationToken);

        IQueryable<T> Select();

        Task Delete(Guid id, CancellationToken cancellationToken);

        Task Update(T entity, CancellationToken cancellationToken);

        EntityEntry<T> Entry(T entity);
    }
}
