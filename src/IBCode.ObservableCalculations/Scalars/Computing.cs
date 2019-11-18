using System;
using System.Linq.Expressions;
using IBCode.ObservableComputations.Common;

namespace IBCode.ObservableComputations
{
	public class Computing<TResult> : ScalarComputing<TResult>
	{
		public Expression<Func<TResult>> GetValueExpression => _getValueExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public bool IsDefaulted => _isDefaulted;

		private readonly Expression<Func<TResult>> _getValueExpressionOriginal;
		//private readonly Expression<Func<TResult>> _getValueExpression;
		private readonly Func<TResult> _getValueFunc;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly ExpressionWatcher _getValueExpressionWatcher;
		private readonly bool _isDefaulted;

		public Computing(
			Expression<Func<TResult>> getValueExpression)
		{
			if (getValueExpression != null)
			{
				_getValueExpressionOriginal = getValueExpression;
				Expression<Func<TResult>> getValueExpression1 =
					(Expression<Func<TResult>>) new CallToConstantConverter(_getValueExpressionOriginal.Parameters).Visit(_getValueExpressionOriginal);
				// ReSharper disable once PossibleNullReferenceException
				_getValueFunc = getValueExpression1.Compile();
				_getValueExpressionWatcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(getValueExpression1));
				_getValueExpressionWatcher.ValueChanged = getValueExpressionWatcherOnValueChanged;	
		
				setValue(_getValueFunc());
			}
			else
			{
				_isDefaulted = true;
				setValue(default(TResult));
			}
		}

		private void getValueExpressionWatcherOnValueChanged(ExpressionWatcher expressionWatcher)
		{
			setValue(_getValueFunc());
		}
	}
}
