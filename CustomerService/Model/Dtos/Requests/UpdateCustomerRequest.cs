using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Model.Dtos.Requests
{
    public class UpdateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool isDeleted { get; set; }
        public UpdateAddressRequest Address { get; set; }
    }
}