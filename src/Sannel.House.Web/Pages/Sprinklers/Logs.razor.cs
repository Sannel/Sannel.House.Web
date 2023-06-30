using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using Sannel.House.Sprinklers.Shared;

namespace Sannel.House.Web.Pages.Sprinklers;

[Authorize(AuthPolicies.Sprinklers.ZONE_READ)]
public partial class Logs
{
	[Inject]
	public required SprinklersClient Client { get; set; }

	public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(6);
	public TimeSpan EndTime { get; set; } = TimeSpan.FromHours(16);

	public ConcurrentBag<AppointmentData> Runs = new();

	[NotNull]
	public required RadzenScheduler<AppointmentData> scheduler;

	public required RadzenDayView DayView { get; set; }

	private TimeSpan ToAdd(string runLength)
	{
		return TimeSpan.TryParse(runLength, out var value) ? value : TimeSpan.Zero;
	}

	protected override async Task OnInitializedAsync()
	{
		var now = DateTimeOffset.Now;
		var lastMonth = now.AddMonths(-1);

		await LoadRangeAsync(lastMonth, now.AddDays(1));
		now = lastMonth;
		lastMonth = now.AddMonths(-1);
		await LoadRangeAsync(lastMonth, now);
		now = lastMonth;
		lastMonth = now.AddMonths(-1);
		await LoadRangeAsync(lastMonth, now);


	}

	protected async Task LoadRangeAsync(DateTimeOffset start, DateTimeOffset end)
	{
		/*var runs = await Client.V1_Log_GetRunsForRangeAsync(start, end);
		foreach (var r in runs)
		{
			AddZoneRun(r);
		}

		CalculateTimeRange();*/
		await scheduler.Reload();
	}

	protected void CalculateTimeRange()
	{
		var min = Runs.Select(i => TimeOnly.FromDateTime(i.Start)).Min();
		var max = Runs.Select(i => TimeOnly.FromDateTime(i.End)).Max();


		StartTime = min.ToTimeSpan();
		EndTime = max.ToTimeSpan();
	}

	/*protected void AddZoneRun(ZoneRunDto zoneRun)
	{
		var start = zoneRun.ActionDate.AddSeconds(30).LocalDateTime;
		var end = zoneRun.ActionDate.Add(ToAdd(zoneRun.RunLength)).AddSeconds(-30).LocalDateTime;

		var d = new AppointmentData()
		{
			Text = $@"{zoneRun.StationName} ({zoneRun.ActionDate})[{zoneRun.RunLength}]",
			Data = zoneRun,
			Start = start,
			End = end
		};
		
		Runs.Add(d);
	}*/
	private void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<AppointmentData> args)
	{
		/*if(args?.Data?.Data is ZoneRunDto dto)
		{
			args.Attributes["style"] = $"background-color: {dto.StationColor};";
		}*/
	}
}
