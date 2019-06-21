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
    public class SelectNewWatchNextItemCommandValidator : AbstractValidator<SelectNewWatchNextItemCommand>
    {
        private readonly IWatchlistItemReader _watchlistItemReader;

        public SelectNewWatchNextItemCommandValidator(IWatchlistItemReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;

            RuleFor(x => x.UserId).MustAsync(UserDoesNotHaveWatchNextItem)
                .WithMessage("User already has a watch next item. Mark it as watched or skipped first.");

            RuleFor(x => x.UserId).MustAsync(UserHasUnwatchedItems)
                .WithMessage("You have no more unwatched items. Cannot select a new watch next item.");
        }

        private async Task<bool> UserDoesNotHaveWatchNextItem(string userId, CancellationToken cancellationToken)
        {
            return (await _watchlistItemReader.GetCurrentWatchNextItem(userId)) == null;
        }

        private async Task<bool> UserHasUnwatchedItems(string userId, CancellationToken cancellationToken)
        {
            var unwatchedItemCount = await _watchlistItemReader.GetUnwatchedItemCount(userId);
            return unwatchedItemCount > 0;
        }
    }
}
