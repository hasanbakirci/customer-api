using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Model.Dtos.Responses
{
    public class CustomerPaginationResponse
    {
        public List<CustomerResponse> CustomerResponses { get; set; }
        public int Page { get; set; }
        public int FormSize { get; set; }
        public long TotalItemCount { get; set; }
        public int CurrentItemCount { get; set; }
    }
}