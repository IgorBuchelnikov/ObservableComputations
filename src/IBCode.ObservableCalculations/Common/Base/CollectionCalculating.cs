using System;
using IBCode.ObservableCalculations.Common.Interface;


namespace IBCode.ObservableCalculations.Common
{
	public abstract class CollectionCalculating<TItem> : CollectionCalculatingBase<TItem>, IConsistent
	{
		protected int _initialCapacity;
		public CollectionCalculating(int capacity = 0) : base(capacity)
		{
			_initialCapacity = capacity;

			if (Configuration.SaveInstantiatingStackTrace)
			{
				InstantiatingStackTrace = Environment.StackTrace;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public string InstantiatingStackTrace { get; }

		protected bool _consistent = true;

		public bool Consistent => _consistent;

		protected void checkConsistent()
		{
			if (!_consistent)
				throw new ObservableCalculationsException(
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change after Consistent property becomes true");
		}
	}

}
