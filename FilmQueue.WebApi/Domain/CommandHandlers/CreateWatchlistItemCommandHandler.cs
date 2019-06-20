using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class CreateWatchlistItemCommandHandler : ICommandHandler<CreateWatchlistItemCommand>
    {
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IValidationService _validationService;
        private readonly IEventService _eventService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWatchlistItemCommandHandler(
            IWatchlistItemWriter watchlistItemWriter,
            IValidationService validationService,
            IEventService eventService,
            IUnitOfWork unitOfWork)
        {
            _watchlistItemWriter = watchlistItemWriter;
            _validationService = validationService;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(CreateWatchlistItemCommand command)
        {
            var validationContext = new ValidationContext<CreateWatchlistItemCommand>(command);
            await _validationService.Validate(validationContext);

            if (!validationContext.IsValid)
            {
                await _eventService.RaiseEvent(new WatchlistItemCreationFailedEvent
                {
                    ValidationMessages = validationContext.ValidationMessages
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
