using System.ComponentModel;

namespace ObservableComputations
{
	public interface ICanNotifyMethodChanged : INotifyMethodChanged
	{
		bool CanNotifyMethodChanged(string methodName, int argumentsCount, IComputing computing);
	}
}
