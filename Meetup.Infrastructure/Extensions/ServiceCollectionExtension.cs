using System.Reflection;
using Meetup.Core.Application;
using Meetup.Infrastructure.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
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