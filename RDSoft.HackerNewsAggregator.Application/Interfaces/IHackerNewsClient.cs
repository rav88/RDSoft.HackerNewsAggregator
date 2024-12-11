using RDSoft.HackerNewsAggregator.Application.DTOs;

namespace RDSoft.HackerNewsAggregator.Application.Interfaces
{
	public interface IHackerNewsClient
	{
		Task<IEnumerable<int>> GetBestStoryIdsAsync();
		Task<StoryDto> GetStoryDetailsAsync(int storyId);
	}
}
