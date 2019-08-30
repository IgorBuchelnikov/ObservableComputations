using System.ComponentModel;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface ICalculating : INotifyPropertyChanged, IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
	}
}
