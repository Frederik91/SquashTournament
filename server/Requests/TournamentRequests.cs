namespace SquashTournament.Server.Requests;

public class Tournament 
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}

public class GetAllTournamentsRequest
{
    public DateTimeOffset? Start { get; set; }
    public DateTimeOffset? End { get; set; }
}

public class GetAllTournamentsResponse
{
    public List<Tournament> Tournaments { get; set; } = new();
}

public class CreateTournamentRequest
{
    public required string Name { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}