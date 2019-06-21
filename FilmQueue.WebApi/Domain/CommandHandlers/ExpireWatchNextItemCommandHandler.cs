using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Infrastructure.Events;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class ExpireWatchNextItemCommandHandler : ICommandHandler<ExpireWatchNextItemCommand>
    {
        private readonly IValidator<ExpireWatchNextItemCommand> _validator;
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public ExpireWatchNextItemCommandHandler(
            IValidator<ExpireWatchNextItemCommand> validator,
            IWatchlistItemWriter watchlistItemWriter,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _validator = validator;
            _watchlistItemWriter = watchlistItemWriter;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ExpireWatchNextItemCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent(validationResult));
                return;
            }

            _unitOfWork.Execute(() =>
            {
                _watchlistItemWriter.SetWatchNextEndDateToNow(command.ItemId);
            });

            await _eventService.RaiseEvent(new WatchNextItemExpiredEvent
            {
                ItemId = command.ItemId
            });
        }
    }
}
