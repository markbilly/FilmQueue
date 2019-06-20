using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class UpdateWatchlistItemCommandHandler : ICommandHandler<UpdateWatchlistItemCommand>
    {
        private readonly IWatchlistItemReader _watchlistItemReader;
        private readonly IValidationService _validationService;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public UpdateWatchlistItemCommandHandler(
            IWatchlistItemReader watchlistItemReader,
            IValidationService validationService,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _watchlistItemReader = watchlistItemReader;
            _validationService = validationService;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(UpdateWatchlistItemCommand command)
        {
            var validationContext = new ValidationContext<UpdateWatchlistItemCommand>(command);
            await _validationService.Validate(validationContext);

            if (!validationContext.IsValid)
            {
                await _eventService.RaiseEvent(new WatchlistItemUpdateFailedEvent
                {
                    ItemNotFound = validationContext.ValidationMessages.ContainsKey("notfound"),
                    ValidationMessages = validationContext.ValidationMessages
                });

                return;
            }

            var item = await _watchlistItemReader.GetItemById(command.ItemId);

            _unitOfWork.Execute(() =>
            {
                item.Title = command.Title;
                item.RuntimeInMinutes = command.RuntimeInMinutes;

                // TODO: Audit the changes
            });

            await _eventService.RaiseEvent(new WatchlistItemUpdatedEvent
            {
                Item = new WatchlistItem
                {
                    Id = item.Id,
                    Title = item.Title,
                    RuntimeInMinutes = item.RuntimeInMinutes,
                    Watched = item.WatchedDateTime.HasValue
                }
            });
        }
    }
}
