using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ServerResponse;
using Core.Validation;
using CustomerService.Clients.MessageQueueClients;
using CustomerService.Extensions;
using CustomerService.Model;
using CustomerService.Model.Dtos.Requests;
using CustomerService.Model.Dtos.Requests.RequestsValidations;
using CustomerService.Model.Dtos.Responses;
using CustomerService.Repositories.Interfaces;
using CustomerService.Services.Interfaces;

namespace CustomerService.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessageQueueClient _messageQueueClient;

        public CustomersService(ICustomerRepository customerRepository, IMessageQueueClient messageQueueClient)
        {
            _customerRepository = customerRepository;
            _messageQueueClient = messageQueueClient;
        }

        public async Task<Response<Guid>> Create(CreateCustomerRequest request)
        {
            ValidationTool.Validate(new CreateCustomerRequestValidator(), request);

            var customer = request.ConvertToCustomer();
            var result = await _customerRepository.Create(customer);
            _messageQueueClient.Publish<Customer>(RabbitMQHelper.CreatedQueue,customer);
            return new SuccessResponse<Guid>(result);
            
        }

        public async Task<Response<bool>> CreateMany(List<CreateCustomerRequest> requests)
        {
            foreach(var request in requests){
                ValidationTool.Validate(new CreateCustomerRequestValidator(), request);
            }
            
            var customers = requests.ConvertToCustomerList();
            var result = await _customerRepository.CreateMany(customers);
            if(result)
                return new SuccessResponse<bool>(result);
            return new ErrorResponse<bool>(ResponseStatus.BadRequest,default,ResultMessage.Error);
        }

        public async Task<Response<bool>> Delete(Guid id)
        {
            var result = await _customerRepository.Delete(id);
            if (result)
            {
                _messageQueueClient.Publish(RabbitMQHelper.DeletedQueue,id);
                return new SuccessResponse<bool>(result);
            }
                
            return new ErrorResponse<bool>(result);
        }

        public async Task<Response<IEnumerable<CustomerResponse>>> GetAll()
        {
            var result = await _customerRepository.GetAll();
            return new SuccessResponse<IEnumerable<CustomerResponse>>(result.ConvertToCustomerListResponse());
        }

        public async Task<Response<IEnumerable<CustomerResponse>>> GetAllBySoftDeleted()
        {
            var customers = await _customerRepository.GetAllBySoftDeleted();
            return new SuccessResponse<IEnumerable<CustomerResponse>>(customers.ConvertToCustomerListResponse());
        }

        public async Task<Response<CustomerResponse>> GetById(Guid id)
        {
            var result = await _customerRepository.Get(id);
            if(result is not null)
                return new SuccessResponse<CustomerResponse>(result.ConvertToCustomerResponse());
            return new ErrorResponse<CustomerResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundCustomer);
        }

        public async Task<Response<CustomerPaginationResponse>> Page(int from, int size)
        {
            var customers = await _customerRepository.Page(from,size);
            var totalItemCount = await _customerRepository.TotalCountOfCustomer();
            return new SuccessResponse<CustomerPaginationResponse>(customers.ConvertToPaginationCustomerResponse(from, size,totalItemCount));
        }

        public async Task<Response<bool>> SoftDelete(Guid id)
        {
            var result = await _customerRepository.SoftDelete(id);
            if(result)
                return new SuccessResponse<bool>(result);
            return new ErrorResponse<bool>(result);
        }

        public async Task<Response<bool>> Update(Guid id,UpdateCustomerRequest request)
        {
            ValidationTool.Validate(new UpdateCustomerRequestValidator(), request);

            var customer = request.ConverToCustomer(id);
            var result = await _customerRepository.Update(customer.Id, customer);
            if (result)
            { 
               _messageQueueClient.Publish<Customer>(RabbitMQHelper.UpdatedQueue,customer);
               return new SuccessResponse<bool>(result);
            }
            return new ErrorResponse<bool>(result);
            
        }

        public async Task<Response<bool>> Validate(Guid id)
        {
            var result = await _customerRepository.Validate(id);
            if (result)
                return new SuccessResponse<bool>(result);
            return new ErrorResponse<bool>(result);
        }
    }
}