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
    public class UpdateWatchNextItemCommandHandler : ICommandHandler<UpdateWatchNextItemCommand>
    {
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IValidationService _validationService;
        private readonly IWatchlistReader _watchlistReader;
        private readonly IEventService _eventService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWatchNextItemCommandHandler(
            IWatchlistItemWriter watchlistItemWriter,
            IValidationService validationService,
            IWatchlistReader watchlistReader,
            IEventService eventService,
            IUnitOfWork unitOfWork)
        {
            _watchlistItemWriter = watchlistItemWriter;
            _validationService = validationService;
            _watchlistReader = watchlistReader;
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

            var current = await _watchlistReader.GetCurrentWatchNextItem(command.UserId);

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
