using Meetup.Core.Domain.Entities;
using Meetup.Core.Domain.Exceptions;

namespace Meetup.Core.Application.Interfaces;

public interface IMeetupRepository
{
    /// <summary>
    ///     Creating a meetup in db.
    /// </summary>
    /// <param name="meetup"></param>
    /// <param name="token"></param>
    /// <exception cref="RepositoryException"></exception>
    /// <exception cref="TaskCanceledException"></exception>
    /// <returns>
    ///     Meetup's id (id > 0) if was created.
    ///     Otherwise returns 0.
    /// </returns>
    public Task<int> CreateAsync(MeetupEntity meetup, CancellationToken token = default);

	/// <summary>
	///     Getting meetups from db.
	/// </summary>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>
	/// <returns>
	///     <see cref="IEnumerable{T}"/> of <see cref="MeetupEntity"/> if any in db.
	///     Otherwise returns <see cref="Enumerable.Empty{TResult}"/> of <see cref="MeetupEntity"/>.
	/// </returns>
	public Task<IEnumerable<MeetupEntity>> GetAllOrEmptyAsync(CancellationToken token = default);

	/// <summary>
	///     Getting meetup by id from db.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>
	/// <returns>
	///		Instance of <see cref="MeetupEntity"/> if was found.
	///		Otherwise returns <see cref="MeetupEntity.Empty"/>.
	/// </returns>
	public Task<MeetupEntity> GetByIdOrEmptyAsync(int id, CancellationToken token = default);

	/// <summary>
	///		Updating meetup in db.
	/// </summary>
	/// <param name="meetup"></param>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>
	/// <returns>
	///		Meetup's id (id > 0) if was updated.
	///     Otherwise returns 0.
	/// </returns>
	public Task<int> UpdateAsync(MeetupEntity meetup, CancellationToken token = default);

	/// <summary>
	///		Deleting meetup from db.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>	
	/// <returns>
	///		Meetup's id (id > 0) if was deleted.
	///     Otherwise returns 0.
	/// </returns>
	public Task<int> DeleteAsync(int id, CancellationToken token = default);
}