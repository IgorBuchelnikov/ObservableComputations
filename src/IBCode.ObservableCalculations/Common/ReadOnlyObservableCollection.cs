using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IBCode.ObservableCalculations.Common
{
	public class ReadOnlyObservableCollection<TItem> : ReadOnlyCollection<TItem>, INotifyCollectionChanged
	{
		public ReadOnlyObservableCollection(IList<TItem> list) : base(list)
		{
		}

		public ReadOnlyObservableCollection(TItem item) : base(wrapItemInArray(item))
		{
		}

		private static TItem[] wrapItemInArray(TItem item)
		{
			return new []{item};
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
