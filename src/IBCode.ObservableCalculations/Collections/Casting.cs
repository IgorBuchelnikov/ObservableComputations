using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class Casting<TResultItem> : CollectionCalculating<TResultItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private IList _sourceAsList;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private INotifyCollectionChanged _source;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		[ObservableCalculationsCall]
		public Casting(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Casting(
			INotifyCollectionChanged source) : base(Utils.getCapacity(source))
		{
			_source = source;
			initializeFromSource();
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				baseClearItems();	
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				_sourceNotifyCollectionChangedEventHandler = null;
				_sourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceScalar != null) _source =  _sourceScalar.Value;
			_sourceAsList = _source as IList;

			if (_sourceAsList != null)
			{
				int count = _sourceAsList.Count;
				for (int index = 0; index < count; index++)
				{
					object sourceItem = _sourceAsList[index];
					baseInsertItem(index, (TResultItem)sourceItem);
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
				_sourceWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
			}
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent();

			_consistent = false;
			OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

			initializeFromSource();

			_consistent = true;
			OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					IList newItems = e.NewItems;
					if (newItems.Count > 1) throw new ObservableCalculationsException("Adding of multiple items is not supported");
					object addedItem = newItems[0];
					baseInsertItem(e.NewStartingIndex, (TResultItem)addedItem);								
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems.Count > 1) throw new ObservableCalculationsException("Removing of multiple items is not supported");
					baseRemoveItem(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex = e.OldStartingIndex;
					int newStartingIndex = e.NewStartingIndex;
					if (oldStartingIndex == newStartingIndex) return;
					baseMoveItem(oldStartingIndex, newStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					IList newItems1 = e.NewItems;
					if (newItems1.Count > 1) throw new ObservableCalculationsException("Replacing of multiple items is not supported");
					object newItem = newItems1[0];
					baseSetItem(e.NewStartingIndex, (TResultItem)newItem);
					break;
				case NotifyCollectionChangedAction.Reset:
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					initializeFromSource();

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
			}			
		}

		~Casting()
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
			IList source = _sourceScalar.getValue(_source, new ObservableCollection<object>()) as IList;
			// ReSharper disable once PossibleNullReferenceException
			if (Count != source.Count) throw new ObservableCalculationsException("Consistency violation: Casting.1");

			for (int i = 0; i < source.Count; i++)
			{
				object sourceItem = source[i];
				TResultItem resultItem = this[i];

				if (!resultItem.IsSameAs(sourceItem)) throw new ObservableCalculationsException("Consistency violation: Casting.2");
			}
		}
	}
}
