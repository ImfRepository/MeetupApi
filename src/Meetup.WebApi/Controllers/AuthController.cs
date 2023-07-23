using Meetup.Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ITokenService _tokenService;
		private readonly IConfiguration _config;

		public AuthController(IConfiguration config, ITokenService tokenService)
		{
			_config = config;
			_tokenService = tokenService;
		}


		[HttpGet]
		public IActionResult GetToken([FromQuery] bool isAdmin)
		{
			try
			{
				var token = _tokenService.GetToken(_config, isAdmin);

				if (token == string.Empty)
					return BadRequest();

				var response = new
				{
					Authorization = "Bearer " + token
				};
				return Ok(response);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest();
			}

		}
	}
}
