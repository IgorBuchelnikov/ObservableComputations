using System.ComponentModel;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface IConsistent : INotifyPropertyChanged
	{
		bool Consistent {get;}
	}
}
