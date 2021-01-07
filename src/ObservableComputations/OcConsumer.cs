// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;

namespace ObservableComputations
{
	public class OcConsumer : IDisposable
	{
		List<IComputingInternal> _computings = new List<IComputingInternal>();
		private readonly object _tag;
		public object Tag => _tag;

		public OcConsumer(object tag = null)
		{
			_tag = tag;
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
			int computingsCount = _computings.Count;
			for (int index = computingsCount - 1; index >= 0; index--)
				_computings[index].RemoveConsumer(this);

			_computings = new List<IComputingInternal>();
		}
		#endregion

		internal void AddComputing(IComputingInternal computing)
		{
			_computings.Add(computing);
		}
	}
}
