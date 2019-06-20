using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Requests;
using FilmQueue.WebApi.Domain.Responses;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmQueue.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/watchlist")]
    public class WatchlistController : ControllerBase
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IWatchlistReader _watchlistReader;
        private readonly IEventService _eventService;

        public WatchlistController(
            ICurrentUserAccessor currentUserAccessor,
            IWatchlistReader watchlistReader,
            IEventService eventService)
        {
            _currentUserAccessor = currentUserAccessor;
            _watchlistReader = watchlistReader;
            _eventService = eventService;
        }
        
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<QueryResponse<string>>), 200)]
        public async Task<IActionResult> GetWatchlistItems(int page = 1, int pageSize = 5)
        {
            var items = await _watchlistReader.GetItemsByUserId(_currentUserAccessor.CurrentUser.Id, pageSize, (page - 1) * pageSize);

            return Ok(QueryResponse<WatchlistItem>.FromEnumerable(items, item => new WatchlistItem(item.Title, item.RuntimeInMinutes)));
        }

        [HttpGet("items/{id}")]
        [ProducesResponseType(typeof(WatchlistItem), 200)]
        public async Task<IActionResult> GetWatchlistItem([FromRoute] long id)
        {
            var item = await _watchlistReader.GetItemById(id);

            return Ok(new WatchlistItem(item.Title, item.RuntimeInMinutes));
        }

        [HttpPost("items")]
        [ProducesResponseType(typeof(WatchlistItem), 201)]
        public async Task<IActionResult> AddToWatchlist([FromBody] AddToWatchlistRequest addToWatchlistRequest)
        {
            WatchlistItemCreatedEvent createdEvent = null;
            IDictionary<string, string> validationMessages = null;

            await _eventService.Subscribe<WatchlistItemCreatedEvent>((e) =>
            {
                createdEvent = e;
            });

            await _eventService.Subscribe<WatchlistItemCreationFailedEvent>((e) =>
            {
                validationMessages = e.ValidationMessages;
            });

            await _eventService.QueueCommand(new CreateWatchlistItemCommand
            {
                Title = addToWatchlistRequest.Title,
                RuntimeInMinutes = addToWatchlistRequest.RuntimeInMinutes,
                UserId = _currentUserAccessor.CurrentUser.Id
            });

            if (validationMessages != null)
            {
                return BadRequest(validationMessages);
            }
                        
            return CreatedAtAction(nameof(GetWatchlistItem), new { id = createdEvent.ItemId }, createdEvent.Item);
        }
    }
}
