// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ObservableComputations
{
	public class PreviousTracking<TResult> : ScalarComputing<TResult>, IHasSources
	{
		public IReadScalar<TResult> Source => _source;
		public TResult PreviousValue => _previousValue;
		public bool IsEverChanged => _isEverChanged;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source});

		private readonly IReadScalar<TResult> _source;
		private TResult _previousValue;
		private bool _isEverChanged;
		private readonly Action _changeValueAction;


		[ObservableComputationsCall]
		public PreviousTracking(
			IReadScalar<TResult> source)
		{
			_changeValueAction = () =>
			{
				TResult newValue = _source.Value;
				_previousValue = _value;

				if (!_isEverChanged)
				{
					_isEverChanged = true;
					raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
				}

				raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
				setValue(newValue);
			};

			_source = source;
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
				_source.PropertyChanged += handleSourceScalarPropertyChanged;
				setValue(_source.Value);
				_sourceReadAndSubscribed = true;
			}
			else
			{
				if (_isEverChanged)
				{
					_isEverChanged = false;
					raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
				}

				setDefaultValue();

				_previousValue = default;
				raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
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

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			if (!_value.Equals(_source.Value))
				throw new ValidateInternalConsistencyException("Consistency violation: PreviousTracking.1");
		}
	}
}