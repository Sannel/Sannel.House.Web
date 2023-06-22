namespace Sannel.House.Web.Foundation.Core.ViewModels;

public class BusyHelper : IDisposable
{
	public event Action? OnDisposed;
	public void Dispose()
	{
		OnDisposed?.Invoke();
	}
}
