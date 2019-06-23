﻿using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.EventHandlers
{
    public class WatchlistItemWatchedEventHandler : IEventHandler<WatchlistItemWatchedEvent>
    {
        private readonly IFilmReader _watchlistItemReader;
        private readonly IEventService _eventService;

        public WatchlistItemWatchedEventHandler(
            IFilmReader watchlistItemReader,
            IEventService eventService)
        {
            _watchlistItemReader = watchlistItemReader;
            _eventService = eventService;
        }

        public async Task Handle(WatchlistItemWatchedEvent @event)
        {
            var record = await _watchlistItemReader.GetFilmById(@event.ItemId);

            if (IsActiveWatchNext(record))
            {
                await _eventService.IssueCommand(new ExpireWatchNextItemCommand
                {
                    ItemId = @event.ItemId
                });
            }
        }

        private bool IsActiveWatchNext(FilmRecord record)
        {
            return record.WatchNextStart.HasValue && !record.WatchNextEnd.HasValue;
        }
    }
}
