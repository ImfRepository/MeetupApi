using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Tests;


//public class MeetupRepositoryTests
//{
//	private readonly IServiceProvider _services;
//	private static readonly DateTime BaseTime = new DateTime(2035, 8, 1, 
//			12, 0, 0)
//		.ToUniversalTime();

//	public MeetupRepositoryTests()
//	{
//		var config = new ConfigurationBuilder()
//			.AddUserSecrets<TestConfig>()
//			.Build();

//		_services = new ServiceCollection()
//			.AddInfrastructure(config)
//			.BuildServiceProvider();

//		var repo = GetRepository();
//		repo.CreateAsync(BaseModel);
//	}

//	private static MeetupModel BaseModel =>
//		new()
//		{
//			Name = "idk",
//			Description = "hmm, maybe",
//			Organizer = "me?",
//			Speaker = "again me?",
//			Place = "empty street",
//			Time = BaseTime,
//			Plan = new Dictionary<DateTime, string>()
//			{
//				{ BaseTime, "build" },
//				{ BaseTime + TimeSpan.FromMinutes(15), "test" },
//				{ BaseTime + TimeSpan.FromMinutes(30), "run" }
//			}
//		};

//	[Fact]
//	public async Task GetById_ReturnsEmpty()
//	{
//		// Arrange

//		// Act
//		var repo = GetRepository();
//		var actual = await repo.GetByIdOrEmptyAsync(0);

//		// Assert
//		Assert.Equal(MeetupModel.Empty, actual);
//	}

//	//[Fact, TestPriority(19)]
//	//public async Task Create()
//	//{
//	//	// Arrange
//	//	var repo = GetRepository();
//	//	var expected = (await repo.GetAllOrEmptyAsync()).Count() + 1;

//	//	// Act
//	//	_currentMeetupId = await repo.CreateAsync(BaseModel);
//	//	var actual = (await repo.GetAllOrEmptyAsync()).Count();

//	//	// Assert
//	//	Assert.Equal(expected, actual);
//	//}

//	[Fact]
//	public async Task GetAll_ReturnsAny()
//	{
//		// Arrange

//		// Act
//		var repo = GetRepository();
//		var actual = await repo.GetAllOrEmptyAsync();

//		// Assert
//		Assert.True(actual.Any());
//	}

//	[Fact]
//	public async Task GetById_ReturnsExistingOne()
//	{
//		// Arrange

//		// Act
//		var repo = GetRepository();
//		var id = (await repo.GetAllOrEmptyAsync()).First().Id ?? 0;
//		var actual = await repo.GetByIdOrEmptyAsync(id);

//		// Assert
//		Assert.NotEqual(MeetupModel.Empty, actual);
//	}

//	[Fact]
//	public async Task UpdateName()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var expected = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//		if(expected == MeetupModel.Empty)
//			Assert.Fail("No meetups in db.");

//		expected.Name = "ik";

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected.Name, actual.Name);
//	}

//	[Fact, TestPriority(14)]
//	public async Task UpdateDescription()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var expected = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//		if (expected == MeetupModel.Empty)
//			Assert.Fail("No meetups in db.");

//		expected.Description = "all clear";

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected.Description, actual.Description);
//	}

//	[Fact, TestPriority(14)]
//	public async Task UpdateSpeaker()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var expected = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//		if (expected == MeetupModel.Empty)
//			Assert.Fail("No meetups in db.");

//		expected.Speaker = "Dale Carnegie";

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected.Speaker, actual.Speaker);
//	}

//	[Fact, TestPriority(13)]
//	public async Task UpdateTime()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var expected = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//		if (expected == MeetupModel.Empty)
//			Assert.Fail("No meetups in db.");

//		expected.Time = BaseTime + TimeSpan.FromMinutes(-5);

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected.Time, actual.Time);
//	}

//	[Fact, TestPriority(12)]
//	public async Task UpdatePlace()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var expected = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//		if (expected == MeetupModel.Empty)
//			Assert.Fail("No meetups in db.");

//		expected.Place = "railways";

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected.Place, actual.Place);
//	}

//	[Fact, TestPriority(12)]
//	public async Task UpdateOrganizer()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var expected = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//		if (expected == MeetupModel.Empty)
//			Assert.Fail("No meetups in db.");

//		expected.Organizer = "finally not me";

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected.Organizer, actual.Organizer);
//	}

//	[Fact, TestPriority(11)]
//	public async Task UpdatePlan_AddOneStep()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var expected = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//		if (expected == MeetupModel.Empty)
//			Assert.Fail("No meetups in db.");

//		expected.Plan.Add(BaseTime + TimeSpan.FromMinutes(45), "deploy");

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected.Plan.Count, actual.Plan.Count);
//	}

//	[Fact, TestPriority(10)]
//	public async Task UpdatePlan_RemoveOneStep()
//	{
//		// Arrange
//		var time = BaseTime + TimeSpan.FromMinutes(45);
//		var repo = GetRepository();
//		var expected = (await repo.GetAllOrEmptyAsync())
//			.FirstOrDefault(e => e.Plan.ContainsKey(time));
//		if (expected == null)
//			Assert.Fail("No meetups in db.");

//		expected.Plan.Remove(time);

//		// Act
//		await repo.UpdateAsync(expected);
//		var actual = await repo.GetByIdOrEmptyAsync(_currentMeetupId);

//		// Assert
//		Assert.Equal(expected, actual);
//	}

//	//[Fact, TestPriority(0)]
//	//public async Task Delete()
//	//{
//	//	// Arrange
//	//	var repo = GetRepository();
//	//	var expected = (await repo.GetAllOrEmptyAsync()).Count() - 1;
//	//	if(expected < 0)
//	//		Assert.Fail("No meetups in db.");

//	//	// Act
//	//	var toDelete = await repo.GetByIdOrEmptyAsync(_currentMeetupId);
//	//	await repo.DeleteAsync(toDelete.Id ?? 0);

//	//	var actual = (await repo.GetAllOrEmptyAsync()).Count();

//	//	// Assert
//	//	Assert.Equal(expected, actual);
//	//}

//	//[Fact]
//	//public async Task DeleteAll()
//	//{
//	//	// Arrange

//	//	// Act
//	//	var repo = GetRepository();
//	//	foreach (var meetup in await repo.GetAllOrEmptyAsync())
//	//	{
//	//		await repo.DeleteAsync(meetup.Id ?? 0);
//	//	}
//	//	var actual = (await repo.GetAllOrEmptyAsync()).Count();

//	//	// Assert
//	//	Assert.Equal(0, actual);
//	//}

//	[Fact]
//	public async Task CreateDelete()
//	{
//		// Arrange
//		var repo = GetRepository();
//		var first = (await repo.GetAllOrEmptyAsync()).Count();

//		// Act
//		await repo.CreateAsync(BaseModel);
//		var second = (await repo.GetAllOrEmptyAsync()).Count();

//		// Assert
//		Assert.Equal(first + 1, second);

//		// Act
//		var id = (await repo.GetAllOrEmptyAsync()).SingleOrDefault()?.Id ?? 0;
//		var toDelete = await repo.GetByIdOrEmptyAsync(id);
//		await repo.DeleteAsync(toDelete.Id ?? 0);

//		var third = (await repo.GetAllOrEmptyAsync()).Count();

//		// Assert
//		Assert.Equal(first, third);
//	}

//	private IMeetupRepository GetRepository()
//	{
//		var repo = _services.GetService<IMeetupRepository>();
//		return repo ?? throw new NullReferenceException();
//	}
//}

////internal class TestConfig
////{
////	public string PgContext { get; set; }
////}