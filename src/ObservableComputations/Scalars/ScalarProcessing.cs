using System;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class ScalarProcessing<TValue, TReturnValue> : ScalarComputing<TReturnValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Func<TValue, IScalarComputing, TReturnValue, TReturnValue> NewValueProcessor => _newValueProcessor;

		private IReadScalar<TValue> _scalar;

		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		private Func<TValue, IScalarComputing, TReturnValue, TReturnValue> _newValueProcessor;

		private TReturnValue _returnValue;

		[ObservableComputationsCall]
		public ScalarProcessing(
			IReadScalar<TValue> scalar,
			Func<TValue, IScalarComputing, TReturnValue, TReturnValue> newValueProcessor,
			bool processNow = true) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
			if (processNow)
			{
				setValue(processNewValue(scalar.Value));
			}
		}


		private ScalarProcessing(
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

			_handledEventSender = sender;
			_handledEventArgs = e;

			_returnValue = processNewValue(_scalar.Value);
			setValue(_returnValue);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private TReturnValue processNewValue(TValue newValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{

				Thread currentThread = Thread.CurrentThread;
				IComputing computing = DebugInfo._computingsExecutingUserCode.ContainsKey(currentThread)
					? DebugInfo._computingsExecutingUserCode[currentThread]
					: null;
				DebugInfo._computingsExecutingUserCode[currentThread] = this;
				_userCodeIsCalledFrom = computing;

				TReturnValue returnValue = _newValueProcessor(newValue, this, _returnValue);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;

				return returnValue;
			}
			else
			{
				return _newValueProcessor(newValue, this, _returnValue);
			}
		}


		~ScalarProcessing()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}
