using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDSoft.HackerNewsAggregator.Application.Interfaces
{
	public interface IMemoryCacheService
	{
		T Get<T>(string key);
		void Set<T>(string key, T value, TimeSpan duration);
		bool TryGetValue<T>(string key, out T value);
		void Remove(string key);
	}
}
