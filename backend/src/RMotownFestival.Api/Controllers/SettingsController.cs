
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using RMotownFestival.Api.Options;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly AppSettingsOptions _options;
        private readonly IFeatureManagerSnapshot _featerManager;
        
        public SettingsController(IOptions<AppSettingsOptions> options,
            IFeatureManagerSnapshot featureManager)
        {
            _options = options.Value;
            _featerManager = featureManager;
        }

        [HttpGet("Features")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Features()
        {
            string message = await _featerManager.IsEnabledAsync("BuyTickets")
                ? "The ticket sale has started. Go go go!" :
                "You cannot buy any tickets at the moment.";

            return Ok(message);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AppSettingsOptions))]
        public IActionResult Get()
        {
            return Ok(_options);
        }
    }
}
