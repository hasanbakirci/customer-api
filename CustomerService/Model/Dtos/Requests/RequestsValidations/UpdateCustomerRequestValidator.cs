using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace CustomerService.Model.Dtos.Requests.RequestsValidations
{
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(r => r.Name).MinimumLength(2).MaximumLength(25);
            RuleFor(r => r.Email).EmailAddress();

            RuleFor(r => r.Address.AddressLine).MinimumLength(3).MaximumLength(150).NotEmpty();
            RuleFor(r => r.Address.City).MinimumLength(3).MaximumLength(25).NotEmpty();
            RuleFor(r => r.Address.Country).MinimumLength(3).MaximumLength(25).NotEmpty();
            RuleFor(r => r.Address.CityCode).GreaterThan(0).NotEmpty();
        }
    }
}