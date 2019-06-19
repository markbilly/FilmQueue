using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public interface IEventSubscription
    {
        void ExecuteEventHandler(IEvent eventToHandle);
    }

    public class EventSubscription<T> : IEventSubscription where T : class, IEvent
    {
        private readonly Action<T> _eventHandler;

        public EventSubscription(Action<T> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public void ExecuteEventHandler(IEvent eventToHandle)
        {
            _eventHandler.Invoke(eventToHandle as T);
        }
    }
}
