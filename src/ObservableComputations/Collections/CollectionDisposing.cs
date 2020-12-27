using System;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public class CollectionDisposing<TSourceItem> : CollectionProcessingVoid<TSourceItem>
		where TSourceItem : IDisposable
	{
		public CollectionDisposing(IReadScalar<INotifyCollectionChanged> sourceScalar) : base(sourceScalar, null, getOldItemsProcessor())
		{
		}

		public CollectionDisposing(INotifyCollectionChanged source) : base(source, null, getOldItemsProcessor())
		{
		}

		private static Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> getOldItemsProcessor()
		{
			return (oldItems, _) =>
			{
				int oldItemsLength = oldItems.Length;
				for (int index = 0; index < oldItemsLength; index++)
				{
					oldItems[index].Dispose();
				}
			};
		}
	}
}
