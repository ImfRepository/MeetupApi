using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Meetup.Core.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Meetup.Infrastructure.Services;

internal class TokenService : ITokenService
{
	public string GetToken(IConfiguration config, bool isAdmin)
	{
		try
		{
			var now = DateTime.UtcNow;
			var random = new Random();
			var claims = new List<Claim>
			{
				new("name", ((char)('a' + random.Next(20))).ToString()),
				new("role", isAdmin ? "admin" : "user")
			};

			var jwt = new JwtSecurityToken(
				issuer: config["JWT:Issuer"],
				audience: config["JWT:Audience"],
				claims: claims,
				expires: now.AddHours(2),
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
						Encoding.ASCII.GetBytes(config["JWT:Key"] ?? throw new NullReferenceException("JWT:Key"))),
					SecurityAlgorithms.HmacSha256));

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return string.Empty;
		}
	}
}