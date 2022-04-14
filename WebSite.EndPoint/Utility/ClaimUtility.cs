using System.Security.Claims;

namespace WebSite.EndPoint.Utility
{
    public static class ClaimUtility
    {
        public static string GetUserId(ClaimsPrincipal claims)
        {
            var cliamsIdentity = claims.Identity as ClaimsIdentity;
            string userId = cliamsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId;  
        }
    }
}
