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

		private readonly PropertyChangedEventHandler _propertyHolderScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _propertyHolderScalarWeakPropertyChangedEventHandler;

		private PropertyChangedEventHandler _propertyHolderPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _propertyHolderWeakPropertyChangedEventHandler;

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyName;
			_propertyName = propertyName;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			BindingFlags bindingAttr) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.BindingAttr;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			Type returnType) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.ReturnType;
			_propertyName = propertyName;
			_returnType = returnType;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			Type returnType,
			Type[] types) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Types;
			_propertyName = propertyName;
			_returnType = returnType;
			_types = types;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Modifiers;
			_propertyName = propertyName;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			string propertyName,
			BindingFlags bindingAttr,
			Binder binder,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Binder;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_binder = binder;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			Func<PropertyInfo, bool> propertyInfoPredicate) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicate;
			_propertyInfoPredicate = propertyInfoPredicate;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar, 
			Func<PropertyInfo, bool> propertyInfoPredicate,
			BindingFlags bindingAttr) : this(propertyHolderScalar)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicateBindingAttr;
			_propertyInfoPredicate = propertyInfoPredicate;
			_bindingAttr = bindingAttr;
			registerPropertyHolder();
		}

		private PropertyAccessing(
			IReadScalar<INotifyPropertyChanged> propertyHolderScalar)
		{
			_propertyHolderScalar = propertyHolderScalar;
			_propertyHolder = _propertyHolderScalar.Value;
			_propertyHolderScalarPropertyChangedEventHandler = handlePropertyHolderScalarPropertyChanged;
			_propertyHolderScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_propertyHolderScalarPropertyChangedEventHandler);
			_propertyHolderScalar.PropertyChanged += _propertyHolderScalarWeakPropertyChangedEventHandler.Handle;
			_setValueAction = result => _propertyInfo.SetValue(_propertyHolder, result);
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder, 
			string propertyName) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyName;
			_propertyName = propertyName;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder, 
			string propertyName,
			BindingFlags bindingAttr) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.BindingAttr;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			Type returnType) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.ReturnType;
			_propertyName = propertyName;
			_returnType = returnType;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			Type returnType,
			Type[] types) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Types;
			_propertyName = propertyName;
			_returnType = returnType;
			_types = types;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Modifiers;
			_propertyName = propertyName;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			string propertyName,
			BindingFlags bindingAttr,
			Binder binder,
			Type returnType,
			Type[] types,
			ParameterModifier[] modifiers) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.Binder;
			_propertyName = propertyName;
			_bindingAttr = bindingAttr;
			_binder = binder;
			_returnType = returnType;
			_modifiers = modifiers;
			_types = types;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			Func<PropertyInfo, bool> propertyInfoPredicate) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicate;
			_propertyInfoPredicate = propertyInfoPredicate;
			registerPropertyHolder();
		}

		[ObservableComputationsCall]
		public PropertyAccessing(
			INotifyPropertyChanged propertyHolder,  
			Func<PropertyInfo, bool> propertyInfoPredicate,
			BindingFlags bindingAttr) : this(propertyHolder)
		{
			_propertyInfoGettingType = PropertyInfoGettingType.PropertyInfoPredicateBindingAttr;
			_propertyInfoPredicate = propertyInfoPredicate;
			_bindingAttr = bindingAttr;
			registerPropertyHolder();
		}

		private PropertyAccessing(
			INotifyPropertyChanged propertyHolder)
		{
			_propertyHolder = propertyHolder;
			_setValueAction = result => _propertyInfo.SetValue(_propertyHolder, result);
		}


		private void registerPropertyHolder()
		{
			if (_propertyHolderWeakPropertyChangedEventHandler != null)
			{
				_propertyHolder.PropertyChanged -= _propertyHolderWeakPropertyChangedEventHandler.Handle;
			}

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

			_propertyHolderType = _propertyHolder.GetType();
			_propertyInfo = null;
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

			_propertyHolderPropertyChangedEventHandler = handlePropertyHolderPropertyChanged;
			_propertyHolderWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_propertyHolderPropertyChangedEventHandler);
			_propertyHolder.PropertyChanged += _propertyHolderWeakPropertyChangedEventHandler.Handle;

			setValue((TResult) _propertyInfo.GetValue(_propertyHolder));
		}

		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			setValue((TResult) _propertyInfo.GetValue(_propertyHolder));
		}

		private void handlePropertyHolderScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;
			_propertyHolder = _propertyHolderScalar.Value;
			registerPropertyHolder();
		}

		~PropertyAccessing()
		{
			if (_propertyHolderScalarWeakPropertyChangedEventHandler != null)
				_propertyHolderScalar.PropertyChanged -= _propertyHolderScalarWeakPropertyChangedEventHandler.Handle;
		}
	}
}
