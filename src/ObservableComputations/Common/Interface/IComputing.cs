using System.ComponentModel;

namespace ObservableComputations.Common.Interface
{
	public interface IComputing : IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
	}
}
