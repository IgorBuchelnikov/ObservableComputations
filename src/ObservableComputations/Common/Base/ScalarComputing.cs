using System;
using System.ComponentModel;
using System.Threading;
using ObservableComputations.Common;
using ObservableComputations.Interface;

namespace ObservableComputations.Base
{
	public abstract class ScalarComputing<TValue> : IScalar<TValue>, IReadScalar<TValue>, IWriteScalar<TValue>, IScalarComputing
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}

		public ScalarComputing()
		{
			if (Configuration.SaveInstantiatingStackTrace)
			{
				_instantiatingStackTrace = Environment.StackTrace;
			}
		}

		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		public string InstantiatingStackTrace => _instantiatingStackTrace;

		protected TValue _value;

		public event EventHandler PostValueChanged;

		public event EventHandler PreValueChanged;

		#region Implementation of IScalar<TSourceItem>
		public TValue Value
		{
			get => _value;
			set
			{
				bool trackComputingsExecutingUserCode = Configuration.TrackComputingsExecutingUserCode;
				if (trackComputingsExecutingUserCode)
				{
					DebugInfo._computingsExecutingUserCode[Thread.CurrentThread] = this;
				}

				_setValueAction(value);

				if (trackComputingsExecutingUserCode)
				{
					DebugInfo._computingsExecutingUserCode.Remove(Thread.CurrentThread);
				}
			}
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

			PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			PropertyChanged?.Invoke(this, Utils.ValueObjectPropertyChangedEventArgs);
			PostValueChanged?.Invoke(this, null);		
		}

		protected void checkConsistency()
		{
			if (!_isConsistent)
				throw new ObservableComputationsException(this,
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.");
		}

		protected void raiseConsistencyRestored()
		{
			ConsistencyRestored?.Invoke(this, null);
		}

		protected void raisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool _isConsistent = true;
		private readonly string _instantiatingStackTrace;
		public bool IsConsistent => _isConsistent;
		public event EventHandler ConsistencyRestored;


		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
