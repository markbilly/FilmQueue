using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandValidators
{
    public class UpdateWatchlistItemWatchedCommandValidator : AbstractValidator<UpdateWatchlistItemWatchedCommand>
    {
        private readonly IFilmReader _watchlistItemReader;

        public UpdateWatchlistItemWatchedCommandValidator(IFilmReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;

            RuleFor(x => x.ItemId).MustAsync(WatchlistItemExists)
                .ResourceNotFoundRule();

            RuleFor(x => x.Watched).Must(x => x == true)
                .WithMessage("You cannot unwatch an item.");

        }

        // TODO: Abstract away to rule
        private async Task<bool> WatchlistItemExists(long itemId, CancellationToken cancellationToken)
        {
            return (await _watchlistItemReader.GetFilmById(itemId)) != null;
        }
    }
}
