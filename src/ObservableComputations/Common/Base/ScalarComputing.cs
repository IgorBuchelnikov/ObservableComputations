using System;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
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
				if (Configuration.TrackComputingsExecutingUserCode)
				{
					Thread currentThread = Thread.CurrentThread;
					DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
					DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				
					_setValueAction(value);

					if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
					else DebugInfo._computingsExecutingUserCode[currentThread] = computing;

					return;
				}

				_setValueAction(value);
			}
		}

		#endregion

		object _lockModifySetValueActionKey;

		public void LockModifySetValueAction(object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (_lockModifySetValueActionKey == null)
				_lockModifySetValueActionKey = key;
			else
				throw new ObservableComputationsException(this,
					"Modifying of SetValueAction is already locked. Unlock first.");
		}

		public void UnlockModifySetValueAction(object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (_lockModifySetValueActionKey == null)
				throw new ObservableComputationsException(this,
					"Modifying of SetValueAction is not locked. Lock first.");

			if (_lockModifySetValueActionKey == key)
				_lockModifySetValueActionKey = null;
			else
				throw new ObservableComputationsException(this, "Wrong key to unlock modifying of SetValueAction.");
		}

		public bool IsModifySetValueActionLocked()
		{
			return _lockModifySetValueActionKey != null;
		}

		protected Action<TValue> _setValueAction;

		public Action<TValue> SetValueAction
		{
			get => _setValueAction;
			set
			{
				if (_setValueAction != value)
				{
					if (_lockModifySetValueActionKey != null)
						throw new ObservableComputationsException(this, "Modifying of SetValueAction is locked. Unlock first.");

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

		protected void raisePropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
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
