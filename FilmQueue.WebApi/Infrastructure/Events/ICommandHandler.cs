using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface ICommandHandler<T> : IDependency where T : ICommand
    {
        Task Execute(T command);
    }
}
