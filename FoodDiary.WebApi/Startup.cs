using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FoodDiary.WebApi.Infrastructure.Autofac;
using FoodDiary.WebApi.Infrastructure.EntityFramework;
using FoodDiary.WebApi.Infrastructure.Swagger;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace FoodDiary.WebApi
{
    public class Startup
    {
        private static readonly string IDSERVER_URL = "https://localhost:44312";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = IDSERVER_URL;
                    options.ApiName = "api";
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Food Diary Web API", Version = "v1" });
                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Flow = "implicit",
                    AuthorizationUrl = IDSERVER_URL + "/connect/authorize",
                    Scopes = new Dictionary<string, string>
                    {
                        { "api.all", "Food Diary Web API - full access" }
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddDbContext<FoodDiaryDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"]);
            });

            // AUTOFAC
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Food Diary Web API V1");
                options.DefaultModelsExpandDepth(-1);
                options.OAuthClientId("api_swagger");
                options.OAuthAppName("Food Diary Web API - Swagger");
            });

            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            };

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
