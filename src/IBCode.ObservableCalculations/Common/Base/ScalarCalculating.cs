using System;
using System.ComponentModel;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations.Common
{
	public abstract class ScalarCalculating<TValue> : IScalar<TValue>, IReadScalar<TValue>, IWriteScalar<TValue>
	{
		public string DebugTag;

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
		public bool RaisePostValueChanged;
		public event EventHandler PostValueChanged;

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

		public Type Type => typeof(TValue);

		#endregion


		protected void setValue(TValue value)
		{
			_value = value;
			raiseValueChanged();			
		}

		protected void raiseValueChanged()
		{
			PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			PropertyChanged?.Invoke(this, Utils.ValueObjectPropertyChangedEventArgs);
			if (RaisePostValueChanged) PostValueChanged?.Invoke(this, new EventArgs());
		}

		protected void raisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
