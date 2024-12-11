using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Application.Services;
using RDSoft.HackerNewsAggregator.Infrastructure.Cache;
using RDSoft.HackerNewsAggregator.Infrastructure.Clients;
using RDSoft.HackerNewsAggregator.Infrastructure.Config;
using RDSoft.HackerNewsAggregator.Infrastructure.Serialization;
using System.Text.Json;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Extensions
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddCaching(this IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddScoped<IMemoryCacheService, MemoryCacheService>();

			return services;
		}

		public static IServiceCollection AddHackerNewsClient(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient<IHackerNewsClient, HackerNewsClient>(client =>
			{
				var options = configuration.GetSection("Endpoints").GetSection("HackerNews").Get<HackerNewsOptions>();
				client.BaseAddress = new Uri(options.BaseUrl);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});

			return services;
		}

		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IBestStoriesService, BestStoriesService>(); 
			services.AddSingleton<IFactory<JsonSerializerOptions>, JsonOptionsFactory>();

			return services;
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			return services;
		}
	}
}
