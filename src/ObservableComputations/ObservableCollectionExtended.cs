// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public class ObservableCollectionExtended<TItem> : ObservableCollectionWithTickTackVersion<TItem>, INotifyCollectionChangedExtended, IHasItemType
	{
		public event EventHandler PreCollectionChanged;
		public event EventHandler PostCollectionChanged;
		NotifyCollectionChangedAction? _currentChange;
		TItem _newItem;
		int _oldIndex = -1;
		int _newIndex = -1;

		public NotifyCollectionChangedAction? CurrentChange => _currentChange;
		public TItem NewItem => _newItem;
		public object NewItemObject => _newItem;
		public int OldIndex => _oldIndex;
		public int NewIndex => _newIndex;

		public ObservableCollectionExtended() : base()
		{

		}

		public ObservableCollectionExtended(List<TItem> list) : base(list)
		{

		}

		public ObservableCollectionExtended(IEnumerable<TItem> collection) : base(collection)
		{

		}

		protected override void InsertItem(int index, TItem item)
		{
			TickTackVersion = !TickTackVersion;

			_currentChange = NotifyCollectionChangedAction.Add;
			_newIndex = index;
			_newItem = item;

			PreCollectionChanged?.Invoke(this, null);
			base.InsertItem(index, item);
			PostCollectionChanged?.Invoke(this, null);	

			_currentChange = null;
			_newIndex = -1;
			_newItem = default(TItem);
		}

		
		protected override void MoveItem(int oldIndex, int newIndex)
		{
			TickTackVersion = !TickTackVersion;

			_currentChange = NotifyCollectionChangedAction.Move;
			_oldIndex = oldIndex;
			_newIndex = newIndex;

			PreCollectionChanged?.Invoke(this, null);
			base.MoveItem(oldIndex, newIndex);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_oldIndex = -1;
			_newIndex = -1;
		}
	
		protected override void RemoveItem(int index)
		{
			TickTackVersion = !TickTackVersion;

			_currentChange = NotifyCollectionChangedAction.Remove;
			_oldIndex = index;

			PreCollectionChanged?.Invoke(this, null);
			base.RemoveItem(index);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_oldIndex = -1;
		}

		protected override void SetItem(int index, TItem item)
		{
			TickTackVersion = !TickTackVersion;
			
			_currentChange = NotifyCollectionChangedAction.Replace;
			_newItem = item;
			_newIndex = index;
			_oldIndex = index;

			PreCollectionChanged?.Invoke(this, null);
			base.SetItem(index, item);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_newItem = default;
			_newIndex = -1;
			_oldIndex = -1;
		}

		protected override void ClearItems()
		{
			TickTackVersion = !TickTackVersion;

			_currentChange = NotifyCollectionChangedAction.Reset;

			PreCollectionChanged?.Invoke(this, null);
			base.ClearItems();
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
		}

		public Type ItemType => typeof(TItem);
	}
}
