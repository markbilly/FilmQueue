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
    public class SelectNewWatchNextItemCommandHandler : ICommandHandler<SelectNewWatchNextItemCommand>
    {
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IWatchlistItemReader _watchlistItemReader;
        private readonly IValidationService _validationService;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public SelectNewWatchNextItemCommandHandler(
            IWatchlistItemWriter watchlistItemWriter,
            IWatchlistItemReader watchlistItemReader,
            IValidationService validationService,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _watchlistItemWriter = watchlistItemWriter;
            _watchlistItemReader = watchlistItemReader;
            _validationService = validationService;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(SelectNewWatchNextItemCommand command)
        {
            var validationContext = new ValidationContext<SelectNewWatchNextItemCommand>(command);
            await _validationService.Validate(validationContext);

            if (!validationContext.IsValid)
            {
                await _eventService.RaiseEvent(new NewWatchNextItemSelectionFailedEvent
                {
                    ValidationMessages = validationContext.ValidationMessages
                });

                return;
            }

            var randomUnwatchedItem = await _watchlistItemReader.GetRandomUnwatchedItem(command.UserId);

            _unitOfWork.Execute(() =>
            {
                _watchlistItemWriter.SetWatchNextStartDateToNow(randomUnwatchedItem.Id);
            });

            await _eventService.RaiseEvent(new NewWatchNextItemSelectedEvent
            {
                ItemId = randomUnwatchedItem.Id
            });
        }
    }
}
