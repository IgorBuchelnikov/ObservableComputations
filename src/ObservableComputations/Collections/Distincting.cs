// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class Distincting<TSourceItem> : Selecting<Group<TSourceItem, TSourceItem>, TSourceItem>, IHasSources
	{
		// ReSharper disable once UnusedMember.Local
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		public virtual IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _sourceDistincting;

		public IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;

		public override ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		protected IReadScalar<IEqualityComparer<TSourceItem>> _equalityComparerScalar;

		protected IEqualityComparer<TSourceItem> _equalityComparer;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _sourceDistincting;

		public override int InitialCapacity => ((IHasInitialCapacity) _source).InitialCapacity;

		[ObservableComputationsCall]
		public Distincting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) : base(getSource(sourceScalar, equalityComparerScalar, initialCapacity), g => g.Key)
		{
			_sourceScalar = sourceScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Distincting(
			INotifyCollectionChanged source,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) : base(getSource(source, equalityComparerScalar, initialCapacity), g => g.Key)
		{
			_sourceDistincting = source;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Distincting(
			INotifyCollectionChanged source,
			IEqualityComparer<TSourceItem> equalityComparer,
			int initialCapacity = 0) : base(getSource(source, equalityComparer, initialCapacity), g => g.Key)
		{
			_sourceDistincting = source;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Distincting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) : base(getSource(sourceScalar, equalityComparer, initialCapacity), g => g.Key)
		{
			_sourceScalar = sourceScalar;
			_equalityComparer = equalityComparer;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar,
			int initialCapacity) =>
			sourceScalar.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparerScalar, initialCapacity);

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar,
			int initialCapacity) =>
			source.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparerScalar, initialCapacity);

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			IEqualityComparer<TSourceItem> equalityComparer,
			int initialCapacity) =>
			source.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparer, initialCapacity);

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IEqualityComparer<TSourceItem> equalityComparer,
			int initialCapacity) =>
			sourceScalar.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparer, initialCapacity);

		[ExcludeFromCodeCoverage]
		public new void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_sourceDistincting, new ObservableCollection<TSourceItem>());
			IEqualityComparer<TSourceItem> equalityComparer = _equalityComparerScalar.getValue(_equalityComparer);
			if (_equalityComparerScalar == null)
				equalityComparer = EqualityComparer<TSourceItem>.Default;

			if (!this.SequenceEqual(source.Distinct(equalityComparer)))
				throw new ValidateInternalConsistencyException("Consistency violation: Distincting.1");
		}
	}
}
