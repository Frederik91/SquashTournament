using Microsoft.EntityFrameworkCore;
using SquashTournament.Server.Data;
using SquashTournament.Server.Models;
using SquashTournament.Server.Requests;

namespace SquashTournament.Server.Endpoints;

public static class TournamentEndpoints
{   

    public static WebApplication AddTournamentEndpoints(this WebApplication app)
    {
        app.MapGet("api/tournaments", async (HttpContext context, ApplicationDbContext dbContext) =>
        {
            var tournaments = await dbContext.Tournaments.ToListAsync();
            await context.Response.WriteAsJsonAsync(tournaments);
        })
        .WithName("GetTournaments")
        .WithDescription("Get all tournaments")
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPost("api/tournaments", async (HttpContext context, ApplicationDbContext dbContext, CreateTournamentRequest request) =>
        {
            var tournament = new TournamentTbl
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Start = request.Start,
                End = request.End
            };

            dbContext.Tournaments.Add(tournament);
            await dbContext.SaveChangesAsync();

            context.Response.StatusCode = StatusCodes.Status201Created;
            var result = new Tournament {
                Id = tournament.Id,
                Name = tournament.Name,
                Start = tournament.Start,
                End = tournament.End
            };
            await context.Response.WriteAsJsonAsync(result);
        })
        .WithName("CreateTournament")
        .WithDescription("Create a new tournament")
        .WithOpenApi();

        return app;
    }
}