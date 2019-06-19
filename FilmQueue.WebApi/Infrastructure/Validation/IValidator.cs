using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Validation
{
    public interface IValidator<TModel> where TModel : class
    {
        Task Validate(ValidationContext<TModel> validationContext);
    }
}
