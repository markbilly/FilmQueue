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
    [Route("users/me/watchnext")]
    public class WatchNextController : ApiController
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IWatchNextReader _watchNextReader;
        private readonly IEventService _eventService;

        public WatchNextController(
            ICurrentUserAccessor currentUserAccessor,
            IWatchNextReader watchNextReader,
            IEventService eventService)
        {
            _currentUserAccessor = currentUserAccessor;
            _watchNextReader = watchNextReader;
            _eventService = eventService;
        }

        /// <summary>
        /// Get your watch next
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(FilmResponse), 200)]
        public async Task<IActionResult> GetWatchNext()
        {
            var record = await _watchNextReader.GetActiveWatchNext(_currentUserAccessor.CurrentUser.Id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(FilmResponse.FromRecord(record));
        }

        /// <summary>
        /// Skip your watch next
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> ExpireWatchNext()
        {
            var record = await _watchNextReader.GetActiveWatchNext(_currentUserAccessor.CurrentUser.Id);

            if (record == null)
            {
                return NotFound();
            }

            IActionResult result = null;

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                result = FromFailedValidationResult(failedEvent);
            });

            await _eventService.Subscribe<WatchNextSelectionExpiredEvent>((successEvent) =>
            {
                result = NoContent();
            });

            await _eventService.IssueCommand(new ExpireWatchNextSelectionCommand
            {
                FilmId = record.Id,
                Reason = WatchNextExpiryReason.Skipped
            });

            return result;
        }

        /// <summary>
        /// Select your watch next
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(FilmResponse), 201)]
        public async Task<IActionResult> SelectWatchNext([FromBody] SelectWatchNextRequest request)
        {
            IActionResult result = null;

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                result = FromFailedValidationResult(failedEvent);
            });

            await _eventService.Subscribe<WatchNextSelectedEvent>((successEvent) =>
            {
                result = CreatedAtAction(
                    nameof(GetWatchNext), 
                    new { id = successEvent.Film.Id }, 
                    FilmResponse.FromDomainModel(successEvent.Film));
            });

            await _eventService.IssueCommand(new SelectWatchNextCommand
            {
                UserId = _currentUserAccessor.CurrentUser.Id,
                FilmId = request.FilmId,
                SelectRandomFilm = request.SelectRandomFilm
            });

            return result;
        }
    }
}
