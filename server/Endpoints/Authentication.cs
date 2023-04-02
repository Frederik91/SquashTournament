using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SquashTournament.Server.Models.Authentication;

namespace SquashTournament.Server.Endpoints.Authentication;

public static class AuthenticationEndpoints
{
    public static WebApplication AddAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/signup", async (HttpContext context, SignUpRequest request, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) =>
        {
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
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

        app.MapPost("/signin", async (HttpContext context, SignInRequest request, SignInManager<ApplicationUser> signInManager) =>
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

        app.MapPost("/signout", async (HttpContext context, SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            context.Response.StatusCode = StatusCodes.Status200OK;
        });

        return app;
    }
}