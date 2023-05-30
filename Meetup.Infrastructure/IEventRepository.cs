using Meetup.Core.Domain;
using Meetup.Infrastructure.SQL;

namespace Meetup.Infrastructure;

public interface IEventRepository
{
	public Task<bool> CreateAsync(Event meetup, CancellationToken token = default);

	public Task<IEnumerable<Event>> GetAllAsync(CancellationToken token = default);

	public Task<Event> GetByIdAsync(int id, CancellationToken token = default);

	public Task<bool> UpdateAsync(Event meetup, CancellationToken token = default);

	public Task<bool> DeleteAsync(int id, CancellationToken token = default);
}