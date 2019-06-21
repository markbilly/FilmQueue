using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Domain.Validators;
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
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public CreateWatchlistItemCommandHandler(
            IWatchlistItemWriter watchlistItemWriter,
            IValidationService validationService,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _watchlistItemWriter = watchlistItemWriter;
            _validationService = validationService;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(CreateWatchlistItemCommand command)
        {
            var validationResult = await _validationService.Validate(command);
            
            if (!validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent<CreateWatchlistItemCommand>(validationResult));
                return;
            }

            var record = await _unitOfWork.Execute(async () =>
            {
                return await _watchlistItemWriter.Create(command.UserId, command.Title, command.RuntimeInMinutes);
            });

            await _eventService.RaiseEvent(new WatchlistItemCreatedEvent
            {
                ItemId = record.Id,
                Item = WatchlistItem.FromRecord(record)
            });
        }
    }
}
