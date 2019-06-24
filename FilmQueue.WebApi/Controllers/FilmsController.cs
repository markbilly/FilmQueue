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
using FilmQueue.WebApi.Infrastructure.FluentValidation;

namespace FilmQueue.WebApi.Controllers
{
    [Authorize]
    [Route("users/me/films")]
    public class FilmsController : ApiController
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IFilmReader _filmReader;
        private readonly IEventService _eventService;

        public FilmsController(
            ICurrentUserAccessor currentUserAccessor,
            IFilmReader filmReader,
            IEventService eventService)
        {
            _currentUserAccessor = currentUserAccessor;
            _filmReader = filmReader;
            _eventService = eventService;
        }

        /// <summary>
        /// Get a film you have saved
        /// </summary>
        /// <param name="id">ID for the film</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FilmResponse), 200)]
        public async Task<IActionResult> GetFilm([FromRoute] long id)
        {
            var record = await _filmReader.GetFilmById(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(FilmResponse.FromRecord(record));
        }

        /// <summary>
        /// Edit a film you have saved
        /// </summary>
        /// <param name="id">ID for the film</param>
        /// <param name="request">Updated properties for the film</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FilmResponse), 200)]
        public async Task<IActionResult> UpdateFilm([FromRoute] long id, [FromBody] UpdateFilmRequest request)
        {
            IActionResult result = null;

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                result = FromFailedValidationResult(failedEvent);
            });

            await _eventService.Subscribe<FilmUpdatedEvent>((updatedEvent) =>
            {
                var response = FilmResponse.FromDomainModel(updatedEvent.Film);
                result = Ok(response);
            });

            await _eventService.IssueCommand(new UpdateFilmCommand
            {
                FilmId = id,
                Title = request.Title,
                RuntimeInMinutes = request.RuntimeInMinutes
            });

            return result;
        }

        /// <summary>
        /// Update a film to indicate you have watched it
        /// </summary>
        /// <param name="id">ID for the film</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}/watched")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SetFilmToWatched([FromRoute] long id, [FromBody] UpdateFilmWatchedRequest request)
        {
            IActionResult result = null;

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                result = FromFailedValidationResult(failedEvent);
            });

            await _eventService.IssueCommand(new UpdateFilmWatchedCommand
            {
                ItemId = id,
                Watched = request.Watched
            });

            return result ?? Ok(request.Watched);
        }

        /// <summary>
        /// Save a film you want to watch
        /// </summary>
        /// <param name="request">Film properties</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(FilmResponse), 201)]
        public async Task<IActionResult> CreateFilm([FromBody] CreateFilmRequest request)
        {
            IActionResult result = null;

            await _eventService.Subscribe<ValidationFailedEvent>((failedEvent) =>
            {
                result = FromFailedValidationResult(failedEvent);
            });

            await _eventService.Subscribe<FilmCreatedEvent>((createdEvent) =>
            {
                result = CreatedAtAction(
                    nameof(GetFilm),
                    new { id = createdEvent.FilmId },
                    FilmResponse.FromDomainModel(createdEvent.Film));
            });

            await _eventService.IssueCommand(new CreateFilmCommand
            {
                Title = request.Title,
                RuntimeInMinutes = request.RuntimeInMinutes,
                UserId = _currentUserAccessor.CurrentUser.Id
            });

            return result;
        }
    }
}
