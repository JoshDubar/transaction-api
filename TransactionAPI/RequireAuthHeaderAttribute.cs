using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TransactionAPI
{
    public class RequireAuthHeaderAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (string.IsNullOrEmpty(context.HttpContext.Request.Headers["Authorization"])) {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
