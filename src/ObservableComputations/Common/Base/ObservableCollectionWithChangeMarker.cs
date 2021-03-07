// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObservableComputations
{
	public class ObservableCollectionWithChangeMarker<TItem> : ObservableCollection<TItem>, IHasChangeMarker
	{
		internal bool ChangeMarkerField;

		public ObservableCollectionWithChangeMarker(List<TItem> list) : base(list)
		{

		}

		public ObservableCollectionWithChangeMarker(IEnumerable<TItem> collection) : base(collection)
		{

		}

		public ObservableCollectionWithChangeMarker()
		{
		}

		bool IHasChangeMarker.ChangeMarker
		{
			get => ChangeMarkerField;
		}
	}
}
