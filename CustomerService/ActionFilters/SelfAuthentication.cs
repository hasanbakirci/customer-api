using System;
using System.Threading.Tasks;
using Core.ServerResponse;
using CustomerService.Clients.AuthClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerService.ActionFilters
{
    public class SelfAuthenticationAttribute: TypeFilterAttribute
    {
        public SelfAuthenticationAttribute() : base(typeof(SelfAuthentication))
        {
        }

    }
    public class SelfAuthentication : IAsyncActionFilter
    {
        private readonly IAuthClient _authClient;

        public SelfAuthentication(IAuthClient authClient)
        {
            _authClient = authClient;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var tokenWithBearerKeyword = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (String.IsNullOrEmpty(tokenWithBearerKeyword) || !tokenWithBearerKeyword.StartsWith("Bearer "))
            {
                context.Result =
                    new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized, "Token not found."));
                return;
            }
            if(!(context.ActionArguments["id"] is Guid id))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized, "Id not found."));
                return;
            }

            var token = tokenWithBearerKeyword.Split("Bearer ")[1];
            var tokenResponse = await _authClient.TokenValidate(token);
            if (tokenResponse.Data.Status == false || id.ToString() != tokenResponse.Data.Id)
            {
                context.Result =
                    new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized, "Invalid Token"));
                return;
            }

            await next();
        }
    }
}