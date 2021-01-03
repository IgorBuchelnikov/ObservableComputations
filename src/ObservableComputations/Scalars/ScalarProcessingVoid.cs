using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessingVoid<TValue> : ScalarComputing<TValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Action<TValue, ScalarProcessingVoid<TValue>> NewValueProcessor => _newValueProcessor;

		private readonly IReadScalar<TValue> _scalar;

		private readonly Action<TValue, ScalarProcessingVoid<TValue>> _newValueProcessor;

		[ObservableComputationsCall]
		public ScalarProcessingVoid(
			IReadScalar<TValue> scalar,
			Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
		}

		private ScalarProcessingVoid(
			IReadScalar<TValue> scalar)
		{
			_scalar = scalar;
		}

		private void setNewValue()
		{
			TValue newValue = _scalar.Value;
			setNewValue(newValue);
		}

		private void setNewValue(TValue newValue)
		{
			processNewValue(newValue);
			setValue(newValue);
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
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

		private void processNewValue(TValue newValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_newValueProcessor(newValue, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
			}
			else
			{
				_newValueProcessor(newValue, this);
			}
		}

		private bool _initializedFromSource;
		#region Overrides of ScalarComputing<TResult>

		protected override void initializeFromSource()
		{
			if (_initializedFromSource)
			{
				_scalar.PropertyChanged -= handleScalarPropertyChanged;
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
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			(_scalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		#endregion
	}
}
