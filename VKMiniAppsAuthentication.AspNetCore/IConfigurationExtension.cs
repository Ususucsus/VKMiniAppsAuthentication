using Microsoft.Extensions.Configuration;

namespace VKMiniAppsAuthentication.AspNetCore
{
    public static class ConfigurationExtension
    {
        public static string GetVkMiniAppSecret(this IConfiguration configuration)
        {
            return configuration.GetSection("VkMiniAppSecret").Value;
        }
    }
}