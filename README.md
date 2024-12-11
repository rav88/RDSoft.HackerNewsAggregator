# RDSoft.HackerNewsAggregator

## Description
This app is a HackerNews Stories aggregator and wrapper from the HackerNews API with simple caching.<br />
It downloads the Story's ID list, then gets the information for every Story, and orders them descending according to the commentCount value.<br />
It uses simple memory cache, to store the data for 5minutes locally in memory, to prevent frequent external API calls.
If the calls are more frequent than 5 minutes (parametrized), then the data is sent to the user from cached data.
  
Adventages: Simplicity. Less workload for the external API's<br />
Disadvantages: Timespans when data are not the most up-to-date
  
Endpoints used:
 - https://hacker-news.firebaseio.com/v0/
 
## Enchancements for the future
 - Use Polly.NET
  
For instance, using Polly's Retry policy allows the application to automatically retry failed operations, which is particularly useful when handling transient errors like network timeouts.<br />
The Circuit Breaker policy can prevent the app from repeatedly attempting operations that are likely to fail, thereby helping to maintain overall system health.<br />
**2 Man/day improvement**

- Use Queueing system for the API clients<br />
**2-3 Man/day improvement**
  
- Use redis as cache

Redis offers optional data persistence by periodically writing data to disk. This feature ensures that cached data can be recovered after a system crash, combining the benefits of caching and durability.<br />
  Integrating Redis as a caching layer in the app can significantly enhance performance and scalability.<br />
  Depending on the needs.<br />
**2-3 man/day**
  
- Use CORS scheduler
  
Use a Scheduler and create a worker, to store the cache data more systematically over time
**1-2 man/day**

## Run with dotnet cli
```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project .\RDSoft.HackerNewsAggregator.Api\  
```

Requires .NET 8.0.404
  
Runs on http://localhost:3000/ <br />
Endpoint: GET http://localhost:3000/api/v1/BestStories?count=N where N > 0 <br />
Swagger: http://localhost:3000/swagger/index.html <br />
  

## Deploy as a Docker Container
```bash
docker build -t rdsoft-hackernewsaggregator:v1.0 . 
docker run -d -p 8080:8080 --name hackernews_aggregator_api rdsoft-hackernewsaggregator:v1.0
```
Endpoint: GET http://localhost:8080/api/v1/BestStories?count=N where N > 0
  
Runs in production mode - no Swagger

# Explanation of Structure

## 1. `src/` - Main Application  

### A. `RDSoft.HackerNewsService.API`  
**Purpose:** API endpoints and application configuration.  
**Subdirectories:**  
- `Controllers/` — API controllers.  
- `Program.cs` — Application configuration (routing, dependency injection, middleware).  
- Configuration files (`appsettings.json`) — Environment-specific configuration.  

### B. `RDSoft.HackerNewsService.Application`  
**Purpose:** Application logic, interface definitions, and DTOs.  
**Subdirectories:**  
- `Interfaces/` — Interfaces for application services and external clients.  
- `Services/` — Implementations of business logic services.  
- `DTOs/` — Data Transfer Objects used for communication between layers.  

### C. `RDSoft.HackerNewsService.Infrastructure`  
**Purpose:** Infrastructure logic (e.g., integration with external APIs, caching).  
**Subdirectories:**  
- `Clients/` — Hacker News API client.  
- `Extensions/` — Extensions for service registration in dependency injection.  
- `Config/` — Application configurations (e.g., options for `HttpClient`).  
- `Cache/` — Caching mechanisms.  

### D. `RDSoft.HackerNewsService.Domain`  
**Purpose:** Domain layer (entities, exceptions).  
**Subdirectories:**  
- `Entities/` — Classes representing domain data.  
- `Exceptions/` — Custom exceptions.  