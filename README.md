# VKMiniAppsAuthentication

## Usage

Specify app secret key in appsettings.*.json

    ...
    "VkMiniAppSecret": "<app secret>"
    ...


Add authentication middleware in Startup class (assumed that there are parameters in "X-VkParameters" header)

    app.UseVkMiniAppsAuthentication(configuration.GetVkMiniAppSecret());
    
    
Pull parameters from HttpContext.Items dictionary

    var userId = (string) HttpContext.Items["vk_user_id"];
    var platform = (string) HttpContext.Items["vk_platform"];
    
    
## Other

VK Mini Apps doc: https://vk.com/dev/vk_apps_docs
