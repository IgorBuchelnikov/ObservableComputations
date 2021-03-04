// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class OfTypeComputing<TResultItem> : Casting<TResultItem>, IHasSourceCollections
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarOfTypeComputing;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _sourceOfTypeComputing;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarOfTypeComputing;
		private readonly INotifyCollectionChanged _sourceOfTypeComputing;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public OfTypeComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(getSource(sourceScalar))
		{
			_sourceScalarOfTypeComputing = sourceScalar;
		}

		[ObservableComputationsCall]
		public OfTypeComputing(
			INotifyCollectionChanged source) : base(getSource(source))
		{
			_sourceOfTypeComputing = source;
		}

		private static INotifyCollectionChanged getSource(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			return sourceScalar.Casting<object>().Filtering(item => item is TResultItem);
		}

		private static INotifyCollectionChanged getSource(INotifyCollectionChanged source)
		{
			return source.Casting<object>().Filtering(item => item is TResultItem);
		}

		// ReSharper disable once InconsistentNaming
		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			IList source = _sourceScalarOfTypeComputing.getValue(_sourceOfTypeComputing, new ObservableCollection<object>()) as IList;

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.OfType<TResultItem>()))
				 throw new ValidateInternalConsistencyException("Consistency violation: OfTypeComputing.1");
		}
	}
}
