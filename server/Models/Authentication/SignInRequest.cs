namespace SquashTournament.Server.Models.Authentication;

public class SignInRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}