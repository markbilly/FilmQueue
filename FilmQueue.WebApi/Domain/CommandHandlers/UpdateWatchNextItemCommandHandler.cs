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
    public class UpdateWatchNextItemCommandHandler : ICommandHandler<UpdateWatchNextItemCommand>
    {
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IWatchlistItemReader _watchlistItemReader;
        private readonly IValidationService _validationService;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public UpdateWatchNextItemCommandHandler(
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

        public async Task Execute(UpdateWatchNextItemCommand command)
        {
            var validationContext = new ValidationContext<UpdateWatchNextItemCommand>(command);
            await _validationService.Validate(validationContext);

            if (!validationContext.IsValid)
            {
                await _eventService.RaiseEvent(new WatchNextItemUpdateFailedEvent
                {
                    NoWatchNextItemFound = validationContext.ValidationMessages.ContainsKey("notfound"),
                    ValidationMessages = validationContext.ValidationMessages
                });

                return;
            }

            var currentRecord = await _watchlistItemReader.GetCurrentWatchNextItem(command.UserId);

            _unitOfWork.Execute(() =>
            {
                _watchlistItemWriter.SetWatchedDateToNow(currentRecord.Id);
                _watchlistItemWriter.SetWatchNextEndDateToNow(currentRecord.Id);

                // TODO: Audit the changes
            });

            await _eventService.RaiseEvent(new WatchNextItemUpdatedEvent
            {
                Item = WatchlistItem.FromRecord(currentRecord)
            });
        }
    }
}
