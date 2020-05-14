using System;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class ValuesProcessingVoid<TValue> : ScalarComputing<TValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Action<TValue, ValuesProcessingVoid<TValue>, IReadScalar<TValue>, EventArgs> NewValueProcessor => _newValueProcessor;

		private IReadScalar<TValue> _scalar;

		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		private Action<TValue, ValuesProcessingVoid<TValue>, IReadScalar<TValue>, EventArgs> _newValueProcessor;

		[ObservableComputationsCall]
		public ValuesProcessingVoid(
			IReadScalar<TValue> scalar,
			Action<TValue, ValuesProcessingVoid<TValue>, IReadScalar<TValue>, EventArgs> newValueProcessor,
			bool processNow = true) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
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
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;
				_userCodeIsCalledFrom = computing;

				_newValueProcessor(newValue, this, sender, eventArgs);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				_newValueProcessor(newValue, this, sender, eventArgs);
			}
		}


		~ValuesProcessingVoid()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}
