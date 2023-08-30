using Microsoft.AspNetCore.Authorization;
using WebApiClientCore;

namespace ServiceEngineMasaCore.Blazor.Extensions
{
    public static class ApiRequestContextExtensions
    {
        public static bool IsAllowAnonymous(this ApiRequestContext context)
        {
            return context
                .ActionDescriptor
                .Member
                .GetCustomAttributes<AllowAnonymousAttribute>()
                .Any();
        }
    }
}
