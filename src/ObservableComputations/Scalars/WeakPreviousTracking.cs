using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class WeakPreviousTracking<TResult> : ScalarComputing<TResult>
		where TResult : class
	{
		public IReadScalar<TResult> Scalar => _scalar;

		public bool TryGetPreviousValue(out TResult result)
		{
			return _previousValue.TryGetTarget(out result);
		}

		private WeakReference<TResult> _previousValue;
		private IReadScalar<TResult> _scalar;

		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		[ObservableComputationsCall]
		public WeakPreviousTracking(
			IReadScalar<TResult> scalar)
		{
			_scalar = scalar;
			_value = _scalar.Value;
			_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
			_scalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
			_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;
			TResult newValue = _scalar.Value;
			TResult previousValue = _value;
			_previousValue = new WeakReference<TResult>(previousValue);
			setValue(newValue);
			previousValue = null;
		}

		~WeakPreviousTracking()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}