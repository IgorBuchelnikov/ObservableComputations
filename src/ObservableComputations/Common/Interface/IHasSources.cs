using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface IHasSources
	{
		ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection {get;}
		ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection {get;}
	}
}
