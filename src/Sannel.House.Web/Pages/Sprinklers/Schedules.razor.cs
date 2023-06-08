using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Sannel.House.Clients;

namespace Sannel.House.Web.Pages.Sprinklers;

[Authorize(AuthPolicies.Sprinklers.SCHEDULE_READ)]
public partial class Schedules
{
	[Inject]
	public required SprinklersClient Client { get; set; }

	[Inject]
	public required ILogger<Schedules> Logger { get; set; }

	protected ICollection<ScheduleProgramResponse>? ScheduleList { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await GetSchedulesAsync();
	}

	protected async Task GetSchedulesAsync()
	{
		try
		{
			ScheduleList = await Client.V1_Schedule_GetSchedulesAsync();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Unabe to get data");
		}
	}

	protected async Task ToggleScheduleAsync(Guid id, bool value)
	{
		try
		{
			await Client.V1_Schedule_UpdateScheduleStatusAsync(id, value);
			await GetSchedulesAsync();
			StateHasChanged();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error updating value");
		}
	}
}
