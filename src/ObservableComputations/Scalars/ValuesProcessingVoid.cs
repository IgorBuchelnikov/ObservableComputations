using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class ValuesProcessingVoid<TValue> : ScalarComputing<TValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Action<TValue, IReadScalar<TValue>, EventArgs> NewValueProcessorAction => _newValueProcessorAction;

		private IReadScalar<TValue> _scalar;

		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		private Action<TValue, IReadScalar<TValue>, EventArgs> _newValueProcessorAction;

		[ObservableComputationsCall]
		public ValuesProcessingVoid(
			IReadScalar<TValue> scalar,
			Action<TValue, IReadScalar<TValue>, EventArgs> newValueProcessor,
			bool processNow = true) : this(scalar)
		{
			_newValueProcessorAction = newValueProcessor;
			TValue scalarValue = scalar.Value;
			if (processNow) processNewValue(scalarValue, null, null);
			setValue(scalarValue);
		}


		private ValuesProcessingVoid(
			IReadScalar<TValue> scalar)
		{
			_scalar = scalar;
			_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
			_scalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
			_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TValue>.Value)) return;
			TValue newValue = _scalar.Value;
			processNewValue(newValue, _scalar, e);
			setValue(newValue);
		}

		private void processNewValue(TValue newValue, IReadScalar<TValue> sender, EventArgs eventArgs)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				IComputing computing = DebugInfo._computingsExecutingUserCode.ContainsKey(currentThread)
					? DebugInfo._computingsExecutingUserCode[currentThread]
					: null;
				DebugInfo._computingsExecutingUserCode[currentThread] = this;

				_newValueProcessorAction(newValue, sender, eventArgs);

				if (computing == null) DebugInfo._computingsExecutingUserCode.Remove(currentThread);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
			}
			else
			{
				_newValueProcessorAction(newValue, sender, eventArgs);
			}
		}


		~ValuesProcessingVoid()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}
