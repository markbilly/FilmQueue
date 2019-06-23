using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.EventHandlers
{
    public class FilmUpdatedToWatchedEventHandler : IEventHandler<FilmUpdatedToWatchedEvent>
    {
        private readonly IWatchNextReader _watchNextReader;
        private readonly IEventService _eventService;

        public FilmUpdatedToWatchedEventHandler(
            IWatchNextReader watchNextReader,
            IEventService eventService)
        {
            _watchNextReader = watchNextReader;
            _eventService = eventService;
        }

        public async Task Handle(FilmUpdatedToWatchedEvent @event)
        {
            if (await _watchNextReader.IsFilmWatchNextSelection(@event.FilmId))
            {
                await _eventService.IssueCommand(new ExpireWatchNextSelectionCommand
                {
                    FilmId = @event.FilmId,
                    Reason = WatchNextExpiryReason.Watched
                });
            }
        }
    }
}
