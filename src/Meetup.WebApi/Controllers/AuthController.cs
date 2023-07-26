using Meetup.Core.Application.Common.Interfaces;
using Meetup.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.WebApi.Controllers;

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
		var result = _tokenService.GetToken(_config, isAdmin);

		return result.ToActionResult();
	}
}
