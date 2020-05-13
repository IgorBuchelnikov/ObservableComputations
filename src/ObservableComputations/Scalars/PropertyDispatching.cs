using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class PropertyDispatching<TResult> : IReadScalar<TResult>, IWriteScalar<TResult>, IScalar<TResult>, IComputing
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

			ParameterExpression parameterExpression = Expression.Parameter(typeof(TResult));
			_setAction = Expression.Lambda<Action<TResult>>(
				Expression.Assign(propertyExpression.Body, parameterExpression), 
				parameterExpression).Compile();

			_getAction = propertyExpression.Compile();

			MemberExpression memberExpression = (MemberExpression) propertyExpression.Body;
			_propertyHolder = (INotifyPropertyChanged) ((ConstantExpression) memberExpression.Expression).Value;

			_propertyHolderPropertyChangedEventHandler = handlePropertyHolderPropertyChanged;
			_propertyHolderWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_propertyHolderPropertyChangedEventHandler);

			_propertyHolder.PropertyChanged += _propertyHolderWeakPropertyChangedEventHandler.Handle;
			_value = _getAction();

			void raiseValuePropertyChanged()
			{
				PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			}

			_destinationDispatcher.BeginInvoke(raiseValuePropertyChanged, this);
			
			if (Configuration.SaveInstantiatingStackTrace)
				_instantiatingStackTrace = Environment.StackTrace;

		}

		private void handlePropertyHolderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_value = _getAction();

			void raiseValuePropertyChanged()
			{
				PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			}

			_destinationDispatcher.BeginInvoke(raiseValuePropertyChanged, this);	
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
					_setAction(value);
				}

				_sourceDispatcher.BeginInvoke(set, this);
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

		#endregion
	}
}
