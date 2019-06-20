using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Validation
{
    public class ValidationContext<TModel> where TModel : class
    {
        public ValidationContext(TModel target)
        {
            ValidationTarget = target;
        }

        public TModel ValidationTarget { get; set; }
        public IDictionary<string, string> ValidationMessages { get; set; } = new Dictionary<string, string>();

        public bool IsValid => !ValidationMessages.Any();
    }
}
