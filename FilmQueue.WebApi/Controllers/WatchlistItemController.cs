using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Models.Requests;
using FilmQueue.WebApi.Models.Responses;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;

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
            var record = await _watchlistItemReader.GetById(id);

            return Ok(WatchlistItemResponse.FromRecord(record));
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

            return Ok(WatchlistItemResponse.FromDomainModel(item));
        }

        [HttpPost]
        [ProducesResponseType(typeof(WatchlistItemResponse), 201)]
        public async Task<IActionResult> CreateWatchlistItem([FromBody] CreateToWatchlistItemRequest createWatchlistItemRequest)
        {
            long itemId = 0;
            WatchlistItem item = null;

            await _eventService.Subscribe<WatchlistItemCreatedEvent>((createdEvent) =>
            {
                itemId = createdEvent.ItemId;
                item = createdEvent.Item;
            });

            await _eventService.Subscribe<ValidationFailedEvent<CreateWatchlistItemCommand>>((failedEvent) =>
            {
                failedEvent.ValidationResult.AddToModelState(ModelState, null);
            });

            await _eventService.QueueCommand(new CreateWatchlistItemCommand
            {
                Title = createWatchlistItemRequest.Title,
                RuntimeInMinutes = createWatchlistItemRequest.RuntimeInMinutes,
                UserId = _currentUserAccessor.CurrentUser.Id
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(GetWatchlistItem), new { id = itemId }, WatchlistItemResponse.FromDomainModel(item));
        }
    }
}
