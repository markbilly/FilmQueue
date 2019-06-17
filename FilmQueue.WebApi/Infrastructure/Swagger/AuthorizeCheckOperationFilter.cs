using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Swagger
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var hasAuthorizeAttribute = context.ApiDescription.ControllerAttributes().Concat(context.ApiDescription.ActionAttributes())
#pragma warning restore CS0618 // Type or member is obsolete
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
