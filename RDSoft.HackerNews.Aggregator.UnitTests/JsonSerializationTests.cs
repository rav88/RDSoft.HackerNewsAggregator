using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Infrastructure.Serialization;
using System.Text.Json;
using RDSoft.HackerNewsAggregator.Application.DTOs;
using RDSoft.HackerNewsAggregator.Domain.Entities;

namespace RDSoft.HackerNews.Aggregator.UnitTests
{
	public class JsonSerializationTests
	{
		private IFactory<JsonSerializerOptions> _jsonOptionsFactory;

		public JsonSerializationTests()
		{
			_jsonOptionsFactory = new JsonOptionsFactory();
		}

		[Fact]
		public void JsonSerializationOptionsFactory_CreatesOptions()
		{
			// Arrange
			var factory = new JsonOptionsFactory();
			// Act
			var options = factory.Create();
			// Assert
			Assert.NotNull(options);
			Assert.False(options.WriteIndented);
			Assert.Equal(JsonNamingPolicy.CamelCase, options.PropertyNamingPolicy);
			Assert.Single(options.Converters);
			Assert.IsType<UnixTimeToDateTimeConverter>(options.Converters[0]);
		}

		[Theory]
		[InlineData("2021-02-07T03:58:29Z")]
		[InlineData("2022-11-30T11:44:07Z")]
		[InlineData("2024-08-01T17:13:57Z")]
		public void UnixTimeToDateTimeConverter_ValidationTest(DateTime dt)
		{
			dt = dt.ToUniversalTime();
			var unixTimeString = ConvertToUnixTimestamp(dt);

			var jsonStory =
				$"{{\"by\":\"author\",\"descendants\":123,\"id\":234,\"kids\":[42314852,42314698],\"score\":1234,\"time\":{unixTimeString},\"title\":\"Test title\",\"type\":\"story\",\"url\":\"http://test.com\"}}";
			var _jsonOptions = _jsonOptionsFactory.Create();

			var storyDto = JsonSerializer.Deserialize<StoryDto>(jsonStory, _jsonOptions);

			var expectedStoryDto = new StoryDto()
			{
				Title = "Test title",
				Time = dt,
				By = "author",
				Descendants = 123,
				Url = "http://test.com",
				Score = 1234
			};

			Assert.NotNull(storyDto);
			Assert.Equal(expectedStoryDto.Title, storyDto.Title);
			Assert.Equal(expectedStoryDto.Time, storyDto.Time);
			Assert.Equal(expectedStoryDto.By, storyDto.By);
			Assert.Equal(expectedStoryDto.Descendants, storyDto.Descendants);
			Assert.Equal(expectedStoryDto.Url, storyDto.Url);
			Assert.Equal(expectedStoryDto.Score, storyDto.Score);
		}

		[Fact]
		public void UnixTimeToDateTimeConverter_UnixTimeNotANumber()
		{
			var unixTimeString = "STRING_INSTEAD_OF_A_NUMBER";

			var jsonStory = $"{{\"time\":{unixTimeString},\"}}";
			var _jsonOptions = _jsonOptionsFactory.Create();

			Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<StoryDto>(jsonStory, _jsonOptions));
		}

		private long ConvertToUnixTimestamp(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
		}
	}
}
