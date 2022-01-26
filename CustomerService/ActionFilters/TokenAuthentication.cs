using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Helper;
using Core.ServerResponse;
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

    public class TokenAuthentication : IActionFilter
    {
        private readonly string[] _roles;

        public TokenAuthentication(string[] roles)
        {
            _roles = roles;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var tokenWithBearerKeyword = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (String.IsNullOrEmpty(tokenWithBearerKeyword) || !tokenWithBearerKeyword.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Token not found."));
                return;
            }

            var token = tokenWithBearerKeyword.Split("Bearer ")[1];
            var tokenResponse = JwtHelper.ValidateJwtToken(token);
            if (tokenResponse.Status == false || !_roles.ToList().Contains(tokenResponse.Role))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Invalid Token"));
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }
    }
}
