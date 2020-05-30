using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ObservableComputations
{
	public class Paging<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections
	{
		public INotifyCollectionChanged Source => _source;
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public int PageSize
		{
			get => _pageSize;
			set
			{
				int originalPageSize = _pageSize;
				int originalUpperIndex = _upperIndex;
				_pageSize = value;

				int sourceCount = _sourceAsList.Count;

				_pageCount = (int) Math.Ceiling(sourceCount  / (double) _pageSize);
				bool currentPageChanged = false;
				if (_currentPage > _pageCount)
				{
					_currentPage = _pageCount;
					currentPageChanged = true;		
				}

				_lowerIndex = _pageSize * (_currentPage - 1);
				_upperIndex = _lowerIndex + _pageSize;

				_isConsistent = false;
				if (originalPageSize < _pageSize)
				{
					int index = originalPageSize - 1;	
					for (var sourceIndex = originalUpperIndex; sourceIndex < sourceCount && sourceIndex < _upperIndex; sourceIndex++)
					{
						TSourceItem sourceItem = _sourceAsList[sourceIndex];
						baseInsertItem(index++, sourceItem);
					}
				}
				else if (originalPageSize > _pageSize)
				{
					int index = originalPageSize - 1;
					index = index <	Count ? index : Count - 1;

					for (; index >= _pageSize; index--)
					{
						baseRemoveItem(index);
					}
				}
				_isConsistent = true;
				raiseConsistencyRestored();

				OnPropertyChanged(Utils.PageSizePropertyChangedEventArgs);
				if (currentPageChanged) OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
			}
		}

		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				_isConsistent = false;

				_currentPage = value;
				if (_currentPage < 1) _currentPage = 1;
				if (_currentPage > _pageCount) _currentPage = _pageCount;

				_lowerIndex = _pageSize * (_currentPage - 1);
				_upperIndex = _lowerIndex + _pageSize;

				int sourceCount = _sourceAsList.Count;
				int index = 0;
				for (var sourceIndex = _lowerIndex; sourceIndex < sourceCount && sourceIndex < _upperIndex; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					baseSetItem(index++, sourceItem);
				}

				int count = Count;
				int removingIndex = index;

				for (; index < count; index++)
				{
					baseRemoveItem(removingIndex);
				}

				_isConsistent = true;
				raiseConsistencyRestored();

				OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
			}
		}

		public int PageCount => _pageCount;
		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private int _pageSize;
		private int _currentPage;
		private int _pageCount;

		private int _lowerIndex;
		private int _upperIndex;


		[ObservableComputationsCall]
		public Paging(
			INotifyCollectionChanged source,
			int pageSize,
			int initialPage = 1)
		{
			_pageSize = pageSize;
			_currentPage = initialPage;
			_source = source;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public Paging(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			int pageSize,
			int initialPage = 1) 
		{
			_pageSize = pageSize;
			_currentPage = initialPage;
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
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
				_pageCount = 0;
				_currentPage = 1;
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
			_sourceAsList = _source as IList<TSourceItem>;

			if (_sourceAsList != null)
			{
				_sourceAsIHasChangeMarker = _sourceAsList as IHasChangeMarker;

				if (_sourceAsIHasChangeMarker != null)
				{
					_lastProcessedSourceChangeMarker = _sourceAsIHasChangeMarker.ChangeMarker;
				}
				else
				{
					_sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourceAsList;

					_sourcePropertyChangedEventHandler = (sender1, args1) =>
					{
						if (args1.PropertyName == "Item[]")
							_indexerPropertyChangedEventRaised =
								true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_sourceWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

					_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;
				}

				int index = 0;
				int count = _sourceAsList.Count;

				_pageCount = (int) Math.Ceiling(count  / (double) _pageSize);
				bool currentPageChanged = false;
				if (_currentPage > _pageCount)
				{
					_currentPage = _pageCount;
					currentPageChanged = true;
				}

				_lowerIndex = _pageSize * (_currentPage - 1);
				_upperIndex = _lowerIndex + _pageSize;
	
				for (var sourceIndex = _lowerIndex; sourceIndex < count && sourceIndex < _upperIndex; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					baseInsertItem(index++, sourceItem);
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
				_sourceWeakNotifyCollectionChangedEventHandler =
					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);
				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;

				if (currentPageChanged) OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
			}
			else
			{
				if (_pageCount != 0)
				{
					_pageCount = 0;
				}

				if (_currentPage != 1)
				{
					_currentPage = 1;
				}				
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsIHasChangeMarker.ChangeMarker)
			{
				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;
				_indexerPropertyChangedEventRaised = false;

				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
						_isConsistent = false;
						int newStartingIndex = e.NewStartingIndex;

						int originalPageCount2 = _pageCount;
						int sourceCount2 = _sourceAsList.Count;
						_pageCount = (int) Math.Ceiling(sourceCount2  / (double) _pageSize);

						if (newStartingIndex < _lowerIndex)
						{
							baseInsertItem(0,  _sourceAsList[_lowerIndex]);
							
						}
						else if (newStartingIndex < _upperIndex)
						{
							baseInsertItem(newStartingIndex - _lowerIndex, (TSourceItem) e.NewItems[0]);
						}
						else
						{
							_isConsistent = true;
							raiseConsistencyRestored();

							if (_pageCount != originalPageCount2)
								OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);							
						}

						if (Count > _pageSize) baseRemoveItem(_pageSize);

						_isConsistent = true;
						raiseConsistencyRestored();

						if (_pageCount != originalPageCount2)
							OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);
						break;
					case NotifyCollectionChangedAction.Remove:
						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
						_isConsistent = false;
						var oldStartingIndex = e.OldStartingIndex;

						int originalPageCount1 = _pageCount;
						int originalCurrentPage1 = _currentPage;

						int sourceCount = _sourceAsList.Count;
						_pageCount = (int) Math.Ceiling(sourceCount  / (double) _pageSize);

						if (oldStartingIndex < _lowerIndex)
						{
							baseRemoveItem(0);
						}
						else if (oldStartingIndex < _upperIndex)
						{
							baseRemoveItem(oldStartingIndex - _lowerIndex);
						}
						else
						{
							_isConsistent = true;
							raiseConsistencyRestored();

							if (_pageCount != originalPageCount1)
								OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);

							return;
						}	

						int count = Count;						

						if (count < _pageSize && sourceCount > _upperIndex - 1) 
							baseInsertItem(_pageSize - 1, _sourceAsList[_upperIndex - 1]);						
						else if (count == 0 && _currentPage > 1)
						{
							_currentPage = _currentPage - 1;
							_lowerIndex = _pageSize * (_currentPage - 1);
							_upperIndex = _lowerIndex + _pageSize;

							int index = 0;

							for (var sourceIndex = _lowerIndex; sourceIndex < sourceCount && sourceIndex < _upperIndex; sourceIndex++)
							{
								TSourceItem sourceItem = _sourceAsList[sourceIndex];
								baseInsertItem(index++, sourceItem);
							}
						}

						_isConsistent = true;
						raiseConsistencyRestored();

						if (_pageCount != originalPageCount1)
							OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);

						if (_currentPage != originalCurrentPage1)
							OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
						break;
					case NotifyCollectionChangedAction.Replace:
						int newStartingIndex2 = e.NewStartingIndex;
						if (newStartingIndex2 >= _lowerIndex && newStartingIndex2 < _upperIndex
						    && (newStartingIndex2 >= _lowerIndex && newStartingIndex2 < _upperIndex))
						{
							baseSetItem(newStartingIndex2 - _lowerIndex, (TSourceItem) e.NewItems[0]);
						}
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;

						if (oldStartingIndex1 == newStartingIndex1) return;

						if ((oldStartingIndex1 >= _lowerIndex && oldStartingIndex1 < _upperIndex)
						    && (newStartingIndex1 >= _lowerIndex && newStartingIndex1 < _upperIndex))
						{
							baseMoveItem(oldStartingIndex1 - _lowerIndex, newStartingIndex1 - _lowerIndex);
						}
						else if ((oldStartingIndex1 < _lowerIndex || oldStartingIndex1 >= _upperIndex)
						         && (newStartingIndex1 >= _lowerIndex && newStartingIndex1 < _upperIndex))
						{
							_isConsistent = false;
							if (oldStartingIndex1 < _lowerIndex)
							{
								baseRemoveItem(0);
								baseInsertItem(newStartingIndex1 - _lowerIndex, (TSourceItem) e.NewItems[0]);
							}
							else
							{
								baseInsertItem(newStartingIndex1 - _lowerIndex, (TSourceItem) e.NewItems[0]);
								baseRemoveItem(Count - 1);								
							}

							_isConsistent = true;
							raiseConsistencyRestored();
						}
						else if ((newStartingIndex1 < _lowerIndex || newStartingIndex1 >= _upperIndex)
						         && (oldStartingIndex1 >= _lowerIndex && oldStartingIndex1 < _upperIndex))
						{
							_isConsistent = false;

							baseRemoveItem(oldStartingIndex1 - _lowerIndex);

							if (newStartingIndex1 < _lowerIndex)
								baseInsertItem(0, _sourceAsList[_lowerIndex]);
							else
								baseInsertItem(_pageSize - 1, _sourceAsList[_upperIndex - 1]);

							_isConsistent = true;
							raiseConsistencyRestored();
						}
						else if ((newStartingIndex1 < _lowerIndex && oldStartingIndex1 >= _upperIndex)
						         || (oldStartingIndex1 < _lowerIndex && newStartingIndex1 >= _upperIndex))
						{
							_isConsistent = false;

							if (oldStartingIndex1 < _lowerIndex)
							{
								baseRemoveItem(0);
								baseInsertItem(_pageSize - 1, _sourceAsList[_upperIndex - 1]);
							}
							else
							{
								baseInsertItem(0,  _sourceAsList[_lowerIndex]);
								baseRemoveItem(_pageSize);
							}

							_isConsistent = true;
							raiseConsistencyRestored();
						}

						break;
					case NotifyCollectionChangedAction.Reset:
						int originalPageCount = _pageCount;
						int originalCurrentPage = _currentPage;
						_isConsistent = false;
						initializeFromSource();
						_isConsistent = true;
						raiseConsistencyRestored();

						if (_pageCount != originalPageCount)
							OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);

						if (_currentPage != originalCurrentPage)
							OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);

						break;
				}
			}
		}

		~Paging()
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
			int pageSize = PageSize;
			int startIndex =  (CurrentPage - 1) * pageSize;


			if (_lowerIndex != PageSize * (CurrentPage - 1))
				throw new ObservableComputationsException(this, "Consistency violation: Paging.2");

			if (_upperIndex != _lowerIndex + PageSize)
				throw new ObservableComputationsException(this, "Consistency violation: Paging.3");

			if (_pageCount != (int) Math.Ceiling(source.Count  / (double) _pageSize))
				throw new ObservableComputationsException(this, "Consistency violation: Paging.4");

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(startIndex).Take(pageSize)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Paging.1");
			}
		}
	}
}
