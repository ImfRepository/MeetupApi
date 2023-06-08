using System.Reflection;
using Meetup.Core.Application.Interfaces;
using Meetup.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///     Register services.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config">Might have value to key = "ConnectionStrings:PgContext"</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        string pgConnectionString = config.GetConnectionString("PgContext")
                                    ?? throw new ArgumentNullException(nameof(pgConnectionString));

        services.AddDbContext<PgContext>(opt
            => opt.UseNpgsql(pgConnectionString).EnableSensitiveDataLogging());

        services.AddTransient<IMeetupRepository, EfRepository>();

        services.AddAutoMapper(opt => opt.AddMaps(
            Assembly.GetAssembly(typeof(ServiceCollectionExtension))));
        return services;
    }
}