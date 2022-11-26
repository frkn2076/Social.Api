using System.Security.Claims;
using System.Security.Principal;

namespace Api.Extensions;

public static class Extension
{
    public static T GetOptions<T>(this IConfiguration configuration) where T : new()
    {
        var t = new T();
        configuration.GetSection(typeof(T).Name).Bind(t);
        return t;
    }

    public static bool IsRefreshToken(this IPrincipal claimsPrincipal)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        var isRefreshToken = identity?.FindFirst(ClaimTypes.AuthorizationDecision)?.Value;
        return isRefreshToken == "RefreshToken";
    }
}
