using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations
{
	public class CollectionDisposing<TSourceItem> : CollectionProcessingVoid<TSourceItem>
		where TSourceItem :IDisposable
	{
		public CollectionDisposing(IReadScalar<INotifyCollectionChanged> sourceScalar) : base(sourceScalar, null, getOldItemsProcessor(), null)
		{
		}

		public CollectionDisposing(INotifyCollectionChanged source) : base(source, null, getOldItemsProcessor(), null)
		{
		}

		private static Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> getOldItemsProcessor()
		{
			return (oldItems, _) =>
			{
				int oldItemsLength = oldItems.Length;
				for (var index = 0; index < oldItemsLength; index++)
				{
					oldItems[index].Dispose();
				}
			};
		}
	}
}
