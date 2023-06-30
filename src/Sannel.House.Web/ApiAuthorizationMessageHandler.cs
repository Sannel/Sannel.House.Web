using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Sannel.House.Web;

public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
	public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, IConfiguration config) : base(provider, navigation)
	{

		var section = config.GetSection("ClientUrls");
		var configUrls = section.Get<Dictionary<string, string>>() ?? throw new Exception("ClientUrls is not configured correctly");
		var urls = configUrls.Values.Distinct().ToArray();

		ConfigureHandler(urls);
	}
}
