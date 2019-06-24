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
    public class UpdateFilmWatchedCommandHandler : ICommandHandler<UpdateFilmWatchedCommand>
    {
        private readonly IValidator<UpdateFilmWatchedCommand> _validator;
        private readonly IFilmWriter _filmWriter;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public UpdateFilmWatchedCommandHandler(
            IValidator<UpdateFilmWatchedCommand> validator,
            IFilmWriter filmWriter,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _validator = validator;
            _filmWriter = filmWriter;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateFilmWatchedCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent(validationResult));
                return;
            }

            _unitOfWork.Execute(() =>
            {
                _filmWriter.MarkFilmAsWatched(command.ItemId);
            });

            await _eventService.RaiseEvent(new FilmUpdatedToWatchedEvent
            {
                FilmId = command.ItemId
            });
        }
    }
}
