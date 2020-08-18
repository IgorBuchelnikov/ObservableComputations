using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarPausing<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		private IReadScalar<TResult> _scalar;
		public IReadScalar<bool> IsPausedScalar => _isPausedScalar;
		public IReadScalar<int?> LastChangesCountOnResumeScalar => _lastChangesCountOnResumeScalar;

		public bool Resuming => _resuming;

		private IReadScalar<bool> _isPausedScalar;

		private IReadScalar<int?> _lastChangesCountOnResumeScalar;

		private bool _resuming;

		public int? LastChangesCountOnResume
		{
			get => _lastChangesCountOnResume;
			set
			{
				if (_lastChangesCountOnResumeScalar != null) throw new ObservableComputationsException("Modifying of LastChangesCountOnResume property is controlled by LastChangesCountOnResumeScalar");

				_lastChangesCountOnResume = value;
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

			int startIndex = count - _lastChangesCountOnResume != null ? count = _lastChangesCountOnResume.Value : count;

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
		private int? _lastChangesCountOnResume;

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			bool initialIsPaused = false,
			int? lastChangesCountOnResume = null)
		{
			_isPaused = initialIsPaused;
			_lastChangesCountOnResume = lastChangesCountOnResume;
			_scalar = scalar;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			IReadScalar<bool> isPausedScalar,
			int? lastChangesCountOnResume = null)
		{
			_isPausedScalar = isPausedScalar;
			_lastChangesCountOnResume = lastChangesCountOnResume;
            _scalar = scalar;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			IReadScalar<bool> isPausedScalar,
			IReadScalar<int?> lastChangesCountOnResumeScalar)
		{
			_isPausedScalar = isPausedScalar;
			_lastChangesCountOnResumeScalar = lastChangesCountOnResumeScalar;
            _scalar = scalar;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			bool initialIsPaused,
			IReadScalar<int?> lastChangesCountOnResumeScalar)
		{
			_isPaused = initialIsPaused;
			_lastChangesCountOnResumeScalar = lastChangesCountOnResumeScalar;
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
            if (_lastChangesCountOnResumeScalar != null)
            {
                _lastChangesCountOnResumeScalar.PropertyChanged += handleLastChangesCountOnResumeScalarValueChanged;
                _lastChangesCountOnResume = _lastChangesCountOnResumeScalar.Value;
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

            if (_lastChangesCountOnResumeScalar != null)
            {
                _lastChangesCountOnResumeScalar.PropertyChanged -= handleLastChangesCountOnResumeScalarValueChanged;			
            }
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_isPausedScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_lastChangesCountOnResumeScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_isPausedScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_lastChangesCountOnResumeScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
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

		private void handleLastChangesCountOnResumeScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			_lastChangesCountOnResume = _lastChangesCountOnResumeScalar.Value;

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