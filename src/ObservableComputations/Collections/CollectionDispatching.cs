// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class CollectionDispatching<TSourceItem> : CollectionComputing<TSourceItem>, IHasSources, ISourceIndexerPropertyTracker
	{
		public virtual INotifyCollectionChanged Source => _source;
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		public int DestinationOcDispatcherPriority => _destinationOcDispatcherPriority;
		public int SourceOcDispatcherPriority => _sourceOcDispatcherPriority;
		public object DestinationOcDispatcherParameter => _destinationOcDispatcherParameter;
		public object SourceOcDispatcherParameter => _sourceOcDispatcherParameter;

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private bool _countPropertyChangedEventRaised;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private readonly IOcDispatcher _destinationOcDispatcher;
		private readonly IOcDispatcher _sourceOcDispatcher;

		private IHasTickTackVersion _sourceAsIHasTickTackVersion;
		private bool _lastProcessedSourceTickTackVersion;

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
			invokeSourceDispatcher(() => 
				doProcessSource(null, null, true));
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

			doProcessSource(sender, e, true);
		}

		private void doProcessSource(object sender, EventArgs e, bool replaceSource)
		{
			if (_sourceReadAndSubscribed)
			{
				_sourceReadAndSubscribed = false;

				if (replaceSource)
					Utils.unsubscribeSource(
						_source, 
						ref _sourceAsINotifyPropertyChanged, 
						this, 
						handleSourceCollectionChanged);

			}

			if (replaceSource)
				Utils.replaceSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
					out _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				if (replaceSource)
					Utils.subscribeSource(
						out _sourceAsIHasTickTackVersion,
						_sourceAsList,
						ref _lastProcessedSourceTickTackVersion,
						ref _sourceAsINotifyPropertyChanged,
						(ISourceIndexerPropertyTracker) this,
						_source,
						handleSourceCollectionChanged);

				int count = _sourceAsList.Count;
				TSourceItem[] sourceCopy = new TSourceItem[count];
				_sourceAsList.CopyTo(sourceCopy, 0);

				void resetAction()
				{
					int originalCount = _items.Count;

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
	 
				_sourceReadAndSubscribed = true;
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
				e.Action,
				ref _countPropertyChangedEventRaised,
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourceTickTackVersion, 
				_sourceAsIHasTickTackVersion)) return;

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
					doProcessSource(sender, e, false);
					break;
			}

			Utils.postHandleChange(
				out _handledEventSender,
				out _handledEventArgs);
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _countPropertyChangedEventRaised, ref _indexerPropertyChangedEventRaised);
		}

		#endregion

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			invokeSourceDispatcher(() => 
				Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source));
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			invokeSourceDispatcher(() => 
				Utils.unsubscribeSourceScalar(_sourceScalar, handleSourceScalarValueChanged));
		}

		protected override void initialize()
		{
			invokeSourceDispatcher(() => 
				Utils.initializeSourceScalar(_sourceScalar, ref _source, handleSourceScalarValueChanged));
		}

		protected override void uninitialize()
		{
			invokeSourceDispatcher(() => 
				Utils.unsubscribeSourceScalar(_sourceScalar, handleSourceScalarValueChanged));
		}

		protected override void clearCachedScalarArgumentValues()
		{
			invokeSourceDispatcher(() => 
				Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source));
		}

		private void invokeSourceDispatcher(Action action)
		{
			if (_sourceOcDispatcher != null)
			{
				_sourceOcDispatcher.Invoke(
					action,
					_sourceOcDispatcherPriority,
					_sourceOcDispatcherParameter,
					this);
			}
			else
				action();
		}

		internal override void InitializeInvolvedMembersTreeNodeImpl(InvolvedMembersTreeNode involvedMembersTreeNode)
		{

		}

		protected override void raisePropertyChanged(PropertyChangedEventArgs e)
		{
			_destinationOcDispatcher.Invoke(
				() => OnPropertyChanged(e), 
				_destinationOcDispatcherPriority,
				_destinationOcDispatcherParameter,
				this);
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			
			bool conststent = true;

			_destinationOcDispatcher.Invoke(() => {});
			_destinationOcDispatcher.Invoke(() =>
			{
				conststent = this.SequenceEqual(source);
			});

			if (!conststent)
				throw new ValidateInternalConsistencyException("Consistency violation: CollectionDispatching.1");

		}
	}
}
