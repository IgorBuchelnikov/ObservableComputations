// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ObservableComputations
{
	public class Aggregating<TSourceItem, TResult> : ScalarComputing<TResult>, IHasSources, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResult, TResult> AggregateFunc => _aggregateFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResult, TResult> DeaggregateFunc => _deaggregateFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		private IList<TSourceItem> _sourceAsList;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TResult, TResult> _aggregateFunc;
		private readonly Func<TSourceItem, TResult, TResult> _deaggregateFunc;
		private INotifyCollectionChanged _source;

		private bool _countPropertyChangedEventRaised;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasTickTackVersion _sourceAsIHasTickTackVersion;
		private bool _lastProcessedSourceTickTackVersion;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		[ObservableComputationsCall]
		public Aggregating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Func<TSourceItem, TResult, TResult> aggregateFunc,
			Func<TSourceItem, TResult, TResult> deaggregateFunc) : this(aggregateFunc, deaggregateFunc)
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Aggregating(
			INotifyCollectionChanged source,
			Func<TSourceItem, TResult, TResult> aggregateFunc,
			Func<TSourceItem, TResult, TResult> deaggregateFunc)
			: this(aggregateFunc, deaggregateFunc)
		{
			_source = source;
		}

		private Aggregating(Func<TSourceItem, TResult, TResult> aggregateFunc, Func<TSourceItem, TResult, TResult> deaggregateFunc)
		{
			_aggregateFunc = aggregateFunc;
			_deaggregateFunc = deaggregateFunc;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		protected override void processSource()
		{
			processSource(true);
		}

		private void processSource(bool replaceSource)
		{
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

			if (_isActive && _source != null)
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

				TResult value = default(TResult);
				int count = _sourceAsList.Count;
				for (int index = 0; index < count; index++)
					value = aggregate(_sourceAsList[index], value);
				setValue(value);

				_sourceReadAndSubscribed = true;
			}
			else
				setDefaultValue();
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
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
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
					TSourceItem addedSourceItem = (TSourceItem) e.NewItems[0];
					setValue(aggregate(addedSourceItem, _value));
					break;
				case NotifyCollectionChangedAction.Remove:
					//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
					TSourceItem removedSourceItem = (TSourceItem) e.OldItems[0];
					setValue(deaggregate(removedSourceItem, _value));
					break;
				case NotifyCollectionChangedAction.Replace:
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
					TSourceItem newItem = (TSourceItem) e.NewItems[0];
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TResult result = deaggregate(oldItem, _value);
					setValue(aggregate(newItem, result));
					break;
				case NotifyCollectionChangedAction.Reset:
					processSource(false);
					break;
			}
		}

		private TResult aggregate(TSourceItem addedSourceItem, TResult value)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TResult result = _aggregateFunc(addedSourceItem, value);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return _aggregateFunc(addedSourceItem, value);
		}

		private TResult deaggregate(TSourceItem removedSourceItem, TResult value)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TResult result = _deaggregateFunc(removedSourceItem, value);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return _deaggregateFunc(removedSourceItem, value);
		}


		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		protected override void initialize()
		{
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
		}

		protected override void uninitialize()
		{
			Utils.unsubscribeSourceScalar(_sourceScalar, scalarValueChangedHandler);
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source);
		}

		internal override void InitializeInvolvedMembersTreeNodeImpl(InvolvedMembersTreeNode involvedMembersTreeNode)
		{
			Utils.AddInvolvedMembersTreeNodeChild(involvedMembersTreeNode, _sourceScalar);
			Utils.AddInvolvedMembersTreeNodeChild(involvedMembersTreeNode, _source);
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			TResult result = default(TResult);

			for (int i = 0; i < sourceCount; i++)
				result = _aggregateFunc(source[i], result);

			// ReSharper disable once PossibleNullReferenceException
			if (!result.Equals(_value)) throw new ValidateInternalConsistencyException("Consistency violation: Aggregating.3");
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _countPropertyChangedEventRaised, ref _indexerPropertyChangedEventRaised);
		}

		#endregion
	}
}
