using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace IBCode.ObservableCalculations
{
	public class Binding<TValue>
	{
		readonly Expression<Func<TValue>> _getExpression;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		readonly Calculating<TValue> _gettingExpressionValue;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		readonly PropertyChangedEventHandler _gettingExpressionValueHandlePropertyChanged;
		readonly Action<TValue> _setAction;

		// ReSharper disable once ConvertToAutoProperty
		public Expression<Func<TValue>> GetExpression => _getExpression;
		// ReSharper disable once ConvertToAutoProperty
		public Action<TValue> SetAction => _setAction;

		public Binding(Expression<Func<TValue>> getExpression, Action<TValue> setAction)
		{

			_getExpression = getExpression;
			_setAction = setAction;
			_gettingExpressionValue = new Calculating<TValue>(getExpression);

			_gettingExpressionValueHandlePropertyChanged = (sender, args) =>
			{
				if (args.PropertyName == nameof(Calculating<TValue>.Value))
				{
					setAction(_gettingExpressionValue.Value);
				}
			};

			_gettingExpressionValue.PropertyChanged += _gettingExpressionValueHandlePropertyChanged;
		}
	}
}
