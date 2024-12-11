using System.Text.Json.Serialization;

namespace RDSoft.HackerNewsAggregator.Application.DTOs;

public class StoryDto
{
	public string? Title { get; set; }

	public string? Url { get; set; }

	public string? By { get; set; }

	public DateTime Time { get; set; }

	public int Score { get; set; }

	public int Descendants { get; set; }
}
