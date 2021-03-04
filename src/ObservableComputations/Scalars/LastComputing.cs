// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class LastComputing<TSourceItem> : ItemComputing<TSourceItem>
	{
		[ObservableComputationsCall]
		public LastComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem defaultValue = default(TSourceItem)) : base(sourceScalar, getIndex(sourceScalar), defaultValue)
		{
		}

		[ObservableComputationsCall]
		public LastComputing(
			INotifyCollectionChanged source,
			TSourceItem defaultValue = default(TSourceItem)) : base(source, getIndex(source), defaultValue)
		{
		}

		private static IReadScalar<int> getIndex(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			Expression<Func<int>> indexExpression = () => sourceScalar.Value != null && ((IList) sourceScalar.Value).Count > 0 ? ((IList) sourceScalar.Value).Count - 1 : 0;
			return indexExpression.Computing();
		}

		private static IReadScalar<int> getIndex(INotifyCollectionChanged source)
		{
			IList list = (IList)source;
			Expression<Func<int>> indexExpression = () => list.Count > 0 ? list.Count - 1 : 0;
			return indexExpression.Computing();
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());
			TSourceItem defaultValue = _defaultValue;

			if (!EqualityComparer<TSourceItem>.Default.Equals(_value, source.Count > 0 ? source.Last() : defaultValue))
				throw new ValidateInternalConsistencyException("Consistency violation: LastComputing.1");
		}

	}
}
