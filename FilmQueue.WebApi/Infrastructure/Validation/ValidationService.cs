using Autofac;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Validation
{
    public interface IValidationService : IDependency
    {
        Task<ValidationResult> Validate<TModel>(TModel instance) where TModel : class;
        Task Validate<TModel>(ValidationContext<TModel> validationContext) where TModel : class;
    }

    public class ValidationService : IValidationService
    {
        private readonly Dictionary<Type, IEnumerable<object>> _validationRulesByModelType;
        private readonly Dictionary<Type, object> _validatorsByModelType;
        private readonly ILifetimeScope _container;

        public ValidationService(ILifetimeScope container)
        {
            _validationRulesByModelType = new Dictionary<Type, IEnumerable<object>>();
            _validatorsByModelType = new Dictionary<Type, object>();
            _container = container;
        }

        public async Task<ValidationResult> Validate<TModel>(TModel instance) where TModel : class
        {
            var modelType = typeof(TModel);
            var validatorType = typeof(FluentValidation.IValidator<>).MakeGenericType(modelType);

            if (!_validatorsByModelType.TryGetValue(modelType, out object validator))
            {
                validator = _container.Resolve(validatorType);

                _validatorsByModelType.Add(modelType, validator);
            }

            // Note: this needs to be so specific because the "ValidateAsync" method on "AbstractValidator" is overloaded
            var validationAsyncMethodInfo = validatorType.GetMethod("ValidateAsync", new[] { modelType, typeof(CancellationToken) });

            return await (Task<ValidationResult>)validationAsyncMethodInfo.Invoke(validator, new object[] { instance, default(CancellationToken) });
        }

        public async Task Validate<TModel>(ValidationContext<TModel> validationContext) where TModel : class
        {
            var modelType = typeof(TModel);
            var ruleType = typeof(IValidator<>).MakeGenericType(modelType);

            if (!_validationRulesByModelType.TryGetValue(modelType, out IEnumerable<object> rules))
            {
                rules = (IEnumerable<object>)_container.Resolve(typeof(IEnumerable<>).MakeGenericType(ruleType));

                _validationRulesByModelType.Add(modelType, rules);
            }

            foreach (var rule in rules)
            {
                await (Task)ruleType.GetMethod("Validate").Invoke(rule, new object[] { validationContext });
            }
        }
    }
}
