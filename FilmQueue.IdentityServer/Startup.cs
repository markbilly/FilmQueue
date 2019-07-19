using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace FilmQueue.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                });
            });

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = (builder) =>
                    {
                        builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                        });
                    };
                })
                .AddInMemoryClients(new Client[]
                {
                    new Client
                    {
                        ClientId = "cli",
                        ClientName = "Film Queue CLI",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = new[] { new Secret("<secret_password>".Sha256()) },
                        AllowedScopes = new[] { "api.cli" }
                    },
                    new Client
                    {
                        ClientId = "api_swagger",
                        ClientName = "Film Queue Web API - Swagger UI",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowAccessTokensViaBrowser = true,
                        RedirectUris = new[] { Configuration["Url.Api"] + "/swagger/oauth2-redirect.html" },
                        AllowedScopes = { "api.all" }
                    },
                    new Client
                    {
                        ClientId = "web",
                        ClientName = "Film Queue Web App (MVC)",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowedScopes = new[]
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "api.all"
                        },
                        RedirectUris = new[] { Configuration["Url.Mvc"] + "/signin-oidc" },
                        PostLogoutRedirectUris = new[] { Configuration["Url.Mvc"] }
                    },
                    new Client
                    {
                        ClientId = "ng-film-queue",
                        ClientName = "Film Queue Web App (Angular)",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowedScopes = new[]
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "api.all"
                        },
                        RedirectUris = new[] { Configuration["Url.Spa"] + "/auth-callback" },
                        PostLogoutRedirectUris = new[] { Configuration["Url.Spa"] },
                        AllowedCorsOrigins = new[] { Configuration["Url.Spa"] },
                        AllowAccessTokensViaBrowser = true,
                        AccessTokenLifetime = 3600
                    }
                })
                .AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email()
                })
                .AddInMemoryApiResources(new ApiResource[]
                {
                    new ApiResource
                    {
                        Name = "api",
                        DisplayName = "Film Queue API",
                        Description = "Film Queue API Access",
                        ApiSecrets = new[] { new Secret("<scope_secret>".Sha256()) },
                        Scopes = new[]
                        {
                            new Scope("api.cli"),
                            new Scope("api.all")
                        }
                    }
                })
                .AddAspNetIdentity<IdentityUser>()
                .AddDeveloperSigningCredential();

            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Film Queue Identity Web API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Film Queue Identity Web API V1");
                options.DefaultModelsExpandDepth(-1);
            });

            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            };

            app.UseMvcWithDefaultRoute();
        }
    }
}
