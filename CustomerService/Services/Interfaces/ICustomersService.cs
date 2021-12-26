using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.ServerResponse;
using CustomerService.Model.Dtos.Requests;
using CustomerService.Model.Dtos.Responses;

namespace CustomerService.Services.Interfaces
{
    public interface ICustomersService
    {
        Task<Response<Guid>> Create(CreateCustomerRequest request);
        Task<Response<IEnumerable<CustomerResponse>>> GetAll();
        Task<Response<CustomerResponse>> GetById(Guid id);
        Task<Response<bool>> Delete(Guid id);
        Task<Response<bool>> Update(Guid id,UpdateCustomerRequest request);
        Task<Response<IEnumerable<CustomerResponse>>> GetAllBySoftDeleted();
        Task<Response<bool>> CreateMany(List<CreateCustomerRequest> requests);
        Task<Response<bool>> SoftDelete(Guid id);
        Task<Response<bool>> Validate(Guid id);
        
    }
}