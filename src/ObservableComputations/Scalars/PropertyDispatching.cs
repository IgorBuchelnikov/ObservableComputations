using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableComputations
{
	public class PropertyDispatching<THolder, TResult> : ScalarComputing<TResult>
		where THolder : INotifyPropertyChanged
	{
		public THolder PropertyHolder => _propertyHolder;
		public Expression<Func<TResult>>  PropertyExpression => _propertyExpression;
		public IDispatcher SourceDispatcher => _sourceDispatcher;
		public IDispatcher DestinationDispatcher => _destinationDispatcher;
		public IPropertySourceDispatcher PropertySourceDispatcher => _propertySourceDispatcher;


        private static ConcurrentDictionary<PropertyInfo, PropertyAccessors>
			_propertyAccessors =
				new ConcurrentDictionary<PropertyInfo, PropertyAccessors>();
		
		private THolder _propertyHolder;
		private Expression<Func<TResult>> _propertyExpression;

		private IDispatcher _sourceDispatcher;
		private IDispatcher _destinationDispatcher;
		private IPropertySourceDispatcher _propertySourceDispatcher;

		private Action<THolder, TResult> _setter;
		private Func<THolder, TResult> _getter;

		[ObservableComputationsCall]
		public PropertyDispatching(
			Expression<Func<TResult>> propertyExpression,
			IDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_sourceDispatcher = sourceDispatcher;
			_destinationDispatcher = destinationDispatcher;
            _propertyExpression = propertyExpression;

            lockChangeSetValueHandle();
        }

        [ObservableComputationsCall]
		public PropertyDispatching(
			Expression<Func<TResult>> propertyExpression,
			IDispatcher destinationDispatcher,
			IPropertySourceDispatcher sourceDispatcher)
		{
			_propertySourceDispatcher = sourceDispatcher;
			_destinationDispatcher = destinationDispatcher;
            _propertyExpression = propertyExpression;
            lockChangeSetValueHandle();
        }

        private void lockChangeSetValueHandle()
        {
            PropertyChanged += (sender, args) =>
            {
                if (args == Utils.SetValueActionPropertyChangedEventArgs)
                    throw new ObjectDisposedException(
                        $"Cannot set property {nameof(PropertyDispatching<THolder, TResult>)}.{nameof(SetValueAction)}, since that it is predefined");
            };
        }

		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_handledEventSender = sender;
			_handledEventArgs = e;

            TResult value = getValue();
			_destinationDispatcher.Invoke(() => setValue(value), this);
			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private TResult getValue()
        {
            TResult value;

			if (Configuration.TrackComputingsExecutingUserCode)
			{

                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
                value = _getter(_propertyHolder);
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
			}
			else
			{
                value = _getter(_propertyHolder);
			}

            return value;
        }

		private struct PropertyAccessors
		{
			public PropertyAccessors(Func<THolder, TResult> getter, Action<THolder, TResult> setter)
			{
				Getter = getter;
				Setter = setter;
			}

			public Func<THolder, TResult> Getter;
			public Action<THolder, TResult> Setter;
		}

        #region Overrides of ScalarComputing<TResult>

        protected override void initializeFromSource()
        {

        }

        protected override void initialize()
		{
			MemberExpression memberExpression = (MemberExpression) _propertyExpression.Body;

			PropertyInfo propertyInfo = (PropertyInfo) ((MemberExpression) _propertyExpression.Body).Member;
			PropertyAccessors propertyAcessors;

			if (!_propertyAccessors.TryGetValue(propertyInfo, out propertyAcessors))
			{
				ParameterExpression valueParameterExpression = Expression.Parameter(typeof(TResult));
				ParameterExpression holderParameterExpression = Expression.Parameter(typeof(THolder));
				var setter = Expression.Lambda<Action<THolder, TResult>>(
					Expression.Assign(Expression.Property(holderParameterExpression, propertyInfo), valueParameterExpression),
					holderParameterExpression, valueParameterExpression).Compile();

				var getter = Expression.Lambda<Func<THolder, TResult>>(
					Expression.Property(holderParameterExpression, propertyInfo),
					holderParameterExpression).Compile();

				propertyAcessors = new PropertyAccessors(getter, setter);
				_propertyAccessors.TryAdd(propertyInfo, propertyAcessors);
			}

			_getter = propertyAcessors.Getter;
			_setter = propertyAcessors.Setter;
            _setValueAction = value =>
            {
                void set() => _setter(_propertyHolder, value);

                if (_sourceDispatcher != null) _sourceDispatcher.Invoke(set, this);
                else if (_propertySourceDispatcher != null) _propertySourceDispatcher.Invoke(set, this, false, value);
                else set();
            };

			_propertyHolder = (THolder) ((ConstantExpression) memberExpression.Expression).Value;

			void readAndSubscribe()
			{
                TResult value = getValue();
				_propertyHolder.PropertyChanged += handlePropertyHolderPropertyChanged;
				_destinationDispatcher.Invoke(() => setValue(value), this);
			}

			if (_sourceDispatcher != null) _sourceDispatcher.Invoke(readAndSubscribe, this);
			else if (_propertySourceDispatcher != null) _propertySourceDispatcher.Invoke(readAndSubscribe, this, true, null);
            else readAndSubscribe();
		}

        protected override void uninitialize()
        {
            _propertyHolder.PropertyChanged -= handlePropertyHolderPropertyChanged;
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
        }

        #endregion
    }
}
