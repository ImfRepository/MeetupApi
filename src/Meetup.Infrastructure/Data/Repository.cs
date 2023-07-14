using AutoMapper;
using LanguageExt.Common;
using Meetup.Core.Application.Interfaces;
using Meetup.Core.Domain.Exceptions;
using Meetup.Core.Domain.Models;
using Meetup.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

internal class Repository : IMeetupRepository
{
	private readonly IMapper _mapper;
	private readonly PgContext _pgContext;

	public Repository(PgContext pgContext, IMapper mapper)
	{
		_pgContext = pgContext ?? throw new ArgumentNullException(nameof(pgContext));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<Result<int>> CreateAsync(MeetupModel meetup, CancellationToken token = default)
	{
		try
		{
			var entity = _mapper.Map<MeetupEntity>(meetup);

			// id = 0 to use added state by attach.
			entity.Id = 0;
			entity.Organizer!.Id = await FindOrganizer(meetup.Organizer);
			entity.Place!.Id = await FindPlace(meetup.Place);

			if (token.IsCancellationRequested)
				return new Result<int>(new TaskCanceledException());

			_pgContext.Meetups.Attach(entity);
			await _pgContext.SaveChangesAsync(token);

			return entity.Id;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return new Result<int>(new RepositoryException("Internal error."));
		}
	}

	public async Task<Result<IEnumerable<MeetupModel>>> GetAllOrEmptyAsync(CancellationToken token = default)
	{
		try
		{
			var sqlModels = await _pgContext.Meetups
				.AsNoTracking()
				.Include(e => e.Organizer)
				.Include(e => e.Place)
				.Include(e => e.PlanSteps)
				.ToListAsync(token);

			if (token.IsCancellationRequested)
				return new Result<IEnumerable<MeetupModel>>(new TaskCanceledException());

			return sqlModels.Any()
				? new Result<IEnumerable<MeetupModel>>(_mapper.Map<IEnumerable<MeetupModel>>(sqlModels))
				: new Result<IEnumerable<MeetupModel>>(new RepositoryException("No models in db."));
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return new Result<IEnumerable<MeetupModel>>(new RepositoryException("Internal error."));
		}
	}

	public async Task<Result<MeetupModel>> GetByIdOrEmptyAsync(int id, CancellationToken token = default)
	{
		try
		{
			var meetup = await _pgContext.Meetups.Where(e => e.Id == id)
				.AsNoTracking()
				.Include(e => e.Organizer)
				.Include(e => e.Place)
				.Include(e => e.PlanSteps)
				.FirstOrDefaultAsync(token);

			if (token.IsCancellationRequested)
				return new Result<MeetupModel>(new TaskCanceledException());

			return meetup == null
				? new Result<MeetupModel>(new RepositoryException($"There is no model with id = {id}."))
				: _mapper.Map<MeetupModel>(meetup);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return new Result<MeetupModel>(new RepositoryException("Internal error."));
		}
	}

	public async Task<Result<bool>> UpdateAsync(MeetupModel meetup, CancellationToken token = default)
	{
		try
		{
			var entity = _mapper.Map<MeetupEntity>(meetup);

			var oldOne = await _pgContext.Meetups
				.Where(e => e.Id == entity.Id)
				.Include(e => e.PlanSteps)
				.FirstOrDefaultAsync(token);

			if (oldOne == null)
				return new Result<bool>(new RepositoryException($"There is no model with id = {entity.Id}."));

			oldOne.Name = entity.Name;
			oldOne.Description = entity.Description;
			oldOne.Speaker = entity.Speaker;
			oldOne.Time = entity.Time;
			oldOne.PlanSteps = entity.PlanSteps;
			await UpdateOrg(oldOne, entity.Organizer!);
			await UpdatePlace(oldOne, entity.Place!);

			if (token.IsCancellationRequested)
				return new Result<bool>(new TaskCanceledException());

			await _pgContext.SaveChangesAsync(token);
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return new Result<bool>(new RepositoryException("Internal error."));
		}
	}

	public async Task<Result<bool>> DeleteAsync(int id, CancellationToken token = default)
	{
		try
		{
			await _pgContext.PlanSteps.Where(s => s.MeetupId == id)
				.ExecuteDeleteAsync(token);

			// No sense to save meetup, when we already deleted plan steps
			await _pgContext.Meetups.Where(m => m.Id == id)
				.ExecuteDeleteAsync(CancellationToken.None);

			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return new Result<bool>(new RepositoryException("Internal error."));
		}
	}

	private async Task<int> FindOrganizer(string name)
	{
		return await _pgContext.Organizers
			.Where(e => e.Name == name)
			.AsNoTracking()
			.Select(e => e.Id)
			.FirstOrDefaultAsync();
	}

	private async Task<int> FindPlace(string name)
	{
		return await _pgContext.Places
			.Where(e => e.Name == name)
			.AsNoTracking()
			.Select(e => e.Id)
			.FirstOrDefaultAsync();
	}

	private async Task UpdateOrg(MeetupEntity entity, OrganizerEntity newOrg)
	{
		var orgId = await FindOrganizer(newOrg.Name);

		if (orgId != 0)
		{ 
			entity.OrganizerId = orgId;
			entity.Organizer = null;
			return;
		}

		entity.Organizer = newOrg;
		entity.Organizer.Id = 0;
	}

	private async Task UpdatePlace(MeetupEntity entity, PlaceEntity newPlace)
	{
		var placeId = await FindPlace(newPlace.Name);

		if (placeId != 0)
		{
			entity.PlaceId = placeId;
			entity.Place = null;
			return;
		}

		entity.Place = newPlace;
		entity.Place.Id = 0;
	}
}