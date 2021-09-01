using System.ComponentModel;

namespace ObservableComputations
{
	public interface ICanNotifyPropertyChanged : INotifyPropertyChanged
	{
		bool CanNotifyPropertyChanged(string propertyName, IComputing computing);
	}
}
