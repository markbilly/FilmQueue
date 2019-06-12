using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.Infrastructure
{
    public interface ICurrentUserAccessor : IDependency
    {
        CurrentUser CurrentUser { get; }
    }

    public class DefaultCurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUser CurrentUser => new CurrentUser(_httpContextAccessor.HttpContext.User);
    }

    public class CurrentUser : IIdentity
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        internal CurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }

        public string Id => _claimsPrincipal.FindFirst("sub").Value;
        public string AuthenticationType => _claimsPrincipal.Identity.AuthenticationType;
        public bool IsAuthenticated => _claimsPrincipal.Identity.IsAuthenticated;
        public string Name => _claimsPrincipal.Identity.Name;
    }
}
