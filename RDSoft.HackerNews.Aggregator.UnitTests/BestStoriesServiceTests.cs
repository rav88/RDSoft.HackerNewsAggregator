using Moq;
using RDSoft.HackerNewsAggregator.Application.DTOs;
using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Application.Services;
using Range = Moq.Range;

namespace RDSoft.HackerNews.Aggregator.UnitTests
{
	public class BestStoriesServiceTests
	{
		private readonly Mock<IHackerNewsClient> _mockHackerNewsClient;
		private readonly Mock<IMemoryCacheService> _mockMemoryCacheService;

		private readonly BestStoriesService _bestStoriesService;

		public BestStoriesServiceTests()
		{
			// Mocks initialization
			_mockHackerNewsClient = new Mock<IHackerNewsClient>();
			_mockMemoryCacheService = new Mock<IMemoryCacheService>();

			// Real services initialization
			_bestStoriesService = new BestStoriesService(_mockHackerNewsClient.Object, _mockMemoryCacheService.Object);
		}

		[Fact]
		public async Task GetBestStoriesAsync_ReturnsStoriesFromCache_WhenCacheIsNotEmpty()
		{
			// Arrange
			var cachedStories = new List<StoryDto>
			{
				new() { Title = "Cached Story 1" },
				new() { Title = "Cached Story 2" },
				new() { Title = "Cached Story 3" }
			};

			var cacheKey = "BestStoriesCache";
			List<int> cachedIds = [1, 2, 3];

			object cacheEntry = cachedStories;
			_mockMemoryCacheService
				.Setup(mc => mc.TryGetValue(cacheKey, out cacheEntry))
				.Returns(true)
				.Callback((object key, out object value) => { value = key.Equals(cacheKey) ? cachedStories : null; });
			_mockHackerNewsClient
				.Setup(client => client.GetStoryDetailsAsync(It.IsAny<int>()))
				.ReturnsAsync(new StoryDto()
				{
					By = It.IsAny<string>(),
					Descendants = It.IsInRange(0, 9999, Range.Inclusive),
					Title = It.IsAny<string>(),
					Time = It.IsAny<DateTime>(),
					Score = It.IsInRange(0, 9999, Range.Inclusive),
					Url = It.IsAny<string>()
				});

			// Act
			var isCache = _mockMemoryCacheService.Object.TryGetValue(cacheKey, out List<StoryDto> actualValue);
			var result = await _bestStoriesService.GetBestStoriesAsync(cachedStories.Count);
			var expectedResult = result.ToList();

			// Assert
			Assert.Equal(cachedStories[0].Title, expectedResult[0].Title);
			Assert.Equal(cachedStories[1].Title, expectedResult[1].Title);
			Assert.Equal(cachedStories[2].Title, expectedResult[2].Title);
			Assert.True(isCache);
			_mockHackerNewsClient.Verify(client => client.GetBestStoryIdsAsync(), Times.Never);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		public async Task GetBestStoriesAsync_ReturnsStoriesFromClient_WhenCacheIsEmpty(int count)
		{
			// Arrange
			var cacheKey = "BestStoriesCache";

			List<int> fetchedIds = [];
			for (int i = 0; i < count; i++)
			{
				fetchedIds.Add(i+1);
			}

			object cacheEntry = null;
			_mockMemoryCacheService
				.Setup(mc => mc.TryGetValue(cacheKey, out cacheEntry))
				.Returns(false);
			_mockHackerNewsClient
				.Setup(client => client.GetBestStoryIdsAsync())
				.ReturnsAsync(fetchedIds);
			_mockHackerNewsClient
				.Setup(client => client.GetStoryDetailsAsync(It.IsAny<int>()))
				.ReturnsAsync(new StoryDto()
				{
					By = It.IsAny<string>(),
					Descendants = It.IsInRange(0,9999, Range.Inclusive),
					Title = It.IsAny<string>(),
					Time = It.IsAny<DateTime>(),
					Score = It.IsInRange(0, 9999, Range.Inclusive),
					Url = It.IsAny<string>()
				});

			// Act
			var result = await _bestStoriesService.GetBestStoriesAsync(count);

			// Assert
			Assert.Equal(fetchedIds.Count(), result.Count());
			_mockHackerNewsClient.Verify(client => client.GetBestStoryIdsAsync(), Times.Once);
		}

		[Fact]
		public async Task GetBestStoryIdsAsync_ReturnsListOfIds()
		{
			// Arrange
			var expectedIds = new List<int> { 1, 2, 3, 4, 5 };

			_mockHackerNewsClient.Setup(client => client.GetBestStoryIdsAsync())
				.ReturnsAsync(expectedIds);

			foreach (int id in expectedIds)
			{
				_mockHackerNewsClient.Setup(client => client.GetStoryDetailsAsync(id))
					.ReturnsAsync(GetMockedStoryDetailsForId(id));
			}

			// Act
			var actualIds = await _bestStoriesService.GetBestStoriesAsync(expectedIds.Count);

			// Assert
			Assert.NotNull(actualIds);
			Assert.Equal(expectedIds.Count, actualIds.Count());
			_mockHackerNewsClient.Verify(client => client.GetBestStoryIdsAsync(), Times.Once);
		}

		private StoryDto GetMockedStoryDetailsForId(int id)
		{
			return new StoryDto
			{
				Title = $"Title {id}",
				Descendants = It.IsInRange(0, 9999, Range.Inclusive),
				Time = DateTime.UtcNow,
				Score = It.IsInRange(0, 9999, Range.Inclusive),
				By = $"Random {id}"
			};
		}
	}
}