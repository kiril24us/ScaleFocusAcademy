using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectManagementApplication.Services.Auth;

namespace ProjectManagementApplication.Api.Attributes
{
    public class PermissionsAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (Authentication.CurrentUser.Role.ToString() == "RegularUser")
            {
                filterContext.Result = new StatusCodeResult(401);
            }
        }
    }
}

