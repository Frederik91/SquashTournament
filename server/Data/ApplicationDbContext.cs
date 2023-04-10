using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SquashTournament.Server.Models;
using SquashTournament.Server.Models.Authentication;

namespace SquashTournament.Server.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<TournamentTbl> Tournaments { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}
