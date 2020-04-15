using System.Collections.Generic;
using System.Collections.ObjectModel;
using ObservableComputations;

namespace ObservableComputations
{
	public class ObservableCollectionWithChangeMarker<TItem> : ObservableCollection<TItem>, IHasChangeMarker
	{
		internal bool ChangeMarkerField;

		public ObservableCollectionWithChangeMarker(List<TItem> list) : base(list)
		{

		}


		protected ObservableCollectionWithChangeMarker()
		{
		}

		bool IHasChangeMarker.ChangeMarker
		{
			get => ChangeMarkerField;
			set => ChangeMarkerField = value;
		}
	}
}
