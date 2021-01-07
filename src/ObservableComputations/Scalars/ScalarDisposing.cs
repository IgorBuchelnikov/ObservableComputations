// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;

namespace ObservableComputations
{
	public class ScalarDisposing<TValue> : ScalarProcessingVoid<TValue> where TValue : IDisposable
	{
		public ScalarDisposing(IReadScalar<TValue> scalar) : base(scalar, null, getOldValueProcessor())
		{
		}

		private static Action<TValue, ScalarProcessingVoid<TValue>> getOldValueProcessor()
		{
			return (oldValue, scalarDisposing) => oldValue.Dispose();
		}
	}
}
