using System.Collections.Generic;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarPausing<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		private IReadScalar<TResult> _scalar;
		public IReadScalar<bool> IsPausedScalar => _isPausedScalar;
		public IReadScalar<int?> LastChangesToApplyOnResumeCountScalar => _lastChangesToApplyOnResumeCountScalar;

		public bool Resuming => _resuming;

		private IReadScalar<bool> _isPausedScalar;

		private IReadScalar<int?> _lastChangesToApplyOnResumeCountScalar;

		private bool _resuming;

		public int? LastChangesCountOnResume
		{
			get => _lastChangesToApplyOnResumeCount;
			set
			{
				if (_lastChangesToApplyOnResumeCountScalar != null) throw new ObservableComputationsException("Modifying of LastChangesToApplyOnResumeCount property is controlled by LastChangesToApplyOnResumeCountScalar");

				_lastChangesToApplyOnResumeCount = value;
			}
		}

		private bool _isPaused;
		public bool IsPaused
		{
			get => _isPaused;
			set
			{
				if (_isPausedScalar != null) throw new ObservableComputationsException("Modifying of IsPaused property is controlled by IsPausedScalar");

				_resuming = _isPaused != value && value;
				_isPaused = value;
				raisePropertyChanged(Utils.PausedPropertyChangedEventArgs);

				if (_resuming)
				{
					resume();
				}

			}
		}

		private void resume()
		{
			DefferedScalarAction<TResult> defferedScalarAction;
			int count = _defferedScalarActions.Count;

			_isConsistent = false;

			int startIndex = count - _lastChangesToApplyOnResumeCount ?? count;

			for (int i = 0; i < count; i++)
			{
				defferedScalarAction = _defferedScalarActions.Dequeue();
				if (i >= startIndex)
					handleScalarPropertyChanged(defferedScalarAction.EventSender, defferedScalarAction.PropertyChangedEventAgs,
						defferedScalarAction.Value);
			}

			_isConsistent = true;
			raiseConsistencyRestored();

			_resuming = false;
		}

		Queue<DefferedScalarAction<TResult>> _defferedScalarActions = new Queue<DefferedScalarAction<TResult>>();
		private int? _lastChangesToApplyOnResumeCount;

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			bool initialIsPaused = false,
			int? lastChangesToApplyOnResumeCount = null)
		{
			_isPaused = initialIsPaused;
			_lastChangesToApplyOnResumeCount = lastChangesToApplyOnResumeCount;
			_scalar = scalar;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			IReadScalar<bool> isPausedScalar,
			int? lastChangesToApplyOnResumeCount = null)
		{
			_isPausedScalar = isPausedScalar;
			_lastChangesToApplyOnResumeCount = lastChangesToApplyOnResumeCount;
            _scalar = scalar;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			IReadScalar<bool> isPausedScalar,
			IReadScalar<int?> lastChangesToApplyOnResumeCountScalar)
		{
			_isPausedScalar = isPausedScalar;
			_lastChangesToApplyOnResumeCountScalar = lastChangesToApplyOnResumeCountScalar;
            _scalar = scalar;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			bool initialIsPaused,
			IReadScalar<int?> lastChangesToApplyOnResumeCountScalar)
		{
			_isPaused = initialIsPaused;
			_lastChangesToApplyOnResumeCountScalar = lastChangesToApplyOnResumeCountScalar;
            _scalar = scalar;

		}

        private void initializeIsPauserScalar()
        {
            if (_isPausedScalar != null)
            {
                _isPausedScalar.PropertyChanged += handleIsPausedScalarValueChanged;
                _isPaused = _isPausedScalar.Value;
            }
        }

        private void initializeLastChangesCountOnResumeScalar()
        {
            if (_lastChangesToApplyOnResumeCountScalar != null)
            {
                _lastChangesToApplyOnResumeCountScalar.PropertyChanged += handleLastChangesToApplyOnResumeCountScalarValueChanged;
                _lastChangesToApplyOnResumeCount = _lastChangesToApplyOnResumeCountScalar.Value;
            }
        }

        protected override void initializeFromSource()
        {
        }

        protected override void initialize()
		{
			if (_isPaused) _defferedScalarActions.Enqueue(new DefferedScalarAction<TResult>(null, null, _scalar.Value));
			else setValue(_scalar.Value);

			_scalar.PropertyChanged += handleScalarPropertyChanged;

            initializeIsPauserScalar();
            initializeLastChangesCountOnResumeScalar();
        }

        protected override void uninitialize()
        {
            _scalar.PropertyChanged -= handleScalarPropertyChanged;

            if (_isPausedScalar != null)
            {
                _isPausedScalar.PropertyChanged -= handleIsPausedScalarValueChanged;			
            }

            if (_lastChangesToApplyOnResumeCountScalar != null)
            {
                _lastChangesToApplyOnResumeCountScalar.PropertyChanged -= handleLastChangesToApplyOnResumeCountScalarValueChanged;			
            }
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_isPausedScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_lastChangesToApplyOnResumeCountScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_isPausedScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_lastChangesToApplyOnResumeCountScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;
			handleScalarPropertyChanged(sender, e, _scalar.Value);
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e, TResult newScalarValue)
		{
			_handledEventSender = sender;
			_handledEventArgs = e;

			if (_isPaused) _defferedScalarActions.Enqueue(new DefferedScalarAction<TResult>(null, null, newScalarValue));
			else setValue(newScalarValue);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleIsPausedScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			bool newValue = _isPausedScalar.Value;
			_resuming = _isPaused != newValue && newValue;
			_isPaused = newValue;

			if (_resuming) resume();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleLastChangesToApplyOnResumeCountScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			_lastChangesToApplyOnResumeCount = _lastChangesToApplyOnResumeCountScalar.Value;

			if (_resuming) resume();

			_handledEventSender = null;
			_handledEventArgs = null;
		}
	}

	internal struct DefferedScalarAction<TResult>
	{
		public object EventSender;
		public PropertyChangedEventArgs PropertyChangedEventAgs;
		public TResult Value;

		public DefferedScalarAction(object eventSender, PropertyChangedEventArgs eventAgs, TResult value)
		{
			EventSender = eventSender;
			PropertyChangedEventAgs = eventAgs;
			Value = value;
		}
	}
}