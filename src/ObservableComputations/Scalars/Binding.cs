// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Binding<TValue> : ScalarComputing<TValue>
	{
		readonly Expression<Func<TValue>> _getSourceExpression;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		IReadScalar<TValue> _source;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		PropertyChangedEventHandler _gettingExpressionValueHandlePropertyChanged;
		Action<TValue, Binding<TValue>> _modifyTargetAction;

		// ReSharper disable once ConvertToAutoProperty
		public Action<TValue, Binding<TValue>> ModifyTargetAction => _modifyTargetAction;

		public IReadScalar<TValue> Source => _source;

		private bool _applyOnActivation;
		public bool ApplyOnActivation
		{
			get => _applyOnActivation;
			set
			{
				_applyOnActivation = value;
				raisePropertyChanged(Utils.ApplyOnActivationPropertyChangedEventArgs);
			}
		}

		[ObservableComputationsCall]
		public Binding(IReadScalar<TValue> source, Action<TValue, Binding<TValue>> modifyTargetAction, bool applyOnActivation = true)
		{
			_modifyTargetAction = modifyTargetAction;
			_source = source;
			_applyOnActivation = applyOnActivation;

			_gettingExpressionValueHandlePropertyChanged = (sender, args) =>
			{
				if (args.PropertyName == nameof(Computing<TValue>.Value))
				{
					_handledEventSender = sender;
					_handledEventArgs = args;
					if (_applyOnSourceChanged) Apply();
					setValue(_source.Value);
					_handledEventSender = null;
					_handledEventArgs = null;
				}
			};
		}


		private bool _applyOnSourceChanged = true;
		public bool ApplyOnSourceChanged
		{
			get => _applyOnSourceChanged;
			set
			{
				_applyOnSourceChanged = value;
				raisePropertyChanged(Utils.ApplyOnSourceChangedPropertyChangedEventArgs);
			}
		}

		public void Apply()
		{
			_modifyTargetAction(_source.Value, this);
		}

		#region Overrides of ScalarComputing<TValue>

		protected override void processSource()
		{
			if (_sourceReadAndSubscribed)
			{
				_source.PropertyChanged -= _gettingExpressionValueHandlePropertyChanged;
				_sourceReadAndSubscribed = false;
			}

			if (_isActive)
			{
				_source.PropertyChanged += _gettingExpressionValueHandlePropertyChanged;

				if (_applyOnActivation) Apply();
				_sourceReadAndSubscribed = true;
			}
			else
			{
				setDefaultValue();
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
	}
}
