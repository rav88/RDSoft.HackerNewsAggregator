using Microsoft.AspNetCore.Mvc;
using RDSoft.HackerNewsAggregator.Application.Interfaces;
using RDSoft.HackerNewsAggregator.Domain.Entities;

namespace RDSoft.HackerNewsAggregator.Controllers
{
    [ApiController]
	[Route("api/v1/[controller]")]
	public class BestStoriesController(ILogger<BestStoriesController> logger, IBestStoriesService bestStoriesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Story>> Get(int count)
        {
	        var stories = await bestStoriesService.GetBestStoriesAsync(count);

	        return stories;
        }
    }
}
