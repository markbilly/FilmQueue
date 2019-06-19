using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class WatchNextController : ControllerBase
    {
        private readonly IWatchNextService _watchNextService;

        public WatchNextController(IWatchNextServiceFactory watchNextServiceFactory)
        {
            _watchNextService = watchNextServiceFactory.GetForCurrentUser();
        }

        [HttpGet("watchlist/items/watchnext")]
        [ProducesResponseType(typeof(WatchlistItem), 200)]
        public async Task<IActionResult> GetWatchNext()
        {
            var current = await _watchNextService.GetCurrentWatchNextItem();

            if (current == null)
            {
                return NotFound();
            }

            return Ok(current);
        }

        [HttpPost("newwatchnextrequests")]
        public async Task<IActionResult> GenerateWatchNext()
        {
            var current = await _watchNextService.GetCurrentWatchNextItem();

            if (current != null)
            {
                return BadRequest("You already have a watch next item. Mark it as watched or skipped first.");
            }

            var newItem = await _watchNextService.SelectNewWatchNextItem();

            return Ok();
        }

        [HttpPut("watchlist/items/watchnext")]
        public async Task<IActionResult> UpdateWatchNext([FromBody] UpdateWatchNextRequest updateWatchNextRequest)
        {
            var current = await _watchNextService.GetCurrentWatchNextItem();

            if (current == null)
            {
                return NotFound();
            }

            if (updateWatchNextRequest.IsWatched != true)
            {
                return BadRequest("Disallowed action. You may only set watch next item to 'watched'");
            }

            await _watchNextService.MarkCurrentWatchNextAsWatched();

            return NoContent();
        }
    }
}
