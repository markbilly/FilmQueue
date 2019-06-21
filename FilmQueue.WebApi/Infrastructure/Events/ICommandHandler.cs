using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface ICommandHandler<TCommand> : IDependency where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}
