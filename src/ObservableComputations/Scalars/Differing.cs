// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ObservableComputations
{
	public class Differing<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Source => _source;
		public IEqualityComparer<TResult> EqualityComparer =>_equalityComparer;
		public IReadScalar<IEqualityComparer<TResult>> EqualityComparerScalar =>_equalityComparerScalar;

		private readonly IReadScalar<TResult> _source;
		private IEqualityComparer<TResult> _equalityComparer;


		private readonly IReadScalar<IEqualityComparer<TResult>> _equalityComparerScalar;
		private readonly Action _changeValueAction;
		private readonly Action _setEqualityComparerAction;

		[ObservableComputationsCall]
		public Differing(
			IReadScalar<TResult> source,
			IEqualityComparer<TResult> equalityComparer = null) : this(source)
		{
			_equalityComparer = equalityComparer ?? EqualityComparer<TResult>.Default;
		}

		[ObservableComputationsCall]
		public Differing(
			IReadScalar<TResult> source,
			IReadScalar<IEqualityComparer<TResult>> equalityComparerScalar) : this(source)
		{
			_equalityComparerScalar = equalityComparerScalar;
		}

		private Differing(
			IReadScalar<TResult> source)
		{
			_source = source;

			_changeValueAction = () =>
			{
				TResult newValue = _source.Value;
				if (!_equalityComparer.Equals(newValue, _value))
				{
					setValue(newValue);
				}
			};

			_setEqualityComparerAction = () =>
			{
				_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;
			};
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

		private void handleEqualityComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			Utils.processChange(
				sender, 
				e, 
				_setEqualityComparerAction,
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
				if (_equalityComparerScalar != null)
				{
					_equalityComparerScalar.PropertyChanged -= handleEqualityComparerScalarValueChanged;
					_equalityComparer = null;	  
				}

				_source.PropertyChanged -= handleSourceScalarPropertyChanged;
				_sourceReadAndSubscribed = false;
			}

			if (_isActive)
			{
				if (_equalityComparerScalar != null)
				{
					_equalityComparerScalar.PropertyChanged += handleEqualityComparerScalarValueChanged;
					_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;	  
				}

				_source.PropertyChanged += handleSourceScalarPropertyChanged;
				setValue(_source.Value);
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
			(_equalityComparerScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_equalityComparerScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		#endregion

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			if (!_value.Equals(_source.Value))
				throw new ValidateInternalConsistencyException("Consistency violation: Differing.1");
		}
	}
}
