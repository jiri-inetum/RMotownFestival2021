using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using RMotownFestival.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : Controller
    {
        public CosmosClient _cosmosClient { get; set; }
        public Container _websiteArticlesContainer { get; set; }

        public ArticlesController(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration.GetConnectionString("CosmosConnection"));
            _cosmosClient.CreateDatabaseIfNotExistsAsync("RMotownArticles");
            _cosmosClient.GetDatabase("RMotownArticles").CreateContainerIfNotExistsAsync("WebsiteArticles", "/tag");
            _websiteArticlesContainer = _cosmosClient.GetContainer("RMotownArticles", "WebsiteArticles");

        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Article))]
        public async Task<IActionResult> GetAsync()
        {
            var result = new List<Article>();

            var queryDefinition = _websiteArticlesContainer.GetItemLinqQueryable<Article>()
                .Where(p => p.Status == nameof(Status.Published))
                .OrderBy(p => p.Date);

            var iterator = queryDefinition.ToFeedIterator();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                result = response.ToList();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostArticle()
        {

            var dummyArticle = new Article()
            {
                Id = "myid3",
                Title = "mytitle",
                Date = DateTime.Now,
                Message = "mymessage",
                Status = "Published",
                Tag = "mytag",
                ImagePath = null
            };
            await _websiteArticlesContainer.CreateItemAsync(dummyArticle);
            return Ok();
        }
    }
}
