using Microsoft.AspNetCore.Components;
using Sannel.House.Web.ViewModels.Sprinklers;

namespace Sannel.House.Web.Pages.Sprinklers;

public partial class Index
{
	[Inject]
	public required IndexViewModel ViewModel { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await ViewModel.LoadAsync();
	}
}
