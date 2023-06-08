using Meetup.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

try
{
	var builder = WebApplication.CreateBuilder(args);

	var config = builder.Configuration;

	// Add services to the container.
	builder.Services.AddInfrastructure(config);

	builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,opt =>
		{
			opt.RequireHttpsMetadata = false;
			opt.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = config["JWT:Issuer"],

				ValidateAudience = true,
				ValidAudience = config["JWT:Audience"],
				ValidateLifetime = true,

				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.ASCII.GetBytes(config["JWT:Key"] ?? throw new NullReferenceException("JWT:Key"))),
				ValidateIssuerSigningKey = true,
			};
		});

	builder.Services.AddControllers();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(opt =>
	{
		opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
		{
			Description = "JWT Authorization header \"Authorization\": \"Bearer {token}\"",
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.ApiKey,
			Scheme = "Bearer"
		});

		opt.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Name = "Bearer",
					In = ParameterLocation.Header,
					Reference = new OpenApiReference
					{
						Type=ReferenceType.SecurityScheme,
						Id="Bearer"
					}
				},
				new List<string>()
			}
		});
	});

	var app = builder.Build();

	app.UseSwagger();
	app.UseSwaggerUI();

	app.UseHttpsRedirection();

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
catch (Exception ex)
{
	Console.WriteLine(ex);
	Console.ReadLine();
}
