using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FluentValidation;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandValidators
{
    public class ExpireWatchNextSelectionCommandValidator : AbstractValidator<ExpireWatchNextSelectionCommand>
    {
        private readonly IWatchNextReader _watchNextReader;
        private readonly IFilmReader _filmReader;

        public ExpireWatchNextSelectionCommandValidator(
            IWatchNextReader watchNextReader,
            IFilmReader filmReader)
        {
            _watchNextReader = watchNextReader;
            _filmReader = filmReader;

            RuleFor(x => x.FilmId).MustAsync(FilmExists)
                .NotFoundRule();

            RuleFor(x => x.FilmId).MustAsync(IsActiveWatchNext)
                .WithMessage("Film is not the active watch next selection.");
        }

        private async Task<bool> FilmExists(long filmId, CancellationToken cancellationToken)
        {
            return (await _filmReader.GetFilmById(filmId)) != null;
        }

        private Task<bool> IsActiveWatchNext(long filmId, CancellationToken cancellationToken)
        {
            return _watchNextReader.IsFilmWatchNextSelection(filmId);
        }
    }
}
