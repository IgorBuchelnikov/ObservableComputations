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


		[ObservableComputationsCall]
		public PreviousTracking(
			IReadScalar<TResult> scalar)
		{
			_scalar = scalar;
			_value = _scalar.Value;
			_scalar.PropertyChanged += handleScalarPropertyChanged;
		}


		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			TResult newValue = _scalar.Value;
			_previousValue = _value;
			_isConsistent = false;

			if (!_isEverChanged)
			{
				_isEverChanged = true;
				raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
			}

			raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
			setValue(newValue);
			raiseConsistencyRestored();

			_handledEventSender = null;
			_handledEventArgs = null;
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