﻿using FilmQueue.WebApi.DataAccess;
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

namespace FilmQueue.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
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

        [HttpGet("watchlist/items/watchnext")]
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

        [HttpPost("newwatchnextrequests")]
        public async Task<IActionResult> SelectNewWatchNextItem()
        {
            IDictionary<string, string> validationMessages = null;

            await _eventService.Subscribe<NewWatchNextItemSelectionFailedEvent>((e) =>
            {
                validationMessages = e.ValidationMessages;
            });

            await _eventService.QueueCommand(new SelectNewWatchNextItemCommand
            {
                UserId = _currentUserAccessor.CurrentUser.Id
            });

            if (validationMessages != null)
            {
                return BadRequest(validationMessages);
            }

            return NoContent();
        }

        [HttpPut("watchlist/items/watchnext")]
        public async Task<IActionResult> UpdateWatchNext([FromBody] UpdateWatchNextRequest updateWatchNextRequest)
        {
            bool notFound = false;
            IDictionary<string, string> validationMessages = null;

            await _eventService.Subscribe<WatchNextItemUpdateFailedEvent>((e) =>
            {
                notFound = e.NoWatchNextItemFound;
                validationMessages = e.ValidationMessages;
            });

            await _eventService.QueueCommand(new UpdateWatchNextItemCommand
            {
                UserId = _currentUserAccessor.CurrentUser.Id,
                IsWatchedValue = updateWatchNextRequest.IsWatched
            });

            if (notFound)
            {
                return NotFound();
            }

            if (validationMessages != null)
            {
                return BadRequest(validationMessages);
            }

            return NoContent();
        }
    }
}
