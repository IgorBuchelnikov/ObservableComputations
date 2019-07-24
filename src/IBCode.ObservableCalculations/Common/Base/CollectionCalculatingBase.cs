using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using IBCode.ObservableCalculations.Common.Base;

namespace IBCode.ObservableCalculations.Common
{
	public class CollectionCalculatingBase<TItem> : ObservableCollectionWithChangeMarker<TItem>
	{
		public string DebugTag;

		public CollectionCalculatingBase(int capacity = 0) : base(new List<TItem>(capacity))
		{
		}

		public bool RaisePostCollectionChanged;
		public event NotifyCollectionChangedEventHandler PostCollectionChanged;

		private Action<int, TItem> _insertItemAction;
		public Action<int, TItem> InsertItemAction
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _insertItemAction;
			set
			{
				if (_insertItemAction != value)
				{
					_insertItemAction = value;
					OnPropertyChanged(Utils.InsertItemActionPropertyChangedEventArgs);
				}

			}
		}

		private Action<int> _removeItemAction;
		public Action<int> RemoveItemAction
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _removeItemAction;
			set
			{
				if (_removeItemAction != value)
				{
					_removeItemAction = value;
					OnPropertyChanged(Utils.RemoveItemActionPropertyChangedEventArgs);
				}
			}
		}

		private Action<int, TItem> _setItemAction;
		// ReSharper disable once MemberCanBePrivate.Global
		public Action<int, TItem> SetItemAction
		{
			get => _setItemAction;
			set
			{
				if (_setItemAction != value)
				{
					_setItemAction = value;
					OnPropertyChanged(Utils.SetItemActionPropertyChangedEventArgs);
				}
			}
		}

		private Action<int, int> _moveItemAction;
		// ReSharper disable once MemberCanBePrivate.Global
		public Action<int, int> MoveItemAction
		{
			get => _moveItemAction;
			set
			{
				if (_moveItemAction != value)
				{
					_moveItemAction = value;
					OnPropertyChanged(Utils.MoveItemActionPropertyChangedEventArgs);
				}
			}
		}

		private Action _clearItemsAction;

		// ReSharper disable once MemberCanBePrivate.Global
		public Action ClearItemsAction
		{
			get => _clearItemsAction;
			set
			{
				if (_clearItemsAction != value)
				{
					_clearItemsAction = value;
					OnPropertyChanged(Utils.ClearItemsActionPropertyChangedEventArgs);
				}
			}
		}

		#region Overrides of ObservableCollection<TResult>
		protected override void InsertItem(int index, TItem item)
		{
			InsertItemAction(index, item);
		}

		protected override void MoveItem(int oldIndex, int newIndex)
		{
			MoveItemAction(oldIndex, newIndex);
		}

		protected override void RemoveItem(int index)
		{
			RemoveItemAction(index);
		}

		protected override void SetItem(int index, TItem item)
		{
			SetItemAction(index, item);
		}

		protected override void ClearItems()
		{
			ClearItemsAction();
		}
		#endregion


		private void raisePostCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
		{
			PostCollectionChanged?.Invoke(this, eventArgs);
		}

		
	    private void raisePostCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
	    {
			raisePostCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
	    }

		
	    private void raisePostCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
	    {
			raisePostCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
	    }

		
	    private void raisePostCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
	    {
			raisePostCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
	    }

		
	    private void raisePostCollectionChangedReset()
	    {
			raisePostCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	    }

		
		protected internal void baseInsertItem(int index, TItem item)
		{
			ChangeMarker = !ChangeMarker;
			base.InsertItem(index, item);
			if (RaisePostCollectionChanged)
				raisePostCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
		}

		
		protected internal void baseMoveItem(int oldIndex, int newIndex)
		{
			ChangeMarker = !ChangeMarker;
			TItem item = this[oldIndex];
			base.MoveItem(oldIndex, newIndex);
			if (RaisePostCollectionChanged)
				raisePostCollectionChanged(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
		}

		
		protected internal void baseRemoveItem(int index)
		{
			ChangeMarker = !ChangeMarker;
			TItem item = this[index];
			base.RemoveItem(index);
			if (RaisePostCollectionChanged)
				raisePostCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
		}

		
		protected internal void baseSetItem(int index, TItem item)
		{
			ChangeMarker = !ChangeMarker;
			TItem oldItem = this[index];
			base.SetItem(index, item);
			if (RaisePostCollectionChanged)
				raisePostCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
		}

		
		protected internal void baseClearItems()
		{
			ChangeMarker = !ChangeMarker;
			base.ClearItems();
			if (RaisePostCollectionChanged)
				raisePostCollectionChangedReset();
		}
	}

}