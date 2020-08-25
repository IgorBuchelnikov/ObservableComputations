using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ObservableComputations
{
	public class Paging<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		public INotifyCollectionChanged Source => _source;
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;
		public IReadScalar<int> PageSizeScalar => _pageSizeScalar;
		public IReadScalar<int> CurrentPageScalar => _currentPageScalar;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public int PageSize
		{
			get => _pageSize;
			set
			{
				if (_pageSizeScalar != null) throw new ObservableComputationsException("Modifying of PageSize property is controlled by PageSizeScalar");

				int originalPageSize = _pageSize;
				_pageSize = value;

				processPageSizeChanged(originalPageSize);
			}
		}

		private void processPageSizeChanged(int originalPageSize)
		{
			int originalUpperIndex = _upperIndex;
			int sourceCount = _sourceAsList.Count;

			_pageCount = (int) Math.Ceiling(sourceCount / (double) _pageSize);
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
				for (var sourceIndex = originalUpperIndex;
					sourceIndex < sourceCount && sourceIndex < _upperIndex;
					sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					baseInsertItem(index++, sourceItem);
				}
			}
			else if (originalPageSize > _pageSize)
			{
				int index = originalPageSize - 1;
				index = index < Count ? index : Count - 1;

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

		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				if (_currentPageScalar != null) throw new ObservableComputationsException("Modifying of CurrentPage property is controlled by CurrentPagecalar");

				_currentPage = value;

				processCurentPageChanged();
			}
		}

		private void processCurentPageChanged()
		{
			_isConsistent = false;
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

		public int PageCount => _pageCount;
		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private bool _sourceInitialized;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IReadScalar<int> _pageSizeScalar;

		private IReadScalar<int> _currentPageScalar;

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
            checkPageSize();
            checkCurrentPage();
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
            checkPageSize();
            checkCurrentPage();
		}

		[ObservableComputationsCall]
		public Paging(
			INotifyCollectionChanged source,
			IReadScalar<int> pageSizeScalar,
			int initialPage = 1)
		{
			_pageSizeScalar = pageSizeScalar;
			_currentPage = initialPage;
			_source = source;
        }

		[ObservableComputationsCall]
		public Paging(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<int> pageSizeScalar,
			int initialPage = 1) 
		{
			_pageSizeScalar = pageSizeScalar;
			_currentPage = initialPage;
			_sourceScalar = sourceScalar;
            checkCurrentPage();
		}

		[ObservableComputationsCall]
		public Paging(
			INotifyCollectionChanged source,
			IReadScalar<int> pageSizeScalar,
			IReadScalar<int> currentPageScalar)
		{
			_pageSizeScalar = pageSizeScalar;
			_currentPageScalar = currentPageScalar;
			_source = source;
		}

		[ObservableComputationsCall]
		public Paging(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<int> pageSizeScalar,
			IReadScalar<int> currentPageScalar) 
		{
			_pageSizeScalar = pageSizeScalar;
			_currentPageScalar = currentPageScalar;
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Paging(
			INotifyCollectionChanged source,
			int pageSize,
			IReadScalar<int> currentPageScalar)
		{
			_pageSize = pageSize;
			_currentPageScalar = currentPageScalar;
			_source = source;
            checkPageSize();
		}

		[ObservableComputationsCall]
		public Paging(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			int pageSize,
			IReadScalar<int> currentPageScalar) 
		{
			_pageSize = pageSize;
			_currentPageScalar = currentPageScalar;
			_sourceScalar = sourceScalar;
            checkPageSize();
		}

        private void checkPageSize()
        {
            if (_pageSize <= 0)
                throw new ObservableComputationsException($"Invalid PageSize value '{_pageSize}' for Paging computation");
        }

        private void checkCurrentPage()
        {
            if (_currentPage <= 0)
                throw new ObservableComputationsException($"Invalid CurrentPage value '{_currentPage}' for Paging computation");
        }

        private void initializePageSizeScalar()
        {
            if (_pageSizeScalar != null)
            {
                _pageSizeScalar.PropertyChanged += handlePageSizeScalarValueChanged;
                _pageSize = _pageSizeScalar.Value;
                checkPageSize();
            }
        }

		private void handlePageSizeScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			int originalPageSize = _pageSize;
			_pageSize = _pageSizeScalar.Value;
            checkPageSize();
			processPageSizeChanged(originalPageSize);

			_handledEventSender = null;
			_handledEventArgs = null;
		}


        private void initializeCurrentPageScalar()
        {
            if (_currentPageScalar != null)
            {
                _currentPageScalar.PropertyChanged += handleCurrentPageScalarValueChanged;
                _currentPage = _currentPageScalar.Value;
                checkCurrentPage();
            }
        }

		private void handleCurrentPageScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			_currentPage = _currentPageScalar.Value;
            checkCurrentPage();
			processCurentPageChanged();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		protected override void initializeFromSource()
		{
			int originalCount = _items.Count;

			if (_sourceInitialized)
			{
				_source.CollectionChanged -= handleSourceCollectionChanged;

                if (_sourceAsINotifyPropertyChanged != null)
                {
                    _sourceAsINotifyPropertyChanged.PropertyChanged -=
                        ((ISourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
                    _sourceAsINotifyPropertyChanged = null;
                }

                _sourceInitialized = false;
            }

            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				Utils.initializeFromHasChangeMarker(
                    ref _sourceAsIHasChangeMarker, 
                    _sourceAsList, 
                    ref _lastProcessedSourceChangeMarker, 
                    ref _sourceAsINotifyPropertyChanged,
                    (ISourceIndexerPropertyTracker)this);

                int newIndex = 0;
				int count = _sourceAsList.Count;

				_pageCount = (int) Math.Ceiling(count  / (double) _pageSize);
				bool currentPageChanged = false;
				if (_currentPage > _pageCount)
				{
					_currentPage = _pageCount > 0 ? _pageCount : 1;
					currentPageChanged = true;
				}

				_lowerIndex = _pageSize * (_currentPage - 1);
				_upperIndex = _lowerIndex + _pageSize;
	
				for (var sourceIndex = _lowerIndex; sourceIndex < count && sourceIndex < _upperIndex; sourceIndex++)
				{
					if (originalCount > sourceIndex)
						_items[newIndex++] = _sourceAsList[sourceIndex];
					else
						_items.Insert(newIndex++, _sourceAsList[sourceIndex]);
				}

				for (int index1 = originalCount - 1; index1 >= newIndex; index1--)
				{
					_items.RemoveAt(index1);
				}

				_source.CollectionChanged += handleSourceCollectionChanged;
                _sourceInitialized = true;

				if (currentPageChanged) OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
			}
			else
			{
				_items.Clear();

                // ReSharper disable once RedundantCheckBeforeAssignment
                if (_pageCount != 0)
				{
					_pageCount = 0;
				}

                // ReSharper disable once RedundantCheckBeforeAssignment
                if (_currentPage != 1)
				{
					_currentPage = 1;
				}				
			}

			reset();
		}




        private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
                sender, 
                e, 
                _isConsistent, 
                this, 
                ref _indexerPropertyChangedEventRaised, 
                ref _lastProcessedSourceChangeMarker, 
                _sourceAsIHasChangeMarker, 
                ref _handledEventSender, 
                ref _handledEventArgs)) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    //if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
                    _isConsistent = false;
                    int newStartingIndex = e.NewStartingIndex;

                    int originalPageCount2 = _pageCount;
                    int sourceCount2 = _sourceAsList.Count;
                    _pageCount = (int) Math.Ceiling(sourceCount2 / (double) _pageSize);

                    if (newStartingIndex < _lowerIndex)
                    {
                        baseInsertItem(0, _sourceAsList[_lowerIndex]);
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
                    _pageCount = (int) Math.Ceiling(sourceCount / (double) _pageSize);

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

                        _handledEventSender = null;
                        _handledEventArgs = null;

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

                        for (var sourceIndex = _lowerIndex;
                            sourceIndex < sourceCount && sourceIndex < _upperIndex;
                            sourceIndex++)
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
                                                         && (newStartingIndex2 >= _lowerIndex &&
                                                             newStartingIndex2 < _upperIndex))
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
                            baseInsertItem(0, _sourceAsList[_lowerIndex]);
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

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);
		}

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_pageSizeScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_currentPageScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)        
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_pageSizeScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_currentPageScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
            initializePageSizeScalar();
            initializeCurrentPageScalar();
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, scalarValueChangedHandler, ref _source);
            if (_pageSizeScalar != null) _pageSizeScalar.PropertyChanged -= handlePageSizeScalarValueChanged;
            if (_currentPageScalar != null) _currentPageScalar.PropertyChanged -= handleCurrentPageScalarValueChanged;
            if (_sourceAsINotifyPropertyChanged != null)
                _sourceAsINotifyPropertyChanged.PropertyChanged -= ((ISourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
        }

        #region Implementation of ISourceIndexerPropertyTracker

        void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Utils.HandleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
        }

        #endregion

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int pageSize = PageSize;
			int startIndex =  (CurrentPage - 1) * pageSize;


			if (_lowerIndex != PageSize * (CurrentPage - 1))
				throw new ObservableComputationsException(this, "Consistency violation: Paging.2");

			if (_upperIndex != _lowerIndex + PageSize)
				throw new ObservableComputationsException(this, "Consistency violation: Paging.3");

            // ReSharper disable once PossibleNullReferenceException
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
