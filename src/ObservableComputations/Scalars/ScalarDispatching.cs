using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarDispatching<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;

		// ReSharper disable once UnusedMember.Local
		private DispatcherPriorities DispatcherPriorities => _dispatcherPriorities;
		// ReSharper disable once UnusedMember.Local
		private DispatcherParameters DispatcherParameters => _dispatcherParameters;

		private readonly IOcDispatcher _destinationOcDispatcher;
		private readonly IOcDispatcher _sourceOcDispatcher;

		private readonly IReadScalar<TResult> _scalar;
		private readonly Action _changeValueAction;

		private readonly DispatcherPriorities _dispatcherPriorities;
		private readonly DispatcherParameters _dispatcherParameters;

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			DispatcherPriorities? dispatcherPriorities = null,
			DispatcherParameters? dispatcherParameters = null)
		{
			_destinationOcDispatcher = destinationOcDispatcher;
			_scalar = scalar;
			_sourceOcDispatcher = sourceOcDispatcher;
			_changeValueAction = () =>
			{
				TResult newValue = _scalar.Value;
				void setNewValue() => setValue(newValue);

				_destinationOcDispatcher.Invoke(
					setNewValue, 
					_dispatcherPriorities._destinationDispatcherPriority,
					_dispatcherParameters._distinationDispatcherParameter,
					this);
			};

			_dispatcherPriorities = dispatcherPriorities ?? new DispatcherPriorities(0, 0);
			_dispatcherParameters = dispatcherParameters ?? new DispatcherParameters(null, null);
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
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
				void readAndSubscribe()
				{
					TResult newValue = _scalar.Value;
					void setNewValue() => setValue(newValue);

					_scalar.PropertyChanged += handleScalarPropertyChanged;
					_destinationOcDispatcher.Invoke(
						setNewValue, 
						_dispatcherPriorities._destinationDispatcherPriority,
						_dispatcherParameters._distinationDispatcherParameter, 
						this);
				}

				if (_sourceOcDispatcher != null)
					_sourceOcDispatcher.Invoke(
						readAndSubscribe, 
						_dispatcherPriorities._sourceDispatcherPriority,
						_dispatcherParameters._sourceDispatcherParameter, 
						this);
				else
					readAndSubscribe();

				_initializedFromSource = true;
			}
			else
			{
				void setNewValue() => setDefaultValue();
				_destinationOcDispatcher.Invoke(
					setNewValue, 
					_dispatcherPriorities._destinationDispatcherPriority,
					_dispatcherParameters._distinationDispatcherParameter,  
					this);
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