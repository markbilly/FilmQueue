using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class UpdateFilmCommandHandler : ICommandHandler<UpdateFilmCommand>
    {
        private readonly IFilmReader _filmReader;
        private readonly IValidator<UpdateFilmCommand> _validator;
        private readonly IEventService _eventService;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public UpdateFilmCommandHandler(
            IFilmReader filmReader,
            IValidator<UpdateFilmCommand> validator,
            IEventService eventService,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _filmReader = filmReader;
            _validator = validator;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateFilmCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsResourceNotFoundResult())
            {
                await _eventService.RaiseEvent(new ResourceNotFoundEvent(command.FilmId));
                return;
            }

            if (!validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent(validationResult));
                return;
            }

            var record = await _filmReader.GetFilmById(command.FilmId);

            _unitOfWork.Execute(() =>
            {
                record.Title = command.Title;
                record.RuntimeInMinutes = command.RuntimeInMinutes;

                // TODO: Audit the changes
            });

            await _eventService.RaiseEvent(new FilmUpdatedEvent
            {
                Film = Film.FromRecord(record)
            });
        }
    }
}
