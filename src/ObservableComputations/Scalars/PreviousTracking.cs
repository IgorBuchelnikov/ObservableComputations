// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class PreviousTracking<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public TResult PreviousValue => _previousValue;
		public bool IsEverChanged => _isEverChanged;

		private readonly IReadScalar<TResult> _scalar;
		private TResult _previousValue;
		private bool _isEverChanged;
		private readonly Action _changeValueAction;


		[ObservableComputationsCall]
		public PreviousTracking(
			IReadScalar<TResult> scalar)
		{
			_changeValueAction = () =>
			{
				TResult newValue = _scalar.Value;
				_previousValue = _value;

				if (!_isEverChanged)
				{
					_isEverChanged = true;
					raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
				}

				raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
				setValue(newValue);
			};

			_scalar = scalar;
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
				_scalar.PropertyChanged += handleScalarPropertyChanged;
				setValue(_scalar.Value);
				_sourceEnumerated = true;
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