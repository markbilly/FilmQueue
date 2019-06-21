using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface IEventService : IDependency
    {
        Task RaiseEvent<TEvent>(TEvent eventToRaise) where TEvent : class, IEvent;
        Task IssueCommand<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task Subscribe<TEvent>(Action<TEvent> eventHandler) where TEvent : class, IEvent;
    }
}
