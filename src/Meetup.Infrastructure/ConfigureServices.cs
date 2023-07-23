using System.Reflection;
using Meetup.Core.Application.Common.Interfaces;
using Meetup.Infrastructure.Data;
using Meetup.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Infrastructure;

public static class ConfigureServices
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        string pgConnectionString = config.GetConnectionString("PgContext")
                                    ?? throw new ArgumentNullException(nameof(pgConnectionString));

        var assembly = Assembly.GetAssembly(typeof(ConfigureServices));

        services.AddDbContext<PgContext>(opt
            => opt.UseNpgsql(pgConnectionString));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PgContext>());

        services.AddTransient<ITokenService, TokenService>();

        services.AddAutoMapper(opt => opt.AddMaps(assembly));

        return services;
    }
}