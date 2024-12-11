# Downlaod ASP.NET Core linux image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 3000

# Download SDK .NET 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy files of the solution
COPY ["RDSoft.HackerNewsAggregator.sln", "./"]
COPY ["RDSoft.HackerNewsAggregator.Api/RDSoft.HackerNewsAggregator.Api.csproj", "RDSoft.HackerNewsAggregator.Api/"]
COPY ["RDSoft.HackerNewsAggregator.Application/RDSoft.HackerNewsAggregator.Application.csproj", "RDSoft.HackerNewsAggregator.Application/"]
COPY ["RDSoft.HackerNewsAggregator.Domain/RDSoft.HackerNewsAggregator.Domain.csproj", "RDSoft.HackerNewsAggregator.Domain/"]
COPY ["RDSoft.HackerNewsAggregator.Infrastructure/RDSoft.HackerNewsAggregator.Infrastructure.csproj", "RDSoft.HackerNewsAggregator.Infrastructure/"]

COPY ["RDSoft.HackerNews.Aggregator.UnitTests/RDSoft.HackerNews.Aggregator.UnitTests.csproj", "RDSoft.HackerNews.Aggregator.UnitTests/"]
COPY ["RDSoft.HackerNews.Aggregator.IntegrationTests/RDSoft.HackerNews.Aggregator.IntegrationTests.csproj", "RDSoft.HackerNews.Aggregator.IntegrationTests/"]

# Restore packages
RUN dotnet restore "RDSoft.HackerNewsAggregator.sln"

# Copy all
COPY . .

# Configure workdir
WORKDIR "/src/RDSoft.HackerNewsAggregator.Api"
RUN dotnet build "RDSoft.HackerNewsAggregator.Api.csproj" -c Debug -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "RDSoft.HackerNewsAggregator.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

# Create docker image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RDSoft.HackerNewsAggregator.Api.dll"]