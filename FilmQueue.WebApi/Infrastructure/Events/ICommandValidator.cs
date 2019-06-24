using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface ICommandValidator : IDependency
    {
        Task<bool> Validate<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
