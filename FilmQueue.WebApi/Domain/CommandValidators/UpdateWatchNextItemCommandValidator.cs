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
    public class UpdateWatchNextItemCommandValidator : AbstractValidator<UpdateWatchNextItemCommand>
    {
        private readonly IWatchlistItemReader _watchlistItemReader;

        public UpdateWatchNextItemCommandValidator(IWatchlistItemReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;

            RuleFor(x => x.UserId).MustAsync(UserHasWatchNextItem)
                .WithMessage("User does not currently have a watch next item.");

            RuleFor(x => x.IsWatchedValue).Must(x => x == true)
                .WithMessage("Disallowed action. You may only set watch next item to 'watched'");
        }

        private async Task<bool> UserHasWatchNextItem(string userId, CancellationToken cancellationToken)
        {
            return (await _watchlistItemReader.GetCurrentWatchNextItem(userId)) != null;
        }
    }
}
