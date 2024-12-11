# RDSoft.HackerNewsAggregator
HackerNews Stories aggregator from the HackerNews API with cache.

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