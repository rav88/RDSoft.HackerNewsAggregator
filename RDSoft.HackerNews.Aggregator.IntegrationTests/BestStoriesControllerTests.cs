using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using Moq;
using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Controllers;
using RDSoft.HackerNewsAggregator.Domain.Entities;
using Microsoft.Extensions.Logging;
using RDSoft.HackerNewsAggregator.Application.DTOs;
using RDSoft.HackerNewsAggregator.Application.Services;
using RDSoft.HackerNewsAggregator.Infrastructure.Cache;
using RDSoft.HackerNewsAggregator.Infrastructure.Clients;
using RDSoft.HackerNewsAggregator.Infrastructure.Serialization;
using System.Net;
using System.Text;
using Moq.Protected;

namespace RDSoft.HackerNews.Aggregator.IntegrationTests
{
	public class BestStoriesControllerTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(1000)]
		public async Task GetBestStories_ReturnsOkResult_WithListOfStories(int count)
		{
			// Arrange
			var maxResults = 500;
			var expectedList = new List<Story>();
			for (int i = 0; i < count && i < maxResults; i++)
			{
				expectedList.Add(new Story());
			}

			var expectedStoriesCount = expectedList.Count() > maxResults ? maxResults : expectedList.Count();

			var mockService = new Mock<IBestStoriesService>();
			var mockLogService = new Mock<ILogger<BestStoriesController>>();
			mockService.Setup(service => service.GetBestStoriesAsync(It.IsAny<int>()))
				.ReturnsAsync(expectedList);

			var controller = new BestStoriesController(mockLogService.Object, mockService.Object);

			// Act
			var result = await controller.Get(It.IsAny<int>());

			// Assert
			Assert.NotNull(result);
			var stories = Assert.IsAssignableFrom<IEnumerable<Story>>(result);
			Assert.Equal(expectedStoriesCount, stories.Count());
		}

		[Fact]
		public async Task GetBestStories_ReturnsOkResult_MockedApi()
		{
			// Arrange
			var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			var httpClient = new HttpClient(mockHandler.Object)
			{
				BaseAddress = new Uri("https://example.com/")
			};

			var loggerService = new Mock<ILogger<BestStoriesService>>();
			var loggerCache = new Mock<ILogger<MemoryCacheService>>();
			var loggerController = new Mock<ILogger<BestStoriesController>>();

			var memoryCacheService = new MemoryCacheService(new MemoryCache(new MemoryCacheOptions()), loggerCache.Object);
			var jsonOptionsFactory = new JsonOptionsFactory();
			var hackerNewsClient = new HackerNewsClient(httpClient, jsonOptionsFactory);
			var bestStoriesService = new BestStoriesService(hackerNewsClient, memoryCacheService, loggerService.Object);
			var controller = new BestStoriesController(loggerController.Object, bestStoriesService);

			var storyDto = new StoryDto
			{
				Title = "werwerw",
				By = "dfsdfsdf",
				Time = DateTime.Today.ToUniversalTime(),
				Score = 1234,
				Descendants = 1235,
				Url = "http://example.com"
			};
			var ids = new List<int> { 1 };

			var jsonResponseIds = JsonSerializer.Serialize(ids, jsonOptionsFactory.Create());
			var jsonResponseStories = JsonSerializer.Serialize(storyDto, jsonOptionsFactory.Create());

			mockHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.Is<HttpRequestMessage>(req =>
						 req.Method == HttpMethod.Get &&
						 req.RequestUri == new Uri("https://example.com/v0/beststories.json")),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(jsonResponseIds, Encoding.UTF8, "application/json")
				})
				.Verifiable();

			mockHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.Is<HttpRequestMessage>(req =>
						req.Method == HttpMethod.Get &&
						req.RequestUri == new Uri("https://example.com/v0/item/1.json")),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(jsonResponseStories, Encoding.UTF8, "application/json")
				})
				.Verifiable();




			// Act
			var result = await controller.Get(1);

			// Assert
			Assert.NotNull(result);
			Assert.IsAssignableFrom<IEnumerable<Story>>(result);

			Assert.Equal(storyDto.Title, result.ToArray()[0].Title);
			Assert.Equal(storyDto.Time, result.ToArray()[0].Time);
			Assert.Equal(storyDto.By, result.ToArray()[0].PostedBy);
			Assert.Equal(storyDto.Descendants, result.ToArray()[0].CommentCount);
			Assert.Equal(storyDto.Url, result.ToArray()[0].Uri);
			Assert.Equal(storyDto.Score, result.ToArray()[0].Score);
		}
	}
}