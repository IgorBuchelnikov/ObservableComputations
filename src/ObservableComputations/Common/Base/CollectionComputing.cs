using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ObservableComputations
{
	public abstract class CollectionComputing<TItem> : ObservableCollectionWithChangeMarker<TItem>, ICollectionComputing, IComputingInternal
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}
		internal IList<TItem> _items;
		internal Queue<IProcessable>[] _deferredProcessings;
		protected int _deferredQueuesCount = 2; 

		public CollectionComputing(int initialCapacity = 0) : base(new List<TItem>(initialCapacity))
		{
			_initialCapacity = initialCapacity;

			if (Configuration.SaveInstantiatingStackTrace)
			{
				_instantiatingStackTrace = Environment.StackTrace;
			}

			_items = Items;
		}

		public event EventHandler PreCollectionChanged;
		public event EventHandler PostCollectionChanged;


		private Action<int, TItem> _insertItemRequestHandler;
		public Action<int, TItem> InsertItemRequestHandler
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _insertItemRequestHandler;
			set
			{
				if (_insertItemRequestHandler != value)
				{
					_insertItemRequestHandler = value;
					OnPropertyChanged(Utils.InsertItemRequestHandlerPropertyChangedEventArgs);
				}

			}
		}


		public Action<int> RemoveItemRequestHandler
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _removeItemRequestHandler;
			set
			{
				if (_removeItemRequestHandler != value)
				{
					_removeItemRequestHandler = value;
					OnPropertyChanged(Utils.RemoveItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		private Action<int, TItem> _setItemRequestHandler;
		// ReSharper disable once MemberCanBePrivate.Global
		public Action<int, TItem> SetItemRequestHandler
		{
			get => _setItemRequestHandler;
			set
			{
				if (_setItemRequestHandler != value)
				{
					_setItemRequestHandler = value;
					OnPropertyChanged(Utils.SetItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		private Action<int, int> _moveItemRequestHandler;
		// ReSharper disable once MemberCanBePrivate.Global
		public Action<int, int> MoveItemRequestHandler
		{
			get => _moveItemRequestHandler;
			set
			{
				if (_moveItemRequestHandler != value)
				{
					_moveItemRequestHandler = value;
					OnPropertyChanged(Utils.MoveItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		private Action _clearItemsRequestHandler;

		// ReSharper disable once MemberCanBePrivate.Global
		public Action ClearItemsRequestHandler
		{
			get => _clearItemsRequestHandler;
			set
			{
				if (_clearItemsRequestHandler != value)
				{
					_clearItemsRequestHandler = value;
					OnPropertyChanged(Utils.ClearItemsRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		#region Overrides of ObservableCollection<TResult>
		protected override void InsertItem(int index, TItem item)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_insertItemRequestHandler(index, item);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_insertItemRequestHandler(index, item);
		}

		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_moveItemRequestHandler(oldIndex, newIndex);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_moveItemRequestHandler(oldIndex, newIndex);
		}

		protected override void RemoveItem(int index)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_removeItemRequestHandler(index);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_removeItemRequestHandler(index);
		}

		protected override void SetItem(int index, TItem item)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_setItemRequestHandler(index, item);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_setItemRequestHandler(index, item);
		}

		protected override void ClearItems()
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_clearItemsRequestHandler();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_clearItemsRequestHandler();
		}
		#endregion

		private Action<int> _removeItemRequestHandler;
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
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
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
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
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
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				perform();			
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
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
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
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
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
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
			Utils.processResetChange(
				sender, 
				args, 
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs, 
				_scalarValueChangedHandlerAction, 
				_deferredQueuesCount,
				ref _deferredProcessings, this);
		}

		protected abstract void initializeFromSource();
		protected abstract void initialize();
		protected abstract void uninitialize();


		public Type ItemType => typeof(TItem);

		public virtual int InitialCapacity => _initialCapacity;
		internal int _initialCapacity;

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

		protected List<OcConsumer> _consumers = new List<OcConsumer>();
		internal  List<IComputingInternal> _downstreamConsumedComputings = new List<IComputingInternal>();
		protected bool _isActive;
		public bool IsActive => _isActive;

		bool _initializationInProgress;
		bool _uninitializationInProgress;
		public bool ActivationInProgress => _initializationInProgress;
		public bool InactivationInProgress => _uninitializationInProgress;

		void IComputingInternal.SetInactivationInProgress(bool value)
		{
			_uninitializationInProgress = value;
		}

		void IComputingInternal.SetActivationInProgress(bool value)
		{
			_initializationInProgress = value;
		}

		public ReadOnlyCollection<object> ConsumerTags =>
			new ReadOnlyCollection<object>(_consumers.Union(_downstreamConsumedComputings.SelectMany(c => c.Consumers.Select(cons => cons.Tag))).ToList());

		#region Implementation of IComputingInternal
		IEnumerable<OcConsumer> IComputingInternal.Consumers => _consumers;

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

		void ICanInitializeFromSource.InitializeFromSource()
		{
			initializeFromSource();
		}

		void IComputingInternal.OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{
			OnPropertyChanged(propertyChangedEventArgs);
		}

		void IComputingInternal.SetIsActive(bool value)
		{
			_isActive = value;
		}


		void IComputingInternal.AddConsumer(OcConsumer addingOcConsumer)
		{
			Utils.addConsumer(
				addingOcConsumer, 
				_consumers,
				_downstreamConsumedComputings, 
				this, 
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings,
				_deferredQueuesCount);
		}


		void IComputingInternal.RemoveConsumer(OcConsumer removingOcConsumer)
		{
			Utils.removeConsumer(
				removingOcConsumer, 
				_consumers, 
				_downstreamConsumedComputings, 
				this,
				ref _isConsistent, 
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				_deferredQueuesCount);
		}

		void IComputingInternal.AddDownstreamConsumedComputing(IComputingInternal computing)
		{
			Utils.addDownstreamConsumedComputing(
				computing, 
				_downstreamConsumedComputings, 
				_consumers, 
				this,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings,
				_deferredQueuesCount);
		}

		void IComputingInternal.RemoveDownstreamConsumedComputing(IComputingInternal computing)
		{
			Utils.removeDownstreamConsumedComputing(
				computing, 
				_downstreamConsumedComputings, 
				this, 
				ref _isConsistent,
				_consumers,
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				_deferredQueuesCount);
		}

		void IComputingInternal.RaiseConsistencyRestored()
		{
			raiseConsistencyRestored();
		}

		protected void checkConsistent(object sender, EventArgs eventArgs)
		{
			if (!_isConsistent)
				throw new ObservableComputationsInconsistencyException(this,
					$"It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
		}

		#endregion

		internal abstract void addToUpstreamComputings(IComputingInternal computing);
		internal abstract void removeFromUpstreamComputings(IComputingInternal computing);

		#region Overrides of Object

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(DebugTag))
				return $"{DebugTag} ({base.ToString()})";

			return base.ToString();
		}

		#endregion
	}

}
