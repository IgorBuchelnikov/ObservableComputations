using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class ItemsProcessing<TSourceItem, TReturnValue> : CollectionComputing<TReturnValue>, IHasSourceCollections
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;


		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public Func<TSourceItem, ICollectionComputing, TReturnValue> NewItemProcessor => _newItemProcessor;
		public Action<TSourceItem, ICollectionComputing, TReturnValue> OldItemProcessor => _oldItemProcessor;
		public Action<TSourceItem, ICollectionComputing, TReturnValue> MoveItemProcessor => _moveItemProcessor;

		private readonly Func<TSourceItem, ICollectionComputing, TReturnValue> _newItemProcessor;
		private readonly Action<TSourceItem, ICollectionComputing, TReturnValue> _oldItemProcessor;
		private readonly Action<TSourceItem, ICollectionComputing, TReturnValue> _moveItemProcessor;

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;
		private bool _lastProcessedSourceChangeMarker;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private INotifyCollectionChanged _source;

		[ObservableComputationsCall]
		public ItemsProcessing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource(null, null);
		}

		[ObservableComputationsCall]
		public ItemsProcessing(
			INotifyCollectionChanged source,
			Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(source))
		{
			_source = source;
			initializeFromSource(null, null);
		}

		private ItemsProcessing(
			Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor, 
			int capacity) : base(capacity)
		{
			_newItemProcessor = newItemProcessor;
			_oldItemProcessor = oldItemProcessor;
			_moveItemProcessor = moveItemProcessor;
		}

		private void initializeFromSource(object sender, EventArgs eventArgs)
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				int count = Count;
				for (int i = 0; i < count; i++)
				{
					TSourceItem sourceItem = _sourceAsList[i];
					TReturnValue returnValue = this[0];
					baseRemoveItem(0);
					if (_oldItemProcessor != null) processOldItem(sourceItem, returnValue);
				}

				if (_rootSourceWrapper)
				{
					_sourceAsList.CollectionChanged -= _sourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
					_sourceWeakNotifyCollectionChangedEventHandler = null;
				}

				_sourceNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceScalar != null)
				_source = _sourceScalar.Value;
			_sourceAsList = null;

			if (_source != null)
			{
				if (_source is ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList)
				{
					_sourceAsList = sourceAsList;
					_rootSourceWrapper = false;
				}
				else
				{
					_sourceAsList = new RootSourceWrapper<TSourceItem>(_source);
					_rootSourceWrapper = true;
				}

				_lastProcessedSourceChangeMarker = _sourceAsList.ChangeMarkerField;

				int count = _sourceAsList.Count;
				for (int index = 0; index < count; index++)
				{
					TSourceItem sourceItem = _sourceAsList[index];
					TReturnValue returnValue = _newItemProcessor != null ? processNewItem(sourceItem) : default(TReturnValue);

					baseInsertItem(index, returnValue);
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;

				if (_rootSourceWrapper)
				{
					_sourceAsList.CollectionChanged += _sourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_sourceWeakNotifyCollectionChangedEventHandler =
						new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

					_sourceAsList.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				}
			}
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent(sender, e);

			_processingEventSender = sender;
			_processingEventArgs = e;

			_isConsistent = false;

			initializeFromSource(sender, e);

			_isConsistent = true;
			raiseConsistencyRestored();

			_processingEventSender = null;
			_processingEventArgs = null;
		}


		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent(sender, e);
			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarkerField) return;
			
			_processingEventSender = sender;
			_processingEventArgs = e;

			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					_isConsistent = false;
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = _sourceAsList[newStartingIndex];
					TReturnValue returnValue = _newItemProcessor != null ? processNewItem(addedItem) : default(TReturnValue);

					baseInsertItem(newStartingIndex, returnValue);
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Remove:
					_isConsistent = false;
					int oldStartingIndex = e.OldStartingIndex;
					TSourceItem removedItem = (TSourceItem) e.OldItems[0];
					TReturnValue returnValue1 = this[oldStartingIndex];
					baseRemoveItem(oldStartingIndex);
					if (_oldItemProcessor != null) processOldItem(removedItem, returnValue1);
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Replace:
					_isConsistent = false;
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TSourceItem newItem = _sourceAsList[newStartingIndex1];
					TReturnValue returnValueOld = this[newStartingIndex1];

					TReturnValue returnValue2 = _newItemProcessor != null ? processNewItem(newItem) : default;
					baseSetItem(newStartingIndex1, returnValue2);
					if (_oldItemProcessor != null) processOldItem(oldItem, returnValueOld);
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 != newStartingIndex2)
					{
						baseMoveItem(oldStartingIndex2, newStartingIndex2);
						if (_moveItemProcessor!= null) processMovedItem(_sourceAsList[newStartingIndex2], this[newStartingIndex2]);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					_isConsistent = false;
					initializeFromSource(sender, e);
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
			}

			_processingEventSender = null;
			_processingEventArgs = null;
		}

		private TReturnValue processNewItem(TSourceItem sourceItem)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				TReturnValue returnValue = _newItemProcessor(sourceItem, this);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return returnValue;
			}

			return  _newItemProcessor(sourceItem, this);
		}

		private void processOldItem(TSourceItem sourceItem, TReturnValue returnValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				_oldItemProcessor(sourceItem, this, returnValue);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return;
			}

			_oldItemProcessor(sourceItem, this, returnValue);
		}


		private void processMovedItem(TSourceItem sourceItem, TReturnValue returnValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				_moveItemProcessor(sourceItem, this, returnValue);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return;
			}

			_moveItemProcessor(sourceItem, this, returnValue);
		}

		~ItemsProcessing()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;
			}
		}
	}
}
