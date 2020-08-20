using System;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class ScalarProcessing<TValue, TReturnValue> : ScalarComputing<TReturnValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Func<TValue, IScalarComputing, TReturnValue, TReturnValue> NewValueProcessor => _newValueProcessor;

		private IReadScalar<TValue> _scalar;

		private Func<TValue, IScalarComputing, TReturnValue, TReturnValue> _newValueProcessor;

		private TReturnValue _returnValue;

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
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TValue>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			_returnValue = processNewValue(_scalar.Value);
			setValue(_returnValue);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private TReturnValue processNewValue(TValue newValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				TReturnValue returnValue = _newValueProcessor(newValue, this, _returnValue);
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);

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
