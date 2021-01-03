using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarPausing<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		private readonly IReadScalar<TResult> _scalar;
		public IReadScalar<bool> IsPausedScalar => _isPausedScalar;
		public IReadScalar<int?> LastChangesToApplyOnResumeCountScalar => _lastChangesToApplyOnResumeCountScalar;

		public bool Resuming => _resuming;

		private readonly IReadScalar<bool> _isPausedScalar;

		private readonly IReadScalar<int?> _lastChangesToApplyOnResumeCountScalar;

		private bool _resuming;

		private readonly Action _changeValueAction;

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
				checkConsistent(null, null);

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
			_isConsistent = false;

			int count = _deferredScalarActions.Count;
			int startIndex = count - _lastChangesToApplyOnResumeCount ?? count;

			for (int i = 0; i < count; i++)
			{
				DeferredScalarAction<TResult> deferredScalarAction = _deferredScalarActions.Dequeue();
				if (i >= startIndex)
				{
					_handledEventSender = deferredScalarAction.EventSender;
					_handledEventArgs = deferredScalarAction.PropertyChangedEventAgs;

					setValue(deferredScalarAction.Value);

					_handledEventSender = null;
					_handledEventArgs = null;
				}
			}
			_resuming = false;

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		readonly Queue<DeferredScalarAction<TResult>> _deferredScalarActions = new Queue<DeferredScalarAction<TResult>>();
		private int? _lastChangesToApplyOnResumeCount;

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			bool initialIsPaused = false,
			int? lastChangesToApplyOnResumeCount = 1) : this()
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

		private ScalarPausing()
		{
			_changeValueAction = () => setValue(_scalar.Value);
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

		private bool _initializedFromSource;

		protected override void initializeFromSource()
		{
			if (_initializedFromSource)
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

				_deferredScalarActions.Clear();
				_initializedFromSource = false;
			}

			if (_isActive)
			{
				if (_isPaused) _deferredScalarActions.Enqueue(new DeferredScalarAction<TResult>(null, null, _scalar.Value));
				else setValue(_scalar.Value);

				_scalar.PropertyChanged += handleScalarPropertyChanged;

				initializeIsPauserScalar();
				initializeLastChangesCountOnResumeScalar();
				_initializedFromSource = true;
			}
			else
			{
				setDefaultValue();
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
			(_isPausedScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_lastChangesToApplyOnResumeCountScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			(_scalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_isPausedScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_lastChangesToApplyOnResumeCountScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

  //	  private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		//{
		//	if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;
		//	handleScalarPropertyChanged(sender, e, _scalar.Value);
		//}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_isPaused) 
				_deferredScalarActions.Enqueue(new DeferredScalarAction<TResult>(sender, e, _scalar.Value));
			else			 
				Utils.processChange(
					sender, 
					e, 
					_changeValueAction,
					ref _isConsistent, 
					ref _handledEventSender, 
					ref _handledEventArgs, 
					0, _deferredQueuesCount,
					ref _deferredProcessings, this);

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

	internal struct DeferredScalarAction<TResult>
	{
		public object EventSender;
		public PropertyChangedEventArgs PropertyChangedEventAgs;
		public TResult Value;

		public DeferredScalarAction(object eventSender, PropertyChangedEventArgs eventAgs, TResult value)
		{
			EventSender = eventSender;
			PropertyChangedEventAgs = eventAgs;
			Value = value;
		}
	}
}