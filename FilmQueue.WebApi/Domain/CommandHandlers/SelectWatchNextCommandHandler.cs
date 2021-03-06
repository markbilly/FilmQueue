﻿using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class SelectWatchNextCommandHandler : ICommandHandler<SelectWatchNextCommand>
    {
        private readonly IWatchNextWriter _watchNextWriter;
        private readonly IFilmReader _filmReader;
        private readonly IValidator<SelectWatchNextCommand> _validator;
        private readonly IEventService _eventService;
        private readonly IWatchNextReader _watchNextReader;

        public SelectWatchNextCommandHandler(
            IWatchNextWriter watchNextWriter,
            IFilmReader filmReader,
            IValidator<SelectWatchNextCommand> validator,
            IEventService eventService,
            IWatchNextReader watchNextReader)
        {
            _watchNextWriter = watchNextWriter;
            _filmReader = filmReader;
            _validator = validator;
            _eventService = eventService;
            _watchNextReader = watchNextReader;
        }

        public async Task Handle(SelectWatchNextCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent(validationResult));
                return;
            }

            FilmRecord filmRecord;
            if (command.FilmId.HasValue)
            {
                filmRecord = await _filmReader.GetFilmById(command.FilmId.Value);
            }
            else
            {
                var mostRecentWatchNext = await _watchNextReader.GetMostRecentWatchNext(command.UserId);
                filmRecord = await _filmReader.GetRandomUnwatchedFilm(command.UserId, mostRecentWatchNext?.Id);
            }

            await _watchNextWriter.MakeSelection(filmRecord.Id, filmRecord.OwnedByUserId);

            await _eventService.RaiseEvent(new WatchNextSelectedEvent
            {
                Film = Film.FromRecord(filmRecord)
            });
        }
    }
}
