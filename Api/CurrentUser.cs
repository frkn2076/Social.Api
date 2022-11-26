using Api.Enums;
using System.Security.Claims;

namespace Api;

public class CurrentUser
{
    private readonly ClaimsIdentity _claimsIdentity;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _claimsIdentity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
    }

    public int GetId()
    {
        var idClaim = _claimsIdentity.FindFirst(ClaimTypes.SerialNumber)?.Value;
        int.TryParse(idClaim, out int id);
        return id;
    }

    public string? GetUserName()
    {
        return _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public bool IsAdmin()
    {
        return _claimsIdentity.FindFirst(ClaimTypes.Role)?.Value == Roles.Admin;
    }
}
