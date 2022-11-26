using Api.Extensions;
using System.Net;

namespace Api.Middleware;

/// <summary>
/// Disables to use refresh token like a access token
/// </summary>
public class AuthenticationMiddlewareForRefreshToken
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddlewareForRefreshToken(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string authHeader = context.Request.Headers["Authorization"];

        if (authHeader != null && context.Request.Path.Value?.ToLower() != "/authentication/refresh" && context.User.IsRefreshToken())
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            await _next(context);
        }
    }
}
