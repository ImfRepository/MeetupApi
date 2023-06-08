using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Meetup.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _config;

		public AuthController(IConfiguration config)
		{
			_config = config;
		}


		[HttpGet]
		public IActionResult GetToken([FromQuery] bool isAdmin)
		{
			var now = DateTime.UtcNow;

			var random = new Random();

			var claims = new List<Claim>
			{
				new("name", ((char)('a' + random.Next(20))).ToString()),
				new("role", isAdmin ? "admin" : "user")
			};

			var jwt = new JwtSecurityToken(
				issuer: _config["JWT:Issuer"],
				audience: _config["JWT:Audience"],
				claims: claims,
				expires: now.AddHours(2),
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
					Encoding.ASCII.GetBytes(_config["JWT:Key"] ?? throw new NullReferenceException("JWT:Key"))), 
					SecurityAlgorithms.HmacSha256));

			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = new
			{
				Authorization = "Bearer " + encodedJwt
			};

			return Ok(response);
		}


	}
}
