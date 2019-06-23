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
    public class SelectWatchNextCommandValidator : AbstractValidator<SelectWatchNextCommand>
    {
        private readonly IWatchNextReader _watchNextReader;
        private readonly IFilmReader _filmReader;

        public SelectWatchNextCommandValidator(
            IWatchNextReader watchNextReader,
            IFilmReader filmReader)
        {
            _watchNextReader = watchNextReader;
            _filmReader = filmReader;

            When(x => !x.FilmId.HasValue, () =>
            {
                RuleFor(x => x.SelectRandomFilm).Must(x => x == true)
                    .WithMessage("Must request a randomised film for watch next when not providing a film by ID.");

                RuleFor(x => x.UserId).NotEmpty()
                    .WithMessage("Must specify the user ID when requesting a randomised film for watch next.");
            });

            When(x => x.FilmId.HasValue, () =>
            {
                RuleFor(x => x.SelectRandomFilm).Must(x => x == false)
                    .WithMessage("Cannot request a randomised film when a specific film ID is provided.");
            });

            RuleFor(x => x.UserId).MustAsync(UserDoesNotHaveWatchNextSelection)
                .WithMessage("User already has a watch next selection. Mark it as watched or skipped first.");

            RuleFor(x => x.UserId).MustAsync(UserHasUnwatchedFilms)
                .WithMessage("User has no unwatched films. Unable to select a new watch next.");
        }

        private async Task<bool> UserDoesNotHaveWatchNextSelection(string userId, CancellationToken cancellationToken)
        {
            return (await _watchNextReader.GetActiveWatchNext(userId)) == null;
        }

        private async Task<bool> UserHasUnwatchedFilms(string userId, CancellationToken cancellationToken)
        {
            var unwatchedFilmCount = await _filmReader.GetUnwatchedFilmCount(userId);
            return unwatchedFilmCount > 0;
        }
    }
}
