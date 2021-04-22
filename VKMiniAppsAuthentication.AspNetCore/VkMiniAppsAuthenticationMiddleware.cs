using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VKMiniAppsAuthentication.AspNetCore
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class VkMiniAppsAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _appSecret;

        public VkMiniAppsAuthenticationMiddleware(RequestDelegate next, string appSecret)
        {
            _next = next;
            _appSecret = appSecret;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue("X-VkParameters", out var parameters))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("no authentication header");
                return;
            }

            var authenticator = new Authenticator(_appSecret);
            var authenticationResult = authenticator.Check(parameters, out var parsedParameters);

            if (!authenticationResult)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("authentication failed");
                return;
            }

            foreach (string? key in parsedParameters)
            {
                if (key == null) continue;
                httpContext.Items.Add(key, parsedParameters[key]);
            }

            await _next.Invoke(httpContext);
        }
    }
}