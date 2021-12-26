using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using CustomerService.Model;
using CustomerService.Model.Dtos.Requests;
using CustomerService.Model.Dtos.Responses;

namespace CustomerService.Extensions
{
    public static class ConverterExtensions
    {
        public static Customer ConvertToCustomer(this CreateCustomerRequest request){
            return new Customer
                {
                    Name = request.Name,
                    Email = request.Email,
                    Address = new Address{
                                AddressLine = request.CreateAddressRequest.AddressLine,
                                City = request.CreateAddressRequest.City,
                                Country = request.CreateAddressRequest.Country,
                                CityCode = request.CreateAddressRequest.CityCode
                            }
                };
        }


        public static CustomerResponse ConvertToCustomerResponse(this Customer customer){
            var address = new AddressResponse
                {
                    AddressLine = customer.Address.AddressLine,
                    City = customer.Address.City,
                    Country = customer.Address.Country,
                    CityCode = customer.Address.CityCode
                };
            return new CustomerResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    AddressResponse = address,
                    CreatedAt = customer.CreatedAt,
                    UpdatedAt = customer.UpdatedAt,
                    isDeleted = customer.isDeleted
                };
        }

        public static IEnumerable<CustomerResponse> ConvertToCustomerListResponse(this IEnumerable<Customer> customers){
            List<CustomerResponse> customerResponses = new List<CustomerResponse>();
            customers.ToList().ForEach(c => customerResponses.Add(
                new CustomerResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Email = c.Email,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        isDeleted = c.isDeleted,
                        AddressResponse = new AddressResponse 
                                    {
                                        AddressLine = c.Address.AddressLine,
                                        City = c.Address.City,
                                        Country = c.Address.Country,
                                        CityCode = c.Address.CityCode
                                    }
                    }
            ));
            return customerResponses;
        }


        public static Customer ConverToCustomer(this UpdateCustomerRequest request, Guid id){
            return new Customer
                {
                    Id = id,
                    Name = request.Name,
                    Email = request.Email,
                    isDeleted = request.isDeleted,
                    Address = new Address {
                            AddressLine = request.UpdateAddressRequest.AddressLine,
                            City = request.UpdateAddressRequest.City,
                            Country = request.UpdateAddressRequest.Country,
                            CityCode = request.UpdateAddressRequest.CityCode
                        }
                };
        }

        // public static Address ConvertToAddress(this UpdateAddressRequest request){
        //     return new Address
        //         {
        //             AddressLine = request.AddressLine,
        //             City = request.City,
        //             Country = request.Country,
        //             CityCode = request.CityCode
        //         };
        // }


        public static List<Customer> ConvertToCustomerList(this List<CreateCustomerRequest> requests){
            List<Customer> customers = new List<Customer>();
            requests.ToList().ForEach(c => customers.Add(
                 new Customer
                 {
                     Name = c.Name,
                     Email = c.Email,
                     Address = new Address 
                        {
                            AddressLine = c.CreateAddressRequest.AddressLine,
                            City = c.CreateAddressRequest.City,
                            Country = c.CreateAddressRequest.Country,
                            CityCode = c.CreateAddressRequest.CityCode
                        }
                 }));
                 return customers;

        }
    }
}