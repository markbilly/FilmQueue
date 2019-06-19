using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class UpdateWatchNextItemCommandHandler : ICommandHandler<UpdateWatchNextItemCommand>
    {
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IWatchlistReader _watchlistReader;
        private readonly IEventService _eventService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWatchNextItemCommandHandler(
            IWatchlistItemWriter watchlistItemWriter,
            IWatchlistReader watchlistReader,
            IEventService eventService,
            IUnitOfWork unitOfWork)
        {
            _watchlistItemWriter = watchlistItemWriter;
            _watchlistReader = watchlistReader;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(UpdateWatchNextItemCommand command)
        {
            // USE VALIDATION SERVICE

            var current = await _watchlistReader.GetCurrentWatchNextItem(command.UserId);

            if (current == null)
            {
                await _eventService.RaiseEvent(new WatchNextItemUpdateFailedEvent
                {
                    NoWatchNextItemFound = true
                });

                return;
            }

            if (command.IsWatchedValue != true)
            {
                await _eventService.RaiseEvent(new WatchNextItemUpdateFailedEvent
                {
                    ItemId = current.Id,
                    ValidationMessages = new Dictionary<string, string>
                    {
                        { "state", "Disallowed action. You may only set watch next item to 'watched'" }
                    }
                });

                return;
            }

            _unitOfWork.Execute(() =>
            {
                _watchlistItemWriter.SetWatchedDateToNow(current.Id);
                _watchlistItemWriter.SetWatchNextEndDateToNow(current.Id);
            });

            await _eventService.RaiseEvent(new WatchNextItemUpdatedEvent
            {
                ItemId = current.Id
            });
        }
    }
}
