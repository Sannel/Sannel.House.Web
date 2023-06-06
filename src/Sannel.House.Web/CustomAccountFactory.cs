
using System.Security.Claims;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace Sannel.House.Web;
public class CustomAccountFactory
	: AccountClaimsPrincipalFactory<CustomUserAccount>
{
	private readonly IAccessTokenProviderAccessor _accessor;
	private readonly ILogger<CustomAccountFactory> _logger;
	private readonly IConfiguration _configuration;

	public CustomAccountFactory(IAccessTokenProviderAccessor accessor,
		IConfiguration configuration,
		ILogger<CustomAccountFactory> logger)
		: base(accessor)
	{
		ArgumentNullException.ThrowIfNull(accessor);
		ArgumentNullException.ThrowIfNull(configuration);
		ArgumentNullException.ThrowIfNull(logger);
		_accessor = accessor;
		_configuration = configuration;
		_logger = logger;
	}

	public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
		CustomUserAccount account,
		RemoteAuthenticationUserOptions options)
	{
		var initialUser = await base.CreateUserAsync(account, options);

		if (initialUser.Identity is not null &&
			initialUser.Identity.IsAuthenticated)
		{
			var userIdentity = initialUser.Identity as ClaimsIdentity;

			if (userIdentity is not null)
			{
				var list = account.AdditionalProperties.Select(i => $"{i.Key} = {i.Value}").ToList();
				account?.Roles?.ForEach((rid) =>
				{
					var name = _configuration[$"RolesMapping:{rid}"];
					userIdentity.AddClaim(new Claim(options.RoleClaim, name ?? rid));
				});

			}
		}

		return initialUser;
	}
}
