using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using ProjectManagementApplication.Services.Auth;
using System.Linq;

namespace ProjectManagementApplication.Api.Attributes
{
    public class LoginAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if(Authentication.CurrentUser == null)
            {
                filterContext.Result = new StatusCodeResult(401);
                return;
            }
            int currentUserId = Authentication.CurrentUser.Id;

            StringValues authHeaders;

            filterContext.HttpContext.Request.Headers.TryGetValue("AuthenticationUsernameId", out authHeaders);

            if (authHeaders.Count != 0)
            {
                if (int.Parse(authHeaders.First()) != currentUserId)
                {
                    filterContext.Result = new StatusCodeResult(401);
                    return;
                }
            }
        }
    }
}
