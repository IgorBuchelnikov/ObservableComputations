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

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			IDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_destinationDispatcher = destinationDispatcher;
            _scalar = scalar;
            _sourceDispatcher = sourceDispatcher;
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

        #region Overrides of ScalarComputing<TResult>

        protected override void initializeFromSource()
        {

        }

        protected override void initialize()
        {
            void readAndSubscribe()
            {
                TResult newValue = _scalar.Value;
                void setNewValue() => setValue(newValue);

                _destinationDispatcher.Invoke(setNewValue, this);
                _scalar.PropertyChanged += handleScalarPropertyChanged;
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

        protected override void uninitialize()
        {
            _scalar.PropertyChanged -= handleScalarPropertyChanged;
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        #endregion
    }
}