using Meetup.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();

	app.Run();

}
catch (Exception ex)
{
	Console.WriteLine(ex.ToString());
	Console.ReadLine();
}
