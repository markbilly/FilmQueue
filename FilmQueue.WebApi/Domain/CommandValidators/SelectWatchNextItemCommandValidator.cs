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
    public class SelectWatchNextItemCommandValidator : AbstractValidator<SelectWatchNextItemCommand>
    {
        private readonly IFilmReader _watchlistItemReader;

        public SelectWatchNextItemCommandValidator(IFilmReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;

            When(x => !x.ItemId.HasValue, () =>
            {
                RuleFor(x => x.SelectRandomItem).Must(x => x == true)
                    .WithMessage("Must request a randomised item for watch next when not providing an item by ID.");

                RuleFor(x => x.UserId).NotEmpty()
                    .WithMessage("Must specify the user ID when requesting a randomised watch next item.");
            });

            When(x => x.ItemId.HasValue, () =>
            {
                RuleFor(x => x.SelectRandomItem).Must(x => x == false)
                    .WithMessage("Cannot request a randomised item when a specific item ID is provided.");
            });

            RuleFor(x => x.UserId).MustAsync(UserDoesNotHaveWatchNextItem)
                .WithMessage("User already has a watch next item. Mark it as watched or skipped first.");

            RuleFor(x => x.UserId).MustAsync(UserHasUnwatchedItems)
                .WithMessage("User has no unwatched items. Unable to select a new watch next item.");
        }

        private async Task<bool> UserDoesNotHaveWatchNextItem(string userId, CancellationToken cancellationToken)
        {
            return (await _watchlistItemReader.GetCurrentWatchNextItem(userId)) == null;
        }

        private async Task<bool> UserHasUnwatchedItems(string userId, CancellationToken cancellationToken)
        {
            var unwatchedItemCount = await _watchlistItemReader.GetUnwatchedFilmCount(userId);
            return unwatchedItemCount > 0;
        }
    }
}
