using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Binding<TValue> : INotifyPropertyChanged
	{
		readonly Expression<Func<TValue>> _getSourceExpression;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		readonly Computing<TValue> _gettingExpressionValue;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		readonly PropertyChangedEventHandler _gettingExpressionValueHandlePropertyChanged;
		readonly Action<TValue> _modifyTargetAction;

		// ReSharper disable once ConvertToAutoProperty
		public Expression<Func<TValue>> GetSourceExpression => _getSourceExpression;
		// ReSharper disable once ConvertToAutoProperty
		public Action<TValue> ModifyTargetAction => _modifyTargetAction;

		public Binding(Expression<Func<TValue>> getSourceExpression, Action<TValue> modifyTargetAction, bool bindNow = true)
		{

			_getSourceExpression = getSourceExpression;
			_modifyTargetAction = modifyTargetAction;
			_gettingExpressionValue = new Computing<TValue>(getSourceExpression);

			_gettingExpressionValueHandlePropertyChanged = (sender, args) =>
			{
				if (_isEnabled && args.PropertyName == nameof(Computing<TValue>.Value))
				{
					modifyTargetAction(_gettingExpressionValue.Value);
				}
			};

			_gettingExpressionValue.PropertyChanged += _gettingExpressionValueHandlePropertyChanged;

			if (bindNow)
				modifyTargetAction(_gettingExpressionValue.Value);
		}

		private bool _isEnabled;
		public bool IsEnabled
		{
			get => _isEnabled;
			set
			{
				_isEnabled = value;
				PropertyChanged?.Invoke(this, Utils.IsEnabledPropertyChangedEventArgs);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
