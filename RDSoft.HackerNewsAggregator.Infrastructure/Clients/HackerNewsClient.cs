using RDSoft.HackerNewsAggregator.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RDSoft.HackerNewsAggregator.Application.DTOs;
using Microsoft.Extensions.Options;
using RDSoft.HackerNewsAggregator.Infrastructure.Serialization;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Clients
{
	public class HackerNewsClient(HttpClient httpClient, IFactory<JsonSerializerOptions> jsonOptionsFactory) : IHackerNewsClient
	{
		private readonly JsonSerializerOptions _jsonOptions = jsonOptionsFactory.Create();

		public async Task<IEnumerable<int>> GetBestStoryIdsAsync()
		{
			try
			{
				var response = await httpClient.GetAsync("v0/beststories.json");
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				var result = JsonSerializer.Deserialize<IEnumerable<int>>(content, _jsonOptions);

				return result ?? [];
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to retrieve best story IDs from Hacker News API.", ex);
			}
		}

		public async Task<StoryDto> GetStoryDetailsAsync(int storyId)
		{
			try
			{
				var response = await httpClient.GetAsync($"v0/item/{storyId}.json");
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				var story = JsonSerializer.Deserialize<StoryDto>(content, _jsonOptions);

				if (story == null)
				{
					throw new Exception($"Story with ID {storyId} not found.");
				}

				return story;
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to retrieve details for story ID {storyId} from Hacker News API.", ex);
			}
		}
	}
}
