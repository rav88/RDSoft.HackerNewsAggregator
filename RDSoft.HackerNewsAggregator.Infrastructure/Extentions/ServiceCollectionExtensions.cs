using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Application.Services;
using RDSoft.HackerNewsAggregator.Infrastructure.Clients;
using RDSoft.HackerNewsAggregator.Infrastructure.Config;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		//public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		//{
		//	services.AddScoped<IBestStoriesService, BestStoriesService>();

		//	return services;
		//}

		//public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		//{
		//	services.Configure<HackerNewsOptions>(configuration.GetSection("Endpoints.HackerNews"));

		//	services.AddHttpClient<IHackerNewsClient, HackerNewsClient>((provider, client) =>
		//	{
		//		var options = provider.GetRequiredService<IOptions<HackerNewsOptions>>().Value;
		//		client.BaseAddress = new Uri(options.BaseUrl);
		//		client.DefaultRequestHeaders.Add("Accept", "application/json");
		//	});

		//	services.AddMemoryCache();

		//	return services;
		//}
	}
}
