using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<Guid> Create(T entity);
        Task<ICollection<T>> GetAll();
        Task<bool> Update(Guid id,T entity);
        Task<bool> Delete(Guid id);
        Task<T> Get(Guid id);
    }
}