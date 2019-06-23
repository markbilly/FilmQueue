using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public class ImmediateExecutionEventService : IEventService
    {
        private readonly IDictionary<Type, object> _commandHandlersByCommandType;
        private readonly IDictionary<Type, IEnumerable<object>> _eventHandlersByEventType;
        private readonly IDictionary<Type, IList<IEventSubscription>> _subscriptionsByEventType;
        private readonly ILifetimeScope _container;

        private readonly object _lock = new object();

        public ImmediateExecutionEventService(
            ILifetimeScope container)
        {
            _commandHandlersByCommandType = new Dictionary<Type, object>();
            _eventHandlersByEventType = new Dictionary<Type, IEnumerable<object>>();
            _subscriptionsByEventType = new Dictionary<Type, IList<IEventSubscription>>();
            _container = container;
        }

        public async Task IssueCommand<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandType = typeof(TCommand);
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            if (!_commandHandlersByCommandType.TryGetValue(commandType, out object handler))
            {
                handler = _container.Resolve(handlerType);

                _commandHandlersByCommandType.Add(commandType, handler);
            }

            await (Task)handlerType.GetMethod("Handle").Invoke(handler, new object[] { command });
        }

        public async Task RaiseEvent<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            if (!_eventHandlersByEventType.TryGetValue(eventType, out IEnumerable<object> handlers))
            {
                handlers = (IEnumerable<object>)_container.Resolve(typeof(IEnumerable<>).MakeGenericType(handlerType));

                _eventHandlersByEventType.Add(eventType, handlers);
            }

            foreach (var handler in handlers)
            {
                await (Task)handlerType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }

            IList<IEventSubscription> subscriptions;
            lock (_lock)
            {
                if (!_subscriptionsByEventType.TryGetValue(typeof(TEvent), out subscriptions))
                {
                    return;
                }
            }

            foreach (var subscription in subscriptions)
            {
                subscription.ExecuteEventHandler(@event);
            }
        }

        public Task Subscribe<TEvent>(Action<TEvent> eventHandler) where TEvent : class, IEvent
        {
            var eventType = typeof(TEvent);

            lock (_lock)
            {
                if (!_subscriptionsByEventType.ContainsKey(eventType))
                {
                    _subscriptionsByEventType.Add(eventType, new List<IEventSubscription>());
                }

                _subscriptionsByEventType[eventType].Add(new EventSubscription<TEvent>(eventHandler));
            }

            return Task.CompletedTask;
        }
    }
}
