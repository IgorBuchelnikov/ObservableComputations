using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
				result = _getValueFunc();
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
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
        }

        protected override void initialize()
        {
            Utils.initializeNestedComputings(_nestedComputings, this);
            _getValueExpressionWatcher = new ExpressionWatcher(_expressionInfo);
            _getValueExpressionWatcher.ValueChanged = getValueExpressionWatcherOnValueChanged;
            setValue(getResult());
        }

        protected override void uninitialize()
        {
            _getValueExpressionWatcher.Dispose();
            EventUnsubscriber.QueueSubscriptions(_getValueExpressionWatcher._propertyChangedEventSubscriptions, _getValueExpressionWatcher._methodChangedEventSubscriptions);
            Utils.uninitializeNestedComputings(_nestedComputings, this);
            setValue(default);
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
