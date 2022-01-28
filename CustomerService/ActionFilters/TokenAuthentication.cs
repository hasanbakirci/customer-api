using System;
using System.Linq;
using System.Threading.Tasks;
using Core.ServerResponse;
using CustomerService.Clients.AuthClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerService.ActionFilters
{
    public class TokenAuthenticationAttribute : TypeFilterAttribute
    {
        public TokenAuthenticationAttribute(params string[] roles) : base(typeof(TokenAuthentication))
        {
            Arguments = new object[] {roles};
        }
    }

    public class TokenAuthentication : IAsyncActionFilter
    {
        private readonly string[] _roles;
        private readonly IAuthClient _authClient;

        public TokenAuthentication(IAuthClient authClient, string[] roles)
        {
            _authClient = authClient;
            _roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (String.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Token not found."));
                return;
            }
            
            var tokenIsValid = await _authClient.TokenValidate(token.Substring(7));
            if (tokenIsValid.Success == false || !_roles.ToList().Contains(tokenIsValid.Data.Role) )
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Invalid token"));
                return;
            }

            await next();
        }
        
    }
}
