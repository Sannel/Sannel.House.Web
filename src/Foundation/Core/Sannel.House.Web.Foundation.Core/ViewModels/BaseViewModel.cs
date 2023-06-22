namespace Sannel.House.Web.Foundation.Core.ViewModels;

public abstract class BaseViewModel
{
	public event Action? StateChanged;

	public bool IsBusy { get; set; }

	public IDisposable StartBusy()
	{
		var helper = new BusyHelper();
		helper.OnDisposed += () =>
		{
			IsBusy = false;
			FireStateChanged();
		};
		IsBusy = true;
		FireStateChanged();
		return helper;
	}

	protected void FireStateChanged()
	{
		StateChanged?.Invoke();
	}
}
