using Api.Utils.Constants;
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
        var idClaim = _claimsIdentity.FindFirst(ClaimNames.Id)?.Value;
        int.TryParse(idClaim, out int id);
        return id;
    }

    public string? GetUserName()
    {
        return _claimsIdentity.FindFirst(ClaimNames.UserName)?.Value;
    }

    public string? GetEmail()
    {
        return _claimsIdentity.FindFirst(ClaimNames.Email)?.Value;
    }
}
