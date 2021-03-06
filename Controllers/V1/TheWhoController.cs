using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Indexers;
using Tipalti.TheWho.Logger;
using Tipalti.TheWho.Models;
using Tipalti.TheWho.Services;

namespace Tipalti.TheWho.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [SwaggerTag("Values service API")]
    public class TheWhoController : ControllerBase
    {
        private SearchService _searchService;
        private readonly ILogger _logger;
        private readonly IIndexerUtils _utils;
        private readonly IConfluenceIndexer _confIndexer;
        private readonly IJiraIndexer _jiraIndexer;
        private readonly IServiceIndexer _serviceIndexer;
        private readonly ITeamIndexer _teamIndexer;
        private readonly IWebHostEnvironment _hostEnvironment;

        //this list is only for demonstrating CRUD methods and how to document API
        private static readonly List<int> Values = new List<int>
        {
            1,3,5,8,10
        };

        public TheWhoController(ILogger<ITheWhoLogger> logger, ISearchService searchService,
            IIndexerUtils utils, IConfluenceIndexer confIndexer, IServiceIndexer serviceIndexer,
            ITeamIndexer teamIndexer, IWebHostEnvironment environment, IJiraIndexer jiraIndexer)
        {
            _searchService = (SearchService)searchService;
            _logger = logger;
            _utils = utils;
            _confIndexer = confIndexer;
            _jiraIndexer = jiraIndexer;
            _serviceIndexer = serviceIndexer;
            _teamIndexer = teamIndexer;
            _hostEnvironment = environment;
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <response code="200">List of all values</response>
        /// <returns>A list of values</returns> 
        /*[HttpGet]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<int>>> GetAsync()
        {
            _logger.LogDebug("Values - Get all");
            return await Task.FromResult(Values);

        }*/

        /// <summary>
        /// Get Teams
        /// </summary>
        /// <response code="200">List of all values</response>
        /// <returns>A list of Teams</returns> 
        [HttpGet("GetTeamNames")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public List<string> GetTeamsNames()
        {
            _logger.LogDebug("Values - Get team names");            
            return _utils.GetTeamNames();            
        }

        /// <summary>
        /// Value with specified id
        /// </summary>
        /// <returns>A value with specified id</returns>
        /// <response code="200">Returns the value with the specified id</response>
        /// <response code="404">No value was found with the specified id</response>
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [HttpGet("Search")]
        public List<AllResult> Search(string search)
        {
            return _searchService.SearchResults(search);


        }

        /// <summary>
        /// Create a new value
        /// </summary>
        /// <param name="id">A new value id</param>
        /// <response code="201">The created value</response>
        /// <response code="400">Value already exist</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{id}")]
        public async Task<ActionResult<int>> CreateAsync(int id)
        {
            if (Values.Contains(id))
            {
                return BadRequest();
            }

            Values.Add(id);

            return await Task.FromResult(Created($"{id}",id));
        }

        /// <summary>
        /// Delete a value
        /// </summary>
        /// <param name="id">Id of the value to delete</param>
        /// <response code="200"> Value deleted successfully</response>
        /// <response code="404"> Value was found with the specified id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteAsync(int id)
        {
            if (!Values.Contains(id))
            {
                return NotFound();
            }

            Values.Remove(id);

            return await Task.FromResult(Ok());
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("RunConfluenceIndexer")]
        public async Task RunConfluenceIndexer()
        {
            await _confIndexer.RunAsync();            
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("RunJiraIndexer")]
        public async Task RunJiraIndexer()
        {
            await _jiraIndexer.RunAsync();
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("RunServiceIndexer")]
        public async Task RunServiceIndexer()
        {
            await _serviceIndexer.RunAsync();
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("RunTeamIndexer")]
        public async Task RunTeamIndexer()
        {
            await _teamIndexer.RunAsync(_hostEnvironment.ContentRootPath);
        }
    }
}
