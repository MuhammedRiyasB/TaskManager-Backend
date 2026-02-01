using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
              ?? user.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(id))
            throw new UnauthorizedAccessException("UserId missing in token");

        return int.Parse(id);
    }
}
