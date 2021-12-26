using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;

namespace CustomerService.Model
{
    public class Customer : Document
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
}