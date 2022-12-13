// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

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
		private readonly Func<TResult> _getValueFunc;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private ExpressionWatcher _getValueExpressionWatcher;

		private readonly ExpressionWatcher.ExpressionInfo _expressionInfo;
		private readonly Action _changeValueAction;

		[ObservableComputationsCall]
		public Computing(
			Expression<Func<TResult>> getValueExpression)
		{
			_changeValueAction = () => setValue(getResult());
			_getValueExpressionOriginal = getValueExpression;

			CallToConstantConverter callToConstantConverter = new CallToConstantConverter(_getValueExpressionOriginal.Parameters);
			Expression<Func<TResult>> getValueExpression1 =
				(Expression<Func<TResult>>) callToConstantConverter.Visit(_getValueExpressionOriginal);
			// ReSharper disable once PossibleNullReferenceException
			_getValueFunc = getValueExpression1.Compile();
			_expressionInfo = ExpressionWatcher.GetExpressionInfo(getValueExpression1);
		}


		private void getValueExpressionWatcherOnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			Utils.processExpressionWatcherCurrentComputingsChanges(expressionWatcher, this);
			Utils.processChange(
				sender, 
				eventArgs, 
				_changeValueAction,
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs, 
				0, _deferredQueuesCount,
				ref _deferredProcessings, 
				this,
				false);
		}

		private TResult getResult()
		{
			TResult result;
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				result = _getValueFunc();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
			}
			else
			{
				result = _getValueFunc();
			}

			return result;
		}

		#region Overrides of ScalarComputing<TResult>

		protected override void processSource()
		{
			if (_isActive)
			{

				_getValueExpressionWatcher = new ExpressionWatcher(this, _expressionInfo);
				Utils.initializeExpressionWatcherCurrentComputings(_getValueExpressionWatcher, _expressionInfo._callCount, this);
				_getValueExpressionWatcher.ValueChanged = getValueExpressionWatcherOnValueChanged;
				setValue(getResult());
			}
			else
			{
				_getValueExpressionWatcher.Dispose();
				EventUnsubscriber.QueueSubscriptions(_getValueExpressionWatcher._propertyChangedEventSubscriptions, _getValueExpressionWatcher._methodChangedEventSubscriptions);
				Utils.removeDownstreamConsumedComputing(_getValueExpressionWatcher, this);
				setDefaultValue();				
			}
		}

		protected override void initialize()
		{
		}

		protected override void uninitialize()
		{

		}

		protected override void clearCachedScalarArgumentValues()
		{

		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{

		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{

		}

		#endregion

		public override IEnumerable<IComputing> UpstreamComputingsDirect
		{
			get
			{
				List<IComputing> computings = new List<IComputing>();
				Utils.FillUpstreamComputingsDirect(computings, _getValueExpressionWatcher._currentComputings);
				return computings;
			}
		}

		internal override void RegisterInvolvedMembersAccumulatorImpl(InvolvedMembersAccumulator involvedMembersAccumulator) => 
			_getValueExpressionWatcher.ProcessInvolvedMembersAccumulator(involvedMembersAccumulator, true);

		internal override void UnregisterInvolvedMembersAccumulatorImpl(InvolvedMembersAccumulator involvedMembersAccumulator) => 
			_getValueExpressionWatcher.ProcessInvolvedMembersAccumulator(involvedMembersAccumulator, false);
	}
}
