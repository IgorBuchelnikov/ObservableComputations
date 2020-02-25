using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ObservableComputations;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	public class MinimazingOrMaximazing<TSourceItem> : ScalarComputing<TSourceItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once ConvertToAutoProperty
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IComparer<TSourceItem>> ComparerScalar => _comparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public MinimazingOrMaximazingMode Mode => _mode;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public IComparer<TSourceItem> Comparer => _comparer;

		// ReSharper disable once MemberCanBePrivate.Global
		public TSourceItem DefaultValue => _defaultValue;

		// ReSharper disable once MemberCanBePrivate.Global
		public bool IsDefaulted => _isDefaulted;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private PropertyChangedEventHandler _comparerScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _comparerScalarWeakPropertyChangedEventHandler;

		// ReSharper disable once StaticMemberInGenericType
		private static readonly Func<int, bool> _checkCompareResultPositive = compareResult => compareResult > 0;
		// ReSharper disable once StaticMemberInGenericType
		private static readonly Func<int, bool> _checkCompareResultNegative = compareResult => compareResult < 0;
		private readonly Func<int, bool> _checkCompareResult; 
		private readonly Func<int, bool> _antiCheckCompareResult;

		private IList<TSourceItem> _sourceAsList;

		private int _valueCount;
		private List<TSourceItem> _sourceItems;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly IReadScalar<IComparer<TSourceItem>> _comparerScalar;
		private readonly MinimazingOrMaximazingMode _mode;
		private INotifyCollectionChanged _source;
		private IComparer<TSourceItem> _comparer;
		private TSourceItem _defaultValue;
		private bool _isDefaulted;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsObservableCollectionWithChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private void initializeComparerScalar()
		{
			if (_comparerScalar != null)
			{
				_comparerScalarPropertyChangedEventHandler = handleComparerScalarValueChanged;
				_comparerScalarWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(_comparerScalarPropertyChangedEventHandler);
				_comparerScalar.PropertyChanged += _comparerScalarWeakPropertyChangedEventHandler.Handle;
				_comparer = _comparerScalar.Value ?? Comparer<TSourceItem>.Default;
			}
			else
			{
				_comparer = Comparer<TSourceItem>.Default;
			}
		}

		private void initializeSourceScalar()
		{
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			MinimazingOrMaximazingMode mode,
			IComparer<TSourceItem> comparer = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_comparer = comparer ?? Comparer<TSourceItem>.Default;

			_defaultValue = defaultValue;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			MinimazingOrMaximazingMode mode,
			IReadScalar<IComparer<TSourceItem>> comparerScalar = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			_defaultValue = defaultValue;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			INotifyCollectionChanged source,
			MinimazingOrMaximazingMode mode,
			IComparer<TSourceItem> comparer = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(source))
		{
			_source = source;

			_comparer = comparer ?? Comparer<TSourceItem>.Default;

			_defaultValue = defaultValue;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			INotifyCollectionChanged source,
			MinimazingOrMaximazingMode mode,
			IReadScalar<IComparer<TSourceItem>> comparerScalar = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(source))
		{
			_source = source;

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			_defaultValue = defaultValue;

			initializeFromSource();
		}

		private MinimazingOrMaximazing(MinimazingOrMaximazingMode mode, int capacity)
		{
			_sourceItems = new List<TSourceItem>(capacity);
			_mode = mode;
			switch (_mode)
			{
				case MinimazingOrMaximazingMode.Maximazing:
					_checkCompareResult = _checkCompareResultPositive;
					_antiCheckCompareResult = _checkCompareResultNegative;
					break;
				case MinimazingOrMaximazingMode.Minimazing:
					_checkCompareResult = _checkCompareResultNegative;
					_antiCheckCompareResult = _checkCompareResultPositive;
					break;
			}
		}

		private void handleComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			_comparer = _comparerScalar.Value ?? Comparer<TSourceItem>.Default;
			initializeFromSource();
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			initializeFromSource();
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

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_sourceItems = new List<TSourceItem>(capacity);
			if (_sourceScalar != null) _source = _sourceScalar.Value;
			_sourceAsList = (IList<TSourceItem>) _source;

			if (_source != null)
			{
				_sourceAsObservableCollectionWithChangeMarker = _sourceAsList as ObservableCollectionWithChangeMarker<TSourceItem>;

				if (_sourceAsObservableCollectionWithChangeMarker != null)
				{
					_lastProcessedSourceChangeMarker = _sourceAsObservableCollectionWithChangeMarker.ChangeMarker;
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

			recalculateValue(true);
		}

		private void recalculateValue(bool initializeSourceItems = false)
		{
			int count = _sourceAsList?.Count ?? 0;

			if (count > 0)
			{
				// ReSharper disable once PossibleNullReferenceException
				TSourceItem value = _sourceAsList[0];
				_valueCount = 1;
				if (initializeSourceItems) _sourceItems.Add(value);
				for (int sourceIndex = 1; sourceIndex < count; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					if (initializeSourceItems) _sourceItems.Add(sourceItem);

					int compareResult = _comparer.Compare(sourceItem, value);
					if (_checkCompareResult(compareResult))
					{
						value = sourceItem;
						_valueCount = 1;
					}
					else if (compareResult == 0)
					{
						_valueCount++;
					}
				}

				if (_isDefaulted)
				{
					_isDefaulted = false;
					raisePropertyChanged(nameof(IsDefaulted));
				}

				setValue(value);
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
			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsObservableCollectionWithChangeMarker.ChangeMarker)
			{
				_indexerPropertyChangedEventRaised = false;
				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
						int newIndex = e.NewStartingIndex;
						TSourceItem addedSourceItem = _sourceAsList[newIndex];
						_sourceItems.Insert(newIndex, addedSourceItem);

						if (!_isDefaulted)
						{
							int compareResult = _comparer.Compare(addedSourceItem, Value);
							if (_checkCompareResult(compareResult))
							{
								_valueCount = 1;
								setValue(addedSourceItem);
							}
							else if (compareResult == 0)
							{
								_valueCount++;
							}	
						}
						else
						{
							_isDefaulted = false;
							raisePropertyChanged(nameof(IsDefaulted));
							_valueCount = 1;
							setValue(addedSourceItem);
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
						int oldStartingIndex = e.OldStartingIndex;
						TSourceItem removingSourceItem = _sourceItems[oldStartingIndex];
						_sourceItems.RemoveAt(oldStartingIndex);

						if (_comparer.Compare(removingSourceItem, Value) == 0)
						{
							_valueCount--;
							if (_valueCount == 0)
							{
								recalculateValue();
							}
						}
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
						int replacingSourceIndex = e.NewStartingIndex;
						TSourceItem newItem = _sourceAsList[e.NewStartingIndex];
						TSourceItem oldItem = _sourceItems[e.NewStartingIndex];

						int newCompareResult = _comparer.Compare(newItem, Value);
						int oldCompareResult = _comparer.Compare(oldItem, Value);

						if (_checkCompareResult(newCompareResult))
						{
							_valueCount = 1;
							setValue(newItem);
						}
						else if (_antiCheckCompareResult(newCompareResult))
						{
							if (oldCompareResult == 0)
							{
								_valueCount--;
								if (_valueCount == 0)
								{
									recalculateValue();
								}
							}
						}
						else //if (newCompareResult == 0)
						{
							if (_antiCheckCompareResult(oldCompareResult))
							{
								_valueCount++;
							}
						}

						_sourceItems[replacingSourceIndex] = newItem;

						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 == newStartingIndex1) return;
						TSourceItem movingSourceItem = _sourceItems[oldStartingIndex1];
						_sourceItems.RemoveAt(oldStartingIndex1);
						_sourceItems.Insert(newStartingIndex1, movingSourceItem);
						break;
					case NotifyCollectionChangedAction.Reset:
						initializeFromSource();
						break;
				}	
			}					
		}


		~MinimazingOrMaximazing()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_comparerScalarWeakPropertyChangedEventHandler != null)
			{
				_comparerScalar.PropertyChanged -= _comparerScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_sourceAsINotifyPropertyChanged != null)
				_sourceAsINotifyPropertyChanged.PropertyChanged -=
					_sourceWeakPropertyChangedEventHandler.Handle;
		}

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			if (_sourceItems.Count != source.Count) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.1");
			TSourceItem defaultValue = _defaultValue;

			for (int i = 0; i < source.Count; i++)
			{
				TSourceItem sourceItem = source[i];
				TSourceItem savedSourceItem = _sourceItems[i];
				if (!savedSourceItem.IsSameAs(sourceItem)) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.2");
			}

			if (source.Count > 0)
			{
				TSourceItem result = _mode == MinimazingOrMaximazingMode.Maximazing 
					?  source.Max() 
					: source.Min();

				if (!result.Equals(_value)) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.3");

				if (source.Count(i => i.Equals(result)) != _valueCount) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.4");

				if (IsDefaulted) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.4");
			}
			else
			{
				if (!defaultValue.Equals(_value)) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.5");
				if (!IsDefaulted) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.4");
			}

		}
	}

	public enum MinimazingOrMaximazingMode
	{
		Maximazing,
		Minimazing
	}
}
