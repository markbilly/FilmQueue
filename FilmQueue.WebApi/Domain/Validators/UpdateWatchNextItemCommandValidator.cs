using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Validators
{
    public class UpdateWatchNextItemCommandValidator : IValidator<UpdateWatchNextItemCommand>
    {
        private readonly IWatchlistReader _watchlistReader;

        public UpdateWatchNextItemCommandValidator(
            IWatchlistReader watchlistReader)
        {
            _watchlistReader = watchlistReader;
        }

        public async Task Validate(ValidationContext<UpdateWatchNextItemCommand> validationContext)
        {
            var current = await _watchlistReader.GetCurrentWatchNextItem(validationContext.ValidationTarget.UserId);
            if (current == null)
            {
                validationContext.ValidationMessages.Add("notfound", "User does not currently have a watch next item.");
                return;
            }

            if (validationContext.ValidationTarget.IsWatchedValue != true)
            {
                validationContext.ValidationMessages.Add("state", "Disallowed action. You may only set watch next item to 'watched'");
                return;
            }
        }
    }
}
