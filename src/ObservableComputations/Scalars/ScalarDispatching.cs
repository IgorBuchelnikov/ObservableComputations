// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ObservableComputations
{
	public class ScalarDispatching<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Source => _source;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;

		public int DestinationOcDispatcherPriority => _destinationOcDispatcherPriority;
		public int SourceOcDispatcherPriority => _sourceOcDispatcherPriority;
		public object DestinationOcDispatcherParameter => _destinationOcDispatcherParameter;
		public object SourceOcDispatcherParameter => _sourceOcDispatcherParameter;

		private readonly IOcDispatcher _destinationOcDispatcher;
		private readonly IOcDispatcher _sourceOcDispatcher;

		private readonly IReadScalar<TResult> _source;
		private readonly Action _changeValueAction;

		private readonly int _destinationOcDispatcherPriority;
		private readonly int _sourceOcDispatcherPriority;
		private readonly object _destinationOcDispatcherParameter;
		private readonly object _sourceOcDispatcherParameter;

		[ObservableComputationsCall]
		public ScalarDispatching(
			IReadScalar<TResult> source, 
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			int destinationOcDispatcherPriority = 0,
			int sourceOcDispatcherPriority = 0,
			object destinationOcDispatcherParameter = null,
			object sourceOcDispatcherParameter = null)
		{
			_destinationOcDispatcher = destinationOcDispatcher;
			_source = source;
			_sourceOcDispatcher = sourceOcDispatcher;
			_changeValueAction = () =>
			{
				TResult newValue = _source.Value;
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

		private void handleSourceScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
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
			if (_sourceReadAndSubscribed)
			{
				_source.PropertyChanged -= handleSourceScalarPropertyChanged;
				_sourceReadAndSubscribed = false;
			}

			if (_isActive)
			{
				void readAndSubscribe()
				{
					TResult newValue = _source.Value;
					void setNewValue() => setValue(newValue);

					_source.PropertyChanged += handleSourceScalarPropertyChanged;
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

				_sourceReadAndSubscribed = true;
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

		protected override void clearCachedScalarArgumentValues()
		{

		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
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

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
		{
			bool conststent = true;

			_destinationOcDispatcher.Invoke(() =>
			{
				conststent = _value.Equals(_source.Value);
			});

			if (!conststent)
				throw new ObservableComputationsException(this, "Consistency violation: ScalarDispatching.1");
		}
	}
}