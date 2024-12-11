using Microsoft.Extensions.Caching.Memory;
using RDSoft.HackerNewsAggregator.Application.Interfaces;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Cache
{
	public class MemoryCacheService(IMemoryCache memoryCache) : IMemoryCacheService
	{
		public T Get<T>(string key)
		{
			return memoryCache.TryGetValue(key, out T value) ? value : default;
		}

		public void Set<T>(string key, T value, TimeSpan duration)
		{
			memoryCache.Set(key, value, duration);
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			return memoryCache.TryGetValue(key, out value);
		}

		public void Remove(string key)
		{
			memoryCache.Remove(key);
		}
	}
}
