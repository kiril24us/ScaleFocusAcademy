using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.DTO.Requests.ProjectRequests;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Api.CustomPolicy
{
    public class CreatorHandler : AuthorizationHandler<CreatorRequirement>
    {
        private readonly AppDbContext _databaseContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatorHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatorRequirement requirement)
        {
            string path = _httpContextAccessor.HttpContext.Request.Path;
            string[] splitPath = path.Split("/");

            string currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (splitPath[3] == "AssignProjectToATeam")
            {
                AssignProjectToATeamRequestDTO assignProjectToATeamRequest;

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

                _httpContextAccessor.HttpContext.Request.EnableBuffering();

                using (StreamReader reader = new StreamReader(_httpContextAccessor.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string requestBody = reader.ReadToEndAsync().Result;

                    assignProjectToATeamRequest = JsonSerializer.Deserialize<AssignProjectToATeamRequestDTO>(requestBody, options);
                }

                if (_databaseContext.Projects.Any(x => x.Id == assignProjectToATeamRequest.ProjectId && x.IsActive && x.UserId == currentUserId))
                {
                    context.Succeed(requirement);
                    _httpContextAccessor.HttpContext.Request.Body.Position = 0;
                    return Task.FromResult(0);
                }

                return Task.FromResult(0);
            }

            int productId = int.Parse(splitPath[4]);                     
                                  
            if ((splitPath[3] == "Delete" || splitPath[3] == "Edit") && splitPath[2] == "Projects")
            {
                if (_databaseContext.Projects.Any(x => x.Id == productId && x.UserId == currentUserId))
                {
                    context.Succeed(requirement);
                    return Task.FromResult(0);
                }
            }

            return Task.FromResult(0);
        }
    }
}
