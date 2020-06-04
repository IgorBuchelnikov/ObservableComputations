using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarDispatching<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IDispatcher DestinationDispatcher => _destinationDispatcher;
		public IDispatcher SourceDispatcher => _sourceDispatcher;

		private IDispatcher _destinationDispatcher;
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

		private void initialize(IReadScalar<TResult> scalar, IDispatcher sourceDispatcher)
		{
			_scalar = scalar;
			_sourceDispatcher = sourceDispatcher;

			void readAndSubscribe()
			{
				void setNewValue() => setValue(_scalar.Value);

				_destinationDispatcher.Invoke(setNewValue, this);

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

			_handledEventSender = sender;
			_handledEventArgs = e;


			TResult newValue = _scalar.Value;

			void setNewValue() => setValue(newValue);

			_destinationDispatcher.Invoke(setNewValue, this);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		~ScalarDispatching()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}