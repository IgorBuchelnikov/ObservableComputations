// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface INotifyCollectionChangedExtended : INotifyCollectionChanged
	{
		event EventHandler PreCollectionChanged;
		event EventHandler PostCollectionChanged;

		NotifyCollectionChangedAction? CurrentChange {get;}
		object NewItemObject {get;}
		int OldIndex {get;}
		int NewIndex {get;}
	}
}
