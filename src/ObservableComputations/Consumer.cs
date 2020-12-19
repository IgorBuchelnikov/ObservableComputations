using System;
using System.Collections.Generic;

namespace ObservableComputations
{
	public class Consumer : IDisposable
	{
		List<IComputingInternal> _computings = new List<IComputingInternal>();
		private object _tag;
		public object Tag => _tag;

		public Consumer(object tag = null)
		{
			_tag = tag;
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
			int computingsCount = _computings.Count;
			for (var index = computingsCount - 1; index >= 0; index--)
			{
				IComputingInternal computing = _computings[index];
				computing.RemoveConsumer(this);

			}

			_computings = new List<IComputingInternal>();
		}
		#endregion

		internal void AddComputing(IComputingInternal computing)
		{
			_computings.Add(computing);
		}
	}
}
