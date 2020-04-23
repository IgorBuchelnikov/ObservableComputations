using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarDispatching<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IDestinationScalarDispatcher PostingDestinationDispatcher => _destinationScalarDispatcher;
		public ISourceScalarDispatcher SendingDestinationDispatcher => _sourceScalarDispatcher;

		private IDestinationScalarDispatcher _destinationScalarDispatcher;
		private ISourceScalarDispatcher _sourceScalarDispatcher;

		private IReadScalar<TResult> _scalar;

		private PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			ISourceScalarDispatcher sourceScalarDispatcher,
			IDestinationScalarDispatcher destinationScalarDispatcher)
		{
			_scalar = scalar;
			_sourceScalarDispatcher = sourceScalarDispatcher;
			_destinationScalarDispatcher = destinationScalarDispatcher;

			_sourceScalarDispatcher.InvokeReadAndSubscribe(() =>
			{
				_value = _scalar.Value;
				_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
				_scalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
				_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
			}, this);

		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;

			TResult newValue = _scalar.Value;

			_destinationScalarDispatcher.InvokeSetValue(
				() => setValue(newValue), this, sender, e);
		}

		~ScalarDispatching()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}