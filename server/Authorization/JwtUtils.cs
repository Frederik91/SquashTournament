
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SquashTournament.Server.Authorization;

public interface IJwtUtils
{
    public string GenerateJwtToken(IdentityUser user);
}

public class JwtUtils : IJwtUtils
{
    private readonly AuthAppSettings _appSettings;

    public JwtUtils(IOptions<AuthAppSettings> appSettings)
    {
        _appSettings = appSettings.Value;

        if (string.IsNullOrEmpty(_appSettings.Key))
            throw new Exception("JWT secret not configured");
    }

    public string GenerateJwtToken(IdentityUser user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Key!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _appSettings.Audience,
            Issuer = _appSettings.Issuer
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}