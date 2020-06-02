using System.ComponentModel;

namespace ObservableComputations
{
	public class PreviousTracking<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public TResult PreviousValue => _previousValue;
		public bool IsEverChanged => _isEverChanged;

		private IReadScalar<TResult> _scalar;
		private TResult _previousValue;
		private bool _isEverChanged;

		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		[ObservableComputationsCall]
		public PreviousTracking(
			IReadScalar<TResult> scalar)
		{
			_scalar = scalar;
			_value = _scalar.Value;
			_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
			_scalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
			_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
		}


		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;

			_processingEventSender = sender;
			_processingEventArgs = e;

			TResult newValue = _scalar.Value;
			_previousValue = _value;
			_isConsistent = false;

			if (!_isEverChanged)
			{
				_isEverChanged = true;
				raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
			}

			raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
			setValue(newValue);
			raiseConsistencyRestored();

			_processingEventSender = null;
			_processingEventArgs = null;
		}

		~PreviousTracking()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}