using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface IHasSourceCollections
	{
		ReadOnlyCollection<INotifyCollectionChanged> Sources {get;}
		ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars {get;}
	}
}
