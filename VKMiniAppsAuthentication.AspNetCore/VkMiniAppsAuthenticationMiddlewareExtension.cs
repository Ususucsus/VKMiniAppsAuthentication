using Microsoft.AspNetCore.Builder;

namespace VKMiniAppsAuthentication.AspNetCore
{
    public static class VkMiniAppsAuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseVkMiniAppsAuthentication(this IApplicationBuilder builder,
            string appSecret)
        {
            return builder.UseMiddleware<VkMiniAppsAuthenticationMiddleware>(appSecret);
        }
    }
}