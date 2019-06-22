using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface IEventSubscription
    {
        void ExecuteEventHandler(IEvent @event);
    }

    public class EventSubscription<TEvent> : IEventSubscription where TEvent : class, IEvent
    {
        private readonly Action<TEvent> _eventHandler;

        public EventSubscription(Action<TEvent> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public void ExecuteEventHandler(IEvent @event)
        {
            _eventHandler.Invoke(@event as TEvent);
        }
    }
}
