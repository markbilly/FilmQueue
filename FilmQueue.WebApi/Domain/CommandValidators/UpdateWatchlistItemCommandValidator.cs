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
    public class UpdateWatchlistItemCommandValidator : AbstractValidator<UpdateWatchlistItemCommand>
    {
        private readonly IFilmReader _watchlistItemReader;

        public UpdateWatchlistItemCommandValidator(IFilmReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;

            RuleFor(x => x.ItemId).MustAsync(WatchlistItemExists)
                .ResourceNotFoundRule();

            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Title is a required field.");

            RuleFor(x => x.Title).MaximumLength(200)
                .WithMessage("Title cannot be longer than 200 characters");

            RuleFor(x => x.RuntimeInMinutes).GreaterThan(0)
                .WithMessage("Runtime is not valid. Must be positive whole number.");
        }

        private async Task<bool> WatchlistItemExists(long itemId, CancellationToken cancellationToken)
        {
            return (await _watchlistItemReader.GetFilmById(itemId)) != null;
        }
    }
}
