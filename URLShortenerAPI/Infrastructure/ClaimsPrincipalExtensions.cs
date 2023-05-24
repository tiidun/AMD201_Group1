using System.Security.Claims;

namespace URLShortenerAPI.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string UserId(this ClaimsPrincipal user)
             => user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
