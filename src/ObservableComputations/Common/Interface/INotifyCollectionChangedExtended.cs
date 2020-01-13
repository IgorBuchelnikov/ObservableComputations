using System;
using System.Collections.Specialized;

namespace ObservableComputations.Interface
{
	public interface INotifyCollectionChangedExtended : INotifyCollectionChanged
	{
		event EventHandler PreCollectionChanged;
		event EventHandler PostCollectionChanged;

		NotifyCollectionChangedAction? CurrentChange {get;}
		object NewItemObject {get;}
		int OldIndex {get;}
		int NewIndex {get;}
	}
}
