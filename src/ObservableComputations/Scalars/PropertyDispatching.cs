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
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public IPropertySourceOcDispatcher PropertySourceOcDispatcher => _propertySourceOcDispatcher;
		// ReSharper disable once UnusedMember.Local
		private DispatcherPriorities DispatcherPriorities => _dispatcherPriorities;
		// ReSharper disable once UnusedMember.Local
		private DispatcherParameters DispatcherParameters => _dispatcherParameters;

		private static ConcurrentDictionary<PropertyInfo, PropertyAccessors>
			_propertyAccessors =
				new ConcurrentDictionary<PropertyInfo, PropertyAccessors>();
		
		private THolder _propertyHolder;
		private Expression<Func<TResult>> _propertyExpression;

		private IOcDispatcher _sourceOcDispatcher;
		private IOcDispatcher _destinationOcDispatcher;
		private IPropertySourceOcDispatcher _propertySourceOcDispatcher;

		private Action<THolder, TResult> _setter;
		private Func<THolder, TResult> _getter;

		private DispatcherPriorities _dispatcherPriorities;
		private DispatcherParameters _dispatcherParameters;

		[ObservableComputationsCall]
		public PropertyDispatching(
			Expression<Func<TResult>> propertyExpression,
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			DispatcherPriorities? dispatcherPriorities = null,
			DispatcherParameters? dispatcherParameters = null) : this()
		{
			_sourceOcDispatcher = sourceOcDispatcher;
			_destinationOcDispatcher = destinationOcDispatcher;
			_propertyExpression = propertyExpression;

			_dispatcherPriorities = dispatcherPriorities ?? new DispatcherPriorities(0, 0);
			_dispatcherParameters = dispatcherParameters ?? new DispatcherParameters(null, null);

			lockChangeSetValueHandle();
		}

		[ObservableComputationsCall]
		public PropertyDispatching(
			Expression<Func<TResult>> propertyExpression,
			IOcDispatcher destinationOcDispatcher,
			IPropertySourceOcDispatcher sourceOcDispatcher) : this()
		{
			_propertySourceOcDispatcher = sourceOcDispatcher;
			_destinationOcDispatcher = destinationOcDispatcher;
			_propertyExpression = propertyExpression;
			lockChangeSetValueHandle();
		}

		private PropertyDispatching()
		{
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
			TResult value = getValue();
			_destinationOcDispatcher.Invoke(
				() => setValue(value), 
				_dispatcherPriorities._distinationDispatcherPriority,
				_dispatcherParameters._distinationDispatcherParameter, 
				this);
		}

		private TResult getValue()
		{
			TResult value;

			if (Configuration.TrackComputingsExecutingUserCode)
			{

				var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
				value = _getter(_propertyHolder);
				Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
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

				if (_sourceOcDispatcher != null) _sourceOcDispatcher.Invoke(
					set, 
					_dispatcherPriorities._sourceDispatcherPriority,
					_dispatcherParameters._sourceDispatcherParameter, 
					this);
				else if (_propertySourceOcDispatcher != null) 
					_propertySourceOcDispatcher.Invoke(
						set, 
						_dispatcherPriorities._sourceDispatcherPriority,
						_dispatcherParameters._sourceDispatcherParameter,  
						this, false, value);
				else set();
			};

			_propertyHolder = (THolder) ((ConstantExpression) memberExpression.Expression).Value;

			void readAndSubscribe()
			{
				TResult value = getValue();
				_propertyHolder.PropertyChanged += handlePropertyHolderPropertyChanged;
				_destinationOcDispatcher.Invoke(
					() => setValue(value), 
					_dispatcherPriorities._distinationDispatcherPriority,
					_dispatcherParameters._distinationDispatcherParameter, 
					this);
			}

			if (_sourceOcDispatcher != null) _sourceOcDispatcher.Invoke(
				readAndSubscribe, 
				_dispatcherPriorities._sourceDispatcherPriority,
				_dispatcherParameters._sourceDispatcherParameter, 
				this);
			else if (_propertySourceOcDispatcher != null) 
				_propertySourceOcDispatcher.Invoke(
					readAndSubscribe, 
					_dispatcherPriorities._sourceDispatcherPriority,
					_dispatcherParameters._sourceDispatcherParameter,  
					this, true, null);
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
