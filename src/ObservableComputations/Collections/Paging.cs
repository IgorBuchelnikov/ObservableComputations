﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ObservableComputations.Internal;

namespace ObservableComputations.Internal
{
	public class ZippingForPaging<TSourceItem> : Zipping<int, TSourceItem>
	{
		// ReSharper disable once StaticMemberInGenericType
		private static readonly PropertyChangedEventArgs __pageSizePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(PageSize));
		// ReSharper disable once StaticMemberInGenericType
		private static readonly PropertyChangedEventArgs __currentPageNumPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(CurrentPageNum));

		private int _pageSize = 20;
		public int PageSize
		{
			get => _pageSize;
			set
			{
				_pageSize = value;
				OnPropertyChanged(__pageSizePropertyChangedEventArgs);
			}
		}


		private int _currentPageNum =  1;
		public int CurrentPageNum
		{
			get => _currentPageNum;
			set
			{
				_currentPageNum = value;
				OnPropertyChanged(__currentPageNumPropertyChangedEventArgs);
			}
		}

		public ZippingForPaging(IReadScalar<INotifyCollectionChanged> leftSourceScalar, INotifyCollectionChanged rightSource) : base(leftSourceScalar, rightSource)
		{
		}

		public ZippingForPaging(IReadScalar<INotifyCollectionChanged> leftSourceScalar, IReadScalar<INotifyCollectionChanged> rightSourceScalar) : base(leftSourceScalar, rightSourceScalar)
		{
		}

		public ZippingForPaging(INotifyCollectionChanged leftSource, INotifyCollectionChanged rightSource) : base(leftSource, rightSource)
		{
		}

		public ZippingForPaging(INotifyCollectionChanged leftSource, IReadScalar<INotifyCollectionChanged> rightSourceScalar) : base(leftSource, rightSourceScalar)
		{
		}
	}
}


namespace ObservableComputations
{
	public class Paging<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSources
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarPaging;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _sourcePaging;

		public int PageSize
		{
			get => _zippingForPaging.PageSize;
			set => _zippingForPaging.PageSize = value;
		}

		public int CurrentPageNum
		{
			get => _zippingForPaging.CurrentPageNum;
			set => _zippingForPaging.CurrentPageNum = value;
		}


		public IReadScalar<int> PageCount => _pageCountComputing;
		public IReadScalar<bool> CurrentPageExists => _currentPageExistsComputing = _currentPageExistsComputing ?? 
			new Computing<bool>(() => _zippingForPaging.CurrentPageNum >= 0 || _zippingForPaging.CurrentPageNum < _pageCountComputing.Value);


		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarPaging;
		private readonly INotifyCollectionChanged _sourcePaging;

		private Computing<int> _pageCountComputing;
		private Computing<bool> _currentPageExistsComputing;

		private ZippingForPaging<TSourceItem> _zippingForPaging;

		[ObservableComputationsCall]
		public Paging(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int capacity = 0)
			: base(
				getSource(sourceScalar, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourceScalarPaging = sourceScalar;
			_zippingForPaging = (ZippingForPaging<TSourceItem>)(((Filtering<ZipPair<int, TSourceItem>>)_source)._source);

			_pageCountComputing = new Computing<int>(() => 
				(int)Math.Ceiling(
					new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Value 
					/ (double)_zippingForPaging.PageSize));
		}

		[ObservableComputationsCall]
		public Paging(			
			INotifyCollectionChanged source, 
			int capacity = 0)
			: base(
				getSource(source, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourcePaging = source;
			_zippingForPaging = (ZippingForPaging<TSourceItem>)(((Filtering<ZipPair<int, TSourceItem>>)_source)._source);

			_pageCountComputing = new Computing<int>(() => 
				(int)Math.Ceiling(
					new Computing<int>(() => source != null ? ((IList) source).Count : 0).Value 
					/ (double)_zippingForPaging.PageSize));
		}



		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int capacity)
		{
			return 
				new ZippingForPaging<TSourceItem>(new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).SequenceComputing(), sourceScalar).Using(zipping => 
						new Computing<int>(() => zipping.PageSize * zipping.CurrentPageNum).Using(upperIndex => 
							zipping.Filtering(zp => zp.ItemLeft >= upperIndex.Value - zipping.PageSize  && zp.ItemLeft < upperIndex.Value, capacity))).Value.Value;
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			int capacity)
		{
			return 
				new ZippingForPaging<TSourceItem>(new Computing<int>(() => source != null ? ((IList) source).Count : 0).SequenceComputing(), source).Using(zipping => 
					new Computing<int>(() => zipping.PageSize * zipping.CurrentPageNum).Using(upperIndex => 
						zipping.Filtering(zp => zp.ItemLeft >= upperIndex.Value - zipping.PageSize  && zp.ItemLeft < upperIndex.Value, capacity))).Value.Value;
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarPaging.getValue(_sourcePaging, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int pageSize = PageSize;
			int startIndex =  (CurrentPageNum - 1) * pageSize;

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(startIndex).Take(pageSize)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Paging.1");
			}
		}
	}
}