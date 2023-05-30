using AutoMapper;
using Meetup.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Meetup.Infrastructure.SQL;

internal class Repository : IEventRepository
{
	private readonly PgContext _pgContext;
	private readonly IMapper _mapper;

	public Repository(PgContext pgContext, IMapper mapper)
	{
		_pgContext = pgContext;
		_mapper = mapper;
	}

	public async Task<bool> CreateAsync(Event meetup, CancellationToken token = default)
	{
		EventInfo entity = _mapper.Map<EventInfo>(meetup) 
		                   ?? throw new ArgumentNullException(nameof(entity));

		entity.Id = 0;

		// Check for not created yet meetup -> continue
		if (await _pgContext.Events
			    .Where(e => e.Name == entity.Name)
			    .AnyAsync(token))
			return false;

		entity = await Normalize(entity, token);

		await _pgContext.Events.AddAsync(entity, token);
		await _pgContext.PlanSteps.AddRangeAsync(entity.PlanSteps, token);
		await _pgContext.SaveChangesAsync(token);

		return true;
	}

	public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken token = default)
	{
		var sqlModels = await _pgContext.Events
			.Include(e => e.Organizer)
			.Include(e => e.Place)
			.Include(e => e.PlanSteps)
			.ToListAsync(token);

		return !sqlModels.Any() ? Enumerable.Empty<Event>() 
			: _mapper.Map<IEnumerable<Event>>(sqlModels);
	}

	public async Task<Event> GetByIdAsync(int id, CancellationToken token = default)
	{
		var meetup = await _pgContext.Events.Where(e => e.Id == id)
			.Include(e => e.Organizer)
			.Include(e => e.Place)
			.Include(e => e.PlanSteps)
			.FirstOrDefaultAsync(token);

		if (meetup == null)
			throw new ArgumentNullException(nameof(meetup));

		return _mapper.Map<Event>(meetup);
	}

	public async Task<bool> UpdateAsync(Event meetup, CancellationToken token = default)
	{
		var newOne = _mapper.Map<EventInfo>(meetup);
		var currentOne = await _pgContext.Events
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Name == newOne.Name, token);

		if(currentOne == null)
			return false;
		
		newOne = await Normalize(newOne, token);
		newOne.Id = currentOne.Id;
		
		_pgContext.Events.Update(newOne);

		
		await _pgContext.SaveChangesAsync(token);
		return true;
	}

	public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
	{
		try
		{
			var meetup = await _pgContext.Events
				.Include(e => e.Organizer)
				.Include(e => e.Place)
				.Include(e => e.PlanSteps)
				.FirstOrDefaultAsync(e => e.Id == id, token);

			if (meetup == null) 
				return false;

			var org = meetup.Organizer;
			var place = meetup.Place;

			_pgContext.PlanSteps.RemoveRange(meetup.PlanSteps);
			_pgContext.Events.Remove(meetup);

			if (_pgContext.Events.Count(e => e.PlaceId == place.Id) == 1)
				_pgContext.Places.Remove(place);

			if(_pgContext.Events.Count(e => e.OrganizerId == org.Id) == 1)
				_pgContext.Organizers.Remove(org);

			await _pgContext.SaveChangesAsync(token);
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return false;
		}
	}

	/// <summary>
	///		Checks for an existing place and organizer with names in this meetup.
	///		If they aren't exist, create them.
	///		Also change time format to utc.
	/// </summary>
	/// <param name="entity">Meetup to normalize.</param>
	/// <param name="token"></param>
	/// <returns></returns>
	private async Task<EventInfo> Normalize(EventInfo entity, CancellationToken token = default)
	{
		// Check for not created yet place
		entity.Place = await FindOrCreatePlace(entity.Place, token);

		// Check for not created yet organizer
		entity.Organizer = await FindOrCreateOrganizer(entity.Organizer, token);

		// Always adding plan steps
		entity.PlanSteps = await FindOrCreatePlanSteps(entity.PlanSteps, entity, token);
		foreach (var step in entity.PlanSteps)
		{
			//step.Event = entity;
			step.Time = step.Time.ToUniversalTime();
		}

		// For no problems with pg timestamps
		entity.Time = entity.Time.ToUniversalTime();

		return entity;
	}

	private async Task<Place> FindOrCreatePlace(Place place, CancellationToken token = default)
	{
		var existingPlace = await _pgContext.Places.
			FirstOrDefaultAsync(e => e.Name == place.Name, token);

		if (existingPlace != null)
			place = existingPlace;
		else
			await _pgContext.Places.AddAsync(place, token);

		return place;
	}

	private async Task<Organizer> FindOrCreateOrganizer(Organizer organizer, CancellationToken token = default)
	{
		var existingOrganizer = await _pgContext.Organizers.
			FirstOrDefaultAsync(e => e.Name == organizer.Name, token);

		if (existingOrganizer != null)
			organizer = existingOrganizer;
		else
			await _pgContext.Organizers.AddAsync(organizer, token);

		return organizer;
	}

	//private async Task<PlanStep> FindOrCreatePlanStep(PlanStep step, EventInfo meetup, 
	//	CancellationToken token = default)
	//{
	//	var existingStep = await _pgContext.PlanSteps
	//		.Where(e => e.EventId == meetup.Id)
	//		.FirstOrDefaultAsync(e => e.Name == step.Name, token);

	//	if (existingStep != null)
	//		step = existingStep;
	//	else
	//	{
	//		await _pgContext.PlanSteps.AddAsync(step, token);
	//	}

	//	return step;
	//}

	private async Task<IEnumerable<PlanStep>> FindOrCreatePlanSteps(IEnumerable<PlanStep> steps, EventInfo meetupRefTo,
		CancellationToken token = default)
	{
		steps = steps.ToList();

		var steps1 = steps;
		var existingSteps = _pgContext.PlanSteps
			.Where(e => e.EventId == meetupRefTo.Id)
			.AsNoTracking()
			.ToList()
			.Where(e => steps1
				.FirstOrDefault(step => step.Name == e.Name) != null);

		var stepsToDelete = _pgContext.PlanSteps
			.Where(e => e.EventId == meetupRefTo.Id)
			.AsNoTracking()
			.ToList()
			.Where(e => steps1
				.FirstOrDefault(step => step.Name == e.Name) == null);

		if(stepsToDelete.Any())
			_pgContext.PlanSteps.RemoveRange(stepsToDelete);

		var newSteps = steps
			.Where(e => existingSteps
				.FirstOrDefault(step => step.Name == e.Name) == null);

		steps = existingSteps
			.Concat(newSteps)
			.ToList();

		return steps;
	}
}