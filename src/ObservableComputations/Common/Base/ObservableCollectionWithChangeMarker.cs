// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObservableComputations
{
	public class ObservableCollectionWithTickTackVersion<TItem> : ObservableCollection<TItem>, IHasTickTackVersion
	{
		internal bool TickTackVersion;

		public ObservableCollectionWithTickTackVersion(List<TItem> list) : base(list)
		{

		}

		public ObservableCollectionWithTickTackVersion(IEnumerable<TItem> collection) : base(collection)
		{

		}

		public ObservableCollectionWithTickTackVersion()
		{
		}

		bool IHasTickTackVersion.TickTackVersion
		{
			get => TickTackVersion;
		}
	}
}
