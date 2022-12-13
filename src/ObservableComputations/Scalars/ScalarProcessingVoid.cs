// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ObservableComputations
{
	public class ScalarProcessingVoid<TValue> : ScalarComputing<TValue>, IHasSources
	{
		public IReadScalar<TValue> Source => _source;
		public Action<TValue, ScalarProcessingVoid<TValue>> NewValueProcessor => _newValueProcessor;
		public Action<TValue, ScalarProcessingVoid<TValue>> OldValueProcessor => _oldValueProcessor;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source});

		private readonly IReadScalar<TValue> _source;

		private readonly Action<TValue, ScalarProcessingVoid<TValue>> _newValueProcessor;
		private readonly Action<TValue, ScalarProcessingVoid<TValue>> _oldValueProcessor;

		[ObservableComputationsCall]
		public ScalarProcessingVoid(
			IReadScalar<TValue> source,
			Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor = null,
			Action<TValue, ScalarProcessingVoid<TValue>> oldValueProcessor = null) : this(source)
		{
			_newValueProcessor = newValueProcessor;
			_oldValueProcessor = oldValueProcessor;
		}

		private ScalarProcessingVoid(
			IReadScalar<TValue> source)
		{
			_source = source;
		}

		private void updateValue()
		{
			processOldValue(_value);
			setNewValue();
		}

		private void setNewValue()
		{
			TValue newValue = _source.Value;
			setValue(newValue);
			processNewValue(newValue);
		}


		private void handleSourceScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
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

			if (OcConfiguration.TrackComputingsExecutingUserCode)
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

			if (OcConfiguration.TrackComputingsExecutingUserCode)
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
			if (_sourceReadAndSubscribed)
			{
				_source.PropertyChanged -= handleSourceScalarPropertyChanged;
				processOldValue(_value);
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

		public override IEnumerable<IComputing> UpstreamComputingsDirect
		{
			get
			{
				List<IComputing> computings = new List<IComputing>();
				Utils.FillUpstreamComputingsDirect(computings, _source);
				return computings;
			}
		}
	}
}
