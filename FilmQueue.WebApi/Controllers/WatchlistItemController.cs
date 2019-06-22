﻿using FilmQueue.WebApi.DataAccess;
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
using FilmQueue.WebApi.Infrastructure.FluentValidation;

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
        public async Task<IActionResult> UpdateWatchlistItem([FromRoute] long id, [FromBody] UpdateWatchlistItemRequest request)
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

            await _eventService.Subscribe<WatchlistItemUpdatedEvent>((updatedEvent) =>
            {
                var response = WatchlistItemResponse.FromDomainModel(updatedEvent.Item);
                result = Ok(response);
            });

            await _eventService.IssueCommand(new UpdateWatchlistItemCommand
            {
                ItemId = id,
                Title = request.Title,
                RuntimeInMinutes = request.RuntimeInMinutes
            });

            return result;
        }

        [HttpPut("{id}/watched")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SetItemToWatched([FromBody] UpdateWatchlistItemWatchedRequest request)
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

            await _eventService.IssueCommand(new UpdateWatchlistItemWatchedCommand
            {
                ItemId = request.ItemId,
                Watched = request.Watched
            });

            return result ?? Ok(request.Watched);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WatchlistItemResponse), 201)]
        public async Task<IActionResult> CreateWatchlistItem([FromBody] CreateToWatchlistItemRequest createWatchlistItemRequest)
        {
            IActionResult result = null;

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                failedEvent.ValidationResult.AddToModelState(ModelState, null);
                result = BadRequest(ModelState);
            });

            await _eventService.Subscribe<WatchlistItemCreatedEvent>((createdEvent) =>
            {
                result = CreatedAtAction(
                    nameof(GetWatchlistItem),
                    new { id = createdEvent.ItemId },
                    WatchlistItemResponse.FromDomainModel(createdEvent.Item));
            });

            await _eventService.IssueCommand(new CreateWatchlistItemCommand
            {
                Title = createWatchlistItemRequest.Title,
                RuntimeInMinutes = createWatchlistItemRequest.RuntimeInMinutes,
                UserId = _currentUserAccessor.CurrentUser.Id
            });

            return result;
        }
    }
}
