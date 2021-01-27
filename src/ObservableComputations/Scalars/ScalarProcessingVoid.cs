// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessingVoid<TValue> : ScalarComputing<TValue>
	{
		public IReadScalar<TValue> Scalar => _scalar;
		public Action<TValue, ScalarProcessingVoid<TValue>> NewValueProcessor => _newValueProcessor;
		public Action<TValue, ScalarProcessingVoid<TValue>> OldValueProcessor => _oldValueProcessor;

		private readonly IReadScalar<TValue> _scalar;

		private readonly Action<TValue, ScalarProcessingVoid<TValue>> _newValueProcessor;
		private readonly Action<TValue, ScalarProcessingVoid<TValue>> _oldValueProcessor;

		[ObservableComputationsCall]
		public ScalarProcessingVoid(
			IReadScalar<TValue> scalar,
			Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor = null,
			Action<TValue, ScalarProcessingVoid<TValue>> oldValueProcessor = null) : this(scalar)
		{
			_newValueProcessor = newValueProcessor;
			_oldValueProcessor = oldValueProcessor;
		}

		private ScalarProcessingVoid(
			IReadScalar<TValue> scalar)
		{
			_scalar = scalar;
		}

		private void updateValue()
		{
			processOldValue(_value);
			setNewValue();
		}

		private void setNewValue()
		{
			TValue newValue = _scalar.Value;
			setValue(newValue);
			processNewValue(newValue);
		}


		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
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

		private void processNewValue(TValue newValue)
		{
			if (_newValueProcessor ==  null) return;

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

		private void processOldValue(TValue oldValue)
		{
			if (_oldValueProcessor ==  null) return;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_oldValueProcessor(oldValue, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
			}
			else
			{
				_oldValueProcessor(oldValue, this);
			}
		}

		#region Overrides of ScalarComputing<TResult>

		protected override void processSource()
		{
			if (_sourceEnumerated)
			{
				_scalar.PropertyChanged -= handleScalarPropertyChanged;
				processOldValue(_value);
				_sourceEnumerated = false;
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
				_sourceEnumerated = true;
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
