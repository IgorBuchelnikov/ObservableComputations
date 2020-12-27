using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ObservableComputations
{
	public class Paging<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
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

				int newPageSize = value;

				void action()
				{
					int originalPageSize = _pageSize;
					_pageSize = newPageSize;
					processPageSizeChanged(originalPageSize);
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

		private void processPageSizeChanged(int originalPageSize)
		{
			int originalUpperIndex = _upperIndex;
			int sourceCount = _sourceCopy.Count;

			_pageCount = (int) Math.Ceiling(sourceCount / (double) _pageSize);
			bool currentPageChanged = false;
			if (_currentPage > _pageCount)
			{
				_currentPage = _pageCount;
				currentPageChanged = true;
			}

			_lowerIndex = _pageSize * (_currentPage - 1);
			_upperIndex = _lowerIndex + _pageSize;

			if (originalPageSize < _pageSize)
			{
				int index = originalPageSize;
				for (int sourceIndex = originalUpperIndex;
					sourceIndex < sourceCount && sourceIndex < _upperIndex;
					sourceIndex++)
					baseInsertItem(index++, _sourceCopy[sourceIndex]);
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

			OnPropertyChanged(Utils.PageSizePropertyChangedEventArgs);
			if (currentPageChanged) OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
		}

		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				if (_currentPageScalar != null) throw new ObservableComputationsException("Modifying of CurrentPage property is controlled by CurrentPageScalar");

				int newCurrentPage = value;

				void action()
				{
					_currentPage = newCurrentPage;
					processCurrentPageChanged();
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

		private void processCurrentPageChanged()
		{
			if (_currentPage < 1) _currentPage = 1;
			if (_currentPage > _pageCount) _currentPage = _pageCount;

			_lowerIndex = _pageSize * (_currentPage - 1);
			_upperIndex = _lowerIndex + _pageSize;

			int sourceCount = _sourceCopy.Count;
			int index = 0;
			int thisCount = Count;

			for (int sourceIndex = _lowerIndex; sourceIndex < sourceCount && sourceIndex < _upperIndex; sourceIndex++)
			{
				if (thisCount > index)
					baseSetItem(index, _sourceCopy[sourceIndex]);
				else
					baseInsertItem(index, _sourceCopy[sourceIndex]);

				index++;
			}

			int count = Count;
			int removingIndex = index;

			for (; index < count; index++)
			{
				baseRemoveItem(removingIndex);
			}

			OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
		}

		public int PageCount => _pageCount;
		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private List<TSourceItem> _sourceCopy;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private bool _sourceInitialized;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private readonly IReadScalar<int> _pageSizeScalar;

		private readonly IReadScalar<int> _currentPageScalar;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private int _pageSize;
		private int _currentPage;
		private int _pageCount;

		private int _lowerIndex;
		private int _upperIndex;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;


		[ObservableComputationsCall]
		public Paging(
			INotifyCollectionChanged source,
			int pageSize,
			int initialPage = 1)
		{
			_pageSize = pageSize;
			_currentPage = initialPage;
			_source = source;
			_thisAsSourceCollectionChangeProcessor = this;
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
			_thisAsSourceCollectionChangeProcessor = this;
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
			_thisAsSourceCollectionChangeProcessor = this;
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
			_thisAsSourceCollectionChangeProcessor = this;
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
			_thisAsSourceCollectionChangeProcessor = this;
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
			_thisAsSourceCollectionChangeProcessor = this;
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
			_thisAsSourceCollectionChangeProcessor = this;
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
			_thisAsSourceCollectionChangeProcessor = this;
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
			int pageSize = _pageSizeScalar.Value;

			void action()
			{
				int originalPageSize = _pageSize;
				_pageSize = pageSize;
				checkPageSize();
				processPageSizeChanged(originalPageSize);
			}

			Utils.processChange(
				sender, 
				e, 
				action,
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs, 
				0, _deferredQueuesCount,
				ref _deferredProcessings, this);
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
			int currentPage = _currentPageScalar.Value;

			void action()
			{
				_currentPage = currentPage;
				checkCurrentPage();
				processCurrentPageChanged();
			}

			Utils.processChange(
				sender, 
				e, 
				action,
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs, 
				0, _deferredQueuesCount,
				ref _deferredProcessings, this);
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

				_sourceCopy = null;
				_sourceInitialized = false;
			}

			Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
				out _sourceAsList, true);

			int originalPageCount = _pageCount;
			int originalCurrentPage = _currentPage;

			if (_sourceAsList != null && _isActive)
			{
				Utils.initializeFromHasChangeMarker(
					out _sourceAsIHasChangeMarker, 
					_sourceAsList, 
					ref _lastProcessedSourceChangeMarker, 
					ref _sourceAsINotifyPropertyChanged,
					(ISourceIndexerPropertyTracker)this);

				int newIndex = 0;
				int count = _sourceAsList.Count;
				_sourceCopy = new List<TSourceItem>(_sourceAsList);

				_source.CollectionChanged += handleSourceCollectionChanged;

				_pageCount = (int) Math.Ceiling(count  / (double) _pageSize);
				if (_currentPage > _pageCount)
				{
					_currentPage = _pageCount > 0 ? _pageCount : 1;
				}

				_lowerIndex = _pageSize * (_currentPage - 1);
				_upperIndex = _lowerIndex + _pageSize;
	
				for (int sourceIndex = _lowerIndex; sourceIndex < count && sourceIndex < _upperIndex; sourceIndex++)
				{
					if (originalCount > sourceIndex)
						_items[newIndex++] = _sourceCopy[sourceIndex];
					else
						_items.Insert(newIndex++, _sourceCopy[sourceIndex]);
				}

				for (int index1 = originalCount - 1; index1 >= newIndex; index1--)
				{
					_items.RemoveAt(index1);
				}

				_sourceInitialized = true;
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

			if (_pageCount != originalPageCount)
				OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);

			if (_currentPage != originalCurrentPage)
				OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourceChangeMarker, 
				_sourceAsIHasChangeMarker, 
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

					int newStartingIndex = e.NewStartingIndex;
					TSourceItem newItem = (TSourceItem) e.NewItems[0];
					_sourceCopy.Insert(newStartingIndex, newItem);

					int originalPageCount2 = _pageCount;
					int sourceCount2 = _sourceCopy.Count;
					_pageCount = (int) Math.Ceiling(sourceCount2 / (double) _pageSize);

					if (newStartingIndex < _lowerIndex)
					{
						baseInsertItem(0, _sourceCopy[_lowerIndex]);
					}
					else if (newStartingIndex < _upperIndex)
					{

						baseInsertItem(newStartingIndex - _lowerIndex, newItem);
					}
					else
					{
						if (_pageCount != originalPageCount2)
							OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);
					}

					if (Count > _pageSize) baseRemoveItem(_pageSize);

					if (_pageCount != originalPageCount2)
						OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);
					break;
				case NotifyCollectionChangedAction.Remove:
					// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
					int oldStartingIndex = e.OldStartingIndex;
					_sourceCopy.RemoveAt(oldStartingIndex);
					int originalPageCount1 = _pageCount;
					int originalCurrentPage1 = _currentPage;

					int sourceCount = _sourceCopy.Count;
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
						if (_pageCount != originalPageCount1)
							OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);

						return;
					}

					int count = Count;

					if (count < _pageSize && sourceCount > _upperIndex - 1)
						baseInsertItem(_pageSize - 1, _sourceCopy[_upperIndex - 1]);
					else if (count == 0 && _currentPage > 1)
					{
						_currentPage = _currentPage - 1;
						_lowerIndex = _pageSize * (_currentPage - 1);
						_upperIndex = _lowerIndex + _pageSize;

						int index = 0;

						for (int sourceIndex = _lowerIndex;
							sourceIndex < sourceCount && sourceIndex < _upperIndex;
							sourceIndex++)
						{
							baseInsertItem(index++, _sourceCopy[sourceIndex]);
						}
					}

					if (_pageCount != originalPageCount1)
						OnPropertyChanged(Utils.PageCountPropertyChangedEventArgs);

					if (_currentPage != originalCurrentPage1)
						OnPropertyChanged(Utils.CurrentPagePropertyChangedEventArgs);
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex2 = e.NewStartingIndex;
					TSourceItem newItem1 = (TSourceItem) e.NewItems[0];

					_sourceCopy[newStartingIndex2] = newItem1;

					if (newStartingIndex2 >= _lowerIndex && newStartingIndex2 < _upperIndex
														 && (newStartingIndex2 >= _lowerIndex &&
															 newStartingIndex2 < _upperIndex))
					{
						
						baseSetItem(newStartingIndex2 - _lowerIndex, newItem1);
					}

					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex1 = e.OldStartingIndex;
					int newStartingIndex1 = e.NewStartingIndex;

					TSourceItem sourceItem1 = (TSourceItem) e.NewItems[0];
					_sourceCopy.RemoveAt(oldStartingIndex1);
					_sourceCopy.Insert(newStartingIndex1, sourceItem1);

					if (oldStartingIndex1 == newStartingIndex1) return;

					if ((oldStartingIndex1 >= _lowerIndex && oldStartingIndex1 < _upperIndex)
						&& (newStartingIndex1 >= _lowerIndex && newStartingIndex1 < _upperIndex))
					{
						baseMoveItem(oldStartingIndex1 - _lowerIndex, newStartingIndex1 - _lowerIndex);
					}
					else if ((oldStartingIndex1 < _lowerIndex || oldStartingIndex1 >= _upperIndex)
							 && (newStartingIndex1 >= _lowerIndex && newStartingIndex1 < _upperIndex))
					{
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
					}
					else if ((newStartingIndex1 < _lowerIndex || newStartingIndex1 >= _upperIndex)
							 && (oldStartingIndex1 >= _lowerIndex && oldStartingIndex1 < _upperIndex))
					{
						baseRemoveItem(oldStartingIndex1 - _lowerIndex);

						if (newStartingIndex1 < _lowerIndex)
							baseInsertItem(0, _sourceCopy[_lowerIndex]);
						else
							baseInsertItem(_pageSize - 1, _sourceCopy[_upperIndex - 1]);
					}
					else if ((newStartingIndex1 < _lowerIndex && oldStartingIndex1 >= _upperIndex)
							 || (oldStartingIndex1 < _lowerIndex && newStartingIndex1 >= _upperIndex))
					{
						if (oldStartingIndex1 < _lowerIndex)
						{
							baseRemoveItem(0);
							baseInsertItem(_pageSize - 1, _sourceCopy[_upperIndex - 1]);
						}
						else
						{
							baseInsertItem(0, _sourceCopy[_lowerIndex]);
							baseRemoveItem(_pageSize);
						}
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
			}
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
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
		}

		#endregion

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int pageSize = PageSize;
			int startIndex =  (CurrentPage - 1) * pageSize;

			if (_source != null || (_sourceScalar?.Value != null))
			{
				if (_lowerIndex != PageSize * (CurrentPage - 1))
					throw new ObservableComputationsException(this, "Consistency violation: Paging.2");

				if (_upperIndex != _lowerIndex + PageSize)
					throw new ObservableComputationsException(this, "Consistency violation: Paging.3");

				// ReSharper disable once PossibleNullReferenceException
				if (_pageCount != (int) Math.Ceiling(source.Count  / (double) _pageSize))
					throw new ObservableComputationsException(this, "Consistency violation: Paging.4");
			}

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(startIndex).Take(pageSize)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Paging.1");
			}
		}


	}
}
