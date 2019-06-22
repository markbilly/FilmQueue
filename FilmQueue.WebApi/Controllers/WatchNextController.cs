using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain;
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
using FluentValidation.AspNetCore;

namespace FilmQueue.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/watchlist/items/watchnext")]
    public class WatchNextController : ControllerBase
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IWatchlistItemReader _watchlistItemReader;
        private readonly IEventService _eventService;

        public WatchNextController(
            ICurrentUserAccessor currentUserAccessor,
            IWatchlistItemReader watchlistItemReader,
            IEventService eventService)
        {
            _currentUserAccessor = currentUserAccessor;
            _watchlistItemReader = watchlistItemReader;
            _eventService = eventService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(WatchlistItemResponse), 200)]
        public async Task<IActionResult> GetWatchNext()
        {
            var record = await _watchlistItemReader.GetCurrentWatchNextItem(_currentUserAccessor.CurrentUser.Id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(WatchlistItemResponse.FromRecord(record));
        }

        [HttpDelete]
        public async Task<IActionResult> ExpireWatchNext()
        {
            var record = await _watchlistItemReader.GetCurrentWatchNextItem(_currentUserAccessor.CurrentUser.Id);

            if (record == null)
            {
                return NotFound();
            }

            IActionResult result = null;

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                failedEvent.ValidationResult.AddToModelState(ModelState, null);
                result = BadRequest(ModelState);
            });

            await _eventService.Subscribe<WatchNextItemExpiredEvent>((successEvent) =>
            {
                result = NoContent();
            });

            await _eventService.IssueCommand(new ExpireWatchNextItemCommand
            {
                ItemId = record.Id
            });

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> SelectWatchNext([FromBody] SelectWatchNextItemRequest request)
        {
            IActionResult result = null;

            await _eventService.Subscribe<ResourceNotFoundEvent>((notFoundEvent) =>
            {
                result = NotFound();
            });

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                failedEvent.ValidationResult.AddToModelState(ModelState, null);
                result = BadRequest(ModelState);
            });

            await _eventService.Subscribe<WatchNextItemSelectedEvent>((successEvent) =>
            {
                result = NoContent();
            });

            await _eventService.IssueCommand(new SelectWatchNextItemCommand
            {
                UserId = _currentUserAccessor.CurrentUser.Id,
                ItemId = request.ItemId,
                SelectRandomItem = request.SelectRandomItem
            });

            return result;
        }
    }
}
