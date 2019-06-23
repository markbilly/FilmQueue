using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class UpdateWatchlistItemWatchedCommandHandler : ICommandHandler<UpdateWatchlistItemWatchedCommand>
    {
        private readonly IValidator<UpdateWatchlistItemWatchedCommand> _validator;
        private readonly IFilmWriter _watchlistItemWriter;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public UpdateWatchlistItemWatchedCommandHandler(
            IValidator<UpdateWatchlistItemWatchedCommand> validator,
            IFilmWriter watchlistItemWriter,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _validator = validator;
            _watchlistItemWriter = watchlistItemWriter;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateWatchlistItemWatchedCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsResourceNotFoundResult())
            {
                await _eventService.RaiseEvent(new ResourceNotFoundEvent(command.ItemId));
                return;
            }

            if (!validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent(validationResult));
                return;
            }

            _unitOfWork.Execute(() =>
            {
                _watchlistItemWriter.MarkFilmAsWatched(command.ItemId);
            });

            await _eventService.RaiseEvent(new WatchlistItemWatchedEvent
            {
                ItemId = command.ItemId
            });
        }
    }
}
