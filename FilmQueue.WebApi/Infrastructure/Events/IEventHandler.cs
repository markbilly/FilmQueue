using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface IEventHandler<TEvent> : IDependency
    {
        Task Handle(TEvent @event);
    }
}
