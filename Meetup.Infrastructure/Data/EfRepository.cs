using AutoMapper;
using Meetup.Core.Application;
using Meetup.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

internal class EfRepository : IMeetupRepository
{
	private readonly PgContext _pgContext;
	private readonly IMapper _mapper;

	public EfRepository(PgContext pgContext, IMapper mapper)
	{
		_pgContext = pgContext;
		_mapper = mapper;
	}

	public async Task<int> CreateAsync(MeetupModel meetupModel, CancellationToken token = default)
	{
		var entity = _mapper.Map<MeetupEntity>(meetupModel) 
		             ?? throw new ArgumentNullException();

		entity.Id = 0;

		entity.Place = FindOrCreatePlace(entity.Place);
		entity.Organizer = FindOrCreateOrganizer(entity.Organizer);

		// For no problems with pg timestamps
		entity.Time = entity.Time.ToUniversalTime();
		foreach (var step in entity.PlanSteps)
		{
			step.Meetup = entity;
			step.Time = step.Time.ToUniversalTime();
		}

		_pgContext.Meetups.Update(entity);

		if (token.IsCancellationRequested)
			throw new TaskCanceledException("Task to add meetup was canceled.");

		await _pgContext.SaveChangesAsync(token);

		return entity.Id;
	}

	public async Task<IEnumerable<MeetupModel>> GetAllOrEmptyAsync(CancellationToken token = default)
	{
		var sqlModels = await _pgContext.Meetups
			.AsNoTracking()
			.Include(e => e.Organizer)
			.Include(e => e.Place)
			.Include(e => e.PlanSteps)
			.ToListAsync(token);

		if (token.IsCancellationRequested)
			throw new TaskCanceledException("Task to get all meetups was canceled.");

		return sqlModels.Any() 
			? _mapper.Map<IEnumerable<MeetupModel>>(sqlModels) 
			: Enumerable.Empty<MeetupModel>();
	}

	public async Task<MeetupModel> GetByIdOrEmptyAsync(int id, CancellationToken token = default)
	{
		var meetup = await _pgContext.Meetups.Where(e => e.Id == id)
			.AsNoTracking()
			.Include(e => e.Organizer)
			.Include(e => e.Place)
			.Include(e => e.PlanSteps)
			.FirstOrDefaultAsync(token);

		if (token.IsCancellationRequested)
			throw new TaskCanceledException("Task to get meetup by id was canceled.");

		return meetup == null 
			? MeetupModel.Empty 
			: _mapper.Map<MeetupModel>(meetup);
	}

	public async Task<int> UpdateAsync(MeetupModel meetup, CancellationToken token = default)
	{
		var updated = _mapper.Map<MeetupEntity>(meetup);
		var current = await _pgContext.Meetups
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == updated.Id, token);

		if(current == null)
			return 0;

		updated.Place = FindOrCreatePlace(updated.Place);

		updated.Organizer = FindOrCreateOrganizer(updated.Organizer);

		updated.PlanSteps = UpdatePlanSteps(updated.PlanSteps, updated).ToList();

		updated.Id = current.Id;

		_pgContext.Meetups.Update(updated);

		if (token.IsCancellationRequested)
			throw new TaskCanceledException("Task to update meetup was canceled.");

		await _pgContext.SaveChangesAsync(token);

		return updated.Id;
	}

	public async Task<int> DeleteAsync(int id, CancellationToken token = default)
	{
		var meetup = await _pgContext.Meetups
				.Include(e => e.Organizer)
				.Include(e => e.Place)
				.Include(e => e.PlanSteps)
				.FirstOrDefaultAsync(e => e.Id == id, token);

			if (meetup == null) 
				return 0;

			var org = meetup.Organizer;
			var place = meetup.Place;

			_pgContext.PlanSteps.RemoveRange(meetup.PlanSteps);
			_pgContext.Meetups.Remove(meetup);

			// For now deleting places and organizers if no meetups reference to them.
			// Can be changed later.
			if (_pgContext.Meetups.Count(e => e.PlaceId == place.Id) == 1)
				_pgContext.Places.Remove(place);

			if(_pgContext.Meetups.Count(e => e.OrganizerId == org.Id) == 1)
				_pgContext.Organizers.Remove(org);
			
			if (token.IsCancellationRequested)
				throw new TaskCanceledException("Task to delete meetup was canceled.");

			await _pgContext.SaveChangesAsync(token);
			return id;
	}

	/// <summary>
	///		Looking for place with the same name in db.
	///		Do not track changes if find. In other case add to context,
	///		but do not call SaveChanges().
	/// </summary>
	/// <param name="place"></param>
	/// <returns>Existing place from db or <paramref name="place"></paramref>.</returns>
	private Place FindOrCreatePlace(Place place)
	{
		var existingPlace = _pgContext.Places
			.AsNoTracking()
			.FirstOrDefault(e => e.Name == place.Name);

		if (existingPlace != null)
			place = existingPlace;
		//else
		//	_pgContext.Places.Add(place);

		return place;
	}

	/// <summary>
	///		Looking for organizer with the same name in db.
	///		Do not track changes if find. In other case add to context,
	///		but do not call SaveChanges().
	/// </summary>
	/// <param name="organizer"></param>
	/// <returns>Existing place from db or <paramref name="organizer"></paramref>.</returns>
	private Organizer FindOrCreateOrganizer(Organizer organizer)
	{
		var existingOrganizer = _pgContext.Organizers
			.AsNoTracking()
			.FirstOrDefault(e => e.Name == organizer.Name);

		if (existingOrganizer != null)
			organizer = existingOrganizer;
		//else
		//	_pgContext.Organizers.Add(organizer);

		return organizer;
	}

	/// <summary>
	///		Looking for plan steps in db with matching name.
	///		Do not track changes if find.
	///		Getting obsolete step's ids to new steps.
	///		Also do not call SaveChanges().
	/// </summary>
	/// <param name="steps"></param>
	/// <param name="meetupRefTo"></param>
	/// <returns></returns>
	private IEnumerable<PlanStep> UpdatePlanSteps(IEnumerable<PlanStep> steps, MeetupEntity meetupRefTo)
	{
		steps = steps.ToList();

		var exSteps = _pgContext.PlanSteps
			.AsNoTracking()
			.Where(e => e.MeetupId == meetupRefTo.Id)
			.ToList();

		if (!exSteps.Any())
			return steps;

		// Searching for step with same name
		foreach (var exStep in exSteps)
		{
			var step = steps.FirstOrDefault(s => s.Name == exStep.Name);

			if (step == null) 
				continue;

			step.Id = exStep.Id;
			exStep.Id = 0;
		}

		// Looking for obsolete steps, that can be used.
		// Adding id to update steps rather than add new
		foreach (var step in steps.Where(s => s.Id == 0))
		{
			if(!exSteps.Any())
				continue;

			var exStep = exSteps.First(s => s.Id != 0);
			step.Id = exStep.Id;
			exStep.Id = 0;
		}

		// In case we had more steps than in updated meetup
		if (exSteps.Any(e => e.Id != 0))
			_pgContext.PlanSteps.RemoveRange(exSteps.Where(s => s.Id != 0));

		return steps;
	}
}