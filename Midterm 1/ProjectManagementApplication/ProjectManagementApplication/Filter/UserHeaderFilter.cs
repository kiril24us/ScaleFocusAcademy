using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace ProjectManagementApplication.Api.Filter
{
    public class UserHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            string name = context.MethodInfo.Name;

            if (name != "Login")
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "AuthenticationUsernameId",
                    In = ParameterLocation.Header,
                    Required = true
                });
            }
        }
    }
}
