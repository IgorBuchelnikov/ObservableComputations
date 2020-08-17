using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace ObservableComputations
{
	public class PropertyDispatching<THolder, TResult> : IReadScalar<TResult>, IWriteScalar<TResult>, IScalar<TResult>, IComputing
		where THolder : INotifyPropertyChanged
	{
		public THolder PropertyHolder => _propertyHolder;
		public Expression<Func<TResult>>  PropertyExpression => _propertyExpression;
		public IDispatcher SourceDispatcher => _sourceDispatcher;
		public IDispatcher DestinationDispatcher => _destinationDispatcher;
		public IPropertySourceDispatcher PropertySourceDispatcher => _propertySourceDispatcher;

		internal object _handledEventSender;
		internal EventArgs _handledEventArgs;
		public object HandledEventSender => _handledEventSender;
		public EventArgs HandledEventArgs => _handledEventArgs;
        public bool IsActive { get; }

        private static ConcurrentDictionary<PropertyInfo, PropertyAccessors>
			_propertyAccessors =
				new ConcurrentDictionary<PropertyInfo, PropertyAccessors>();
		
		private THolder _propertyHolder;
		private Expression<Func<TResult>> _propertyExpression;

		// ReSharper disable once FieldCanBeMadeReadOnly.Local
		private PropertyChangedEventHandler _propertyHolderPropertyChangedEventHandler;

		private IDispatcher _sourceDispatcher;
		private IDispatcher _destinationDispatcher;
		private IPropertySourceDispatcher _propertySourceDispatcher;

		private Action<THolder, TResult> _setter;
		private Func<THolder, TResult> _getter;

		private TResult _value;

		[ObservableComputationsCall]
		public PropertyDispatching(
			Expression<Func<TResult>> propertyExpression,
			IDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_sourceDispatcher = sourceDispatcher;
			_destinationDispatcher = destinationDispatcher;
            _propertyExpression = propertyExpression
		}


		[ObservableComputationsCall]
		public PropertyDispatching(
			Expression<Func<TResult>> propertyExpression,
			IDispatcher destinationDispatcher,
			IPropertySourceDispatcher sourceDispatcher)
		{
			_propertySourceDispatcher = sourceDispatcher;
			_destinationDispatcher = destinationDispatcher;
            _propertyExpression = propertyExpression
		}

		private void initialize(Expression<Func<TResult>> propertyExpression)
		{
			MemberExpression memberExpression = (MemberExpression) propertyExpression.Body;

			PropertyInfo propertyInfo = (PropertyInfo) ((MemberExpression) propertyExpression.Body).Member;
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

			_propertyHolder = (THolder) ((ConstantExpression) memberExpression.Expression).Value;

			void readAndSubscribe()
			{
				getValue();

				void raiseValuePropertyChanged()
				{
					PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
				}

				_propertyHolder.PropertyChanged += handlePropertyHolderPropertyChanged;

				_destinationDispatcher.Invoke(raiseValuePropertyChanged, this);
			}

			if (_sourceDispatcher != null)
			{
				if (_sourceDispatcher != null) _sourceDispatcher.Invoke(readAndSubscribe, this);
				else _propertySourceDispatcher.Invoke(readAndSubscribe, this, true, null);
			}
			else
			{
				readAndSubscribe();
			}

			if (Configuration.SaveInstantiatingStackTrace)
				_instantiatingStackTrace = Environment.StackTrace;
		}

		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_handledEventSender = sender;
			_handledEventArgs = e;

			getValue();

			void raiseValuePropertyChanged()
			{
				PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			}

			_destinationDispatcher.Invoke(raiseValuePropertyChanged, this);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void getValue()
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{

				Thread currentThread = Thread.CurrentThread;
				IComputing computing = DebugInfo._computingsExecutingUserCode.ContainsKey(currentThread)
					? DebugInfo._computingsExecutingUserCode[currentThread]
					: null;
				DebugInfo._computingsExecutingUserCode[currentThread] = this;
				_userCodeIsCalledFrom = computing;

				_value = _getter(_propertyHolder);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				_value = _getter(_propertyHolder);
			}
		}

		~PropertyDispatching()
		{
			_propertyHolder.PropertyChanged -= _propertyHolderWeakPropertyChangedEventHandler.Handle;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public TResult Value
		{
			get => _value;
			set
			{
				void set()
				{
					if (Configuration.TrackComputingsExecutingUserCode)
					{

						Thread currentThread = Thread.CurrentThread;
						IComputing computing = DebugInfo._computingsExecutingUserCode.ContainsKey(currentThread)
							? DebugInfo._computingsExecutingUserCode[currentThread]
							: null;
						DebugInfo._computingsExecutingUserCode[currentThread] = this;
						_userCodeIsCalledFrom = computing;

						_setter(_propertyHolder, value);

						if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
						else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
						_userCodeIsCalledFrom = null;
					}
					else
					{
						_setter(_propertyHolder, value);
					}


				}

				if (_sourceDispatcher != null) _sourceDispatcher.Invoke(set, this);
				else if (_propertySourceDispatcher != null) _propertySourceDispatcher.Invoke(set, this, false, value);
				else set();
			}
		}

		#region Implementation of IHasTags

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		#endregion

		#region Implementation of IConsistent

		public bool IsConsistent => true;
		public event EventHandler ConsistencyRestored;

		#endregion

		#region Implementation of IComputing

		private string _instantiatingStackTrace;
		public string InstantiatingStackTrace => _instantiatingStackTrace;

		private IComputing _userCodeIsCalledFrom;
		public IComputing UserCodeIsCalledFrom => _userCodeIsCalledFrom;

		#endregion

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
	}
}
