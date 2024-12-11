using System.Text.Json;
using System.Text.Json.Serialization;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Serialization
{
	public class UnixTimeToDateTimeConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.Number)
			{
				throw new JsonException("Expected number token.");
			}

			long unixTime = reader.GetInt64();
			return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).UtcDateTime;
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			long unixTime = ((DateTimeOffset)value).ToUnixTimeMilliseconds();
			writer.WriteNumberValue(unixTime);
		}
	}
}
