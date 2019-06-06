using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.Infrastructure.Swagger
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasAuthorizeAttribute = context.ApiDescription.ControllerAttributes().Concat(context.ApiDescription.ActionAttributes())
                .OfType<AuthorizeAttribute>()
                .Any();

            if (hasAuthorizeAttribute)
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "oauth2", new[] { "api" } }
                    }
                };
            }
        }
    }
}
