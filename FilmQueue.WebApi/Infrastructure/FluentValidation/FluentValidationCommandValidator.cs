using Autofac;
using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.FluentValidation
{
    public class FluentValidationCommandValidator : ICommandValidator
    {
        private readonly IDictionary<Type, object> _commandValidatorsByCommandType;
        private readonly IEventService _eventService;
        private readonly ILifetimeScope _container;

        public FluentValidationCommandValidator(
            IEventService eventService,
            ILifetimeScope container)
        {
            _commandValidatorsByCommandType = new Dictionary<Type, object>();
            _eventService = eventService;
            _container = container;
        }

        public async Task<bool> Validate<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandType = typeof(TCommand);
            var validatorType = typeof(IValidator<>).MakeGenericType(commandType);

            if (!_commandValidatorsByCommandType.TryGetValue(commandType, out object validator))
            {
                if (_container.TryResolve(validatorType, out validator))
                {
                    _commandValidatorsByCommandType.Add(commandType, validator);
                }
            }

            if (validator != null)
            {
                var validationResult = await(Task<ValidationResult>)validatorType.GetMethod("ValidateAsync").Invoke(validator, new object[] { command, default(CancellationToken) });
                if (!validationResult.IsValid)
                {
                    await _eventService.RaiseEvent(new ValidationFailedEvent(validationResult));
                    return false;
                }
            }

            return true;
        }
    }
}
