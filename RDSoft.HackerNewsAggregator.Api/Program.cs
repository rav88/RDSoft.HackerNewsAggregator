
using RDSoft.HackerNewsAggregator.Infrastructure.Config;
using RDSoft.HackerNewsAggregator.Infrastructure.Extensions;

namespace RDSoft.HackerNewsAggregator
{
    public class Program
    {
        private const string HackerNewsEndpoint = "Endpoints.HackerNews";

		public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add memory cache.
            builder.Services.AddCaching();

			// Register HackerNewsOptions
			builder.Services.Configure<HackerNewsOptions>(builder.Configuration.GetSection(HackerNewsEndpoint));

            // Register HttpClient
            builder.Services.AddHackerNewsClient(builder.Configuration);

            // Register services
            builder.Services.AddServices();

			// Register controllers
			builder.Services.AddControllers();

			// Configure Swagger
			builder.Services.AddSwagger();

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
