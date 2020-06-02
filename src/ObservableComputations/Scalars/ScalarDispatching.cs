using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarDispatching<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IDispatcher DestinationDispatcher => _destinationDispatcher;
		public IScalarDestinationDispatcher ScalarDestinationDispatcher => _scalarDestinationDispatcher;
		public IDispatcher SourceDispatcher => _sourceDispatcher;

		private IDispatcher _destinationDispatcher;
		private IScalarDestinationDispatcher _scalarDestinationDispatcher;
		private IDispatcher _sourceDispatcher;

		private IReadScalar<TResult> _scalar;

		private PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			IDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_destinationDispatcher = destinationDispatcher;
			initialize(scalar, sourceDispatcher);
		}

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			IScalarDestinationDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_scalarDestinationDispatcher = destinationDispatcher;
			initialize(scalar, sourceDispatcher);
		}

		private void initialize(IReadScalar<TResult> scalar, IDispatcher sourceDispatcher)
		{
			_scalar = scalar;
			_sourceDispatcher = sourceDispatcher;

			void readAndSubscribe()
			{
				void setNewValue() => setValue(_scalar.Value);

				if (_destinationDispatcher != null) _destinationDispatcher.Invoke(setNewValue, this);
				else _scalarDestinationDispatcher.Invoke(setNewValue, this, null);

				_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
				_scalarWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
				_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
			}

			if (_sourceDispatcher != null)
			{
				_sourceDispatcher.Invoke(readAndSubscribe, this);
			}
			else
			{
				readAndSubscribe();
			}
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;

			_processingEventSender = sender;
			_processingEventArgs = e;


			TResult newValue = _scalar.Value;

			void setNewValue() => setValue(newValue);

			if (_destinationDispatcher != null) _destinationDispatcher.Invoke(setNewValue, this);
			else _scalarDestinationDispatcher.Invoke(setNewValue, this, e);

			_processingEventSender = null;
			_processingEventArgs = null;
		}

		~ScalarDispatching()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}