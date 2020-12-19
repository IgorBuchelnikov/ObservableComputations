using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class CollectionDispatching<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		public INotifyCollectionChanged Source => _source;
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;
		public ICollectionDestinationOcDispatcher CollectionDestinationOcDispatcher => _collectionDestinationOcDispatcher;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private DispatcherPriorities DispatcherPriorities => _dispatcherPriorities;
		private DispatcherParameters DispatcherParameters => _dispatcherParameters;

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private bool _sourceInitialized;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private readonly IOcDispatcher _destinationOcDispatcher;
		private ICollectionDestinationOcDispatcher _collectionDestinationOcDispatcher;
		private IOcDispatcher _sourceOcDispatcher;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private DispatcherPriorities _dispatcherPriorities;
		private DispatcherParameters _dispatcherParameters;

		[ObservableComputationsCall]
		public CollectionDispatching(
			INotifyCollectionChanged source,
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			DispatcherPriorities? dispatcherPriorities = null,
			DispatcherParameters? dispatcherParameters = null) 
			: this(dispatcherPriorities, dispatcherParameters)
		{
			_destinationOcDispatcher = destinationOcDispatcher;
			_sourceOcDispatcher = sourceOcDispatcher;
			_source = source;		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			INotifyCollectionChanged source,
			ICollectionDestinationOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			DispatcherPriorities? dispatcherPriorities = null,
			DispatcherParameters? dispatcherParameters = null)
			: this(dispatcherPriorities, dispatcherParameters)
		{
			_collectionDestinationOcDispatcher = destinationOcDispatcher;
			_sourceOcDispatcher = sourceOcDispatcher;
			_source = source;
		}

		protected override void initializeFromSource()
		{
			invokeInitializeFromSource(null, null);
		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			DispatcherPriorities? dispatcherPriorities = null,
			DispatcherParameters? dispatcherParameters = null)
			: this(dispatcherPriorities, dispatcherParameters)
		{
			_destinationOcDispatcher = destinationOcDispatcher;
			_sourceOcDispatcher = sourceOcDispatcher;
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			ICollectionDestinationOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			DispatcherPriorities? dispatcherPriorities = null,
			DispatcherParameters? dispatcherParameters = null)
			: this(dispatcherPriorities, dispatcherParameters)
		{
			_collectionDestinationOcDispatcher = destinationOcDispatcher;
			_sourceOcDispatcher = sourceOcDispatcher;
			_sourceScalar = sourceScalar;
		}

		private CollectionDispatching(DispatcherPriorities? dispatcherPriorities, DispatcherParameters? dispatcherParameters)
		{
			_dispatcherPriorities = dispatcherPriorities ?? new DispatcherPriorities(0, 0);
			_dispatcherParameters = dispatcherParameters ?? new DispatcherParameters(null, null);
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			invokeInitializeFromSource(sender, e);
		}


		private void invokeInitializeFromSource(object sender, EventArgs e)
		{
			if (_source != null)
				_source.CollectionChanged -= handleSourceCollectionChanged;

			if (_destinationOcDispatcher != null)
				_destinationOcDispatcher.Invoke(
					() => doInitializeFromSource(sender, e), 
					_dispatcherPriorities._distinationDispatcherPriority,
					_dispatcherParameters._distinationDispatcherParameter,
					this);
			else
				_collectionDestinationOcDispatcher.InvokeInitialization(
					() => doInitializeFromSource(sender, e), 
					_dispatcherPriorities._distinationDispatcherPriority,
					_dispatcherParameters._distinationDispatcherParameter,
					this);
		}

		private void doInitializeFromSource(object sender, EventArgs e)
		{
			int originalCount = _items.Count;

			if (_sourceInitialized)
			{			
				uninitializeSource();

				_sourceInitialized = false;

				void uninitializeSource()
				{
					_source.CollectionChanged -= handleSourceCollectionChanged;

					if (_sourceAsINotifyPropertyChanged != null)
					{
						_sourceAsINotifyPropertyChanged.PropertyChanged -=
							((ISourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
						_sourceAsINotifyPropertyChanged = null;
					}
				}

				if (_sourceOcDispatcher != null)
					_sourceOcDispatcher.Invoke(
						uninitializeSource, 
						_dispatcherPriorities._sourceDispatcherPriority,
						_dispatcherParameters._sourceDispatcherParameter,
						this);
				else
					uninitializeSource();
			}

			Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
				out _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				void readAndSubscibe()
				{
					Utils.initializeFromHasChangeMarker(
						out _sourceAsIHasChangeMarker, 
						_sourceAsList, 
						ref _lastProcessedSourceChangeMarker, 
						ref _sourceAsINotifyPropertyChanged,
						(ISourceIndexerPropertyTracker)this);

					int count = _sourceAsList.Count;
					TSourceItem[] sourceCopy = new TSourceItem[count];
					_sourceAsList.CopyTo(sourceCopy, 0);

					_source.CollectionChanged += handleSourceCollectionChanged;

					Action resetAction = () =>
					{
						_handledEventSender = sender;
						_handledEventArgs = e;

						int sourceIndex = 0;

						for (var index = 0; index < count; index++)
						{
							TSourceItem sourceItem = sourceCopy[index];
							if (originalCount > sourceIndex)
								_items[sourceIndex] = sourceItem;
							else
								_items.Insert(sourceIndex, sourceItem);

							sourceIndex++;
						}

						for (int index = originalCount - 1; index >= sourceIndex; index--)
						{
							_items.RemoveAt(index);
						}

						reset();

						_handledEventSender = null;
						_handledEventArgs = null;
					};

					if (_destinationOcDispatcher != null) 
						_destinationOcDispatcher.Invoke(
							resetAction, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter,
							this);
					else _collectionDestinationOcDispatcher.InvokeCollectionChange(
						resetAction, 
						_dispatcherPriorities._distinationDispatcherPriority,
						_dispatcherParameters._distinationDispatcherParameter, 
						this, 
						NotifyCollectionChangedAction.Reset, 
						null, null, 0, 0);
				}

				if (_sourceOcDispatcher != null)
					_sourceOcDispatcher.Invoke(
						readAndSubscibe, 
						_dispatcherPriorities._sourceDispatcherPriority,
						_dispatcherParameters._sourceDispatcherParameter,
						this);
				else
					readAndSubscibe();
	 
				_sourceInitialized = true;
			}
			else
			{
				Action clearItemsAction = () =>
				{
					_handledEventSender = sender;
					_handledEventArgs = e;
					baseClearItems();
					_handledEventSender = null;
					_handledEventArgs = null;
				};

				if (_destinationOcDispatcher != null) 
					_destinationOcDispatcher.Invoke(
						clearItemsAction, 
						_dispatcherPriorities._distinationDispatcherPriority,
						_dispatcherParameters._distinationDispatcherParameter,
						this);
				else _collectionDestinationOcDispatcher.InvokeCollectionChange(
					clearItemsAction, 
					_dispatcherPriorities._distinationDispatcherPriority,
					_dispatcherParameters._distinationDispatcherParameter, 
					this, NotifyCollectionChangedAction.Reset, 
					null, null, 0, 0);
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourceChangeMarker, 
				_sourceAsIHasChangeMarker)) return;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:

					//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
					void add()
					{
						baseInsertItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
					}

					if (_destinationOcDispatcher != null) 
						_destinationOcDispatcher.Invoke(
							add, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter,
							this);
					else
						_collectionDestinationOcDispatcher.InvokeCollectionChange(
							add, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter, 
							this, 
							NotifyCollectionChangedAction.Add,
							(TSourceItem) e.NewItems[0], null, 
							e.NewStartingIndex, 0);

					break;
				case NotifyCollectionChangedAction.Remove:
					// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");


					void remove()
					{
						baseRemoveItem(e.OldStartingIndex);
					}

					if (_destinationOcDispatcher != null) 
						_destinationOcDispatcher.Invoke(
							remove, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter,
							this);
					else
						_collectionDestinationOcDispatcher.InvokeCollectionChange(
							remove, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter, 
							this, 
							NotifyCollectionChangedAction.Remove,
							null, e.OldItems[0], 0, e.OldStartingIndex);

					break;
				case NotifyCollectionChangedAction.Replace:

					//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
					void replace()
					{
						baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
					}

					if (_destinationOcDispatcher != null) 
						_destinationOcDispatcher.Invoke(
							replace, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter,
							this);
					else
						_collectionDestinationOcDispatcher.InvokeCollectionChange(
							replace, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter, 
							this, 
							NotifyCollectionChangedAction.Replace,
							(TSourceItem) e.NewItems[0], (TSourceItem) e.OldItems[0], 
							e.NewStartingIndex, e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex1 = e.OldStartingIndex;
					int newStartingIndex1 = e.NewStartingIndex;
					if (oldStartingIndex1 == newStartingIndex1) return;

					void move()
					{
						baseMoveItem(oldStartingIndex1, newStartingIndex1);
					}

					if (_destinationOcDispatcher != null) 
						_destinationOcDispatcher.Invoke(
							move, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter,
							this);
					else
						_collectionDestinationOcDispatcher.InvokeCollectionChange(
							move, 
							_dispatcherPriorities._distinationDispatcherPriority,
							_dispatcherParameters._distinationDispatcherParameter, 
							this, 
							NotifyCollectionChangedAction.Move,
							(TSourceItem) e.NewItems[0], (TSourceItem) e.OldItems[0], 
							e.NewStartingIndex, e.OldStartingIndex);

					break;
				case NotifyCollectionChangedAction.Reset:
					invokeInitializeFromSource(sender, e);
					break;
			}

			Utils.postHandleChange(
				out _handledEventSender,
				out _handledEventArgs);
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
		}

		#endregion

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			void perform()
			{
				(_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
				(_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			}

			if (_sourceOcDispatcher != null)
				_sourceOcDispatcher.Invoke(
					perform, 
					_dispatcherPriorities._sourceDispatcherPriority,
					_dispatcherParameters._sourceDispatcherParameter,
					this);
			else
				perform();
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			void perform()
			{
				(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
				(_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			}

			if (_sourceOcDispatcher != null)
				_sourceOcDispatcher.Invoke(
					perform, 
					_dispatcherPriorities._sourceDispatcherPriority,
					_dispatcherParameters._sourceDispatcherParameter,
					this);
			else
				perform();
		}

		protected override void initialize()
		{
			void perform()
			{
				Utils.initializeSourceScalar(_sourceScalar, ref _source, handleSourceScalarValueChanged);
			}

			if (_sourceOcDispatcher != null)
				_sourceOcDispatcher.Invoke(
					perform, 
					_dispatcherPriorities._sourceDispatcherPriority,
					_dispatcherParameters._sourceDispatcherParameter,
					this);
			else
				perform();
		}

		protected override void uninitialize()
		{
			void perform()
			{
				Utils.uninitializeSourceScalar(_sourceScalar, handleSourceScalarValueChanged, ref _source);
			}

			if (_sourceOcDispatcher != null)
				_sourceOcDispatcher.Invoke(
					perform, 
					_dispatcherPriorities._sourceDispatcherPriority,
					_dispatcherParameters._sourceDispatcherParameter,
					this);
			else
				perform();
		}
	}

	public struct DispatcherPriorities
	{
		internal readonly int _sourceDispatcherPriority;
		internal readonly int _distinationDispatcherPriority;

		public int? SourceDispatcherPriority => _sourceDispatcherPriority;
		public int? DistinationDispatcherPriority => _distinationDispatcherPriority;

		public DispatcherPriorities(int sourceDispatcherPriority, int distinationDispatcherPriority)
		{
			_sourceDispatcherPriority = sourceDispatcherPriority;
			_distinationDispatcherPriority = distinationDispatcherPriority;
		}
	}

	public struct DispatcherParameters
	{
		internal readonly object _sourceDispatcherParameter;
		internal readonly object _distinationDispatcherParameter;

		public object SourceDispatcherParameter => _sourceDispatcherParameter;
		public object DistinationDispatcherParameter => _distinationDispatcherParameter;

		public DispatcherParameters(object sourceDispatcherParameter, object distinationDispatcherParameter)
		{
			_sourceDispatcherParameter = sourceDispatcherParameter;
			_distinationDispatcherParameter = distinationDispatcherParameter;
		}
	}
}
