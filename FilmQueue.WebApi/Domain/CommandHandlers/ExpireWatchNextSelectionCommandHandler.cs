using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class ExpireWatchNextSelectionCommandHandler : ICommandHandler<ExpireWatchNextSelectionCommand>
    {
        private readonly IValidator<ExpireWatchNextSelectionCommand> _validator;
        private readonly IWatchNextWriter _watchNextWriter;
        private readonly IEventService _eventService;

        public ExpireWatchNextSelectionCommandHandler(
            IValidator<ExpireWatchNextSelectionCommand> validator,
            IWatchNextWriter watchNextWriter,
            IEventService eventService)
        {
            _validator = validator;
            _watchNextWriter = watchNextWriter;
            _eventService = eventService;
        }

        public async Task Handle(ExpireWatchNextSelectionCommand command)
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

            await _watchNextWriter.ExpireSelection(command.FilmId, command.Reason == WatchNextExpiryReason.Watched);

            await _eventService.RaiseEvent(new WatchNextSelectionExpiredEvent
            {
                FilmId = command.FilmId
            });
        }
    }
}
