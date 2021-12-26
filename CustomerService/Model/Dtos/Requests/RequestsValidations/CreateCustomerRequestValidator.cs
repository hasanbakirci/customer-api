using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace CustomerService.Model.Dtos.Requests.RequestsValidations
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(r => r.Name).MinimumLength(2).MaximumLength(25).NotEmpty();
            RuleFor(r => r.Email).EmailAddress().NotEmpty();

            RuleFor(r => r.CreateAddressRequest.AddressLine).MinimumLength(3).MaximumLength(150).NotEmpty();
            RuleFor(r => r.CreateAddressRequest.City).MinimumLength(3).MaximumLength(25).NotEmpty();
            RuleFor(r => r.CreateAddressRequest.Country).MinimumLength(3).MaximumLength(25).NotEmpty();
            RuleFor(r => r.CreateAddressRequest.CityCode).GreaterThan(0).NotEmpty();
        }
    }
}