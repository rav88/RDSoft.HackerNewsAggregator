using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RDSoft.HackerNewsAggregator.Application.DTOs;
using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Domain.Entities;

namespace RDSoft.HackerNewsAggregator.Application.Services
{
	public class BestStoriesService(IHackerNewsClient hackerNewsClient, IMemoryCacheService cache, ILogger<BestStoriesService> logger) : IBestStoriesService
	{
		private const string CacheKey = "BestStoriesCache";
		private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

		public async Task<IEnumerable<Story>> GetBestStoriesAsync(int n)
		{
			if (n <= 0)
			{
				logger.Log(LogLevel.Warning, $"The number of stories must be greater than zero. Given: {n}!");
				throw new ArgumentException("The number of stories must be greater than zero.", nameof(n));
			}

			var storiesDto = await GetCachedStoriesAsync();

			var stories = storiesDto
				.Select(dto => new Story
				{
					Title = dto.Title,
					Uri = dto.Url,
					PostedBy = dto.By,
					Time = dto.Time,
					Score = dto.Score,
					CommentCount = dto.Descendants
				})
				.ToList();

			return stories.OrderByDescending(s => s.Score).Take(n);
		}

		private async Task<List<StoryDto>> GetCachedStoriesAsync()
		{
			var isCached = cache.TryGetValue(CacheKey, out List<StoryDto> cachedStories);
			if (!isCached)
			{
				cachedStories = await FetchAndCacheStoriesAsync();
			}

			return cachedStories;
		}

		private async Task<List<StoryDto>> FetchAndCacheStoriesAsync()
		{
			logger.Log(LogLevel.Information, "Downloaded data from endpoint.");
			var storyIds = await hackerNewsClient.GetBestStoryIdsAsync();

			var storyTasks = storyIds.Take(1000).Select(hackerNewsClient.GetStoryDetailsAsync);
			var stories = await Task.WhenAll(storyTasks);

			var validStories = stories
				.Where(story => story != null)
				.ToList();

			cache.Set(CacheKey, validStories, CacheDuration);
			logger.Log(LogLevel.Information, "Caching data");

			return validStories;
		}
	}
}
