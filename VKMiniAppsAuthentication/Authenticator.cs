using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace VKMiniAppsAuthentication
{
    public sealed class Authenticator
    {
        private readonly string _appSecret;

        public Authenticator(string appSecret)
        {
            _appSecret = appSecret;
        }

        public bool Check(string parameters, out NameValueCollection parsedParameters)
        {
            parsedParameters = HttpUtility.ParseQueryString(parameters);

            var sign = parsedParameters.Get("sign") ?? string.Empty;
            var hash = GetHash(GetSortedVkParameters(parsedParameters), _appSecret);
            
            return hash == sign;
        }
        
        private static string GetSortedVkParameters(NameValueCollection parametersCollection)
        {
            var vkParameters = parametersCollection.AllKeys
                .Where(key => key != null && key.StartsWith("vk_"))
                .Select(key => $"{key}={Uri.EscapeDataString(parametersCollection[key] ?? string.Empty)}")
                .ToList();
            
            vkParameters.Sort();
            
            var sortedVkParameters = string.Join('&', vkParameters);

            return sortedVkParameters;
        }
        
        private static string GetHash(string sortedVkParameters, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var paramBytes = Encoding.UTF8.GetBytes(sortedVkParameters);
            
            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(paramBytes);
            var hash = Convert.ToBase64String(hashBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
            
            return hash;
        }
    }
}