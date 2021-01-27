// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class CollectionDispatching<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		public virtual INotifyCollectionChanged Source => _source;
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;

		public virtual ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public virtual ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public int DestinationOcDispatcherPriority => _destinationOcDispatcherPriority;
		public int SourceOcDispatcherPriority => _sourceOcDispatcherPriority;
		public object DestinationOcDispatcherParameter => _destinationOcDispatcherParameter;
		public object SourceOcDispatcherParameter => _sourceOcDispatcherParameter;

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private readonly IOcDispatcher _destinationOcDispatcher;
		private readonly IOcDispatcher _sourceOcDispatcher;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private readonly int _destinationOcDispatcherPriority;
		private readonly int _sourceOcDispatcherPriority;
		private readonly object _destinationOcDispatcherParameter;
		private readonly object _sourceOcDispatcherParameter;


		[ObservableComputationsCall]
		public CollectionDispatching(
			INotifyCollectionChanged source,
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			int destinationOcDispatcherPriority = 0,
			int sourceOcDispatcherPriority = 0,
			object destinationOcDispatcherParameter = null,
			object sourceOcDispatcherParameter = null) 
			: this(destinationOcDispatcherPriority, sourceOcDispatcherPriority, destinationOcDispatcherParameter, sourceOcDispatcherParameter)
		{
			_destinationOcDispatcher = destinationOcDispatcher;
			_sourceOcDispatcher = sourceOcDispatcher;
			_source = source;
		}



		protected override void processSource()
		{
			invokeProcessSource(null, null);
		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			int destinationOcDispatcherPriority = 0,
			int sourceOcDispatcherPriority = 0,
			object destinationOcDispatcherParameter = null,
			object sourceOcDispatcherParameter = null)
			: this(destinationOcDispatcherPriority, sourceOcDispatcherPriority, destinationOcDispatcherParameter, sourceOcDispatcherParameter)
		{
			_destinationOcDispatcher = destinationOcDispatcher;
			_sourceOcDispatcher = sourceOcDispatcher;
			_sourceScalar = sourceScalar;
		}

		private CollectionDispatching(			
			int destinationOcDispatcherPriority = 0,
			int sourceOcDispatcherPriority = 0,
			object destinationOcDispatcherParameter = null,
			object sourceOcDispatcherParameter = null)
		{
			_destinationOcDispatcherPriority = destinationOcDispatcherPriority;
			_sourceOcDispatcherPriority = sourceOcDispatcherPriority;
			_destinationOcDispatcherParameter = destinationOcDispatcherParameter;
			_sourceOcDispatcherParameter = sourceOcDispatcherParameter;
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			invokeProcessSource(sender, e);
		}


		private void invokeProcessSource(object sender, EventArgs e)
		{
			if (_source != null)
				_source.CollectionChanged -= handleSourceCollectionChanged;

			_destinationOcDispatcher.Invoke(
				() => doProcessSource(sender, e), 
				_destinationOcDispatcherPriority,
				_destinationOcDispatcherParameter,
				this);
		}

		private void doProcessSource(object sender, EventArgs e)
		{
			int originalCount = _items.Count;

			if (_sourceEnumerated)
			{			
				uninitializeSource();

				_sourceEnumerated = false;

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
						_sourceOcDispatcherPriority,
						_sourceOcDispatcherParameter,
						this);
				else
					uninitializeSource();
			}

			Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
				out _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				void readAndSubscribe()
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

					void resetAction()
					{
						_handledEventSender = sender;
						_handledEventArgs = e;

						int sourceIndex = 0;
						for (int index = 0; index < count; index++)
						{
							if (originalCount > sourceIndex)
								_items[sourceIndex] = sourceCopy[index];
							else
								_items.Insert(sourceIndex, sourceCopy[index]);

							sourceIndex++;
						}

						for (int index = originalCount - 1; index >= sourceIndex; index--)
						{
							_items.RemoveAt(index);
						}

						reset();

						_handledEventSender = null;
						_handledEventArgs = null;
					}

					_destinationOcDispatcher.Invoke(
						resetAction, 
						_destinationOcDispatcherPriority,
						_destinationOcDispatcherParameter,
						this);
				}

				if (_sourceOcDispatcher != null)
					_sourceOcDispatcher.Invoke(
						readAndSubscribe, 
						_sourceOcDispatcherPriority,
						_sourceOcDispatcherParameter,
						this);
				else
					readAndSubscribe();
	 
				_sourceEnumerated = true;
			}
			else
			{
				void clearItemsAction()
				{
					_handledEventSender = sender;
					_handledEventArgs = e;
					baseClearItems();
					_handledEventSender = null;
					_handledEventArgs = null;
				}

				_destinationOcDispatcher.Invoke(
					clearItemsAction, 
					_destinationOcDispatcherPriority,
					_destinationOcDispatcherParameter,
					this);
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

					_destinationOcDispatcher.Invoke(
						add, 
						_destinationOcDispatcherPriority,
						_destinationOcDispatcherParameter,
						this);

					break;
				case NotifyCollectionChangedAction.Remove:
					// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");


					void remove()
					{
						baseRemoveItem(e.OldStartingIndex);
					}

					_destinationOcDispatcher.Invoke(
						remove, 
						_destinationOcDispatcherPriority,
						_destinationOcDispatcherParameter,
						this);

					break;
				case NotifyCollectionChangedAction.Replace:

					//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
					void replace()
					{
						baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
					}

					_destinationOcDispatcher.Invoke(
						replace, 
						_destinationOcDispatcherPriority,
						_destinationOcDispatcherParameter,
						this);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex1 = e.OldStartingIndex;
					int newStartingIndex1 = e.NewStartingIndex;
					if (oldStartingIndex1 == newStartingIndex1) return;

					void move()
					{
						baseMoveItem(oldStartingIndex1, newStartingIndex1);
					}

					_destinationOcDispatcher.Invoke(
						move, 
						_destinationOcDispatcherPriority,
						_destinationOcDispatcherParameter,
						this);

					break;
				case NotifyCollectionChangedAction.Reset:
					invokeProcessSource(sender, e);
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
			void perform() => Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source);

			if (_sourceOcDispatcher != null)
				_sourceOcDispatcher.Invoke(
					perform, 
					_sourceOcDispatcherPriority,
					_sourceOcDispatcherParameter,
					this);
			else
				perform();
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			void perform() => Utils.RemoveDownstreamConsumedComputing(computing, _sourceScalar, _source);

			if (_sourceOcDispatcher != null)
				_sourceOcDispatcher.Invoke(
					perform, 
					_sourceOcDispatcherPriority,
					_sourceOcDispatcherParameter,
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
					_sourceOcDispatcherPriority,
					_sourceOcDispatcherParameter,
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
					_sourceOcDispatcherPriority,
					_sourceOcDispatcherParameter,
					this);
			else
				perform();
		}

		protected override void raisePropertyChanged(PropertyChangedEventArgs e)
		{
			_destinationOcDispatcher.Invoke(
				() => OnPropertyChanged(e), 
				_destinationOcDispatcherPriority,
				_destinationOcDispatcherParameter,
				this);
		}
	}
}
