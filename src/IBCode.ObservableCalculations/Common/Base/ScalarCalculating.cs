using System;
using System.ComponentModel;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations.Common
{
	public abstract class ScalarCalculating<TValue> : IScalar<TValue>, IReadScalar<TValue>, IWriteScalar<TValue>, IScalarCalculating
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}

		public ScalarCalculating()
		{

			if (Configuration.SaveInstantiatingStackTrace)
			{
				InstantiatingStackTrace = Environment.StackTrace;
			}
		}

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
			_newValue = value;
			PreValueChanged?.Invoke(this, null);

			_value = value;
			_newValue = default;

			PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			PropertyChanged?.Invoke(this, Utils.ValueObjectPropertyChangedEventArgs);
			PostValueChanged?.Invoke(this, null);		
		}

		protected void raisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool _consistent = true;
		public bool Consistent => _consistent;
		public event EventHandler ConsistencyRestored;

		protected void checkConsistent()
		{
			if (!_consistent)
				throw new ObservableCalculationsException(
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change after Consistent property becomes true");
		}

		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
