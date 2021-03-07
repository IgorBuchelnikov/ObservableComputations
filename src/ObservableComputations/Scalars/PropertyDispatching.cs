// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ObservableComputations
{
	public class PropertyDispatching<TPropertyHolder, TResult> : ScalarComputing<TResult>
		where TPropertyHolder : INotifyPropertyChanged
	{
		public TPropertyHolder PropertyHolder => _propertyHolder;
		public string  PropertyName => _propertyName;
		public IOcDispatcher SourceOcDispatcher => _sourceOcDispatcher;
		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
		public int DestinationOcDispatcherPriority => _destinationOcDispatcherPriority;
		public int SourceOcDispatcherPriority => _sourceOcDispatcherPriority;
		public object DestinationOcDispatcherParameter => _destinationOcDispatcherParameter;
		public object SourceOcDispatcherParameter => _sourceOcDispatcherParameter;

		private static readonly ConcurrentDictionary<PropertyInfo, PropertyAccessors>
			_propertyAccessors =
				new ConcurrentDictionary<PropertyInfo, PropertyAccessors>();
		
		private TPropertyHolder _propertyHolder;
		private readonly string _propertyName;

		private readonly IOcDispatcher _sourceOcDispatcher;
		private readonly IOcDispatcher _destinationOcDispatcher;

		private Action<TPropertyHolder, TResult> _setter;
		private Func<TPropertyHolder, TResult> _getter;

		private readonly int _destinationOcDispatcherPriority;
		private readonly int _sourceOcDispatcherPriority;
		private readonly object _destinationOcDispatcherParameter;
		private readonly object _sourceOcDispatcherParameter;

		[ObservableComputationsCall]
		public PropertyDispatching(
			TPropertyHolder propertyHolder,
			string propertyName,
			IOcDispatcher destinationOcDispatcher,
			IOcDispatcher sourceOcDispatcher = null,
			int destinationOcDispatcherPriority = 0,
			int sourceOcDispatcherPriority = 0,
			object destinationOcDispatcherParameter = null,
			object sourceOcDispatcherParameter = null)
		{
			_sourceOcDispatcher = sourceOcDispatcher;
			_destinationOcDispatcher = destinationOcDispatcher;
			_propertyHolder = propertyHolder;
			_propertyName = propertyName;

			_destinationOcDispatcherPriority = destinationOcDispatcherPriority;
			_sourceOcDispatcherPriority = sourceOcDispatcherPriority;
			_destinationOcDispatcherParameter = destinationOcDispatcherParameter;
			_sourceOcDispatcherParameter = sourceOcDispatcherParameter;

			PropertyChanged += (sender, args) =>
			{
				if (args == Utils.SetValueRequestHandlerPropertyChangedEventArgs)
					throw new ObservableComputationsException(
						$"Cannot set property {nameof(PropertyDispatching<TPropertyHolder, TResult>)}.{nameof(SetValueRequestHandler)}, since that it is predefined");
			};
		}


		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != _propertyName) return;
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
			public PropertyAccessors(Func<TPropertyHolder, TResult> getter, Action<TPropertyHolder, TResult> setter)
			{
				Getter = getter;
				Setter = setter;
			}

			public readonly Func<TPropertyHolder, TResult> Getter;
			public readonly Action<TPropertyHolder, TResult> Setter;
		}

		#region Overrides of ScalarComputing<TResult>

		protected override void processSource()
		{

		}

		protected override void initialize()
		{
			PropertyInfo propertyInfo = _propertyHolder.GetType().GetProperty(_propertyName);

			if (!_propertyAccessors.TryGetValue(propertyInfo, out PropertyAccessors propertyAccessors))
			{
				ParameterExpression valueParameterExpression = Expression.Parameter(typeof(TResult));
				ParameterExpression holderParameterExpression = Expression.Parameter(typeof(TPropertyHolder));
				Action<TPropertyHolder, TResult> setter = Expression.Lambda<Action<TPropertyHolder, TResult>>(
					Expression.Assign(Expression.Property(holderParameterExpression, propertyInfo), valueParameterExpression),
					holderParameterExpression, valueParameterExpression).Compile();

				Func<TPropertyHolder, TResult> getter = Expression.Lambda<Func<TPropertyHolder, TResult>>(
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

		protected override void clearCachedScalarArgumentValues()
		{

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
