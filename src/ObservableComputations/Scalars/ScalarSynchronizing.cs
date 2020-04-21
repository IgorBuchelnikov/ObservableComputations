using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarSynchronizing<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IDestinationScalarSynchronizer PostingDestinationSynchronizer => _destinationScalarSynchronizer;
		public ISourceScalarSynchronizer SendingDestinationSynchronizer => _sourceScalarSynchronizer;

		private IDestinationScalarSynchronizer _destinationScalarSynchronizer;
		private ISourceScalarSynchronizer _sourceScalarSynchronizer;
		private IPostingSynchronizer _postingDistinationSynchronizer;

		private IReadScalar<TResult> _scalar;

		private PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		[ObservableComputationsCall]
		public ScalarSynchronizing(
			IReadScalar<TResult> scalar, 
			ISourceScalarSynchronizer sourceScalarSynchronizer,
			IDestinationScalarSynchronizer destinationScalarSynchronizer)
		{
			_scalar = scalar;
			_sourceScalarSynchronizer = sourceScalarSynchronizer;
			_destinationScalarSynchronizer = destinationScalarSynchronizer;
			_postingDistinationSynchronizer = destinationScalarSynchronizer as IPostingSynchronizer;

			_sourceScalarSynchronizer.InvokeReadAndSubscribe(() =>
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

			_postingDistinationSynchronizer.WaitLastPostComplete();

			TResult newValue = _scalar.Value;

			_destinationScalarSynchronizer.InvokeSetValue(
				() => setValue(newValue), this, sender, e);
		}

		~ScalarSynchronizing()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}