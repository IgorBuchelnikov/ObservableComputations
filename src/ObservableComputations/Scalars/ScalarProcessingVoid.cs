using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessingVoid<TValue> : ScalarComputing<TValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Action<TValue, ScalarProcessingVoid<TValue>> NewValueProcessor => _newValueProcessor;

		private IReadScalar<TValue> _scalar;

		private Action<TValue, ScalarProcessingVoid<TValue>> _newValueProcessor;

        private Action _changeValueAction;

		[ObservableComputationsCall]
		public ScalarProcessingVoid(
			IReadScalar<TValue> scalar,
			Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor,
			bool processNow = true) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
			TValue scalarValue = scalar.Value;
			if (processNow) processNewValue(scalarValue);
			setValue(scalarValue);
		}


		private ScalarProcessingVoid(
			IReadScalar<TValue> scalar)
		{
			_scalar = scalar;
			_scalar.PropertyChanged += handleScalarPropertyChanged;

            _changeValueAction = () =>
            {
                TValue newValue = _scalar.Value;
                processNewValue(newValue);
                setValue(newValue);
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

		private void processNewValue(TValue newValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
				_newValueProcessor(newValue, this);
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
			}
			else
			{
				_newValueProcessor(newValue, this);
			}
		}

        #region Overrides of ScalarComputing<TValue>

        protected override void initializeFromSource()
        {
        }

        protected override void initialize()
        {
            _scalar.PropertyChanged += handleScalarPropertyChanged;
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
