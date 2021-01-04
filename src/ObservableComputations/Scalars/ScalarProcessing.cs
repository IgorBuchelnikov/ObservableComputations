using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessing<TValue, TReturnValue> : ScalarComputing<TReturnValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Func<TValue, IScalarComputing, TReturnValue> NewValueProcessor => _newValueProcessor;

		private readonly IReadScalar<TValue> _scalar;

		private readonly Func<TValue, IScalarComputing, TReturnValue> _newValueProcessor;
		private readonly Action<TValue, IScalarComputing, TReturnValue> _oldValueProcessor;

		private TValue _oldValue;

		[ObservableComputationsCall]
		public ScalarProcessing(
			IReadScalar<TValue> scalar,
			Func<TValue, IScalarComputing, TReturnValue> newValueProcessor,
			Action<TValue, IScalarComputing, TReturnValue> oldValueProcessor = null) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
			_oldValueProcessor = oldValueProcessor;
		}

		private ScalarProcessing(
			IReadScalar<TValue> scalar)
		{
			_scalar = scalar;
		}

		private void updateValue()
		{
			processOldValue(_oldValue);
			setNewValue();
		}

		private void setNewValue()
		{
			TValue newValue = _scalar.Value;
			_oldValue = newValue;
			setValue(processNewValue(newValue));
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(Value)) return;

			Utils.processChange(
				sender, 
				e, 
				updateValue,
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs, 
				0, _deferredQueuesCount,
				ref _deferredProcessings, this);
		}

		private TReturnValue processNewValue(TValue newValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TReturnValue returnValue = _newValueProcessor(newValue, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);

				return returnValue;
			}
			else
			{
				return _newValueProcessor(newValue, this);
			}
		}

		private void processOldValue(TValue oldValue)
		{
			if (_oldValueProcessor == null) return;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_oldValueProcessor(oldValue, this, _value);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);

			}
			else
			{
				 _oldValueProcessor(oldValue, this, _value);
			}
		}

		private bool _initializedFromSource;
		#region Overrides of ScalarComputing<TResult>

		protected override void initializeFromSource()
		{
			if (_initializedFromSource)
			{
				_scalar.PropertyChanged -= handleScalarPropertyChanged;
				processOldValue(_oldValue);
				_initializedFromSource = false;
			}

			if (_isActive)
			{
				if (_scalar is IComputing scalarComputing)
				{
					if (scalarComputing.IsActive) setNewValue();
				}
				else
					setNewValue();

				_scalar.PropertyChanged += handleScalarPropertyChanged;
				_initializedFromSource = true;
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
