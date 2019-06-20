using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Validation
{
    public interface IValidationService : IDependency
    {
        Task Validate<TModel>(ValidationContext<TModel> validationContext) where TModel : class;
    }

    public class ValidationService : IValidationService
    {
        private readonly Dictionary<Type, IEnumerable<object>> _validationRulesByModelType;
        private readonly ILifetimeScope _container;

        public ValidationService(ILifetimeScope container)
        {
            _validationRulesByModelType = new Dictionary<Type, IEnumerable<object>>();
            _container = container;
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
