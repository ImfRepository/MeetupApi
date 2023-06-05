using AutoMapper;
using Meetup.Core.Domain;
using Meetup.Core.Application;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.SQL;

public class EfRepository : IMeetupRepository
{
	private readonly PgContext _pgContext;
	private readonly IMapper _mapper;

	public EfRepository(PgContext pgContext, IMapper mapper)
	{
		_pgContext = pgContext;
		_mapper = mapper;
	}

	public async Task<bool> CreateAsync(MeetupView meetupView, CancellationToken token = default)
	{
		var entity = _mapper.Map<MeetupEntity>(meetupView) 
		             ?? throw new ArgumentNullException();

		entity.Id = 0;

		// Just not overriding meetups here
		if (await _pgContext.Events
			    .Where(e => e.Name == entity.Name)
			    .AsNoTracking()
			    .AnyAsync(token))
			return false;

		entity = await Normalize(entity, token);

		// For no problems with pg timestamps
		foreach (var step in entity.PlanSteps)
		{
			step.Time = step.Time.ToUniversalTime();
		}
		entity.Time = entity.Time.ToUniversalTime();

		await _pgContext.Events.AddAsync(entity, token);
		await _pgContext.PlanSteps.AddRangeAsync(entity.PlanSteps, token);
		await _pgContext.SaveChangesAsync(token);

		return true;
	}

	public async Task<IEnumerable<MeetupView>> GetAllAsync(CancellationToken token = default)
	{
		var sqlModels = await _pgContext.Events
			.Include(e => e.Organizer)
			.Include(e => e.Place)
			.Include(e => e.PlanSteps)
			.AsNoTracking()
			.ToListAsync(token);
		
		return sqlModels.Any() 
			? _mapper.Map<IEnumerable<MeetupView>>(sqlModels) 
			: Enumerable.Empty<MeetupView>();
	}

	public async Task<MeetupView> GetByIdAsync(int id, CancellationToken token = default)
	{
		var meetup = await _pgContext.Events.Where(e => e.Id == id)
			.Include(e => e.Organizer)
			.Include(e => e.Place)
			.Include(e => e.PlanSteps)
			.AsNoTracking()
			.FirstOrDefaultAsync(token);

		return meetup == null 
			? MeetupView.Empty 
			: _mapper.Map<MeetupView>(meetup);
	}

	public async Task<bool> UpdateAsync(MeetupView meetup, CancellationToken token = default)
	{
		var newOne = _mapper.Map<MeetupEntity>(meetup);
		var currentOne = await _pgContext.Events
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == newOne.Id, token);

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

			// For now deleting places and organizers if no meetups with them.
			// Can be changed later.
			if (_pgContext.Events.Count(e => e.PlaceId == place.Id) == 1)
				_pgContext.Places.Remove(place);

			if(_pgContext.Events.Count(e => e.OrganizerId == org.Id) == 1)
				_pgContext.Organizers.Remove(org);
			
			if (token.IsCancellationRequested)
				throw new TaskCanceledException("Task to delete meetup was canceled.");

			await _pgContext.SaveChangesAsync(token);
			return true;
	}

	/// <summary>
	///		Checks for an existing place and organizer with names in this meetup.
	///		If they aren't exist, create them.
	///		Also change time format to utc.
	/// </summary>
	/// <param name="entity">MeetupView to normalize.</param>
	/// <param name="token"></param>
	/// <returns></returns>
	private async Task<MeetupEntity> Normalize(MeetupEntity entity, CancellationToken token = default)
	{
		// Check for not created yet place
		entity.Place = await FindOrCreatePlace(entity.Place, token);

		// Check for not created yet organizer
		entity.Organizer = await FindOrCreateOrganizer(entity.Organizer, token);

		// 
		entity.PlanSteps = await FindIfExistPlanSteps(entity.PlanSteps, entity, token);

		return entity;
	}

	private async Task<Place> FindOrCreatePlace(Place place, CancellationToken token = default)
	{
		var existingPlace = await _pgContext.Places
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Name == place.Name, token);

		if (existingPlace != null)
			place = existingPlace;
		else
			await _pgContext.Places.AddAsync(place, token);

		return place;
	}

	private async Task<Organizer> FindOrCreateOrganizer(Organizer organizer, CancellationToken token = default)
	{
		var existingOrganizer = await _pgContext.Organizers
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Name == organizer.Name, token);

		if (existingOrganizer != null)
			organizer = existingOrganizer;
		else
			await _pgContext.Organizers.AddAsync(organizer, token);

		return organizer;
	}

	private async Task<IEnumerable<PlanStep>> FindIfExistPlanSteps(IEnumerable<PlanStep> steps, MeetupEntity meetupRefTo,
		CancellationToken token = default)
	{
		steps = steps.ToList();

		var exSteps = await _pgContext.PlanSteps
			.Where(e => e.EventId == meetupRefTo.Id)
			.AsNoTracking()
			.ToListAsync(token);

		if (!exSteps.Any())
			return steps;

		// Searching for step with same name
		foreach (var exStep in exSteps)
		{
			var step = steps.FirstOrDefault(s => s.Name == exStep.Name);

			if (step == null) 
				continue;

			step.Id = exStep.Id;
			exSteps.Remove(exStep);
		}
		
		// Looking for obsolete steps, that can be used.
		// Adding id to update steps rather than add new
		foreach (var step in steps.Where(s => s.Id == 0))
		{
			if(!exSteps.Any())
				continue;

			var exStep = exSteps.First();
			step.Id = exStep.Id;
			exSteps.Remove(exStep);
		}

		// In case we had more steps than in updated meetup
		if(exSteps.Any())
			_pgContext.PlanSteps.RemoveRange(exSteps);

		return steps;
	}
}