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
    public class UpdateFilmWatchedCommandValidator : AbstractValidator<UpdateFilmWatchedCommand>
    {
        private readonly IFilmReader _filmReader;

        public UpdateFilmWatchedCommandValidator(IFilmReader filmReader)
        {
            _filmReader = filmReader;

            RuleFor(x => x.ItemId).MustAsync(FilmExists)
                .NotFoundRule();

            RuleFor(x => x.Watched).Must(x => x == true)
                .WithMessage("You cannot unwatch a film.");

        }

        private async Task<bool> FilmExists(long filmId, CancellationToken cancellationToken)
        {
            return (await _filmReader.GetFilmById(filmId)) != null;
        }
    }
}
