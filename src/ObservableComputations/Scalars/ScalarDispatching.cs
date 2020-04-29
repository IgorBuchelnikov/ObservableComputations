using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarDispatching<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IDispatcher DestinationDispatcher => _destinationDispatcher;
		public IDispatcher SourceDispatcher => _sourceDispatcher;

		private IDestinationScalarDispatcher _destinationScalarDispatcher;
		private ISourceScalarDispatcher _sourceScalarDispatcher;

		private IDispatcher _destinationDispatcher;
		private IDispatcher _sourceDispatcher;

		private IReadScalar<TResult> _scalar;

		private PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;



		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			IDispatcher sourceDispatcher,
			IDispatcher destinationDispatcher)
		{
			_scalar = scalar;
			_sourceDispatcher = sourceDispatcher;
			_destinationDispatcher = destinationDispatcher;

			_sourceScalarDispatcher = sourceDispatcher as ISourceScalarDispatcher;
			_destinationScalarDispatcher = destinationDispatcher as IDestinationScalarDispatcher;

			Action readAndSubscribe = () =>
			{
				_value = _scalar.Value;
				_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
				_scalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
				_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
			};

			if (_sourceScalarDispatcher != null)
				_sourceScalarDispatcher.InvokeReadAndSubscribe(readAndSubscribe, this);
			else
				_sourceDispatcher.Invoke(readAndSubscribe);


		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;

			TResult newValue = _scalar.Value;

			void setNewValue() => setValue(newValue);


			if (_destinationScalarDispatcher != null)
				_destinationScalarDispatcher.InvokeSetValue(setNewValue, this, sender, e);
			else
				_destinationDispatcher.Invoke(setNewValue);
		}

		~ScalarDispatching()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}