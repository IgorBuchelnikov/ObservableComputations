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
