using System.ComponentModel;

namespace IBCode.ObservableComputations.Common.Interface
{
	public interface IComputing : INotifyPropertyChanged, IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
	}
}
