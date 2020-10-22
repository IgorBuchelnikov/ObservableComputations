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
        private Action _changeValueAction;

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			IDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_destinationDispatcher = destinationDispatcher;
            _scalar = scalar;
            _sourceDispatcher = sourceDispatcher;
            _changeValueAction = () =>
            {
                TResult newValue = _scalar.Value;
                void setNewValue() => setValue(newValue);

                _destinationDispatcher.Invoke(setNewValue, this);
            };
        }

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
            Utils.processChange(
                sender, 
                e, 
                _changeValueAction,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, this);
		}

        private bool _initializedFromSource;
        #region Overrides of ScalarComputing<TResult>

        protected override void initializeFromSource()
        {
            if (_initializedFromSource)
            {
                _scalar.PropertyChanged -= handleScalarPropertyChanged;
                _initializedFromSource = false;
            }

            if (_isActive)
            {
                void readAndSubscribe()
                {
                    TResult newValue = _scalar.Value;
                    void setNewValue() => setValue(newValue);

                    _scalar.PropertyChanged += handleScalarPropertyChanged;
                    _destinationDispatcher.Invoke(setNewValue, this);
                }

                if (_sourceDispatcher != null)
                    _sourceDispatcher.Invoke(readAndSubscribe, this);
                else
                    readAndSubscribe();

                _initializedFromSource = true;
            }
            else
            {
                void setNewValue() => setValue(default);
                _destinationDispatcher.Invoke(setNewValue, this);
            }
        }

        protected override void initialize()
        {

        }

        protected override void uninitialize()
        {

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