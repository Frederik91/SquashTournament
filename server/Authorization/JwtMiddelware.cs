using Microsoft.EntityFrameworkCore;
using SquashTournament.Server.Data;

namespace SquashTournament.Server.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ApplicationDbContext dbContext, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (!string.IsNullOrEmpty(userId))
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        await _next(context);
    }
}