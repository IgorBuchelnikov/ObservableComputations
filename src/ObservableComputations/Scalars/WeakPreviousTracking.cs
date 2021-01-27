// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class WeakPreviousTracking<TResult> : ScalarComputing<TResult>
		where TResult : class
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public bool IsEverChanged => _isEverChanged;

		public bool TryGetPreviousValue(out TResult result)
		{
			if (_previousValueWeakReference == null)
			{
				result = null;
				return false;
			}

			return _previousValueWeakReference.TryGetTarget(out result);
		}

		private WeakReference<TResult> _previousValueWeakReference;
		private TResult _previousValue;
		private bool _isEverChanged;

		private readonly IReadScalar<TResult> _scalar;
		private readonly Action _changeValueAction;


		[ObservableComputationsCall]
		public WeakPreviousTracking(
			IReadScalar<TResult> scalar)
		{
			_scalar = scalar;
			_changeValueAction = () =>
			{
				TResult newValue = _scalar.Value;
				_previousValue = _value;
				_previousValueWeakReference = new WeakReference<TResult>(_previousValue);

				if (!_isEverChanged)
				{
					_isEverChanged = true;
					raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
				}

				setValue(newValue);
				_previousValue = null;
			};
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

		~WeakPreviousTracking()
		{
			_scalar.PropertyChanged -= handleScalarPropertyChanged;
		}
	}
}