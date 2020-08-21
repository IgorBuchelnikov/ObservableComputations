using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace ObservableComputations
{
	public abstract class CollectionComputing<TItem> : ObservableCollectionWithChangeMarker<TItem>, ICollectionComputing, IComputingInternal
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}
		internal IList<TItem> _items;

		public CollectionComputing(int capacity = 0) : base(new List<TItem>(capacity))
		{
			_initialCapacity = capacity;

			if (Configuration.SaveInstantiatingStackTrace)
			{
				_instantiatingStackTrace = Environment.StackTrace;
			}

			_items = Items;
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
            void perform()
            {
                PreCollectionChanged?.Invoke(this, null);
                base.InsertItem(index, item);
                PostCollectionChanged?.Invoke(this, null);
            }

            ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Add;
			_newIndex = index;
			_newItem = item;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);

				perform();

                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
			}
			else
            {
                perform();
            }

			_currentChange = null;
			_newIndex = -1;
			_newItem = default(TItem);
		}

		protected internal void baseMoveItem(int oldIndex, int newIndex)
		{
            void perform()
            {
                PreCollectionChanged?.Invoke(this, null);
                base.MoveItem(oldIndex, newIndex);
                PostCollectionChanged?.Invoke(this, null);
            }

            ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Move;
			_oldIndex = oldIndex;
			_newIndex = newIndex;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);

				perform();

                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
			}
			else
			{
                perform();
			}

			_currentChange = null;
			_oldIndex = -1;
			_newIndex = -1;
		}

		
		protected internal void baseRemoveItem(int index)
		{
            void perform()
            {
                PreCollectionChanged?.Invoke(this, null);
                base.RemoveItem(index);
                PostCollectionChanged?.Invoke(this, null);
            }

            ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Remove;
			_oldIndex = index;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);

				perform();

                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
			}
			else
			{
				perform();
			}

			_currentChange = null;
			_oldIndex = -1;
		}

		
		protected internal void baseSetItem(int index, TItem item)
		{
            void perform()
            {
                PreCollectionChanged?.Invoke(this, null);
                base.SetItem(index, item);
                PostCollectionChanged?.Invoke(this, null);
            }

            ChangeMarkerField = !ChangeMarkerField;
			
			_currentChange = NotifyCollectionChangedAction.Replace;
			_newItem = item;
			_newIndex = index;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);

				perform();

                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
			}
			else
            {
                perform();
            }

			_currentChange = null;
			_newItem = default;
			_newIndex = -1;
		}

		
		protected internal void baseClearItems()
		{
            void perform()
            {
                PreCollectionChanged?.Invoke(this, null);
                base.ClearItems();
                PostCollectionChanged?.Invoke(this, null);
            }

            ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Reset;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);

				perform();

                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
			}
			else
			{
				perform();
			}

			_currentChange = null;
		}

		protected void reset()
		{
			ChangeMarkerField = !ChangeMarkerField;
			CheckReentrancy();
			OnPropertyChanged(Utils.CountPropertyChangedEventArgs);
			OnPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			OnCollectionChanged(Utils.ResetNotifyCollectionChangedEventArgs);
		}

        private Action _scalarValueChangedHandlerAction;

        protected PropertyChangedEventHandler getScalarValueChangedHandler(Action action = null)
        {
            return (sender, args) =>
            {
                _scalarValueChangedHandlerAction = action;
                scalarValueChangedHandler(sender, args);
                _scalarValueChangedHandlerAction = null;
            };
        }

        protected void scalarValueChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(IReadScalar<object>.Value)) return;
            checkConsistent(sender, args);

            _handledEventSender = sender;
            _handledEventArgs = args;

            _isConsistent = false;

            _scalarValueChangedHandlerAction?.Invoke();
            initializeFromSource();

            _isConsistent = true;
            raiseConsistencyRestored();

            _handledEventSender = null;
            _handledEventArgs = null;
        }

        protected abstract void initializeFromSource();
        protected abstract void initialize();
        protected abstract void uninitialize();


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
				throw new ObservableComputationsInconsistencyException(this,
					$"The source collection has been changed. It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
		}

        protected List<Consumer> _consumers = new List<Consumer>();
        internal  List<IComputingInternal> _downstreamConsumedComputings = new List<IComputingInternal>();
        protected bool _isActive;
        public bool IsActive => _isActive;

        public ReadOnlyCollection<object> ConsumerTags =>
            new ReadOnlyCollection<object>(_consumers.Union(_downstreamConsumedComputings.SelectMany(c => c.Consumers.Select(cons => cons.Tag))).ToList());

        #region Implementation of IComputingInternal
        IEnumerable<Consumer> IComputingInternal.Consumers => _consumers;

        void IComputingInternal.AddToUpstreamComputings(IComputingInternal computing)
        {
            addToUpstreamComputings(computing);
        }

        void IComputingInternal.RemoveFromUpstreamComputings(IComputingInternal computing)
        {
            removeFromUpstreamComputings(computing);
        }

        void IComputingInternal.Initialize()
        {
            initialize();
        }

        void IComputingInternal.Uninitialize()
        {
            uninitialize();
        }

        void IComputingInternal.InitializeFromSource()
        {
            initializeFromSource();
        }

        void IComputingInternal.AddConsumer(Consumer addingConsumer)
        {
            Utils.AddComsumer(addingConsumer, _consumers, _downstreamConsumedComputings, this, ref _isActive);
        }

        void IComputingInternal.OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged(propertyChangedEventArgs);
        }

        void IComputingInternal.RemoveConsumer(Consumer removingConsumer)
        {
            Utils.RemoveConsumer(removingConsumer, _consumers, _downstreamConsumedComputings, ref _isActive, this);
        }

        void IComputingInternal.AddDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.AddDownstreamConsumedComputing(computing, _downstreamConsumedComputings, _consumers, ref _isActive, this);
        }

        void IComputingInternal.RemoveDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.RemoveDownstreamConsumedComputing(computing, _downstreamConsumedComputings, ref _isActive, this, _consumers);
        }

        void IComputingInternal.RaiseConsistencyRestored()
        {
            raiseConsistencyRestored();
        }

        #endregion

        internal abstract void addToUpstreamComputings(IComputingInternal computing);
        internal abstract void removeFromUpstreamComputings(IComputingInternal computing);
    }

}
