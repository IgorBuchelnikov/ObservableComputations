using System;
using System.ComponentModel;
using System.Reflection;

namespace ObservableComputations
{
	public class PropertyAccessing<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<object> PropertyHolderScalar => _propertyHolderScalar;
		public object PropertyHolder => _propertyHolder;
		public string  PropertyName => _propertyName;
		public BindingFlags  BindingAttr => _bindingAttr;
		public Binder Binder => _binder;
		public Type  ReturnType => _returnType;
		public Type[]  Types => _types;
		public ParameterModifier[]  Modifiers => _modifiers;
		public Func<PropertyInfo, bool>  PropertyInfoPredicate => _propertyInfoPredicate;
		public PropertyInfo  PropertyInfo => _propertyInfo;
		// ReSharper disable once MemberCanBeProtected.Global
		public TResult DefaultValue => _defaultValue;

		private IReadScalar<INotifyPropertyChanged> _propertyHolderScalar;
		private INotifyPropertyChanged _propertyHolder;
		private Type _propertyHolderType;

		private PropertyInfoGettingType _propertyInfoGettingType;

		private string _propertyName;
		private BindingFlags _bindingAttr;
		private Binder _binder;
		private Type _returnType;
		private Type[] _types;
		private ParameterModifier[] _modifiers;
		private Func<PropertyInfo, bool> _propertyInfoPredicate;
		private PropertyInfo _propertyInfo;

		internal TResult _defaultValue;

        private Action _changeValueAction;
        private Action _changeHolderAction;

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
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyName;
			_propertyName = propertyName;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			BindingFlags bindingAttr,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.BindingAttr;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			Type returnType,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.ReturnType;
			_propertyName = propertyName;
			_returnType = returnType;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			Type returnType,
			Type[] types,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Types;
			_propertyName = propertyName;
			_returnType = returnType;
			_types = types;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Modifiers;
			_propertyName = propertyName;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			BindingFlags bindingAttr,
			Binder binder,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Binder;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_binder = binder;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			Func<PropertyInfo, bool> propertyInfoPredicate,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicate;
			_propertyInfoPredicate = propertyInfoPredicate;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			Func<PropertyInfo, bool> propertyInfoPredicate,
			BindingFlags bindingAttr,
			TResult defaultValue = default(TResult)) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicateBindingAttr;
			_propertyInfoPredicate = propertyInfoPredicate;
			_bindingAttr = bindingAttr;
			_defaultValue = defaultValue;
            _propertyHolder = _propertyHolderScalar.Value;
		}

		private PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar) : this()
		{
			_propertyHolderScalar = propertyHolderScalar;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder, 
			string propertyName,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyName;
			_propertyName = propertyName;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder, 
			string propertyName,
			BindingFlags bindingAttr,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.BindingAttr;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			Type returnType,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.ReturnType;
			_propertyName = propertyName;
			_returnType = returnType;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			Type returnType,
			Type[] types,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Types;
			_propertyName = propertyName;
			_returnType = returnType;
			_types = types;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Modifiers;
			_propertyName = propertyName;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			BindingFlags bindingAttr,
			Binder binder,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Binder;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_binder = binder;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			Func<PropertyInfo, bool> propertyInfoPredicate,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicate;
			_propertyInfoPredicate = propertyInfoPredicate;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			Func<PropertyInfo, bool> propertyInfoPredicate,
			BindingFlags bindingAttr,
			TResult defaultValue = default(TResult)) : this()
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicateBindingAttr;
			_propertyInfoPredicate = propertyInfoPredicate;
			_bindingAttr = bindingAttr;
			_defaultValue = defaultValue;
            _propertyHolder = propertyHolder;
		}

		private PropertyAccessing()
		{
			_setValueAction = result => _propertyInfo.SetValue(_propertyHolder, result);
            _changeValueAction = () => 	setValue((TResult) _propertyInfo.GetValue(_propertyHolder));
            _changeHolderAction = () => {            
                if (_propertyHolder != null)
                    _propertyHolder.PropertyChanged -= handlePropertyHolderPropertyChanged;
                _propertyHolder = _propertyHolderScalar.Value;
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

			if (_propertyHolder == null)
			{
				setValue(_defaultValue);
				return;
			}

			_propertyHolderType = _propertyHolder.GetType();

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

			_propertyHolder.PropertyChanged += handlePropertyHolderPropertyChanged;

			setValue((TResult) _propertyInfo.GetValue(_propertyHolder));
		}

		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
            Utils.processChange(
                sender, 
                e, 
                _changeValueAction,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, 
                this,
                false);
		}

		private void handlePropertyHolderScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
            Utils.processChange(
                sender, 
                e, 
                _changeHolderAction,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, this);
		}

        private bool _initializedFromSource;
        #region Overrides of ScalarComputing<TResult>

        protected override void initializeFromSource()
        {
            if (_initializedFromSource)
            {
                if (_propertyHolderScalar != null)
                {
                    _propertyHolderScalar.PropertyChanged -= handlePropertyHolderScalarPropertyChanged;
                    _propertyHolder = null;
                }

                if (_propertyHolder != null)
                    _propertyHolder.PropertyChanged -= handlePropertyHolderPropertyChanged;

                _initializedFromSource = false;
            }

            if (_isActive)
            {
                if (_propertyHolderScalar != null)
                {
                    _propertyHolderScalar.PropertyChanged += handlePropertyHolderScalarPropertyChanged;
                    _propertyHolder = _propertyHolderScalar.Value;
                }

                registerPropertyHolder();
                _initializedFromSource = true;
            }
            else
            {
                setValue(default);
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
            (_propertyHolderScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_propertyHolderScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        #endregion
    }
}
