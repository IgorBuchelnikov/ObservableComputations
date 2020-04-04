using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Binding<TValue> : INotifyPropertyChanged
	{
		readonly Expression<Func<TValue>> _getSourceExpression;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		readonly IReadScalar<TValue> _sourceScalar;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		readonly PropertyChangedEventHandler _gettingExpressionValueHandlePropertyChanged;
		readonly Action<TValue> _modifyTargetAction;

		// ReSharper disable once ConvertToAutoProperty
		public Expression<Func<TValue>> GetSourceExpression => _getSourceExpression;
		// ReSharper disable once ConvertToAutoProperty
		public Action<TValue> ModifyTargetAction => _modifyTargetAction;

		public IReadScalar<TValue> SourceScalar => _sourceScalar;

		[ObservableComputationsCall]
		public Binding(IReadScalar<TValue> sourceScalar, Action<TValue> modifyTargetAction, bool applyNow = true)
		{
			_modifyTargetAction = modifyTargetAction;
			_sourceScalar = sourceScalar;

			_gettingExpressionValueHandlePropertyChanged = (sender, args) =>
			{
				if (!_isDisabled && args.PropertyName == nameof(Computing<TValue>.Value))
				{
					modifyTargetAction(_sourceScalar.Value);
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
		public bool IsDisabled
		{
			get => _isDisabled;
			set
			{
				_isDisabled = value;
				PropertyChanged?.Invoke(this, Utils.IsDisabledPropertyChangedEventArgs);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
