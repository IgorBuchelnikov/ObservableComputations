using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public abstract class ScalarComputing<TValue> : IScalar<TValue>, IReadScalar<TValue>, IWriteScalar<TValue>, IScalarComputing,  IComputingInternal
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}

        // ReSharper disable once MemberCanBePrivate.Global
        public bool IsDefaulted => _isDefaulted;

        protected bool _isDefaulted = true;


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

		internal IComputing _userCodeIsCalledFrom;
		public IComputing UserCodeIsCalledFrom => _userCodeIsCalledFrom;

		protected TValue _value;

		public event EventHandler PostValueChanged;

		public event EventHandler PreValueChanged;

		internal object _handledEventSender;
		internal EventArgs _handledEventArgs;
		public object HandledEventSender => _handledEventSender;
		public EventArgs HandledEventArgs => _handledEventArgs;

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
					_userCodeIsCalledFrom = computing;

					_setValueAction(value);

					if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
					else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
					_userCodeIsCalledFrom = null;

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

		protected void checkConsistent(object sender, EventArgs eventArgs)
		{
			if (!_isConsistent)
				throw new ObservableComputationsInconsistencyException(this,
					$"The source collection has been changed. It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
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

        private Action _scalarValueChangedHandlerAction;

        protected PropertyChangedEventHandler getScalarValueChangedHandler(Action action = null)
        {
            return (sender, args) =>
            {
                _scalarValueChangedHandlerAction = action;
                scalarValueChangedHandler(sender, args);
                _scalarValueChangedHandlerAction = null;
            };
        }

        protected void scalarValueChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(IReadScalar<object>.Value)) return;
            checkConsistent(sender, args);

            _handledEventSender = sender;
            _handledEventArgs = args;

            _isConsistent = false;

            _scalarValueChangedHandlerAction?.Invoke();
            initializeFromSource();

            _isConsistent = true;
            raiseConsistencyRestored();

            _handledEventSender = null;
            _handledEventArgs = null;
        }

		protected bool _isConsistent = true;
		private readonly string _instantiatingStackTrace;
		public bool IsConsistent => _isConsistent;
		public event EventHandler ConsistencyRestored;

        protected bool _isActive;
        public bool IsActive => _isActive;

        protected PropertyChangedEventHandler getScalarValueChangedHandler(Action action = null)
        {
            return (sender, args) =>
            {
                if (args.PropertyName != nameof(IReadScalar<object>.Value)) return;
                checkConsistent(sender, args);

                _handledEventSender = sender;
                _handledEventArgs = args;

                _isConsistent = false;

                action?.Invoke();
                initializeFromSource();

                _isConsistent = true;
                raiseConsistencyRestored();

                _handledEventSender = null;
                _handledEventArgs = null;
            };
        }

        protected abstract void initializeFromSource();
        protected abstract void initialize();
        protected abstract void uninitialize();
        internal abstract void addToUpstreamComputings(IComputingInternal computing);
        internal abstract void removeFromUpstreamComputings(IComputingInternal computing);


        protected List<Consumer> _consumers = new List<Consumer>();
        internal  List<IComputingInternal> _downstreamConsumedComputings = new List<IComputingInternal>();

        #region Implementation of IComputingInternal
        IEnumerable<Consumer> IComputingInternal.Consumers => _consumers;

        void IComputingInternal.AddToUpstreamComputings(IComputingInternal computing)
        {
            addToUpstreamComputings(computing);
        }

        void IComputingInternal.RemoveFromUpstreamComputings(IComputingInternal computing)
        {
            removeFromUpstreamComputings(computing);
        }

        void IComputingInternal.Initialize()
        {
            initialize();
        }

        void IComputingInternal.Uninitialize()
        {
            uninitialize();
        }

        void IComputingInternal.InitializeFromSource()
        {
            initializeFromSource();
        }

        void IComputingInternal.AddConsumer(Consumer addingConsumer)
        {
            Utils.AddComsumer(addingConsumer, _consumers, _downstreamConsumedComputings, this, ref _isActive);
        }

        void IComputingInternal.OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            raisePropertyChanged(propertyChangedEventArgs);
        }

        void IComputingInternal.RemoveConsumer(Consumer removingConsumer)
        {
            Utils.RemoveConsumer(removingConsumer, _consumers, _downstreamConsumedComputings, ref _isActive, this);
        }

        void IComputingInternal.AddDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.AddDownstreamConsumedComputing(computing, _downstreamConsumedComputings, _consumers, ref _isActive, this);
        }

        void IComputingInternal.RemoveDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.RemoveDownstreamConsumedComputing(computing, _downstreamConsumedComputings, ref _isActive, this, _consumers);
        }

        void IComputingInternal.RaiseConsistencyRestored()
        {
            raiseConsistencyRestored();
        }

        #endregion

		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
