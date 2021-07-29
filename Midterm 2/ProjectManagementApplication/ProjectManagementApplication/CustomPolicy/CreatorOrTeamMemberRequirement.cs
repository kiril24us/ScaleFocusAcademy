using Microsoft.AspNetCore.Authorization;

namespace ProjectManagementApplication.Api.CustomPolicy
{
    public class CreatorOrTeamMemberRequirement : IAuthorizationRequirement
    {

    }
}
