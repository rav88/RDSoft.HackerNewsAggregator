using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RDSoft.HackerNewsAggregator.Application.Interfaces;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Serialization
{
	public class JsonOptionsFactory : IFactory<JsonSerializerOptions>
	{
		public JsonSerializerOptions Create()
		{
			JsonSerializerOptions jsonOptions = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = false
			};

			jsonOptions.Converters.Add(new UnixTimeToDateTimeConverter());

			return jsonOptions;
		}
	}
}
