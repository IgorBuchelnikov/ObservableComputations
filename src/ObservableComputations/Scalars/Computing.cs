using System;
using System.Linq.Expressions;
using System.Threading;

namespace ObservableComputations
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

		[ObservableComputationsCall]
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
		
				setValue(getResult());
			}
			else
			{
				_isDefaulted = true;
				setValue(default(TResult));
			}
		}

		private void getValueExpressionWatcherOnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			_handledEventSender = sender;
			_handledEventArgs = eventArgs;
			setValue(getResult());
			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private TResult getResult()
		{
			TResult result;
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				IComputing computing = DebugInfo._computingsExecutingUserCode.ContainsKey(currentThread)
					? DebugInfo._computingsExecutingUserCode[currentThread]
					: null;
				DebugInfo._computingsExecutingUserCode[currentThread] = this;
				_userCodeIsCalledFrom = computing;

				result = _getValueFunc();

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
			}
			else
			{
				result = _getValueFunc();
			}

			return result;
		}
	}
}
