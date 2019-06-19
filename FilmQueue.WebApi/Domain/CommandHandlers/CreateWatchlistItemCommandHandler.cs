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
    public class CreateWatchlistItemCommandHandler : ICommandHandler<CreateWatchlistItemCommand>
    {
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IEventService _eventService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWatchlistItemCommandHandler(
            IWatchlistItemWriter watchlistItemWriter,
            IEventService eventService,
            IUnitOfWork unitOfWork)
        {
            _watchlistItemWriter = watchlistItemWriter;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(CreateWatchlistItemCommand command)
        {
            // TODO: check that the user exists

            if (string.IsNullOrWhiteSpace(command.Title))
            {
                await _eventService.RaiseEvent(new WatchlistItemCreationFailedEvent
                {
                    ValidationMessages = new Dictionary<string, string>
                    {
                        { "title", "Title is a required field" }
                    }
                });

                return;
            }

            var item = await _unitOfWork.Execute(async () =>
            {
                return await _watchlistItemWriter.Create(command.UserId, command.Title, command.RuntimeInMinutes);
            });

            await _eventService.RaiseEvent(new WatchlistItemCreatedEvent
            {
                ItemId = item.Id,
                Item = new WatchlistItem(item.Title, item.RuntimeInMinutes)
            });
        }
    }
}
