using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class Aggregating<TSourceItem, TResult> : ScalarCalculating<TResult>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResult, TResult> AggregateFunc => _aggregateFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResult, TResult> DeaggregateFunc => _deaggregateFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private IList<TSourceItem> _sourceAsList;

		private List<TSourceItem> _sourceItems;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TResult, TResult> _aggregateFunc;
		private readonly Func<TSourceItem, TResult, TResult> _deaggregateFunc;
		private INotifyCollectionChanged _source;

		[ObservableCalculationsCall]
		public Aggregating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			(Func<TSourceItem, TResult, TResult> aggregateFunc,
			Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs) : this(funcs, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Aggregating(
			INotifyCollectionChanged source,
			(Func<TSourceItem, TResult, TResult> aggregateFunc,
			Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
			: this(funcs, Utils.getCapacity(source))
		{
			_source = source;
			initializeFromSource();
		}

		private Aggregating((Func<TSourceItem, TResult, TResult> aggregateFunc, Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs, int capacity)
		{
			_sourceItems = new List<TSourceItem>(capacity);
			_aggregateFunc = funcs.aggregateFunc;
			_deaggregateFunc = funcs.deaggregateFunc;
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
				_sourceItems = new List<TSourceItem>(capacity);

				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				_sourceNotifyCollectionChangedEventHandler = null;
				_sourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceScalar != null) _source = _sourceScalar.Value;
			_sourceAsList = (IList<TSourceItem>) _source;

			if (_source != null)
			{
				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
				_sourceWeakNotifyCollectionChangedEventHandler =
					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				calculateValueAndRegisterSourceItems();
			}
			else
			{
				setValue(default(TResult));		
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems.Count > 1) throw new ObservableCalculationsException("Adding of multiple items is not supported");
					int newIndex = e.NewStartingIndex;
					TSourceItem addedSourceItem = _sourceAsList[newIndex];
					_sourceItems.Insert(newIndex, addedSourceItem);
					setValue(_aggregateFunc(addedSourceItem, Value));					
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems.Count > 1) throw new ObservableCalculationsException("Removing of multiple items is not supported");
					int oldStartingIndex = e.OldStartingIndex;
					TSourceItem removedSourceItem = _sourceItems[oldStartingIndex];
					_sourceItems.RemoveAt(oldStartingIndex);
					setValue(_deaggregateFunc(removedSourceItem, Value));
					break;
				case NotifyCollectionChangedAction.Replace:
					if (e.NewItems.Count > 1) throw new ObservableCalculationsException("Replacing of multiple items is not supported");
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem newItem = _sourceAsList[newStartingIndex];
					TSourceItem oldItem = _sourceItems[newStartingIndex];
					_sourceItems[newStartingIndex] = newItem;
					TResult result = _deaggregateFunc(oldItem, Value);
					setValue(_aggregateFunc(newItem, result));
					break;
				case NotifyCollectionChangedAction.Move:
					if (e.OldStartingIndex == e.NewStartingIndex) return;
					int oldStartingIndex1 = e.OldStartingIndex;
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem movingItem = _sourceItems[oldStartingIndex1];
					_sourceItems.RemoveAt(oldStartingIndex1);
					_sourceItems.Insert(newStartingIndex1, movingItem);
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
			}			

		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			initializeFromSource();
		}

		private void calculateValueAndRegisterSourceItems()
		{
			_value = default(TResult);
			int count = _sourceAsList.Count;
			for (int index = 0; index < count; index++)
			{
				TSourceItem sourceItem = _sourceAsList[index];
				_value = _aggregateFunc(sourceItem, _value);
				_sourceItems.Add(sourceItem);
			}
			raiseValueChanged();
		}

		~Aggregating()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}
		}

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			if (_sourceItems.Count != sourceCount) throw new ObservableCalculationsException("Consistency violation: Aggregating.1");
			TResult result = default(TResult);

			for (int i = 0; i < sourceCount; i++)
			{
				TSourceItem sourceItem = source[i];
				TSourceItem savedSourceItem = _sourceItems[i];
				result = _aggregateFunc(sourceItem, result);
				if (!savedSourceItem.IsSameAs(sourceItem)) throw new ObservableCalculationsException("Consistency violation: Aggregating.2");
			}

			// ReSharper disable once PossibleNullReferenceException
			if (!result.Equals(_value)) throw new ObservableCalculationsException("Consistency violation: Aggregating.3");
		}
	}
}
