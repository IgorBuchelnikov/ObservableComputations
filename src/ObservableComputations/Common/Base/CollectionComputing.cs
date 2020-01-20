using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using ObservableComputations;

namespace ObservableComputations
{
	public abstract class CollectionComputing<TItem> : ObservableCollectionWithChangeMarker<TItem>, ICollectionComputing
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}

		public CollectionComputing(int capacity = 0) : base(new List<TItem>(capacity))
		{
			_initialCapacity = capacity;

			if (Configuration.SaveInstantiatingStackTrace)
			{
				_instantiatingStackTrace = Environment.StackTrace;
			}
		}

		public event EventHandler PreCollectionChanged;
		public event EventHandler PostCollectionChanged;

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
			bool trackComputingsExecutingUserCode = Configuration.TrackComputingsExecutingUserCode;
			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode[Thread.CurrentThread] = this;
			}

			_insertItemAction(index, item);

			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode.Remove(Thread.CurrentThread);
			}
		}

		protected override void MoveItem(int oldIndex, int newIndex)
		{
			bool trackComputingsExecutingUserCode = Configuration.TrackComputingsExecutingUserCode;
			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode[Thread.CurrentThread] = this;
			}

			_moveItemAction(oldIndex, newIndex);

			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode.Remove(Thread.CurrentThread);
			}
		}

		protected override void RemoveItem(int index)
		{
			bool trackComputingsExecutingUserCode = Configuration.TrackComputingsExecutingUserCode;
			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode[Thread.CurrentThread] = this;
			}

			_removeItemAction(index);

			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode.Remove(Thread.CurrentThread);
			}
		}

		protected override void SetItem(int index, TItem item)
		{
			bool trackComputingsExecutingUserCode = Configuration.TrackComputingsExecutingUserCode;
			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode[Thread.CurrentThread] = this;
			}

			_setItemAction(index, item);

			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode.Remove(Thread.CurrentThread);
			}
		}

		protected override void ClearItems()
		{
			bool trackComputingsExecutingUserCode = Configuration.TrackComputingsExecutingUserCode;
			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode[Thread.CurrentThread] = this;
			}

			_clearItemsAction();

			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode.Remove(Thread.CurrentThread);
			}
		}
		#endregion

		NotifyCollectionChangedAction? _currentChange;
		TItem _newItem;
		int _oldIndex = -1;
		int _newIndex = -1;

		public NotifyCollectionChangedAction? CurrentChange => _currentChange;
		public TItem NewItem => _newItem;
		public object NewItemObject => _newItem;
		public int OldIndex => _oldIndex;
		public int NewIndex => _newIndex;
	
		protected internal void baseInsertItem(int index, TItem item)
		{
			ChangeMarker = !ChangeMarker;

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

		
		protected internal void baseMoveItem(int oldIndex, int newIndex)
		{
			ChangeMarker = !ChangeMarker;

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

		
		protected internal void baseRemoveItem(int index)
		{
			ChangeMarker = !ChangeMarker;

			_currentChange = NotifyCollectionChangedAction.Remove;
			_oldIndex = index;

			PreCollectionChanged?.Invoke(this, null);
			base.RemoveItem(index);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_oldIndex = -1;
		}

		
		protected internal void baseSetItem(int index, TItem item)
		{
			ChangeMarker = !ChangeMarker;
			
			_currentChange = NotifyCollectionChangedAction.Replace;
			_newItem = item;
			_newIndex = index;

			PreCollectionChanged?.Invoke(this, null);
			base.SetItem(index, item);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_newItem = default;
			_newIndex = -1;
		}

		
		protected internal void baseClearItems()
		{
			ChangeMarker = !ChangeMarker;

			_currentChange = NotifyCollectionChangedAction.Reset;

			PreCollectionChanged?.Invoke(this, null);
			base.ClearItems();
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
		}

		public Type ItemType => typeof(TItem);

		protected int _initialCapacity;

		// ReSharper disable once MemberCanBePrivate.Global
		public string InstantiatingStackTrace => _instantiatingStackTrace;

		protected bool _isConsistent = true;
		private readonly string _instantiatingStackTrace;
		public bool IsConsistent => _isConsistent;
		public event EventHandler ConsistencyRestored;

		protected void raiseConsistencyRestored()
		{
			ConsistencyRestored?.Invoke(this, null);
		}

		protected void checkConsistent()
		{
			if (!_isConsistent)
				throw new ObservableComputationsException(this,
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.");
		}
	}

}
