using System.Collections.Generic;
using System.Collections.ObjectModel;
using ObservableComputations;

namespace ObservableComputations
{
	public class ObservableCollectionWithChangeMarker<TItem> : ObservableCollection<TItem>, IHasChangeMarker
	{
		public bool ChangeMarker;

		public ObservableCollectionWithChangeMarker(List<TItem> list) : base(list)
		{

		}

		protected ObservableCollectionWithChangeMarker()
		{
		}

		public bool GetChangeMarker()
		{
			return ChangeMarker;
		}
	}
}
