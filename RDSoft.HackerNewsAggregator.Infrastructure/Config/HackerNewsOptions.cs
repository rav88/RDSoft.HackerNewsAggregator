using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDSoft.HackerNewsAggregator.Infrastructure.Config
{
	public class HackerNewsOptions
	{
		/// <summary>
		/// Base URL for the Hacker News API.
		/// </summary>
		public string BaseUrl { get; set; } = "https://hacker-news.firebaseio.com/";

		/// <summary>
		/// Maximum number of stories to fetch at once.
		/// </summary>
		public int MaxStoriesToFetch { get; set; } = 100;

		/// <summary>
		/// Cache duration for fetched stories in minutes.
		/// </summary>
		public int CacheDurationMinutes { get; set; } = 5;
	}
}
