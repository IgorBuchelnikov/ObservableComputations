using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class PropertyDispatching<TResult> : IReadScalar<TResult>, IWriteScalar<TResult>, IScalar<TResult>
	{
		public object PropertyHolder => _propertyHolder;
		public Expression<Func<TResult>>  PropertyExpression => _propertyExpression;
		public IDispatcher SourceDispatcher => _sourceDispatcher;
		public IDispatcher DestinationDispatcher => _destinationDispatcher;


		private INotifyPropertyChanged _propertyHolder;
		private Expression<Func<TResult>> _propertyExpression;

		// ReSharper disable once FieldCanBeMadeReadOnly.Local
		private PropertyChangedEventHandler _propertyHolderPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _propertyHolderWeakPropertyChangedEventHandler;

		private IDispatcher _sourceDispatcher;
		private IDispatcher _destinationDispatcher;

		private Action<TResult> _setAction;
		private Func<TResult> _getAction;

		private TResult _value;

		[ObservableComputationsCall]
		public PropertyDispatching( 
			Expression<Func<TResult>>  propertyExpression,
			IDispatcher sourceDispatcher,
			IDispatcher destinationDispatcher)
		{
			_sourceDispatcher = sourceDispatcher;
			_destinationDispatcher = destinationDispatcher;

			_propertyExpression = propertyExpression;

			MemberExpression memberExpression = (MemberExpression) propertyExpression.Body;
			_propertyHolder = (INotifyPropertyChanged) ((ConstantExpression) memberExpression.Expression).Value;

			_propertyHolderPropertyChangedEventHandler = handlePropertyHolderPropertyChanged;
			_propertyHolderWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_propertyHolderPropertyChangedEventHandler);
			_propertyHolder.PropertyChanged += _propertyHolderWeakPropertyChangedEventHandler.Handle;

			ParameterExpression parameterExpression = Expression.Parameter(typeof(TResult));
			_setAction = Expression.Lambda<Action<TResult>>(
				Expression.Assign(propertyExpression.Body, parameterExpression), 
				parameterExpression).Compile();

			_getAction = propertyExpression.Compile();
		}

		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_value = _getAction();
			_destinationDispatcher.Invoke(() =>
			{
				PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			});
		}

		~PropertyDispatching()
		{
			_propertyHolder.PropertyChanged -= _propertyHolderWeakPropertyChangedEventHandler.Handle;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public TResult Value
		{
			get => _value; 
			set => _sourceDispatcher.Invoke(() => _setAction(value));
		}
	}
}
