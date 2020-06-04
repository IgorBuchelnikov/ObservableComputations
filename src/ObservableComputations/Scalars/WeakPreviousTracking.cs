using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class WeakPreviousTracking<TResult> : ScalarComputing<TResult>
		where TResult : class
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public bool IsEverChanged => _isEverChanged;

		public bool TryGetPreviousValue(out TResult result)
		{
			if (_previousValueWeakReference == null)
			{
				result = null;
				return false;
			}

			return _previousValueWeakReference.TryGetTarget(out result);
		}

		private WeakReference<TResult> _previousValueWeakReference;
		private TResult _previousValue;
		private bool _isEverChanged;

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

			_handledEventSender = sender;
			_handledEventArgs = e;

			TResult newValue = _scalar.Value;
			_previousValue = _value;
			_previousValueWeakReference = new WeakReference<TResult>(_previousValue);

			if (!_isEverChanged)
			{
				_isEverChanged = true;
				raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
			}

			setValue(newValue);
			_previousValue = null;

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		~WeakPreviousTracking()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}