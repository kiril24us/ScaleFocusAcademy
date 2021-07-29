using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.DTO.Requests.WorkLogRequests;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Api.CustomPolicy
{
    public class AssigneeHandler : AuthorizationHandler<AssigneeRequirement>
    {
        private readonly AppDbContext _databaseContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AssigneeHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AssigneeRequirement requirement)
        {
            string path = _httpContextAccessor.HttpContext.Request.Path;
            string[] splitPath = path.Split("/");

            string currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            if (splitPath[2] == "WorkLogs" && splitPath[3] == "Create")
            {
                WorkLogCreateRequestDTO workLogCreateRequest;

                _httpContextAccessor.HttpContext.Request.EnableBuffering();

                using (StreamReader reader = new StreamReader(_httpContextAccessor.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string requestBody = reader.ReadToEndAsync().Result;

                    workLogCreateRequest = JsonSerializer.Deserialize<WorkLogCreateRequestDTO>(requestBody, options);
                }

                if (_databaseContext.Tasks.Any(x => x.Id == workLogCreateRequest.TaskId && x.AssigneeId == currentUserId))
                {
                    context.Succeed(requirement);
                    _httpContextAccessor.HttpContext.Request.Body.Position = 0;

                    return Task.FromResult(0);
                }
            }

            if (splitPath[2] == "WorkLogs" && (splitPath[3] == "Delete" || splitPath[3] == "Edit"))
            {
                int workLogId = int.Parse(splitPath[4]);

                WorkLogDeleteRequestDTO workLogDeleteRequest;

                _httpContextAccessor.HttpContext.Request.EnableBuffering();

                using (StreamReader reader = new StreamReader(_httpContextAccessor.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string requestBody = reader.ReadToEndAsync().Result;

                    workLogDeleteRequest = JsonSerializer.Deserialize<WorkLogDeleteRequestDTO>(requestBody, options);
                }

                if (_databaseContext.WorkLogs.Any(x => x.Id == workLogId && x.Task.Id == workLogDeleteRequest.TaskId && x.Task.AssigneeId == currentUserId))
                {
                    context.Succeed(requirement);
                    _httpContextAccessor.HttpContext.Request.Body.Position = 0;

                    return Task.FromResult(0);
                }
            }

            if (splitPath[2] == "WorkLogs" && splitPath[3] == "GetAll")
            {
                int taskId = int.Parse(splitPath[4]);

                _httpContextAccessor.HttpContext.Request.EnableBuffering();
               
                int projectId = _databaseContext.Tasks.Where(x => x.Id == taskId).Select(x => x.ProjectId).FirstOrDefault();

                if (_databaseContext.Projects.Any(x => x.Id == projectId && x.IsActive && (x.UserId == currentUserId || (x.Teams.Any(x => x.IsActive)
                   && x.Teams.Any(x => x.Members.Any(x => x.Id == currentUserId) && x.IsActive == true)))))
                {
                    context.Succeed(requirement);

                    return Task.FromResult(0);
                }

                return Task.FromResult(0);
            }

            return Task.FromResult(0);
        }
    }
}
