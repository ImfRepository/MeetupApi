using System.Reflection;
using AutoMapper;
using Meetup.Core.Domain;
using Meetup.Infrastructure.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Infrastructure;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
	{
		string pgConnectionString = config.GetConnectionString("PgContext") 
		                            ?? throw new ArgumentNullException(nameof(pgConnectionString));

		services.AddDbContext<PgContext>(opt
			=> opt.UseNpgsql(pgConnectionString));

		services.AddTransient<IEventRepository, Repository>();

		services.AddAutoMapper(opt => opt.AddMaps(
			Assembly.GetAssembly(typeof(ServiceCollectionExtension))));
		return services;
	}


}