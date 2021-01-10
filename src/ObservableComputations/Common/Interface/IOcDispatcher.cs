// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public interface IOcDispatcher
	{
		void Invoke(Action action, int priority = 0, object parameter = null, object context = null);
	}
}
