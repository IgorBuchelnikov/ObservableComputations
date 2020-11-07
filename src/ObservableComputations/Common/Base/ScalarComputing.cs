using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ObservableComputations
{
	public abstract class ScalarComputing<TValue> : IScalar<TValue>, IReadScalar<TValue>, IWriteScalar<TValue>, IScalarComputing,  IComputingInternal
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}
        internal Queue<IProcessable>[] _deferredProcessings;
        protected int _deferredQueuesCount = 1;

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
                    var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
					_setValueAction(value);
                    Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
					return;
				}

				_setValueAction(value);
			}
		}

		#endregion

		protected Action<TValue> _setValueAction;

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
            void perform()
            {
                _newValue = value;
                PreValueChanged?.Invoke(this, null);

                _value = value;

                PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
                PropertyChanged?.Invoke(this, Utils.ValueObjectPropertyChangedEventArgs);
                PostValueChanged?.Invoke(this, null);
            }

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
                perform();
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
            }
            else
            {
                perform();
            }
        }

		protected void checkConsistent(object sender, EventArgs eventArgs)
		{
			if (!_isConsistent)
				throw new ObservableComputationsInconsistencyException(this,
					$"It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
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
            Utils.processResetChange(
                sender, 
                args, 
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                _scalarValueChangedHandlerAction, 
                _deferredQueuesCount,
                ref _deferredProcessings, this);
        }

		protected bool _isConsistent = true;
		private readonly string _instantiatingStackTrace;
		public bool IsConsistent => _isConsistent;
		public event EventHandler ConsistencyRestored;

        protected bool _isActive;
        public bool IsActive => _isActive;

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

        void ICanInitializeFromSource.InitializeFromSource()
        {
            initializeFromSource();
        }

        void IComputingInternal.OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            raisePropertyChanged(propertyChangedEventArgs);
        }

        public void SetIsActive(bool value)
        {
            _isActive = value;
        }

        void IComputingInternal.AddConsumer(Consumer addingConsumer)
        {
            Utils.addComsumer(
                addingConsumer, 
                _consumers,
                _downstreamConsumedComputings, 
                this, 
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                ref _deferredProcessings,
                _deferredQueuesCount);
        }

        void IComputingInternal.RemoveConsumer(Consumer removingConsumer)
        {
            Utils.removeConsumer(
                removingConsumer, 
                _consumers, 
                _downstreamConsumedComputings, 
                this,
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                _deferredProcessings,
                _deferredQueuesCount);
        }

        void IComputingInternal.AddDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.addDownstreamConsumedComputing(
                computing, 
                _downstreamConsumedComputings, 
                _consumers, 
                this,
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                ref _deferredProcessings,
                _deferredQueuesCount);
        }

        void IComputingInternal.RemoveDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.removeDownstreamConsumedComputing(
                computing, 
                _downstreamConsumedComputings, 
                this, 
                ref _isConsistent,
                _consumers,
                ref _handledEventSender,
                ref _handledEventArgs,
                _deferredProcessings,
                _deferredQueuesCount);
        }

        void IComputingInternal.RaiseConsistencyRestored()
        {
            raiseConsistencyRestored();
        }

        #endregion

        #region Default value conrol

        private TValue _defaultValue;
        private IReadScalar<TValue> _defaultValueScalar;
        private bool _isDefaulted;
        public TValue DefaultValue
        {
            get => _defaultValue;
            private set
            {
                _defaultValue = value;
                PropertyChanged?.Invoke(this, Utils.DefaultValuePropertyChangedEventArgs);
            }
        }

        public IReadScalar<TValue> DefaultValueScalar
        {
            get => _defaultValueScalar;
            private set
            {
                _defaultValueScalar = value;
                PropertyChanged?.Invoke(this, Utils.DefaultValueScalarPropertyChangedEventArgs);
            }
        }

        public bool IsDefaulted
        {
            get => _isDefaulted;
            protected set
            {
                _isDefaulted = value;
                PropertyChanged?.Invoke(this, Utils.IsDefaultedPropertyChangedEventArgs);
            }
        }

        #region Implementation of IScalarComputingInternal<TValue>

        internal void SetDefaultValue(TValue defaultValue)
        {
            Action action = () =>
            {
                DefaultValue = defaultValue;
                if (_isDefaulted) setValue(_defaultValue);
            };


            Utils.processChange(
                null, 
                null, 
                action,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, this);
        }

        internal void SetDefaultValue(IReadScalar<TValue> defaultValueScalar)
        {
            Action action = () =>
            {
                (_defaultValueScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(this);
                DefaultValueScalar = defaultValueScalar;

                if (_isActive) 
                    (_defaultValueScalar as IComputingInternal)?.AddDownstreamConsumedComputing(this);
                
                DefaultValue = defaultValueScalar.Value;
                if (_isDefaulted) 
                    setValue(_defaultValue);
            };


            Utils.processChange(
                null, 
                null, 
                action,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, this);
        }

        #endregion 
        

        #endregion

		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

        #region Overrides of Object

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(DebugTag))
                return $"{DebugTag} ({base.ToString()})";

            return base.ToString();
        }

        #endregion
    }
}





