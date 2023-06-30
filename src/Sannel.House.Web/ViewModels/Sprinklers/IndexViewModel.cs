using System.Runtime.CompilerServices;
using Radzen;
using Sannel.House.Sprinklers.Shared;
using Sannel.House.Sprinklers.Shared.Dtos.Zones;
using Sannel.House.Web.Feature.Sprinklers.Components;
using Sannel.House.Web.Foundation.Core.ViewModels;

namespace Sannel.House.Web.ViewModels.Sprinklers;

public class IndexViewModel : BaseViewModel
{
	private readonly SprinklersClient _client;
	private readonly DialogService _dialogService;

	public IndexViewModel(SprinklersClient client,
		DialogService dialogService)
	{
		ArgumentNullException.ThrowIfNull(client);
		ArgumentNullException.ThrowIfNull(dialogService);
		_client = client;
		_dialogService = dialogService;
	}

	public List<ZoneInfoDto> Zones { get; } = new();

	public bool IsSaving { get; set; }

	public async Task LoadAsync()
	{
		using var helper = StartBusy();
		var result = await _client.V1.GetAllZoneMetaDataAsync();
		Zones.Clear();
		Zones.AddRange(result.Value!);
	}

	public async void OpenEditZone(byte zoneId)
	{
		var zone = Zones.FirstOrDefault(i => i.ZoneId == zoneId);
		if (zone is not null)
		{
			var result = await _dialogService.OpenSideAsync<EditZone>($"Edit Zone {zone.ZoneId}",
				new Dictionary<string, object>()
				{
					{nameof(EditZone.ZoneInfo), zone }
				},
				new SideDialogOptions()
				{
					ShowMask = true,
					Position = DialogPosition.Right,
					ShowClose = false
				});
			if(result is ZoneInfoDto zoneInfo)
			{
				IsSaving = true;
				FireStateChanged();
				var r = await _client.V1.UpdateZoneMetaDataAsync(zoneInfo);
				IsSaving = false;
				FireStateChanged();
			}
		}
	}
}
