using Meetup.Core.Domain;

namespace Meetup.Core.Application;

public interface IMeetupRepository
{
	public Task<int> CreateAsync(MeetupModel meetup, CancellationToken token = default);

	public Task<IEnumerable<MeetupModel>> GetAllOrEmptyAsync(CancellationToken token = default);

	public Task<MeetupModel> GetByIdOrEmptyAsync(int id, CancellationToken token = default);

	public Task<int> UpdateAsync(MeetupModel meetup, CancellationToken token = default);

	public Task<int> DeleteAsync(int id, CancellationToken token = default);
}