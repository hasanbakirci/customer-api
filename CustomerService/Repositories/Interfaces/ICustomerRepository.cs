using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using CustomerService.Model;

namespace CustomerService.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAllBySoftDeleted();
        Task<bool> CreateMany(List<Customer> customers);
        Task<bool> SoftDelete(Guid id);
        Task<bool> Validate(Guid id);
    }
}