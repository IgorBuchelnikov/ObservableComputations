// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarDispatching<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;

		public int DestinationOcDispatcherPriority => _destinationOcDispatcherPriority;
		public int SourceOcDispatcherPriority => _sourceOcDispatcherPriority;
		public object DestinationOcDispatcherParameter => _destinationOcDispatcherParameter;
		public object SourceOcDispatcherParameter => _sourceOcDispatcherParameter;

		private readonly IOcDispatcher _destinationOcDispatcher;
		private readonly IOcDispatcher _sourceOcDispatcher;

		private readonly IReadScalar<TResult> _scalar;
		private readonly Action _changeValueAction;

		private readonly int _destinationOcDispatcherPriority;
		private readonly int _sourceOcDispatcherPriority;
		private readonly object _destinationOcDispatcherParameter;
		private readonly object _sourceOcDispatcherParameter;

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> scalar, 
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			int destinationOcDispatcherPriority = 0,
			int sourceOcDispatcherPriority = 0,
			object destinationOcDispatcherParameter = null,
			object sourceOcDispatcherParameter = null)
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
					_destinationOcDispatcherPriority,
					_destinationOcDispatcherParameter,
					this);
			};

			_destinationOcDispatcherPriority = destinationOcDispatcherPriority;
			_sourceOcDispatcherPriority = sourceOcDispatcherPriority;
			_destinationOcDispatcherParameter = destinationOcDispatcherParameter;
			_sourceOcDispatcherParameter = sourceOcDispatcherParameter;
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

		#region Overrides of ScalarComputing<TResult>

		protected override void processSource()
		{
			if (_sourceEnumerated)
			{
				_scalar.PropertyChanged -= handleScalarPropertyChanged;
				_sourceEnumerated = false;
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
						_destinationOcDispatcherPriority,
						_destinationOcDispatcherParameter, 
						this);
				}

				if (_sourceOcDispatcher != null)
					_sourceOcDispatcher.Invoke(
						readAndSubscribe, 
						_sourceOcDispatcherPriority,
						_sourceOcDispatcherParameter, 
						this);
				else
					readAndSubscribe();

				_sourceEnumerated = true;
			}
			else
			{
				void setNewValue() => setDefaultValue();
				_destinationOcDispatcher.Invoke(
					setNewValue, 
					_destinationOcDispatcherPriority,
					_destinationOcDispatcherParameter,  
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

		protected override void raisePropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			_destinationOcDispatcher.Invoke(
				() => base.raisePropertyChanged(eventArgs), 
				_destinationOcDispatcherPriority,
				_destinationOcDispatcherParameter,
				this);
		}
	}
}