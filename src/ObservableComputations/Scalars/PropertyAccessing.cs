// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ObservableComputations
{
	public class PropertyAccessing<TResult> : ScalarComputing<TResult>, IHasSources
	{
		public IReadScalar<object> SourceScalar => _sourceScalar;
		public object Source => _source;
		public string  PropertyName => _propertyName;
		public BindingFlags  BindingAttr => _bindingAttr;
		public Binder Binder => _binder;
		public Type  ReturnType => _returnType;
		public Type[]  Types => _types;
		public ParameterModifier[]  Modifiers => _modifiers;
		public Func<PropertyInfo, bool>  PropertyInfoPredicate => _propertyInfoPredicate;
		public PropertyInfo  PropertyInfo => _propertyInfo;
		// ReSharper disable once MemberCanBeProtected.Global

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		private readonly IReadScalar<INotifyPropertyChanged> _sourceScalar;
		private INotifyPropertyChanged _source;
		private Type _propertyHolderType;

		private readonly PropertyInfoGettingType _propertyInfoGettingType;

		private readonly string _propertyName;
		private readonly BindingFlags _bindingAttr;
		private readonly Binder _binder;
		private readonly Type _returnType;
		private readonly Type[] _types;
		private readonly ParameterModifier[] _modifiers;
		private readonly Func<PropertyInfo, bool> _propertyInfoPredicate;
		private PropertyInfo _propertyInfo;

		private readonly Action _changeValueAction;
		private readonly Action _changeHolderAction;

		private enum PropertyInfoGettingType
		{
			PropertyName,
			BindingAttr,
			Binder,
			ReturnType,
			Types,
			Modifiers,
			PropertyInfoPredicate,
			PropertyInfoPredicateBindingAttr
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			string propertyName) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyName;
			_propertyName = propertyName;
			_source = _sourceScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			string propertyName,
			BindingFlags bindingAttr) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.BindingAttr;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_source = _sourceScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			string propertyName,
			Type returnType) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.ReturnType;
			_propertyName = propertyName;
			_returnType = returnType;
			_source = _sourceScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			string propertyName,
			Type returnType,
			Type[] types) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Types;
			_propertyName = propertyName;
			_returnType = returnType;
			_types = types;
			_source = _sourceScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			string propertyName,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Modifiers;
			_propertyName = propertyName;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_source = _sourceScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			string propertyName,
			BindingFlags bindingAttr,
			Binder binder,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Binder;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_binder = binder;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_source = _sourceScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			Func<PropertyInfo, bool> propertyInfoPredicate) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicate;
			_propertyInfoPredicate = propertyInfoPredicate;
			_source = _sourceScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar, 
			Func<PropertyInfo, bool> propertyInfoPredicate,
			BindingFlags bindingAttr) : this(sourceScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicateBindingAttr;
			_propertyInfoPredicate = propertyInfoPredicate;
			_bindingAttr = bindingAttr;
			_source = _sourceScalar.Value;
		}

		private PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> sourceScalar) : this()
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source, 
			string propertyName) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyName;
			_propertyName = propertyName;
			_source = source;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source, 
			string propertyName,
			BindingFlags bindingAttr) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.BindingAttr;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_source = source;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source,  
			string propertyName,
			Type returnType) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.ReturnType;
			_propertyName = propertyName;
			_returnType = returnType;
			_source = source;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source,  
			string propertyName,
			Type returnType,
			Type[] types) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Types;
			_propertyName = propertyName;
			_returnType = returnType;
			_types = types;
			_source = source;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source,  
			string propertyName,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Modifiers;
			_propertyName = propertyName;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_source = source;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source,  
			string propertyName,
			BindingFlags bindingAttr,
			Binder binder,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Binder;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_binder = binder;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_source = source;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source,  
			Func<PropertyInfo, bool> propertyInfoPredicate) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicate;
			_propertyInfoPredicate = propertyInfoPredicate;
			_source = source;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged source,  
			Func<PropertyInfo, bool> propertyInfoPredicate,
			BindingFlags bindingAttr) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicateBindingAttr;
			_propertyInfoPredicate = propertyInfoPredicate;
			_bindingAttr = bindingAttr;
			_source = source;
		}

		private PropertyAccessing()
		{
			_setValueRequestHandler = result => _propertyInfo.SetValue(_source, result);
			_changeValueAction = () => 	setValue((TResult) _propertyInfo.GetValue(_source));
			_changeHolderAction = () => {			
				if (_source != null)
					_source.PropertyChanged -= handleSourcePropertyChanged;
				_source = _sourceScalar.Value;
				registerPropertyHolder();
			};
		}

		private void registerPropertyHolder()
		{
			void getPropertyInfo(PropertyInfo[] propertyInfos)
			{
				for (int i = 0; i < propertyInfos.Length; i++)
				{
					PropertyInfo propertyInfo = propertyInfos[i];
					if (_propertyInfoPredicate(propertyInfo))
					{
						_propertyInfo = propertyInfo;
						break;
					}
				}
			}

			_propertyInfo = null;

			if (_source == null)
			{
				setDefaultValue();
				return;
			}

			_propertyHolderType = _source.GetType();

			switch (_propertyInfoGettingType)
			{
				case PropertyInfoGettingType.PropertyName:
					_propertyInfo = _propertyHolderType.GetProperty(_propertyName);
					break;
				case PropertyInfoGettingType.BindingAttr:
					_propertyInfo = _propertyHolderType.GetProperty(_propertyName, _bindingAttr);
					break;
				case PropertyInfoGettingType.Binder:
					_propertyInfo = _propertyHolderType.GetProperty(_propertyName, _bindingAttr, _binder, _returnType, _types, _modifiers);
					break;
				case PropertyInfoGettingType.ReturnType:
					_propertyInfo = _propertyHolderType.GetProperty(_propertyName, _returnType);
					break;
				case PropertyInfoGettingType.Types:
					_propertyInfo =  _propertyHolderType.GetProperty(_propertyName, _returnType, _types);
					break;
				case PropertyInfoGettingType.Modifiers:
					_propertyInfo = _propertyHolderType.GetProperty(_propertyName, _returnType, _types, _modifiers);
					break;
				case PropertyInfoGettingType.PropertyInfoPredicate:
					getPropertyInfo(_propertyHolderType.GetProperties());
					break;
				case PropertyInfoGettingType.PropertyInfoPredicateBindingAttr:
					getPropertyInfo(_propertyHolderType.GetProperties(_bindingAttr));
					break;

			}
			
			if (_propertyInfo == null)
				throw new ObservableComputationsException(this, "Cannot obtain propertyInfo");

			_source.PropertyChanged += handleSourcePropertyChanged;

			setValue((TResult) _propertyInfo.GetValue(_source));
		}

		private void handleSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Utils.processChange(
				sender, 
				e, 
				_changeValueAction,
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs, 
				0, _deferredQueuesCount,
				ref _deferredProcessings, 
				this,
				false);
		}

		private void handleSourceScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Utils.processChange(
				sender, 
				e, 
				_changeHolderAction,
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
				if (_sourceScalar != null)
				{
					_sourceScalar.PropertyChanged -= handleSourceScalarPropertyChanged;
					_source = null;
				}

				if (_source != null)
					_source.PropertyChanged -= handleSourcePropertyChanged;

				_sourceReadAndSubscribed = false;
			}

			if (_isActive)
			{
				if (_sourceScalar != null)
				{
					_sourceScalar.PropertyChanged += handleSourceScalarPropertyChanged;
					_source = _sourceScalar.Value;
				}

				registerPropertyHolder();
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
			(_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			(_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		#endregion

		public override IEnumerable<IComputing> UpstreamComputingsDirect
		{
			get
			{
				List<IComputing> computings = new List<IComputing>();
				Utils.FillUpstreamComputingsDirect(computings, _sourceScalar, _source);
				return computings;
			}
		}

		internal override void RegisterInvolvedMembersAccumulatorImpl(InvolvedMembersAccumulator involvedMembersAccumulator) => 
			involvedMembersAccumulator.RegisterInvolvedMember(new InvolvedMember(_source, _propertyName));

		internal override void UnregisterInvolvedMembersAccumulatorImpl(InvolvedMembersAccumulator involvedMembersAccumulator) => 
			involvedMembersAccumulator.UnregisterInvolvedMember(new InvolvedMember(_source, _propertyName));

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			object source = _sourceScalar != null ? _sourceScalar.Value : _source;

			if (_source != null)
			{
				if (!_source.GetType().GetProperty("Num").GetValue(_source).Equals(_value))
					throw new ValidateInternalConsistencyException("Consistency violation: PropertyAccessing.1");
			}
		}
	}
}
