using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Sannel.House.Web.Feature.Sprinklers.Components;
public partial class EditZone
{

	[Parameter]
	public Sannel.House.Sprinklers.Shared.Dtos.Zones.ZoneInfoDto ZoneInfo { get; set; } = new House.Sprinklers.Shared.Dtos.Zones.ZoneInfoDto();

	[Inject]
	public required DialogService DialogService { get; set; }

	private void Save()
	{
		DialogService.CloseSide(ZoneInfo);
	}

	private void Close()
	{
		DialogService.CloseSide();
	}
}
