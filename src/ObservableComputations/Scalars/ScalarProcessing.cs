// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessing<TValue, TReturnValue> : ScalarComputing<TReturnValue>
	{
		public IReadScalar<TValue> Source => _source;
		public Func<TValue, IScalarComputing, TReturnValue> NewValueProcessor => _newValueProcessor;

		private readonly IReadScalar<TValue> _source;

		private readonly Func<TValue, IScalarComputing, TReturnValue> _newValueProcessor;
		private readonly Action<TValue, IScalarComputing, TReturnValue> _oldValueProcessor;

		private TValue _oldValue;

		[ObservableComputationsCall]
		public ScalarProcessing(
			IReadScalar<TValue> source,
			Func<TValue, IScalarComputing, TReturnValue> newValueProcessor,
			Action<TValue, IScalarComputing, TReturnValue> oldValueProcessor = null) : this(source)
		{
			_newValueProcessor = newValueProcessor;
			_oldValueProcessor = oldValueProcessor;
		}

		private ScalarProcessing(
			IReadScalar<TValue> source)
		{
			_source = source;
		}

		private void updateValue()
		{
			processOldValue(_oldValue);
			setNewValue();
		}

		private void setNewValue()
		{
			TValue newValue = _source.Value;
			_oldValue = newValue;
			setValue(processNewValue(newValue));
		}

		private void handleSourceScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
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

		#region Overrides of ScalarComputing<TResult>

		protected override void processSource()
		{
			if (_sourceReadAndSubscribed)
			{
				_source.PropertyChanged -= handleSourceScalarPropertyChanged;
				processOldValue(_oldValue);
				_sourceReadAndSubscribed = false;
			}

			if (_isActive)
			{
				if (_source is IComputing scalarComputing)
				{
					if (scalarComputing.IsActive) setNewValue();
				}
				else
					setNewValue();

				_source.PropertyChanged += handleSourceScalarPropertyChanged;
				_sourceReadAndSubscribed = true;
			}
			else
				setDefaultValue();
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
	}
}
