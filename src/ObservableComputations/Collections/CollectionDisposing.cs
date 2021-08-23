// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public class CollectionDisposing<TSourceItem> : CollectionProcessingVoid<TSourceItem>
		where TSourceItem : IDisposable
	{
		public CollectionDisposing(IReadScalar<INotifyCollectionChanged> sourceScalar) : base(sourceScalar, null, getOldItemProcessor())
		{
		}

		public CollectionDisposing(INotifyCollectionChanged source) : base(source, null, getOldItemProcessor())
		{
		}

		private static Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> getOldItemProcessor() => 
			(oldItem, _) => oldItem.Dispose();
	}
}
