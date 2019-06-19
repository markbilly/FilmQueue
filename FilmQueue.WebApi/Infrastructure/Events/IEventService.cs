using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface IEventService : IDependency
    {
        Task RaiseEvent<T>(T eventToRaise) where T : class, IEvent;
        Task QueueCommand<T>(T command) where T : class, ICommand;
        Task Subscribe<T>(Action<T> eventHandler) where T : class, IEvent;
    }
}
