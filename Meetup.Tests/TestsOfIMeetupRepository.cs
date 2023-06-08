using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Tests;

public class TestsOfIMeetupRepository
{
	private readonly IServiceProvider _services;
	private static readonly Random Random = new();
	private static readonly DateTime BaseTime = new DateTime(2035, 8, 1,
			12, 0, 0)
		.ToUniversalTime();

	private static MeetupModel RandomModel =>
		new()
		{
			Name = RandomString(10),
			Description = RandomString(20),
			Organizer = RandomString(10),
			Speaker = RandomString(10),
			Place = RandomString(5),
			Time = BaseTime,
			Plan = new Dictionary<DateTime, string>()
			{
				{ BaseTime, RandomString(8) },
				{ BaseTime + TimeSpan.FromMinutes(15), RandomString(8) },
				{ BaseTime + TimeSpan.FromMinutes(30), RandomString(8) }
			}
		};

	public TestsOfIMeetupRepository()
	{
		var config = new ConfigurationBuilder()
			.AddUserSecrets<TestConfig>()
			.Build();

		_services = new ServiceCollection()
			.AddInfrastructure(config)
			.BuildServiceProvider();

		PrepareDb().Wait();
	}

	private async Task PrepareDb()
	{
		var repo = GetRepository();

		// Clearing db
		var ids = (await repo.GetAllOrEmptyAsync()).Select(e => e.Id ?? 0).ToList();
		foreach (var id in ids)
		{
			await repo.DeleteAsync(id);
		}

		// Adding some data
		for (var i = 0; i < 5; i++)
		{
			await repo.CreateAsync(RandomModel);
		}
	}

	private static string RandomString(uint length)
	{
		var builder = new StringBuilder();
		for (var i = 0; i < length; i++)
		{
			builder.Append((char)Random.Next('a', 'z'));
		}

		return builder.ToString();
	}

	private IMeetupRepository GetRepository()
	{
		var repo = _services.GetService<IMeetupRepository>();
		return repo ?? throw new NullReferenceException();
		
	}

	[Fact]
	public async Task GetAll()
	{
		var repo = GetRepository();

		// Arrange

		// Act
		var meetups = await repo.GetAllOrEmptyAsync();

		// Assert
		Assert.NotEmpty(meetups);
	}

	[Fact]
	public async Task GetById()
	{
		var repo = GetRepository();

		// Arrange

		// Act
		var id = (await repo.GetAllOrEmptyAsync()).FirstOrDefault()!.Id ?? 0;
		var meetup = await repo.GetByIdOrEmptyAsync(id);

		// Assert
		Assert.NotEqual(meetup, MeetupModel.Empty);
	}

	[Fact]
	public async Task Create()
	{
		var repo = GetRepository();

		// Arrange
		var expCount = (await repo.GetAllOrEmptyAsync()).Count() + 1;

		// Act
		var id = await repo.CreateAsync(RandomModel);
		var actCount = (await repo.GetAllOrEmptyAsync()).Count();

		// Assert
		Assert.Equal(expCount, actCount);
		Assert.True(id > 0);
	}

	[Fact]
	public async Task Update()
	{
		var repo = GetRepository();

		// Arrange
		var expCount = (await repo.GetAllOrEmptyAsync()).Count();

		// Act
		var id = (await repo.GetAllOrEmptyAsync()).FirstOrDefault()?.Id;
		var updated = RandomModel;
		updated.Id = id;
		id = await repo.UpdateAsync(updated);
		var actCount = (await repo.GetAllOrEmptyAsync()).Count();

		// Assert
		Assert.Equal(expCount, actCount);
		Assert.True(id > 0);
	}

	[Fact]
	public async Task Delete()
	{
		var repo = GetRepository();

		// Arrange
		var expCount = (await repo.GetAllOrEmptyAsync()).Count() - 1;

		// Act
		var id = (await repo.GetAllOrEmptyAsync()).FirstOrDefault()?.Id ?? 0;
		id = await repo.DeleteAsync(id);
		var actCount = (await repo.GetAllOrEmptyAsync()).Count();

		// Assert
		Assert.Equal(expCount, actCount);
		Assert.True(id > 0);
	}
}

internal class TestConfig
{
	public string PgContext { get; set; }
}
