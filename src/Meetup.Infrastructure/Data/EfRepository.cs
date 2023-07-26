using AutoMapper;
using Meetup.Core.Application.Interfaces;
using Meetup.Core.Domain.Entities;
using Meetup.Core.Domain.Exceptions;
using Meetup.Infrastructure.Data.Models;
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

	public async Task<int> CreateAsync(MeetupEntity meetupEntity, CancellationToken token = default)
	{
		try
		{
			var entity = _mapper.Map<MeetupDto>(meetupEntity) 
			             ?? throw new ArgumentNullException();
			entity.Id = 0;

			entity.PlaceDto = FindOrAddToContextPlace(entity.PlaceDto);
			entity.OrganizerDto = FindOrAddToContextOrganizer(entity.OrganizerDto);

			// For no problems with pg timestamps
			entity.Time = entity.Time.ToUniversalTime();
			foreach (var step in entity.PlanSteps)
			{
				step.Meetup = entity;
				step.Time = step.Time.ToUniversalTime();
			}

			_pgContext.Meetups.Attach(entity);
			await _pgContext.SaveChangesAsync(token);
			return entity.Id;
		}
		catch (TaskCanceledException ex)
		{
			throw new TaskCanceledException("Task was canceled.", ex);
		}
		catch (Exception ex)
		{
			throw new RepositoryException("Failed to create meetup.", ex);
		}
	}

	public async Task<IEnumerable<MeetupEntity>> GetAllOrEmptyAsync(CancellationToken token = default)
	{
		try
		{
			var sqlModels = await _pgContext.Meetups
				.AsNoTracking()
				.Include(e => e.OrganizerDto)
				.Include(e => e.PlaceDto)
				.Include(e => e.PlanSteps)
				.ToListAsync(token);

			if (token.IsCancellationRequested)
				throw new TaskCanceledException("Task to get all meetups was canceled.");

			return sqlModels.Any()
				? _mapper.Map<IEnumerable<MeetupEntity>>(sqlModels)
				: Enumerable.Empty<MeetupEntity>();
		}
		catch (TaskCanceledException ex)
		{
			throw new TaskCanceledException("Task was canceled.", ex);
		}
		catch (Exception ex)
		{
			throw new RepositoryException("Failed to get all meetups.", ex);
		}
	}

	public async Task<MeetupEntity> GetByIdOrEmptyAsync(int id, CancellationToken token = default)
	{
		try
		{
			var meetup = await _pgContext.Meetups.Where(e => e.Id == id)
				.AsNoTracking()
				.Include(e => e.OrganizerDto)
				.Include(e => e.PlaceDto)
				.Include(e => e.PlanSteps)
				.FirstOrDefaultAsync(token);

			if (token.IsCancellationRequested)
				throw new TaskCanceledException("Task to get meetup by id was canceled.");

			return meetup == null
				? MeetupEntity.Empty
				: _mapper.Map<MeetupEntity>(meetup);
		}

		catch (TaskCanceledException ex)
		{
			throw new TaskCanceledException("Task was canceled.", ex);
		}
		catch (Exception ex)
		{
			throw new RepositoryException("Failed to get meetup by id.", ex);
		}
	}

	public async Task<int> UpdateAsync(MeetupEntity meetup, CancellationToken token = default)
	{
		try
		{
			var updated = _mapper.Map<MeetupDto>(meetup);

			// Strict order coz of key references
			// 1st (org and place)
			updated.PlaceDto = FindOrAddToContextPlace(updated.PlaceDto);
			updated.OrganizerDto = FindOrAddToContextOrganizer(updated.OrganizerDto);
			await _pgContext.SaveChangesAsync(token);

			// 2nd (meetup)
			updated.PlaceId = updated.PlaceDto.Id;
			updated.OrganizerId = updated.OrganizerDto.Id;
			await _pgContext.Meetups.Where(m => m.Id == updated.Id)
				.ExecuteUpdateAsync(e =>
					e.SetProperty(d => d.Name, updated.Name)
						.SetProperty(d => d.Description, updated.Description)
						.SetProperty(d => d.OrganizerId, updated.Id)
						.SetProperty(d => d.PlaceId, updated.PlaceId)
						.SetProperty(d => d.Speaker, updated.Speaker)
						.SetProperty(d => d.Time, updated.Time), token);

			// 3rd (plan steps)
			var steps = await _pgContext.PlanSteps
				.Where(s => s.MeetupId == updated.Id)
				.ToListAsync(token);

			var stepsToUpdate = updated.PlanSteps.ToList();
			foreach (var step in steps)
			{
				if (!stepsToUpdate.Any())
					_pgContext.Remove(step);

				step.Name = stepsToUpdate.First().Name;
				step.Time = stepsToUpdate.First().Time;
				stepsToUpdate.Remove(stepsToUpdate.First());
			}

			if (stepsToUpdate.Any())
				_pgContext.PlanSteps.AttachRange(stepsToUpdate);
			await _pgContext.SaveChangesAsync(token);

			return updated.Id;
		}
		catch (TaskCanceledException ex)
		{
			throw new TaskCanceledException("Task was canceled.", ex);
		}
		catch (Exception ex)
		{
			throw new RepositoryException("Failed to update meetup.", ex);
		}
	}

	public async Task<int> DeleteAsync(int id, CancellationToken token = default)
	{
		try
		{
			await _pgContext.PlanSteps.Where(s => s.MeetupId == id)
				.ExecuteDeleteAsync(token);

			await _pgContext.Meetups.Where(m => m.Id == id)
				.ExecuteDeleteAsync(token);

			return id;
		}
		catch (TaskCanceledException ex)
		{
			throw new TaskCanceledException("Task was canceled.", ex);
		}
		catch (Exception ex)
		{
			throw new RepositoryException("Failed to delete meetup.", ex);
		}
	}

	/// <summary>
	///		Looking for place with the same name in db.
	///		Do not track changes if find.
	/// </summary>
	/// <returns>Existing place from db or <paramref name="placeDto"></paramref>.</returns>
	private PlaceDto FindOrAddToContextPlace(PlaceDto placeDto)
	{
		var existingPlace = _pgContext.Places
			.AsNoTracking()
			.FirstOrDefault(e => e.Name == placeDto.Name);

		if (existingPlace != null)
			return existingPlace;

		_pgContext.Places.Attach(placeDto);
		return placeDto;
	}

	/// <summary>
	///		Looking for organizer with the same name in db.
	///		Do not track changes if find.
	/// </summary>
	/// <returns>Existing organizer from db or <paramref name="organizerDto"></paramref>.</returns>
	private OrganizerDto FindOrAddToContextOrganizer(OrganizerDto organizerDto)
	{
		var existingOrganizer = _pgContext.Organizers
			.AsNoTracking()
			.FirstOrDefault(e => e.Name == organizerDto.Name);

		if (existingOrganizer != null)
			return existingOrganizer;

		_pgContext.Organizers.Attach(organizerDto);
		return organizerDto;
	}
}