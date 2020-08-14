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
        private bool _sourceInitialized;


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
            if (_sourceInitialized)
            {
                _scalar.PropertyChanged -= handleScalarPropertyChanged;
                _sourceInitialized = false;
            }

            if (_isActive)
            {
                _scalar.PropertyChanged += handleScalarPropertyChanged;
                setValue(_scalar.Value);
                _sourceInitialized = false;
            }
            else
            {
                _previousValue = default;
                raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
                setValue(default);
                if (_isEverChanged)
                {
                    _isEverChanged = false;
                    raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
                }
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