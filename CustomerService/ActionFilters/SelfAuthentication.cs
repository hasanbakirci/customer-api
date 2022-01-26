using System;
using System.Threading.Tasks;
using Core.Helper;
using Core.ServerResponse;
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
            var tokenResponse = JwtHelper.ValidateJwtToken(token);
            if (tokenResponse.Status == false || id.ToString() != tokenResponse.Id)
            {
                context.Result =
                    new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized, "Invalid Token"));
                return;
            }

            await next();
        }
    }
}