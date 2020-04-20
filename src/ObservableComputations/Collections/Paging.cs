using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ObservableComputations
{
	public class Paging<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSources
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarPaging;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _sourcePaging;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> PageSizeScalar => _pageSizeScalar;

		public  IReadScalar<int> CurrentPageNumScalar => _currentPageNumScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public int PageSizePaging => _pageSizePaging;

		public IReadScalar<int> PageCount => _pageCountComputing;
		public IReadScalar<bool> CurrentPageExists => _currentPageExistsComputing = _currentPageExistsComputing ?? 
			new Computing<bool>(() => _currentPageNumScalar.Value >= 0 || _currentPageNumScalar.Value < _pageCountComputing.Value);


		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarPaging;
		private readonly INotifyCollectionChanged _sourcePaging;
		private readonly IReadScalar<int> _pageSizeScalar;
		private readonly int _pageSizePaging;

		private Computing<int> _pageCountComputing;
		private Computing<bool> _currentPageExistsComputing;
		private IReadScalar<int> _currentPageNumScalar;


		[ObservableComputationsCall]
		public Paging(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> pageSizeScalar,
			IReadScalar<int> currentPageNumScalar,
			int capacity = 0)
			: base(
				getSource(sourceScalar, pageSizeScalar, currentPageNumScalar, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourceScalarPaging = sourceScalar;
			_pageSizeScalar = pageSizeScalar;
			_currentPageNumScalar = currentPageNumScalar;

			_pageCountComputing = new Computing<int>(() => 
				(int)Math.Ceiling(
					new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Value 
					/ (double)pageSizeScalar.Value));
		}

		[ObservableComputationsCall]
		public Paging(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int pageSize,
			IReadScalar<int> currentPageNumScalar)
			: base(
				getSource(sourceScalar, pageSize, currentPageNumScalar),
				zipPair => zipPair.ItemRight)
		{
			_sourceScalarPaging = sourceScalar;
			_pageSizePaging = pageSize;
			_currentPageNumScalar = currentPageNumScalar;

			_pageCountComputing = new Computing<int>(() => 
				(int)Math.Ceiling(
					new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Value 
					/ (double)pageSize));
		}

		[ObservableComputationsCall]
		public Paging(			
			INotifyCollectionChanged source, 
			IReadScalar<int> pageSizeScalar,
			IReadScalar<int> currentPageNumScalar,
			int capacity = 0)
			: base(
				getSource(source, pageSizeScalar, currentPageNumScalar, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourcePaging = source;
			_pageSizeScalar = pageSizeScalar;
			_currentPageNumScalar = currentPageNumScalar;

			_pageCountComputing = new Computing<int>(() => 
				(int)Math.Ceiling(
					((IList) source).Count
					/ (double)pageSizeScalar.Value));
		}

		[ObservableComputationsCall]
		public Paging(			
			INotifyCollectionChanged source, 
			int pageSize,
			IReadScalar<int> currentPageNumScalar)
			: base(
				getSource(source, pageSize, currentPageNumScalar),
				zipPair => zipPair.ItemRight)
		{
			_sourcePaging = source;
			_pageSizePaging = pageSize;
			_currentPageNumScalar = currentPageNumScalar;

			_pageCountComputing = new Computing<int>(() => 
				(int)Math.Ceiling(
					((IList) source).Count
					/ (double)pageSize));
		}


		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> pageSizeScalar,
			IReadScalar<int> currentPageNumScalar,
			int capacity)
		{
			return 
				new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).SequenceComputing()
					.Zipping<int, TSourceItem>(sourceScalar).Using(zipping => 
						new Computing<int>(() => pageSizeScalar.Value * currentPageNumScalar.Value).Using(upperIndex => 
							zipping.Filtering(zp => zp.ItemLeft >= upperIndex.Value - pageSizeScalar.Value  && zp.ItemLeft < upperIndex.Value, capacity))).Value.Value;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int pageSize,
			IReadScalar<int> currentPageNumScalar)
		{
			return 
				new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).SequenceComputing()
					.Zipping<int, TSourceItem>(sourceScalar).Using(zipping => 
						new Computing<int>(() => pageSize * currentPageNumScalar.Value).Using(upperIndex => 
							zipping.Filtering(zp => zp.ItemLeft >= upperIndex.Value - pageSize  && zp.ItemLeft < upperIndex.Value, pageSize))).Value.Value;
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			IReadScalar<int> pageSizeScalar,
			IReadScalar<int> currentPageNumScalar,
			int capacity)
		{
			return 
				new Computing<int>(() => source != null ? ((IList) source).Count : 0).SequenceComputing()
					.Zipping<int, TSourceItem>(source).Using(zipping => 
						new Computing<int>(() => pageSizeScalar.Value * currentPageNumScalar.Value).Using(upperIndex => 
							zipping.Filtering(zp => zp.ItemLeft >= upperIndex.Value - pageSizeScalar.Value  && zp.ItemLeft < upperIndex.Value, capacity))).Value.Value;

		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			int pageSize,
			IReadScalar<int> currentPageNumScalar)
		{
			return 
				new Computing<int>(() => source != null ? ((IList) source).Count : 0).SequenceComputing()
					.Zipping<int, TSourceItem>(source).Using(zipping => 
						new Computing<int>(() => pageSize * currentPageNumScalar.Value).Using(upperIndex => 
							zipping.Filtering(zp => zp.ItemLeft >= upperIndex.Value - pageSize  && zp.ItemLeft < upperIndex.Value, pageSize))).Value.Value;
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarPaging.getValue(_sourcePaging, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int pageSize = _pageSizeScalar.getValue(_pageSizePaging);
			int startIndex =  (_currentPageNumScalar.Value - 1) * pageSize;

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(startIndex).Take(pageSize)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Paging.1");
			}
		}
	}
}
