// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class FreezedObservableCollection<TItem> : ReadOnlyCollection<TItem>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		public FreezedObservableCollection(IList<TItem> list) : base(list)
		{
		}

		public FreezedObservableCollection(TItem item) : base(wrapItemInArray(item))
		{
		}

		private static TItem[] wrapItemInArray(TItem item)
		{
			return new []{item};
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
