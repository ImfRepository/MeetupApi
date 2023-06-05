using Meetup.Core.Domain;

namespace Meetup.Core.Application;

public interface IMeetupRepository
{
	public Task<bool> CreateAsync(MeetupView meetup, CancellationToken token = default);

	public Task<IEnumerable<MeetupView>> GetAllAsync(CancellationToken token = default);

	public Task<MeetupView> GetByIdAsync(int id, CancellationToken token = default);

	public Task<bool> UpdateAsync(MeetupView meetup, CancellationToken token = default);

	public Task<bool> DeleteAsync(int id, CancellationToken token = default);
}