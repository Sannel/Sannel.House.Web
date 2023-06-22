using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Sannel.House.Web.Foundation.Core.ViewModels;

namespace Sannel.House.Web.Foundation.Core.Components;
public partial class BusyArea
{
	[Parameter]
	public required RenderFragment ChildContent { get; set; }

	[Parameter]
	public required BaseViewModel ViewModel { get; set; }

	protected override Task OnInitializedAsync()
	{
		ViewModel.StateChanged += () => StateHasChanged();
		return Task.CompletedTask;
	}
}
