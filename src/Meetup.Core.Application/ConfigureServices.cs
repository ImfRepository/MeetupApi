using System.Globalization;
using System.Reflection;
using Meetup.Core.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Core.Application;

public static class ConfigureServices
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();
		services.AddValidatorsFromAssembly(assembly);
		ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(assembly);
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});

		return services;
	}
}