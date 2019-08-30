using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCode.ObservableCalculations.Common.Interface
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
