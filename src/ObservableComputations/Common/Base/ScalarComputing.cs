using System;
using System.ComponentModel;
using ObservableComputations.Common.Interface;

namespace ObservableComputations.Common
{
	public abstract class ScalarComputing<TValue> : IScalar<TValue>, IReadScalar<TValue>, IWriteScalar<TValue>, IScalarComputing
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}

		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		public string InstantiatingStackTrace { get; }

		protected TValue _value;

		public event EventHandler PostValueChanged;

		public event EventHandler PreValueChanged;

		#region Implementation of IScalar<TSourceItem>
		public TValue Value
		{
			get => _value;
			set => _setValueAction(value);
		}
		#endregion

		private Action<TValue> _setValueAction;

		public Action<TValue> SetValueAction
		{
			get => _setValueAction;
			set
			{
				if (_setValueAction != value)
				{
					_setValueAction = value;
					PropertyChanged?.Invoke(this, Utils.SetValueActionPropertyChangedEventArgs);
				}
			}
		}

		#region Implementation of IScalar
		public object ValueObject
		{
			get => Value;
			set => Value = (TValue)value;
		}

		public Type ValueType => typeof(TValue);

		#endregion

		TValue _newValue;
		public TValue NewValue => _newValue;
		public object NewValueObject => _newValue;


		protected void setValue(TValue value)
		{
			if (!_isConsistent)
				throw new ObservableComputationsException(
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.");
			_isConsistent = false;

			_newValue = value;
			PreValueChanged?.Invoke(this, null);

			_value = value;
			_newValue = default;
			_isConsistent = true;
			PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			PropertyChanged?.Invoke(this, Utils.ValueObjectPropertyChangedEventArgs);
			PostValueChanged?.Invoke(this, null);		
		}

		protected void raisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool _isConsistent = true;
		public bool IsConsistent => _isConsistent;
		public event EventHandler ConsistencyRestored;


		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
