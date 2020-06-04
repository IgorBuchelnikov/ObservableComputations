using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

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

		Dictionary<CollectionChangeAction, object> _lockModifyChangeActionsKeys;
		private Dictionary<CollectionChangeAction, object> lockModifyChangeActionsKeys => _lockModifyChangeActionsKeys = 
			_lockModifyChangeActionsKeys ?? new Dictionary<CollectionChangeAction, object>();

		public void LockModifyChangeAction(CollectionChangeAction collectionChangeAction, object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (!lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				lockModifyChangeActionsKeys[collectionChangeAction] = key;
			else
				throw new ObservableComputationsException(this,
					$"Modifying of '{collectionChangeAction.ToString()}' change action is already locked. Unlock first.");
		}

		public void UnlockModifyChangeAction(CollectionChangeAction collectionChangeAction, object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (!lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				throw new ObservableComputationsException(this,
					"Modifying of '{collectionChangeAction.ToString()}' change action is not locked. Lock first.");

			if (ReferenceEquals(lockModifyChangeActionsKeys[collectionChangeAction], key))
				lockModifyChangeActionsKeys.Remove(collectionChangeAction);
			else
				throw new ObservableComputationsException(this,
					"Wrong key to unlock modifying of '{collectionChangeAction.ToString()}' change action.");
		}

		public bool IsModifyChangeActionLocked(CollectionChangeAction collectionChangeAction)
		{
			return lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction);
		}

		private void checkLockModifyChangeAction(CollectionChangeAction collectionChangeAction)
		{
			if (lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				throw new ObservableComputationsException(this,
					"Modifying of '{collectionChangeAction.ToString()}' change action is locked. Unlock first.");
		}


		private Action<int, TItem> _insertItemAction;
		public Action<int, TItem> InsertItemAction
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _insertItemAction;
			set
			{
				if (_insertItemAction != value)
				{
					checkLockModifyChangeAction(CollectionChangeAction.InsertItem);

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
					checkLockModifyChangeAction(CollectionChangeAction.RemoveItem);

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
					checkLockModifyChangeAction(CollectionChangeAction.SetItem);

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
					checkLockModifyChangeAction(CollectionChangeAction.MoveItem);

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
					checkLockModifyChangeAction(CollectionChangeAction.ClearItems);

					_clearItemsAction = value;
					OnPropertyChanged(Utils.ClearItemsActionPropertyChangedEventArgs);
				}
			}
		}

		#region Overrides of ObservableCollection<TResult>
		protected override void InsertItem(int index, TItem item)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;
				
				_insertItemAction(index, item);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return;
			}

			_insertItemAction(index, item);
		}

		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				_moveItemAction(oldIndex, newIndex);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return;
			}

			_moveItemAction(oldIndex, newIndex);
		}

		protected override void RemoveItem(int index)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				_removeItemAction(index);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return;
			}

			_removeItemAction(index);
		}

		protected override void SetItem(int index, TItem item)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				_setItemAction(index, item);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return;
			}

			_setItemAction(index, item);
		}

		protected override void ClearItems()
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				_clearItemsAction();

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return;
			}

			_clearItemsAction();
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
			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Add;
			_newIndex = index;
			_newItem = item;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				PreCollectionChanged?.Invoke(this, null);
				base.InsertItem(index, item);
				PostCollectionChanged?.Invoke(this, null);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				PreCollectionChanged?.Invoke(this, null);
				base.InsertItem(index, item);
				PostCollectionChanged?.Invoke(this, null);				
			}

			_currentChange = null;
			_newIndex = -1;
			_newItem = default(TItem);
		}

		protected internal void baseMoveItem(int oldIndex, int newIndex)
		{
			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Move;
			_oldIndex = oldIndex;
			_newIndex = newIndex;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				PreCollectionChanged?.Invoke(this, null);
				base.MoveItem(oldIndex, newIndex);
				PostCollectionChanged?.Invoke(this, null);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				PreCollectionChanged?.Invoke(this, null);
				base.MoveItem(oldIndex, newIndex);
				PostCollectionChanged?.Invoke(this, null);
			}

			_currentChange = null;
			_oldIndex = -1;
			_newIndex = -1;
		}

		
		protected internal void baseRemoveItem(int index)
		{
			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Remove;
			_oldIndex = index;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				PreCollectionChanged?.Invoke(this, null);
				base.RemoveItem(index);
				PostCollectionChanged?.Invoke(this, null);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				PreCollectionChanged?.Invoke(this, null);
				base.RemoveItem(index);
				PostCollectionChanged?.Invoke(this, null);
			}

			_currentChange = null;
			_oldIndex = -1;
		}

		
		protected internal void baseSetItem(int index, TItem item)
		{
			ChangeMarkerField = !ChangeMarkerField;
			
			_currentChange = NotifyCollectionChangedAction.Replace;
			_newItem = item;
			_newIndex = index;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				PreCollectionChanged?.Invoke(this, null);
				base.SetItem(index, item);
				PostCollectionChanged?.Invoke(this, null);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				PreCollectionChanged?.Invoke(this, null);
				base.SetItem(index, item);
				PostCollectionChanged?.Invoke(this, null);
			}

			_currentChange = null;
			_newItem = default;
			_newIndex = -1;
		}

		
		protected internal void baseClearItems()
		{
			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Reset;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				PreCollectionChanged?.Invoke(this, null);
				base.ClearItems();
				PostCollectionChanged?.Invoke(this, null);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				PreCollectionChanged?.Invoke(this, null);
				base.ClearItems();
				PostCollectionChanged?.Invoke(this, null);
			}

			_currentChange = null;
		}

		public Type ItemType => typeof(TItem);

		protected int _initialCapacity;

		// ReSharper disable once MemberCanBePrivate.Global
		public string InstantiatingStackTrace => _instantiatingStackTrace;

		internal IComputing _userCodeIsCalledFrom;
		public IComputing UserCodeIsCalledFrom => _userCodeIsCalledFrom;

		internal object _handledEventSender;
		internal EventArgs _handledEventArgs;
		public object HandledEventSender => _handledEventSender;
		public EventArgs HandledEventArgs => _handledEventArgs;

		protected bool _isConsistent = true;
		private readonly string _instantiatingStackTrace;


		public bool IsConsistent => _isConsistent;
		public event EventHandler ConsistencyRestored;

		protected void raiseConsistencyRestored()
		{
			ConsistencyRestored?.Invoke(this, null);
		}

		protected void checkConsistent(object sender, EventArgs eventArgs)
		{
			if (!_isConsistent)
				throw new ObservableComputationsException(this,
					$"The source collection has been changed. It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.");
		}
	}

	public enum CollectionChangeAction
	{
		InsertItem,
		RemoveItem,
		SetItem,
		MoveItem,
		ClearItems,
	}

}
