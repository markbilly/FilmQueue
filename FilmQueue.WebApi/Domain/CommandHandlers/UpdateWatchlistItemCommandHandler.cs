using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class UpdateWatchlistItemCommandHandler : ICommandHandler<UpdateWatchlistItemCommand>
    {
        private readonly IWatchlistItemReader _watchlistItemReader;
        private readonly IValidator<UpdateWatchlistItemCommand> _validator;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public UpdateWatchlistItemCommandHandler(
            IWatchlistItemReader watchlistItemReader,
            IValidator<UpdateWatchlistItemCommand> validator,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _watchlistItemReader = watchlistItemReader;
            _validator = validator;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(UpdateWatchlistItemCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent<UpdateWatchlistItemCommand>(validationResult));
                return;
            }

            var record = await _watchlistItemReader.GetById(command.ItemId);

            _unitOfWork.Execute(() =>
            {
                record.Title = command.Title;
                record.RuntimeInMinutes = command.RuntimeInMinutes;

                // TODO: Audit the changes
            });

            await _eventService.RaiseEvent(new WatchlistItemUpdatedEvent
            {
                Item = WatchlistItem.FromRecord(record)
            });
        }
    }
}
