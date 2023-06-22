using System.Runtime.CompilerServices;
using Sannel.House.Clients;
using Sannel.House.Web.Foundation.Core.ViewModels;

namespace Sannel.House.Web.ViewModels.Sprinklers;

public class IndexViewModel : BaseViewModel
{
	private readonly SprinklersClient _client;

	public IndexViewModel(SprinklersClient client)
	{
		ArgumentNullException.ThrowIfNull(client);
		_client = client;
	}

	public List<ZoneInfoDto> Zones { get; } = new();

	public async Task LoadAsync()
	{
		using var helper = StartBusy();
		var items = await _client.V1_Zone_GetAllZoneMetaDataAsync();
		Zones.Clear();
		Zones.AddRange(items);
	}
}
