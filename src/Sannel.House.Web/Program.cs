using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sannel.House;
using Sannel.House.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

{
	using var http = new HttpClient()
	{
		BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
	};

	using var response = await http.GetAsync(Path.Join("app_config", "appsettings.json"));
	using var stream = await response.Content.ReadAsStreamAsync();

	builder.Configuration.AddJsonStream(stream);
}

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ApiAuthorizationMessageHandler>();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("WebApi")
	.AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
{
	var config = sp.GetRequiredService<IConfiguration>();
	var factory = sp.GetRequiredService<IHttpClientFactory>();
	var client = new Sannel.House.Clients.SprinklersClient(config["ClientUrls:Sprinklers"], factory.CreateClient("WebApi"));

	return client;
});

builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
{
	builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
	options.UserOptions.RoleClaim = "role";
	options.ProviderOptions.DefaultAccessTokenScopes.Add("api://sannel.house/access");
}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount,
		CustomAccountFactory>();
builder.Services.AddAuthorizationCore(o =>
{
	o.AddPolicy(AuthPolicies.Sprinklers.ALL, p =>
		p.RequireRole(
			Roles.Sprinklers.SCHEDULE_READ,
			Roles.Sprinklers.SCHEDULE_WRITE,
			Roles.Sprinklers.ZONE_READ,
			Roles.Sprinklers.ZONE_WRITE,
			Roles.ADMIN
		)
	);
	o.AddPolicy(AuthPolicies.Sprinklers.SCHEDULE_READ, p =>
		p.RequireRole(Roles.Sprinklers.SCHEDULE_READ,
			Roles.Sprinklers.SCHEDULE_WRITE,
			Roles.ADMIN
		)
	);
	o.AddPolicy(AuthPolicies.Sprinklers.SCHEDULE_WRITE, p =>
		p.RequireRole(Roles.Sprinklers.SCHEDULE_WRITE,
			Roles.ADMIN
		)
	);
});


await builder.Build().RunAsync();
