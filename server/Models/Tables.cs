using System.ComponentModel.DataAnnotations;

namespace SquashTournament.Server.Models;

public class TournamentTbl
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}