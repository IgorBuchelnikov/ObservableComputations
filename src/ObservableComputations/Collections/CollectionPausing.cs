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
	public class CollectionPausing<TSourceItem> : CollectionComputing<TSourceItem>, IHasSources, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		public virtual INotifyCollectionChanged Source => _source;
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;
		public IReadScalar<bool> IsPausedScalar => _isPausedScalar;
		public bool Resuming => _resuming;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		public bool IsPaused
		{
			get => _isPaused;
			set
			{
				if (_isPausedScalar != null) 
					throw new ObservableComputationsException(this, "Modifying of IsPaused property is controlled by IsPausedScalar");

				checkConsistent(null, null);

				void action()
				{
					_resuming = _isPaused != value && !value;
					_isPaused = value;
					OnPropertyChanged(Utils.IsPausedPropertyChangedEventArgs);

					if (_resuming) resume();
				}

				Utils.processChange(
					null, 
					null, 
					action,
					ref _isConsistent, 
					ref _handledEventSender, 
					ref _handledEventArgs, 
					0, _deferredQueuesCount,
					ref _deferredProcessings, this);
			}
		}

		public CollectionPausingResumeType ResumeType
		{
			get => _resumeType;
			set
			{
				if (_isPaused)
				{
					if (_resumeType == CollectionPausingResumeType.Reset 
						&& value == CollectionPausingResumeType.ReplayChanges)
						throw new ObservableComputationsException(this, "It is impossible to change ResumeType from Reset to ReplayChanges while IsPaused is true");

					if (_resumeType == CollectionPausingResumeType.ReplayChanges
						&& value == CollectionPausingResumeType.Reset)
						_deferredCollectionActions.Clear();

				}

				_resumeType = value;
				OnPropertyChanged(Utils.ResumeTypePropertyChangedEventArgs);
			}
		}

		private void resume()
		{
			if (_resumeType == CollectionPausingResumeType.ReplayChanges)
			{
				int count = _deferredCollectionActions.Count;

				for (int i = 0; i < count; i++)
				{
					DeferredCollectionAction<TSourceItem> deferredCollectionAction = _deferredCollectionActions.Dequeue();
					if (deferredCollectionAction.NotifyCollectionChangedEventArgs != null)
					{
						_handledEventSender = deferredCollectionAction.EventSender;
						_handledEventArgs = deferredCollectionAction.EventArgs;

						_thisAsSourceCollectionChangeProcessor.processSourceCollectionChanged(
							deferredCollectionAction.EventSender,
							deferredCollectionAction.NotifyCollectionChangedEventArgs);

						_handledEventSender = null;
						_handledEventArgs = null;
					}
					else if (deferredCollectionAction.Clear)
					{
						_items.Clear();
					}
					else if (deferredCollectionAction.Reset)
					{
						_handledEventSender = deferredCollectionAction.EventSender;
						_handledEventArgs = deferredCollectionAction.EventArgs;

						reset();

						_handledEventSender = null;
						_handledEventArgs = null;
					}
					else if (deferredCollectionAction.NewItems != null)
					{
						int originalCount = _items.Count;
						TSourceItem[] newItems = deferredCollectionAction.NewItems;
						int count1 = newItems.Length;
						int sourceIndex;
						for (sourceIndex = 0; sourceIndex < count1; sourceIndex++)
						{
							if (originalCount > sourceIndex)
								_items[sourceIndex] = newItems[sourceIndex];
							else
								_items.Insert(sourceIndex, newItems[sourceIndex]);
						}

						for (int index = originalCount - 1; index >= sourceIndex; index--)
						{
							_items.RemoveAt(index);
						}
					}
				}

				if (_sourceAsIHasTickTackVersion != null)
					_lastProcessedSourceTickTackVersion = _sourceAsIHasTickTackVersion.TickTackVersion;

				_resuming = false;

				Utils.postHandleChange(
					ref _handledEventSender,
					ref _handledEventArgs,
					_deferredProcessings,
					ref _isConsistent,
					this);
			}
			else //if (_resumeType == CollectionPausingResumeType.Reset)
			{
				processSource();
			}
		}

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private readonly IReadScalar<bool> _isPausedScalar;

		private bool _countPropertyChangedEventRaised;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasTickTackVersion _sourceAsIHasTickTackVersion;
		private bool _lastProcessedSourceTickTackVersion;
		private bool _isPaused;
		private bool _resuming;
		private CollectionPausingResumeType _resumeType;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		readonly Queue<DeferredCollectionAction<TSourceItem>> _deferredCollectionActions = new Queue<DeferredCollectionAction<TSourceItem>>();

		[ObservableComputationsCall]
		public CollectionPausing(
			INotifyCollectionChanged source,
			bool initialIsPaused = false,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPaused = initialIsPaused;
			_source = source;
			_resumeType = resumeType;
			_thisAsSourceCollectionChangeProcessor = this;		  
		}

		[ObservableComputationsCall]
		public CollectionPausing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			bool initialIsPaused = false,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPaused = initialIsPaused;
			_sourceScalar = sourceScalar;
			_resumeType = resumeType;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public CollectionPausing(
			INotifyCollectionChanged source,
			IReadScalar<bool> isPausedScalar,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPausedScalar = isPausedScalar;
			_source = source;
			_thisAsSourceCollectionChangeProcessor = this;
			_resumeType = resumeType;
		}

		[ObservableComputationsCall]
		public CollectionPausing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<bool> isPausedScalar,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPausedScalar = isPausedScalar;
			_sourceScalar = sourceScalar;
			_thisAsSourceCollectionChangeProcessor = this;
			_resumeType = resumeType;
		}

		private void initializeIsPausedScalar()
		{
			if (_isPausedScalar != null)
			{
				_isPausedScalar.PropertyChanged += handleIsPausedScalarValueChanged;
				_isPaused = _isPausedScalar.Value;
			}
		}

		private void handleIsPausedScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			if (_sourceReadAndSubscribed) checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			bool newValue = _isPausedScalar.Value;
			_resuming = _isPaused != newValue && !newValue;
			_isPaused = newValue;

			if (_resuming) resume();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		protected override void processSource()
		{
			processSource(true);
		}

		private void processSource(bool replaceSource)
		{
			int originalCount = _items.Count;

			if (_sourceReadAndSubscribed)
			{		
				if (replaceSource)
					Utils.unsubscribeSource(
						_source, 
						ref _sourceAsINotifyPropertyChanged, 
						this, 
						handleSourceCollectionChanged);

				_sourceReadAndSubscribed = false;
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
						(ISourceIndexerPropertyTracker)this,
						_source,
						handleSourceCollectionChanged);

				int sourceIndex = 0;

				if (_isPaused)
				{
					_deferredCollectionActions.Enqueue(
						new DeferredCollectionAction<TSourceItem>(_sourceAsList.ToArray()));
				}
				else
				{
					int count = _sourceAsList.Count;
					TSourceItem[] sourceCopy = new TSourceItem[count];
					_sourceAsList.CopyTo(sourceCopy, 0);

					for (; sourceIndex < count; sourceIndex++)
					{
						if (originalCount > sourceIndex)
							_items[sourceIndex] = sourceCopy[sourceIndex];
						else
							_items.Insert(sourceIndex, sourceCopy[sourceIndex]);
					}					
				}

				if (!_isPaused)
				{
					for (int index = originalCount - 1; index >= sourceIndex; index--)
					{
						_items.RemoveAt(index);
					}
				}
		  
				_sourceReadAndSubscribed = true;
			}			
			else 
			{
				if (_isPaused)
					_deferredCollectionActions.Enqueue(new DeferredCollectionAction<TSourceItem>(true));
				else
					_items.Clear();
			}

			if (_isPaused)
				_deferredCollectionActions.Enqueue(new DeferredCollectionAction<TSourceItem>(true, _handledEventSender, _handledEventArgs));
			else
				reset();
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_isPaused && e.Action != NotifyCollectionChangedAction.Reset)
			{
				if (_resumeType == CollectionPausingResumeType.ReplayChanges)
					_deferredCollectionActions.Enqueue(new DeferredCollectionAction<TSourceItem>(sender, e));
				_indexerPropertyChangedEventRaised = false;
				return;
			}

			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _countPropertyChangedEventRaised,
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourceTickTackVersion, 
				_sourceAsIHasTickTackVersion, 
				ref _handledEventSender, 
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, this)) return;

			_thisAsSourceCollectionChangeProcessor.processSourceCollectionChanged(sender, e);

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		void ISourceCollectionChangeProcessor.processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
					baseInsertItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
					break;
				case NotifyCollectionChangedAction.Remove:
					// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
					baseRemoveItem(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
					baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex1 = e.OldStartingIndex;
					int newStartingIndex1 = e.NewStartingIndex;
					if (oldStartingIndex1 != newStartingIndex1)
						baseMoveItem(oldStartingIndex1, newStartingIndex1);

					break;
				case NotifyCollectionChangedAction.Reset:
					processSource(false);
					break;
			}
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source);
			(_isPausedScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _sourceScalar, _source);
			(_isPausedScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		protected override void initialize()
		{	 
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
			initializeIsPausedScalar();
		}

		protected override void uninitialize()
		{
			Utils.unsubscribeSourceScalar(_sourceScalar, scalarValueChangedHandler);
			if (_isPausedScalar != null) _isPausedScalar.PropertyChanged -= handleIsPausedScalarValueChanged;
			_deferredCollectionActions.Clear();
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source);
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _countPropertyChangedEventRaised, ref _indexerPropertyChangedEventRaised);
		}

		#endregion

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			if (!IsPaused)
				if (!source.SequenceEqual(this))
					throw new ValidateInternalConsistencyException("Consistency violation: CollectionPausing.1");
		}
	}

	internal struct DeferredCollectionAction<TSourceItem>
	{
		public readonly object EventSender;
		public readonly NotifyCollectionChangedEventArgs NotifyCollectionChangedEventArgs;
		public readonly EventArgs EventArgs;
		public readonly TSourceItem[] NewItems;
		public readonly bool Clear;
		public readonly bool Reset;

		public DeferredCollectionAction(object eventSender, NotifyCollectionChangedEventArgs eventArgs) : this()
		{
			EventSender = eventSender;
			NotifyCollectionChangedEventArgs = eventArgs;
		}

		public DeferredCollectionAction(TSourceItem[] newItems) : this()
		{
			NewItems = newItems;
		}

		public DeferredCollectionAction(bool clear) : this()
		{
			Clear = clear;
		}

		public DeferredCollectionAction(bool reset, object eventSender, EventArgs eventArgs) : this()
		{
			Reset = reset;
			EventSender = eventSender;
			EventArgs = eventArgs;
		}
	}

	public enum CollectionPausingResumeType
	{
		Reset,
		ReplayChanges
	}
}
