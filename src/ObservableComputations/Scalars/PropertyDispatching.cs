// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

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
		public int DestinationOcDispatcherPriority => _destinationOcDispatcherPriority;
		public int SourceOcDispatcherPriority => _sourceOcDispatcherPriority;
		public object DestinationOcDispatcherParameter => _destinationOcDispatcherParameter;
		public object SourceOcDispatcherParameter => _sourceOcDispatcherParameter;

		private static readonly ConcurrentDictionary<PropertyInfo, PropertyAccessors>
			_propertyAccessors =
				new ConcurrentDictionary<PropertyInfo, PropertyAccessors>();
		
		private THolder _propertyHolder;
		private readonly Expression<Func<TResult>> _propertyExpression;

		private readonly IOcDispatcher _sourceOcDispatcher;
		private readonly IOcDispatcher _destinationOcDispatcher;

		private Action<THolder, TResult> _setter;
		private Func<THolder, TResult> _getter;

		private readonly int _destinationOcDispatcherPriority;
		private readonly int _sourceOcDispatcherPriority;
		private readonly object _destinationOcDispatcherParameter;
		private readonly object _sourceOcDispatcherParameter;

		[ObservableComputationsCall]
		public PropertyDispatching(
			Expression<Func<TResult>> propertyExpression,
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			int destinationOcDispatcherPriority = 0,
			int sourceOcDispatcherPriority = 0,
			object destinationOcDispatcherParameter = null,
			object sourceOcDispatcherParameter = null) : this()
		{
			_sourceOcDispatcher = sourceOcDispatcher;
			_destinationOcDispatcher = destinationOcDispatcher;
			_propertyExpression = propertyExpression;

			_destinationOcDispatcherPriority = destinationOcDispatcherPriority;
			_sourceOcDispatcherPriority = sourceOcDispatcherPriority;
			_destinationOcDispatcherParameter = destinationOcDispatcherParameter;
			_sourceOcDispatcherParameter = sourceOcDispatcherParameter;

			lockChangeSetValueHandle();
		}


		private PropertyDispatching()
		{
		}

		private void lockChangeSetValueHandle()
		{
			PropertyChanged += (sender, args) =>
			{
				if (args == Utils.SetValueRequestHandlerPropertyChangedEventArgs)
					throw new ObjectDisposedException(
						$"Cannot set property {nameof(PropertyDispatching<THolder, TResult>)}.{nameof(SetValueRequestHandler)}, since that it is predefined");
			};
		}

		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			TResult value = getValue();
			_destinationOcDispatcher.Invoke(
				() => setValue(value), 
				_destinationOcDispatcherPriority,
				_destinationOcDispatcherParameter, 
				this);
		}

		private TResult getValue()
		{
			TResult value;

			if (Configuration.TrackComputingsExecutingUserCode)
			{

				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				value = _getter(_propertyHolder);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
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

			public readonly Func<THolder, TResult> Getter;
			public readonly Action<THolder, TResult> Setter;
		}

		#region Overrides of ScalarComputing<TResult>

		protected override void processSource()
		{

		}

		protected override void initialize()
		{
			MemberExpression memberExpression = (MemberExpression) _propertyExpression.Body;

			PropertyInfo propertyInfo = (PropertyInfo) ((MemberExpression) _propertyExpression.Body).Member;

			if (!_propertyAccessors.TryGetValue(propertyInfo, out PropertyAccessors propertyAccessors))
			{
				ParameterExpression valueParameterExpression = Expression.Parameter(typeof(TResult));
				ParameterExpression holderParameterExpression = Expression.Parameter(typeof(THolder));
				Action<THolder, TResult> setter = Expression.Lambda<Action<THolder, TResult>>(
					Expression.Assign(Expression.Property(holderParameterExpression, propertyInfo), valueParameterExpression),
					holderParameterExpression, valueParameterExpression).Compile();

				Func<THolder, TResult> getter = Expression.Lambda<Func<THolder, TResult>>(
					Expression.Property(holderParameterExpression, propertyInfo),
					holderParameterExpression).Compile();

				propertyAccessors = new PropertyAccessors(getter, setter);
				_propertyAccessors.TryAdd(propertyInfo, propertyAccessors);
			}

			_getter = propertyAccessors.Getter;
			_setter = propertyAccessors.Setter;
			_setValueRequestHandler = value =>
			{
				void set() => _setter(_propertyHolder, value);

				if (_sourceOcDispatcher != null)
					_sourceOcDispatcher.Invoke(
						set, 
						_sourceOcDispatcherPriority,
						_sourceOcDispatcherParameter, 
						this);
				else set();
			};

			_propertyHolder = (THolder) ((ConstantExpression) memberExpression.Expression).Value;

			void readAndSubscribe()
			{
				TResult value = getValue();
				_propertyHolder.PropertyChanged += handlePropertyHolderPropertyChanged;
				_destinationOcDispatcher.Invoke(
					() => setValue(value), 
					_destinationOcDispatcherPriority,
					_destinationOcDispatcherParameter, 
					this);
			}

			if (_sourceOcDispatcher != null)
				_sourceOcDispatcher.Invoke(
					readAndSubscribe, 
					_sourceOcDispatcherPriority,
					_sourceOcDispatcherParameter, 
					this);
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

		protected override void raisePropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			_destinationOcDispatcher.Invoke(
				() => base.raisePropertyChanged(eventArgs), 
				_destinationOcDispatcherPriority,
				_destinationOcDispatcherParameter,
				this);
		}
	}
}
