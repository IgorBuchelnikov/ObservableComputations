using ObservableComputations;

namespace ObservableComputations
{
	public interface IComputing : IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
	}
}
