using Microsoft.AspNetCore.Components;
using Sannel.House.Clients;
namespace Sannel.House.Web.Pages.Sprinklers;

public partial class Index
{
	[Inject]
	public required SprinklersClient Client { get; set; }

	public List<ZoneInfoDto> Zones { get; } = new();

	protected override async Task OnInitializedAsync()
	{
		var items = await Client.V1_Zone_GetAllZoneMetaDataAsync();
		Zones.Clear();
		Zones.AddRange(items);
		StateHasChanged();
	}
}
