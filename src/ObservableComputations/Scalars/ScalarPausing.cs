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

		private PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		public int? LastChangesCountOnResume
		{
			get => _lastChangesCountOnResume;
			set => _lastChangesCountOnResume = value;
		}

		private bool _paused;
		public bool Paused
		{
			get => _paused;
			set
			{
				bool resuming = _paused != value && value;
				_paused = value;
				raisePropertyChanged(Utils.PausedPropertyChangedEventArgs);

				DefferedScalarAction<TResult> defferedScalarAction;
				int count = _defferedScalarActions.Count;

				if (resuming)
				{
					_isConsistent = false;

					int startIndex = _lastChangesCountOnResume != null ? count = _lastChangesCountOnResume.Value : 0;

					for (int i = 0; i < count; i++)
					{
						defferedScalarAction = _defferedScalarActions.Dequeue();
						if (i >= startIndex) 
							handleScalarPropertyChanged(defferedScalarAction.EventSender, defferedScalarAction.PropertyChangedEventAgs, defferedScalarAction.Value);
					}

					_isConsistent = true;
					raiseConsistencyRestored();
				}

			}
		}

		Queue<DefferedScalarAction<TResult>> _defferedScalarActions = new Queue<DefferedScalarAction<TResult>>();
		private int? _lastChangesCountOnResume;

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> scalar,
			bool paused = false,
			int? lastChangesCountOnResume = null)
		{
			_paused = paused;
			_lastChangesCountOnResume = lastChangesCountOnResume;
			initialize(scalar);
		}

		private void initialize(IReadScalar<TResult> scalar)
		{
			_scalar = scalar;

			if (_paused) _defferedScalarActions.Enqueue(new DefferedScalarAction<TResult>(null, null, _scalar.Value));
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

			if (_paused) _defferedScalarActions.Enqueue(new DefferedScalarAction<TResult>(null, null, newScalarValue));
			else setValue(newScalarValue);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		~ScalarPausing()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
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