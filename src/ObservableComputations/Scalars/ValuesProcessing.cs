using System;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class ValuesProcessing<TValue, TReturnValue> : ScalarComputing<TReturnValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Func<TValue, ValuesProcessing<TValue, TReturnValue>, IReadScalar<TValue>, EventArgs, TReturnValue> NewValueProcessor => _newValueProcessor;

		private IReadScalar<TValue> _scalar;

		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		private Func<TValue, ValuesProcessing<TValue, TReturnValue>, IReadScalar<TValue>, EventArgs, TReturnValue> _newValueProcessor;

		[ObservableComputationsCall]
		public ValuesProcessing(
			IReadScalar<TValue> scalar,
			Func<TValue, ValuesProcessing<TValue, TReturnValue>, IReadScalar<TValue>, EventArgs, TReturnValue> newValueProcessor,
			bool processNow = true) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
			if (processNow)
			{
				setValue(processNewValue(scalar.Value, null, null));
			}
		}


		private ValuesProcessing(
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
			setValue(processNewValue(_scalar.Value, _scalar, e));
		}

		private TReturnValue processNewValue(TValue newValue, IReadScalar<TValue> sender, EventArgs eventArgs)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{

				Thread currentThread = Thread.CurrentThread;
				IComputing computing = DebugInfo._computingsExecutingUserCode.ContainsKey(currentThread)
					? DebugInfo._computingsExecutingUserCode[currentThread]
					: null;
				DebugInfo._computingsExecutingUserCode[currentThread] = this;

				TReturnValue returnValue = _newValueProcessor(newValue, this, sender, eventArgs);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;

				return returnValue;
			}
			else
			{
				return _newValueProcessor(newValue, this, sender, eventArgs);
			}
		}


		~ValuesProcessing()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}
