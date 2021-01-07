using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Binding<TValue> : INotifyPropertyChanged, IEventHandler, IDisposable
	{
		readonly Expression<Func<TValue>> _getSourceExpression;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		IReadScalar<TValue> _sourceScalar;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		PropertyChangedEventHandler _gettingExpressionValueHandlePropertyChanged;
		Action<TValue> _modifyTargetAction;

		// ReSharper disable once ConvertToAutoProperty
		public Expression<Func<TValue>> GetSourceExpression => _getSourceExpression;
		// ReSharper disable once ConvertToAutoProperty
		public Action<TValue> ModifyTargetAction => _modifyTargetAction;

		public IReadScalar<TValue> SourceScalar => _sourceScalar;

		public object HandledEventSender => _handledEventSender;
		public EventArgs HandledEventArgs => _handledEventArgs;

		public bool IsDisposed => _isDisposed;

		private readonly OcConsumer _consumer = new OcConsumer("Binding consumer");

		[ObservableComputationsCall]
		public Binding(IReadScalar<TValue> sourceScalar, Action<TValue, Binding<TValue>> modifyTargetAction, bool applyNow = true)
		{
			initialize(sourceScalar, value => modifyTargetAction(value, this), applyNow);
		}

		[ObservableComputationsCall]
		public Binding(IReadScalar<TValue> sourceScalar, Action<TValue> modifyTargetAction, bool applyNow = true)
		{
			initialize(sourceScalar, modifyTargetAction, applyNow);
		}

		private void initialize(IReadScalar<TValue> sourceScalar, Action<TValue> modifyTargetAction, bool applyNow)
		{
			_modifyTargetAction = modifyTargetAction;
			_sourceScalar = sourceScalar;
			(_sourceScalar as IComputing)?.For(_consumer);

			_gettingExpressionValueHandlePropertyChanged = (sender, args) =>
			{
				if (!_isDisabled && args.PropertyName == nameof(Computing<TValue>.Value))
				{
					_handledEventSender = sender;
					_handledEventArgs = args;
					modifyTargetAction(_sourceScalar.Value);
					_handledEventSender = null;
					_handledEventArgs = null;
				}
			};

			_sourceScalar.PropertyChanged += _gettingExpressionValueHandlePropertyChanged;

			if (applyNow)
				modifyTargetAction(_sourceScalar.Value);
		}

		[ObservableComputationsCall]
		public Binding(Expression<Func<TValue>> getSourceExpression, Action<TValue> modifyTargetAction, bool applyNow = true) 
			: this(new Computing<TValue>(getSourceExpression), modifyTargetAction, applyNow)
		{
			_getSourceExpression = getSourceExpression;
		}

		private bool _isDisabled;
		private bool _isDisposed;
		private object _handledEventSender;
		private EventArgs _handledEventArgs;

		public bool IsDisabled
		{
			get => _isDisabled;
			set
			{
				if (_isDisposed) throw new ObservableComputationsException("Binding is disposed");
				_isDisabled = value;
				PropertyChanged?.Invoke(this, Utils.IsDisabledPropertyChangedEventArgs);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#region Implementation of IDisposable

		public void Dispose()
		{
			_isDisposed = true;
			_consumer.Dispose();
			_sourceScalar.PropertyChanged -= _gettingExpressionValueHandlePropertyChanged;
			PropertyChanged?.Invoke(this, Utils.IsDisposedPropertyChangedEventArgs);
		}

		#endregion
	}
}
