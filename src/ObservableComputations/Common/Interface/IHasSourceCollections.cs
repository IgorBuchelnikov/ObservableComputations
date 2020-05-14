using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface IHasSourceCollections
	{
		ReadOnlyCollection<INotifyCollectionChanged> SourceCollections {get;}
		ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars {get;}
	}
}
