using System.ComponentModel;

namespace ObservableComputations.Interface
{
	public interface IComputing : IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
	}
}
