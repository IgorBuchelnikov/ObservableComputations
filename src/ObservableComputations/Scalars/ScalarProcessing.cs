using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessing<TValue, TReturnValue> : ScalarComputing<TReturnValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Func<TValue, IScalarComputing, TReturnValue, TReturnValue> NewValueProcessor => _newValueProcessor;

		private IReadScalar<TValue> _scalar;

		private Func<TValue, IScalarComputing, TReturnValue, TReturnValue> _newValueProcessor;

		private TReturnValue _returnValue;
        private Action _changeValueAction;

		[ObservableComputationsCall]
		public ScalarProcessing(
			IReadScalar<TValue> scalar,
			Func<TValue, IScalarComputing, TReturnValue, TReturnValue> newValueProcessor,
			bool processNow = true) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
			if (processNow)
			{
				setValue(processNewValue(scalar.Value));
			}
		}


		private ScalarProcessing(
			IReadScalar<TValue> scalar)
		{
			_scalar = scalar;

            _changeValueAction = () =>
            {
                _returnValue = processNewValue(_scalar.Value);
                setValue(_returnValue);
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

		private TReturnValue processNewValue(TValue newValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
				TReturnValue returnValue = _newValueProcessor(newValue, this, _returnValue);
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);

				return returnValue;
			}
			else
			{
				return _newValueProcessor(newValue, this, _returnValue);
			}
		}

        #region Overrides of ScalarComputing<TReturnValue>

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
