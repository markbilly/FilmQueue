using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandValidators
{
    public class ExpireWatchNextItemCommandValidator : AbstractValidator<ExpireWatchNextItemCommand>
    {
        private readonly IWatchlistItemReader _watchlistItemReader;

        public ExpireWatchNextItemCommandValidator(IWatchlistItemReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;

            RuleFor(x => x.ItemId).MustAsync(WatchlistItemExists)
                .WithMessage("Watchlist item must exist");

            RuleFor(x => x.ItemId).MustAsync(IsActiveWatchNext)
                .WithMessage("Item is not an active watch next.");
        }

        private async Task<bool> WatchlistItemExists(long itemId, CancellationToken cancellationToken)
        {
            return (await _watchlistItemReader.GetById(itemId)) != null;
        }

        private async Task<bool> IsActiveWatchNext(long itemId, CancellationToken cancellationToken)
        {
            var record = await _watchlistItemReader.GetById(itemId);
            return record.WatchNextStart.HasValue && !record.WatchNextEnd.HasValue;
        }
    }
}
