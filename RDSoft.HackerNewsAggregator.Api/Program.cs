
using System.Text.Json;
using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Application.Services;
using RDSoft.HackerNewsAggregator.Infrastructure.Cache;
using RDSoft.HackerNewsAggregator.Infrastructure.Clients;
using RDSoft.HackerNewsAggregator.Infrastructure.Config;
using RDSoft.HackerNewsAggregator.Infrastructure.Extensions;
using RDSoft.HackerNewsAggregator.Infrastructure.Serialization;

namespace RDSoft.HackerNewsAggregator
{
    public class Program
    {
        private const string HackerNewsEndpoint = "Endpoints.HackerNews";

		public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add memory cache.
			builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();

            // Register HackerNewsOptions
            builder.Services.Configure<HackerNewsOptions>(builder.Configuration.GetSection(HackerNewsEndpoint));

            // Register HttpClient
            builder.Services.AddHttpClient<IHackerNewsClient, HackerNewsClient>(client =>
            {
	            // Use options to configure HttpClient
	            var options = builder.Configuration.GetSection("Endpoints").GetSection("HackerNews").Get<HackerNewsOptions>();
	            client.BaseAddress = new Uri(options.BaseUrl);
	            client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // Register services
            builder.Services.AddScoped<IBestStoriesService, BestStoriesService>();
            builder.Services.AddSingleton<IFactory<JsonSerializerOptions>, JsonOptionsFactory>();

			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

			app.MapControllers();

            app.Run();
        }
    }
}
