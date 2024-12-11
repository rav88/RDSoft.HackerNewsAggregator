using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RDSoft.HackerNewsAggregator.Application.Interfaces;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Cache
{
	public class MemoryCacheService(IMemoryCache memoryCache, ILogger<MemoryCacheService> logger) : IMemoryCacheService
	{
		public T Get<T>(string key)
		{
			return (memoryCache.TryGetValue(key, out T? value) ? value : default)!;
		}

		public void Set<T>(string key, T value, TimeSpan duration)
		{
			memoryCache.Set(key, value, duration);
			logger.Log(LogLevel.Information, $"Set cache for key: {key}");
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			if (memoryCache.TryGetValue(key, out T? result))
			{
				value = result!;
				return true;
			}
			value = default!;
			return false;
		}

		public void Remove(string key)
		{
			memoryCache.Remove(key);
			logger.Log(LogLevel.Information, $"Removed cache for key: {key}");
		}
	}
}
