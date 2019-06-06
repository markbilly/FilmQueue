using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FoodDiary.IdentityServer
{
    public class Startup
    {
        private static readonly string MVC_URL = "https://localhost:44355";
        private static readonly string API_URL = "https://localhost:44335";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(new Client[]
                {
                    new Client
                    {
                        ClientId = "cli",
                        ClientName = "Food Diary CLI",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = new[] { new Secret("<secret_password>".Sha256()) },
                        AllowedScopes = new[] { "api.cli" }
                    },
                    new Client
                    {
                        ClientId = "api_swagger",
                        ClientName = "Food Diary Web API - Swagger UI",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowAccessTokensViaBrowser = true,
                        RedirectUris = new[] { API_URL + "/swagger/oauth2-redirect.html" },
                        AllowedScopes = { "api.all" }
                    },
                    new Client
                    {
                        ClientId = "web",
                        ClientName = "Food Diary Web App",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowedScopes = new[]
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email
                        },
                        RedirectUris = new[] { MVC_URL + "/signin-oidc" },
                        PostLogoutRedirectUris = new[] { MVC_URL }
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
                        DisplayName = "Food Diary API",
                        Description = "Food Diary API Access",
                        ApiSecrets = new[] { new Secret("<scope_secret>".Sha256()) },
                        Scopes = new[]
                        {
                            new Scope("api.cli"),
                            new Scope("api.all")
                        }
                    }
                })
                .AddTestUsers(new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                        Username = "bob",
                        Password = "password",
                        Claims = new[]
                        {
                            new Claim(JwtClaimTypes.Email, "bob@example.org")
                        }
                    }
                })
                .AddDeveloperSigningCredential();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
