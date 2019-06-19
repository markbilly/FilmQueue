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
    [Route("api/watchlist/items/watchnext")]
    public class WatchNextController : ControllerBase
    {
        private readonly IWatchNextService _watchNextService;

        public WatchNextController(IWatchNextServiceFactory watchNextServiceFactory)
        {
            _watchNextService = watchNextServiceFactory.GetForCurrentUser();
        }

        [HttpGet]
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

        [HttpPut]
        [ProducesResponseType(typeof(WatchlistItem), 201)]
        public async Task<IActionResult> SelectWatchNext()
        {
            var current = await _watchNextService.GetCurrentWatchNextItem();

            if (current != null)
            {
                return BadRequest("You already have a watch next item. Mark it as watched or skipped first.");
            }

            var newItem = await _watchNextService.SelectNewWatchNextItem();

            return CreatedAtAction(nameof(GetWatchNext), newItem);
        }

        [HttpPut("iswatched")]
        public async Task<IActionResult> MarkAsWatched()
        {
            var current = await _watchNextService.GetCurrentWatchNextItem();

            if (current == null)
            {
                return NotFound();
            }

            await _watchNextService.MarkCurrentWatchNextAsWatched();

            return NoContent();
        }
    }
}
