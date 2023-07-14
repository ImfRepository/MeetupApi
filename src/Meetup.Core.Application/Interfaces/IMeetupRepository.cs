using LanguageExt.Common;
using Meetup.Core.Domain.Exceptions;
using Meetup.Core.Domain.Models;

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
	///     Otherwise returns exceptions in <see cref="Result{A}"/>.
	/// </returns>
	public Task<Result<int>> CreateAsync(MeetupModel meetup, CancellationToken token = default);

	/// <summary>
	///     Getting meetups from db.
	/// </summary>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>
	/// <returns>
	///     <see cref="IEnumerable{T}" /> of <see cref="MeetupModel" /> if any in db.
	///     Otherwise returns exceptions in <see cref="Result{A}"/>.
	/// </returns>
	public Task<Result<IEnumerable<MeetupModel>>> GetAllOrEmptyAsync(CancellationToken token = default);

	/// <summary>
	///     Getting meetupModel by id from db.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>
	/// <returns>
	///     Instance of <see cref="MeetupModel" /> if was found.
	///     Otherwise returns exceptions in <see cref="Result{A}"/>.
	/// </returns>
	public Task<Result<MeetupModel>> GetByIdOrEmptyAsync(int id, CancellationToken token = default);

	/// <summary>
	///     Updating meetupModel in db.
	/// </summary>
	/// <param name="meetup"></param>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>
	/// <returns>
	///     True if was updated.
	///     Otherwise returns exceptions in <see cref="Result{A}"/>.
	/// </returns>
	public Task<Result<bool>> UpdateAsync(MeetupModel meetup, CancellationToken token = default);

	/// <summary>
	///     Deleting meetupModel from db.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <exception cref="RepositoryException"></exception>
	/// <exception cref="TaskCanceledException"></exception>
	/// <returns>
	///     True if was deleted.
	///     Otherwise returns exceptions in <see cref="Result{A}"/>.
	/// </returns>
	public Task<Result<bool>> DeleteAsync(int id, CancellationToken token = default);
}