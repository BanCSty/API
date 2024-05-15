using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity, CancellationToken cancellationToken);

        Task<List<T>> Select(CancellationToken cancellationToken);

        Task Delete(Guid id, CancellationToken cancellationToken);

        Task Update(T entity, CancellationToken cancellationToken);

    }
}
