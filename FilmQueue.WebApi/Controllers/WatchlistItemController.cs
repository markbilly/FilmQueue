using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Domain.Requests;
using FilmQueue.WebApi.Domain.Responses;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
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
    [Route("api/watchlist/items")]
    public class WatchlistItemController : ControllerBase
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IWatchlistItemReader _watchlistItemReader;
        private readonly IEventService _eventService;

        public WatchlistItemController(
            ICurrentUserAccessor currentUserAccessor,
            IWatchlistItemReader watchlistItemReader,
            IEventService eventService)
        {
            _currentUserAccessor = currentUserAccessor;
            _watchlistItemReader = watchlistItemReader;
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WatchlistItemResponse), 200)]
        public async Task<IActionResult> GetWatchlistItem([FromRoute] long id)
        {
            var record = await _watchlistItemReader.GetItemById(id);

            var item = new WatchlistItem
            {
                Id = record.Id,
                Title = record.Title,
                RuntimeInMinutes = record.RuntimeInMinutes,
                Watched = record.WatchedDateTime.HasValue
            };

            return Ok(new WatchlistItemResponse(item));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(WatchlistItemResponse), 200)]
        public async Task<IActionResult> UpdateWatchlistItem([FromRoute] long id, [FromBody] UpdateWatchlistItemRequest updateWatchlistItemRequest)
        {
            WatchlistItem item = null;
            bool itemNotFound = false;
            IDictionary<string, string> validationMessages = null;

            await _eventService.Subscribe<WatchlistItemUpdatedEvent>((updatedEvent) =>
            {
                item = updatedEvent.Item;
            });

            await _eventService.Subscribe<WatchlistItemUpdateFailedEvent>((updateFailedEvent) =>
            {
                itemNotFound = updateFailedEvent.ItemNotFound;
                validationMessages = updateFailedEvent.ValidationMessages;
            });

            await _eventService.QueueCommand(new UpdateWatchlistItemCommand
            {
                ItemId = id,
                Title = updateWatchlistItemRequest.Title,
                RuntimeInMinutes = updateWatchlistItemRequest.RuntimeInMinutes
            });

            if (itemNotFound)
            {
                return NotFound();
            }

            if (validationMessages != null)
            {
                return BadRequest(validationMessages);
            }

            return Ok(new WatchlistItemResponse(item));
        }

        [HttpPost]
        [ProducesResponseType(typeof(WatchlistItemResponse), 201)]
        public async Task<IActionResult> CreateWatchlistItem([FromBody] CreateToWatchlistItemRequest createWatchlistItemRequest)
        {
            long itemId = 0;
            WatchlistItem item = null;
            IDictionary<string, string> validationMessages = null;

            await _eventService.Subscribe<WatchlistItemCreatedEvent>((createdEvent) =>
            {
                itemId = createdEvent.ItemId;
                item = createdEvent.Item;
            });

            await _eventService.Subscribe<WatchlistItemCreationFailedEvent>((createFailedEvent) =>
            {
                validationMessages = createFailedEvent.ValidationMessages;
            });

            await _eventService.QueueCommand(new CreateWatchlistItemCommand
            {
                Title = createWatchlistItemRequest.Title,
                RuntimeInMinutes = createWatchlistItemRequest.RuntimeInMinutes,
                UserId = _currentUserAccessor.CurrentUser.Id
            });

            if (validationMessages != null)
            {
                return BadRequest(validationMessages);
            }

            return CreatedAtAction(nameof(GetWatchlistItem), new { id = itemId }, new WatchlistItemResponse(item));
        }
    }
}
