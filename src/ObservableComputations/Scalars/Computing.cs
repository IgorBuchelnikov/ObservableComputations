using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace ObservableComputations
{
	public class Computing<TResult> : ScalarComputing<TResult>
	{
		public Expression<Func<TResult>> GetValueExpression => _getValueExpressionOriginal;

		private readonly Expression<Func<TResult>> _getValueExpressionOriginal;
		//private readonly Expression<Func<TResult>> _getValueExpression;
		private Func<TResult> _getValueFunc;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private ExpressionWatcher _getValueExpressionWatcher;

        private bool _sourceInitialized;
        private List<IComputingInternal> _nestedComputings;
        private ExpressionWatcher.ExpressionInfo _expressionInfo;

		[ObservableComputationsCall]
		public Computing(
			Expression<Func<TResult>> getValueExpression)
		{
			_getValueExpressionOriginal = getValueExpression;

            CallToConstantConverter callToConstantConverter = new CallToConstantConverter(_getValueExpressionOriginal.Parameters);
            Expression<Func<TResult>> getValueExpression1 =
                (Expression<Func<TResult>>) callToConstantConverter.Visit(_getValueExpressionOriginal);
            // ReSharper disable once PossibleNullReferenceException
            _getValueFunc = getValueExpression1.Compile();
            _expressionInfo = ExpressionWatcher.GetExpressionInfo(getValueExpression1);
            _nestedComputings = callToConstantConverter.NestedComputings;
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

        #region Overrides of ScalarComputing<TResult>

        protected override void initializeFromSource()
        {
            if (_sourceInitialized)
            {
                _getValueExpressionWatcher.Dispose();
                EventUnsubscriber.QueueSubscriptions(_getValueExpressionWatcher._propertyChangedEventSubscriptions, _getValueExpressionWatcher._methodChangedEventSubscriptions);

            }

            if (_getValueExpressionOriginal != null && _isActive)
            {
                _getValueExpressionWatcher = new ExpressionWatcher(_expressionInfo);
                _getValueExpressionWatcher.ValueChanged = getValueExpressionWatcherOnValueChanged;

                _isDefaulted = false;
                setValue(getResult());
            }
            else
            {
                _isDefaulted = true;
                setValue(default);
            }
        }

        protected override void initialize()
        {
            Utils.initializeNestedComputings(_nestedComputings, this);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeNestedComputings(_nestedComputings, this);
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
        }

        #endregion
    }
}
