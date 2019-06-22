using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandHandlers
{
    public class SelectWatchNextItemCommandHandler : ICommandHandler<SelectWatchNextItemCommand>
    {
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IWatchlistItemReader _watchlistItemReader;
        private readonly IValidator<SelectWatchNextItemCommand> _validator;
        private readonly IEventService _eventService;
        private readonly IMemoryCache _memoryCache;
        private readonly FilmQueueDbUnitOfWork _unitOfWork;

        public SelectWatchNextItemCommandHandler(
            IWatchlistItemWriter watchlistItemWriter,
            IWatchlistItemReader watchlistItemReader,
            IValidator<SelectWatchNextItemCommand> validator,
            IEventService eventService,
            IMemoryCache memoryCache,
            FilmQueueDbUnitOfWork unitOfWork)
        {
            _watchlistItemWriter = watchlistItemWriter;
            _watchlistItemReader = watchlistItemReader;
            _validator = validator;
            _eventService = eventService;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SelectWatchNextItemCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsResourceNotFoundResult())
            {
                await _eventService.RaiseEvent(new ResourceNotFoundEvent());
                return;
            }

            if (!validationResult.IsValid)
            {
                await _eventService.RaiseEvent(new ValidationFailedEvent(validationResult));
                return;
            }

            var record = command.ItemId.HasValue
                ? await _watchlistItemReader.GetById(command.ItemId.Value)
                : await _watchlistItemReader.GetRandomUnwatchedItem(command.UserId);

            _unitOfWork.Execute(() =>
            {
                _watchlistItemWriter.SetWatchNextStartDateToNow(record.Id);
                // TODO: Audit change
            });

            _memoryCache.Remove(CacheKeys.WatchNext(command.UserId)); // TODO: Caching stuff should be in data access

            await _eventService.RaiseEvent(new WatchNextItemSelectedEvent
            {
                ItemId = record.Id,
                UserId = record.CreatedByUserId
            });
        }
    }
}
