using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class PreviousTracking<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public TResult PreviousValue => _previousValue;
		public bool IsEverChanged => _isEverChanged;

		private IReadScalar<TResult> _scalar;
		private TResult _previousValue;
		private bool _isEverChanged;
        private Action _changeValueAction;


		[ObservableComputationsCall]
		public PreviousTracking(
			IReadScalar<TResult> scalar)
		{
            _changeValueAction = () =>
            {
                TResult newValue = _scalar.Value;
                _previousValue = _value;

                if (!_isEverChanged)
                {
                    _isEverChanged = true;
                    raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
                }

                raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
                setValue(newValue);
            };

			_scalar = scalar;
			_value = _scalar.Value;
			_scalar.PropertyChanged += handleScalarPropertyChanged;
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

        #region Overrides of ScalarComputing<TResult>

        protected override void initializeFromSource()
        {
        }

        protected override void initialize()
        {
            _scalar.PropertyChanged += handleScalarPropertyChanged;
            setValue(_scalar.Value);
        }

        protected override void uninitialize()
        {
            _scalar.PropertyChanged -= handleScalarPropertyChanged;
            _previousValue = default;
            raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
            setValue(default);
            if (_isEverChanged)
            {
                _isEverChanged = false;
                raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
            }
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