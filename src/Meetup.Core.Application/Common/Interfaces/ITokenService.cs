using Microsoft.Extensions.Configuration;

namespace Meetup.Core.Application.Common.Interfaces;

public interface ITokenService
{
    public string GetToken(IConfiguration config, bool isAdmin);
}