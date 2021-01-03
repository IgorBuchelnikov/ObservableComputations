using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessing<TValue, TReturnValue> : ScalarComputing<TReturnValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Func<TValue, IScalarComputing, TReturnValue, TReturnValue> NewValueProcessor => _newValueProcessor;

		private readonly IReadScalar<TValue> _scalar;

		private readonly Func<TValue, IScalarComputing, TReturnValue, TReturnValue> _newValueProcessor;

		private TReturnValue _returnValue;

		[ObservableComputationsCall]
		public ScalarProcessing(
			IReadScalar<TValue> scalar,
			Func<TValue, IScalarComputing, TReturnValue, TReturnValue> newValueProcessor) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
		}


		private ScalarProcessing(
			IReadScalar<TValue> scalar)
		{
			_scalar = scalar;
		}

		private void setNewValue()
		{
			TValue scalarValue = _scalar.Value;
			setNewValue(scalarValue);
		}

		private void setNewValue(TValue scalarValue)
		{
			_returnValue = processNewValue(scalarValue);
			setValue(_returnValue);
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(Value)) return;

			Utils.processChange(
				sender, 
				e, 
				setNewValue,
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
				TReturnValue returnValue = _newValueProcessor(newValue, this, _returnValue);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);

				return returnValue;
			}
			else
			{
				return _newValueProcessor(newValue, this, _returnValue);
			}
		}

		private bool _initializedFromSource;
		#region Overrides of ScalarComputing<TResult>

		protected override void initializeFromSource()
		{
			if (_initializedFromSource)
			{
				_scalar.PropertyChanged -= handleScalarPropertyChanged;
				setNewValue(default);
				_initializedFromSource = false;
			}

			if (_isActive)
			{
				if (_scalar is IComputing scalarComputing)
				{
					if (scalarComputing.IsActive) setNewValue(_scalar.Value);
				}
				else
					setNewValue(_scalar.Value);

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
