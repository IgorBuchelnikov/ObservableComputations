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

		private PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		private IReadScalar<bool> _isPausedScalar;
		private PropertyChangedEventHandler _isPausedScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _isPausedScalarWeakPropertyChangedEventHandler;

		private IReadScalar<int?> _lastChangesCountOnResumeScalar;
		private PropertyChangedEventHandler _lastChangesCountOnResumeScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _lastChangesCountOnResumeScalarWeakPropertyChangedEventHandler;


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
			initialize(scalar);
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			IReadScalar<bool> isPausedScalar,
			int? lastChangesCountOnResume = null)
		{
			_isPausedScalar = isPausedScalar;
			_isPausedScalarPropertyChangedEventHandler = handleIsPausedScalarValueChanged;
			_isPausedScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_isPausedScalarPropertyChangedEventHandler);
			_isPausedScalar.PropertyChanged += _isPausedScalarWeakPropertyChangedEventHandler.Handle;
			_isPaused = isPausedScalar.Value;

			_lastChangesCountOnResume = lastChangesCountOnResume;

			initialize(scalar);
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			IReadScalar<bool> isPausedScalar,
			IReadScalar<int?> lastChangesCountOnResumeScalar)
		{
			_isPausedScalar = isPausedScalar;
			_isPausedScalarPropertyChangedEventHandler = handleIsPausedScalarValueChanged;
			_isPausedScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_isPausedScalarPropertyChangedEventHandler);
			_isPausedScalar.PropertyChanged += _isPausedScalarWeakPropertyChangedEventHandler.Handle;
			_isPaused = isPausedScalar.Value;

			_lastChangesCountOnResumeScalar = lastChangesCountOnResumeScalar;
			_lastChangesCountOnResumeScalarPropertyChangedEventHandler = handleLastChangesCountOnResumeScalarValueChanged;
			_lastChangesCountOnResumeScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_lastChangesCountOnResumeScalarPropertyChangedEventHandler);
			_lastChangesCountOnResumeScalar.PropertyChanged += _lastChangesCountOnResumeScalarWeakPropertyChangedEventHandler.Handle;
			_lastChangesCountOnResume = lastChangesCountOnResumeScalar.Value;

			initialize(scalar);
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			bool initialIsPaused,
			IReadScalar<int?> lastChangesCountOnResumeScalar)
		{
			_isPaused = initialIsPaused;

			_lastChangesCountOnResumeScalar = lastChangesCountOnResumeScalar;
			_lastChangesCountOnResumeScalarPropertyChangedEventHandler = handleLastChangesCountOnResumeScalarValueChanged;
			_lastChangesCountOnResumeScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_lastChangesCountOnResumeScalarPropertyChangedEventHandler);
			_lastChangesCountOnResumeScalar.PropertyChanged += _lastChangesCountOnResumeScalarWeakPropertyChangedEventHandler.Handle;
			_lastChangesCountOnResume = lastChangesCountOnResumeScalar.Value;

			initialize(scalar);
		}

		private void initialize(IReadScalar<TResult> scalar)
		{
			_scalar = scalar;

			if (_isPaused) _defferedScalarActions.Enqueue(new DefferedScalarAction<TResult>(null, null, _scalar.Value));
			else setValue(_scalar.Value);

			_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
			_scalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
			_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
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

		~ScalarPausing()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;

			if (_isPausedScalarWeakPropertyChangedEventHandler != null)
			{
				_isPausedScalar.PropertyChanged -= _isPausedScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_lastChangesCountOnResumeScalarWeakPropertyChangedEventHandler != null)
			{
				_lastChangesCountOnResumeScalar.PropertyChanged -= _lastChangesCountOnResumeScalarWeakPropertyChangedEventHandler.Handle;			
			}
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