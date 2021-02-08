// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;

namespace ObservableComputations.Test
{
		public class Scalar<TValue> : IReadScalar<TValue>
		{
			public Scalar(TValue value)
			{
				Value = value;
			}

			public event PropertyChangedEventHandler PropertyChanged;
			public TValue Value { get; private set;}

			public void Touch()
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}

			public void Change(TValue newValue)
			{
				Value = newValue;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}

			public Delegate[] PropertyChangedInvocationList => PropertyChanged.GetInvocationList();
		}
}
