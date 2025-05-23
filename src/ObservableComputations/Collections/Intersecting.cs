﻿// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class Intersecting<TSourceItem> : Distincting<TSourceItem>, IHasSources
	{
		private readonly IReadScalar<INotifyCollectionChanged> _source1Scalar;
		private readonly IReadScalar<INotifyCollectionChanged> _source2Scalar;
		private readonly INotifyCollectionChanged _source1;
		private readonly INotifyCollectionChanged _source2;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> Source1Scalar => _source1Scalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> Source2Scalar => _source2Scalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public override IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source1 => _source1;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source2 => _source2;

		public new IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;

		public override ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source1, Source2, Source1Scalar, Source2Scalar});

		[ObservableComputationsCall]
		public Intersecting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1Scalar, source2Scalar, equalityComparerScalar),
				equalityComparerScalar,
				initialCapacity)
		{
			_source1Scalar = source1Scalar;
			_source2Scalar = source2Scalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Intersecting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1Scalar, source2, equalityComparerScalar),
				equalityComparerScalar,
				initialCapacity)
		{
			_source1Scalar = source1Scalar;
			_source2 = source2;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Intersecting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1Scalar, source2, equalityComparer),
				equalityComparer,
				initialCapacity)
		{
			_source1Scalar = source1Scalar;
			_source2 = source2;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Intersecting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1Scalar, source2Scalar, equalityComparer),
				equalityComparer,
				initialCapacity)
		{
			_source1Scalar = source1Scalar;
			_source2Scalar = source2Scalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Intersecting(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1, source2Scalar, equalityComparerScalar),
				equalityComparerScalar,
				initialCapacity)
		{
			_source1 = source1;
			_source2Scalar = source2Scalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Intersecting(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1, source2, equalityComparerScalar),
				equalityComparerScalar,
				initialCapacity)
		{
			_source1 = source1;
			_source2 = source2;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Intersecting(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1, source2, equalityComparer),
				equalityComparer,
				initialCapacity)
		{
			_source1 = source1;
			_source2 = source2;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Intersecting(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: base(
				getSource(source1, source2Scalar, equalityComparer),
				equalityComparer,
				initialCapacity)
		{
			_source1 = source1;
			_source2Scalar = source2Scalar;
			_equalityComparer = equalityComparer;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count > 0)
				.Selecting(jg => jg.OuterItem);
		}

		[ExcludeFromCodeCoverage]
		internal new void ValidateInternalConsistency()
		{
			IList<TSourceItem> source1 = (IList<TSourceItem>) _source1Scalar.getValue(_source1, new ObservableCollection<TSourceItem>());
			IList<TSourceItem> source2 = (IList<TSourceItem>) _source2Scalar.getValue(_source2, new ObservableCollection<TSourceItem>());
			IEqualityComparer<TSourceItem> equalityComparer = _equalityComparerScalar.getValue(_equalityComparer);

			if (!this.SequenceEqual(source1.Intersect(source2, equalityComparer)))
				throw new ValidateInternalConsistencyException("Consistency violation: Intersecting.1");
		}
	}
}
