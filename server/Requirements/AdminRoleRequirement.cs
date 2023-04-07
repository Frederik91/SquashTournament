using Microsoft.AspNetCore.Authorization;
using SquashTournament.Server.Requirements;

namespace SquashTournament.Server.Requirements
{
    public class AdminRoleRequirement : IAuthorizationRequirement
    {
        public AdminRoleRequirement(string role) => Role = role;
        public string Role { get; set; }
    }
}

public class AdminRoleRequirementHandler : AuthorizationHandler<AdminRoleRequirement>
 {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminRoleRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRoleRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Value == requirement.Role))
        {
            context.Succeed(requirement);
        }
        else
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                context.Fail();
                return;
            }
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
            await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { StatusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized. Required admin role." });
            await _httpContextAccessor.HttpContext.Response.CompleteAsync();

            context.Fail();

        }

    }
}