using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCode.ObservableCalculations.Common.Base
{
	public class ObservableCollectionWithChangeMarker<TItem> : ObservableCollection<TItem>
	{
		public bool ChangeMarker;

		public ObservableCollectionWithChangeMarker(List<TItem> list) : base(list)
		{

		}

		protected ObservableCollectionWithChangeMarker()
		{
		}
	}
}
