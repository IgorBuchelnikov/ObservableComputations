using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	public class ItemComputing<TSourceItem> : ScalarComputing<TSourceItem>, IHasSourceCollections
	{
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> IndexValueScalar => _indexValueScalar;

		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public int Index => _index;

		// ReSharper disable once MemberCanBePrivate.Global
		public bool IsDefaulted => _isDefaulted;

		// ReSharper disable once MemberCanBeProtected.Global
		public TSourceItem DefaultValue => _defaultValue;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		protected readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private PropertyChangedEventHandler _indexScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _indexScalarWeakPropertyChangedEventHandler;

		protected INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<int> _indexValueScalar;
		private int _index;
		private bool _isDefaulted;
		internal TSourceItem _defaultValue;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsObservableCollectionWithChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private void initializeIndexScalar()
		{
			_indexScalarPropertyChangedEventHandler = handleIndexScalarValueChanged;
			_indexScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_indexScalarPropertyChangedEventHandler);
			_indexValueScalar.PropertyChanged += _indexScalarWeakPropertyChangedEventHandler.Handle;
			_index = _indexValueScalar.Value;
		}

		private void initializeSourceScalar()
		{
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		[ObservableComputationsCall]
		public ItemComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			int index, 
			TSourceItem defaultValue = default(TSourceItem))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_index = index;
			_defaultValue = defaultValue;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ItemComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<int> indexScalar, 
			TSourceItem defaultValue = default(TSourceItem))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_defaultValue = defaultValue;

			_indexValueScalar = indexScalar;
			initializeIndexScalar();

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ItemComputing(
			INotifyCollectionChanged source,
			int index, 
			TSourceItem defaultValue = default(TSourceItem))
		{
			_source = source;
			_index = index;
			_defaultValue = defaultValue;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ItemComputing(
			INotifyCollectionChanged source,
			IReadScalar<int> indexScalar, 
			TSourceItem defaultValue = default(TSourceItem))
		{
			_source = source;

			_defaultValue = defaultValue;

			_indexValueScalar = indexScalar;
			initializeIndexScalar();

			initializeFromSource();
		}


		private void handleIndexScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;

			_processingEventSender = sender;
			_processingEventArgs = e;

			_index = _indexValueScalar.Value;
			recalculateValue();

			_processingEventSender = null;
			_processingEventArgs = null;
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;

			_processingEventSender = sender;
			_processingEventArgs = e;

			initializeFromSource();

			_processingEventSender = null;
			_processingEventArgs = null;
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				_sourceNotifyCollectionChangedEventHandler = null;
				_sourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceAsINotifyPropertyChanged != null)
			{
				_sourceAsINotifyPropertyChanged.PropertyChanged -=
					_sourceWeakPropertyChangedEventHandler.Handle;

				_sourceAsINotifyPropertyChanged = null;
				_sourcePropertyChangedEventHandler = null;
				_sourceWeakPropertyChangedEventHandler = null;
			}

			if (_sourceScalar != null) _source = _sourceScalar.Value;
			_sourceAsList = (IList<TSourceItem>) _source;

			if (_source != null)
			{
				_sourceAsObservableCollectionWithChangeMarker = _sourceAsList as ObservableCollectionWithChangeMarker<TSourceItem>;

				if (_sourceAsObservableCollectionWithChangeMarker != null)
				{
					_lastProcessedSourceChangeMarker = _sourceAsObservableCollectionWithChangeMarker.ChangeMarkerField;
				}
				else
				{
					_sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourceAsList;

					_sourcePropertyChangedEventHandler = (sender, args) =>
					{
						if (args.PropertyName == "Item[]")
							_indexerPropertyChangedEventRaised =
								true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_sourceWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

					_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
				_sourceWeakNotifyCollectionChangedEventHandler =
					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
			}

			recalculateValue();
		}

		private void recalculateValue()
		{
			if (_sourceAsList != null && _sourceAsList.Count > _index)
			{
				if (_isDefaulted)
				{
					_isDefaulted = false;
					raisePropertyChanged(nameof(IsDefaulted));
				}

				setValue(_sourceAsList[_index]);
			}
			else
			{
				if (!_isDefaulted)
				{
					_isDefaulted = true;
					raisePropertyChanged(nameof(IsDefaulted));
				}

				setValue(_defaultValue);
			}
		}


		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			_processingEventSender = sender;
			_processingEventArgs = e;

			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsObservableCollectionWithChangeMarker.ChangeMarkerField)
			{
				_indexerPropertyChangedEventRaised = false;
				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						if (e.NewItems.Count > 1) throw new Exception("Adding of multiple items is not supported");
						if (e.NewStartingIndex <= _index) recalculateValue();	
						break;
					case NotifyCollectionChangedAction.Remove:
						if (e.OldItems.Count > 1) throw new Exception("Removing of multiple items is not supported");
						if (e.OldStartingIndex <= _index) recalculateValue();	
						break;
					case NotifyCollectionChangedAction.Replace:
						if (e.NewItems.Count > 1) throw new Exception("Replacing of multiple items is not supported");
						if (e.OldStartingIndex == _index) recalculateValue();
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex = e.OldStartingIndex;
						int newStartingIndex = e.NewStartingIndex;
						if (newStartingIndex == oldStartingIndex) return;
						if (newStartingIndex < oldStartingIndex)
						{
							if (_index >= newStartingIndex && _index <= oldStartingIndex)
								setValue(_sourceAsList[_index]);
						}
						else
						{
							if (_index >= oldStartingIndex && _index <= newStartingIndex)
								setValue(_sourceAsList[_index]);						
						}
						break;
					case NotifyCollectionChangedAction.Reset:
						initializeFromSource();
						break;
				}
			}
			
			_processingEventSender = null;
			_processingEventArgs = null;
		}

		~ItemComputing()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_indexScalarWeakPropertyChangedEventHandler != null)
			{
				_indexValueScalar.PropertyChanged -= _indexScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_sourceAsINotifyPropertyChanged != null)
				_sourceAsINotifyPropertyChanged.PropertyChanged -=
					_sourceWeakPropertyChangedEventHandler.Handle;
		}

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int index = _indexValueScalar.getValue(_index);
			TSourceItem defaultValue = _defaultValue;

			// ReSharper disable once PossibleNullReferenceException
			if (source.Count > index)
			{
				if (!source[index].IsSameAs(_value))
					throw new ObservableComputationsException(this, "Consistency violation: ItemComputing.1");
			}
			else
			{
				if (!defaultValue.IsSameAs(_value))
					throw new ObservableComputationsException(this, "Consistency violation: ItemComputing.2");			
			}
		}

	}
}
