using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.DTO.Requests.TaskRequests;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Api.CustomPolicy
{
    public class CreatorOrTeamMemberHandler : AuthorizationHandler<CreatorOrTeamMemberRequirement>
    {
        private readonly AppDbContext _databaseContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatorOrTeamMemberHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatorOrTeamMemberRequirement requirement)
        {
            string path = _httpContextAccessor.HttpContext.Request.Path;
            string[] splitPath = path.Split("/");

            string currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            if(splitPath[2] == "Tasks" && splitPath[3] == "AssignTaskToAUser")
            {
                int taskId = int.Parse(_httpContextAccessor.HttpContext.Request.Query["taskId"]);

                _httpContextAccessor.HttpContext.Request.EnableBuffering();

                AssignTaskToAUserRequestDTO assignTaskToAUserRequest;

                using (StreamReader reader = new StreamReader(_httpContextAccessor.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string requestBody = reader.ReadToEndAsync().Result;

                    assignTaskToAUserRequest = JsonSerializer.Deserialize<AssignTaskToAUserRequestDTO>(requestBody, options);
                }

                if (_databaseContext.Tasks.Any(x => x.IsActive && x.Id == taskId && x.Project.IsActive && (x.Project.UserId == currentUserId || (x.Project.Teams.Any(x => x.IsActive)
                 && x.Project.Teams.Any(x => x.Members.Any(x => x.Id == currentUserId && x.IsActive))))))
                {
                    context.Succeed(requirement);
                    _httpContextAccessor.HttpContext.Request.Body.Position = 0;

                    return Task.FromResult(0);
                }
            }

            if (splitPath[2] == "Tasks" && splitPath[3] == "GetAll")
            {
                int projectId = int.Parse(_httpContextAccessor.HttpContext.Request.Query["ProjectId"]);

                if (_databaseContext.Projects.Any(x => x.Id == projectId && x.IsActive && (x.UserId == currentUserId || (x.Teams.Any(x => x.IsActive)
                   && x.Teams.Any(x => x.Members.Any(x => x.Id == currentUserId) && x.IsActive == true)))))
                {
                    context.Succeed(requirement);

                    return Task.FromResult(0);
                }

                return Task.FromResult(0);
            }

            if (splitPath[2] == "Tasks" && (splitPath[3] == "Create" || splitPath[3] == "Edit" || splitPath[3] == "Delete"))
            {
                TaskCreateRequestDTO taskCreateRequestDTO;

                _httpContextAccessor.HttpContext.Request.EnableBuffering();

                using (StreamReader reader = new StreamReader(_httpContextAccessor.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string requestBody = reader.ReadToEndAsync().Result;

                    taskCreateRequestDTO = JsonSerializer.Deserialize<TaskCreateRequestDTO>(requestBody, options);
                }

                if (_databaseContext.Projects.Any(x => x.Id == taskCreateRequestDTO.ProjectId && x.IsActive && (x.UserId == currentUserId || (x.Teams.Any(x => x.IsActive)
                   && x.Teams.Any(x => x.Members.Any(x => x.Id == currentUserId) && x.IsActive == true)))))
                {
                    context.Succeed(requirement);
                    _httpContextAccessor.HttpContext.Request.Body.Position = 0;

                    return Task.FromResult(0);
                }                                       
            }

            return Task.FromResult(0);
        }
    }
}
