using RDSoft.HackerNewsAggregator.Domain.Entities;

namespace RDSoft.HackerNewsAggregator.Application.Interfaces
{
	public interface IBestStoriesService
	{
		Task<IEnumerable<Story>> GetBestStoriesAsync(int n);
	}
}
