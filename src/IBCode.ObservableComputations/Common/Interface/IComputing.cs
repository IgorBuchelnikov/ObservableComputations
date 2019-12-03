using System.ComponentModel;

namespace ObservableComputations.Common.Interface
{
	public interface IComputing : INotifyPropertyChanged, IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
	}
}
