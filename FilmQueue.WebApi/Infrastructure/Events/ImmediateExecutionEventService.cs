using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public class ImmediateExecutionEventService : IEventService
    {
        private readonly Dictionary<Type, IEnumerable<object>> _commandHandlersByCommandType;
        private readonly Dictionary<Type, IList<IEventSubscription>> _subscriptionsByEventType;
        private readonly ILifetimeScope _container;

        private object _lock = new object();

        public ImmediateExecutionEventService(
            ILifetimeScope container)
        {
            _commandHandlersByCommandType = new Dictionary<Type, IEnumerable<object>>();
            _subscriptionsByEventType = new Dictionary<Type, IList<IEventSubscription>>();
            _container = container;
        }

        public async Task QueueCommand<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandType = typeof(TCommand);
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            if (!_commandHandlersByCommandType.TryGetValue(commandType, out IEnumerable<object> handlers))
            {
                handlers = (IEnumerable<object>)_container.Resolve(typeof(IEnumerable<>).MakeGenericType(handlerType));

                _commandHandlersByCommandType.Add(commandType, handlers);
            }

            foreach (var handler in handlers)
            {
                await (Task)handlerType.GetMethod("Execute").Invoke(handler, new object[] { command });
            }
        }

        public Task RaiseEvent<TEvent>(TEvent eventToRaise) where TEvent : class, IEvent
        {
            IList<IEventSubscription> subscriptions;
            lock (_lock)
            {
                if (!_subscriptionsByEventType.TryGetValue(typeof(TEvent), out subscriptions))
                {
                    return Task.CompletedTask;
                }
            }

            foreach (var subscription in subscriptions)
            {
                subscription.ExecuteEventHandler(eventToRaise);
            }

            return Task.CompletedTask;
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
