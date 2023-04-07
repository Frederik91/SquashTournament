using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SquashTournament.Server.Models.Authentication;

namespace SquashTournament.Server.Endpoints;

public static class AuthenticationEndpoints
{
    public static WebApplication AddAuthenticationEndpoints(this WebApplication app)
    {
        app.MapGet("users", async (HttpContext context, UserManager<IdentityUser> userManager) =>
        {
            var users = await userManager.Users.ToListAsync();
            await context.Response.WriteAsJsonAsync(users);
        })
        .WithName("GetUsers")
        .WithDescription("Get all users")
        .RequireAuthorization("ADMIN");

        app.MapPost("/signup", async (HttpContext context, SignUpRequest request, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) =>
        {
            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                context.Response.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(result.Errors);
            }
        });

        app.MapPost("/signin", async (HttpContext context, SignInRequest request, SignInManager<IdentityUser> signInManager) =>
        {
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid username or password.");
            }
        });

        app.MapPost("/signout", async (HttpContext context, SignInManager<IdentityUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            context.Response.StatusCode = StatusCodes.Status200OK;
        });

        return app;
    }
}