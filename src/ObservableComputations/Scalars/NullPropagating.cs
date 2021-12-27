// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class NullPropagating<TValue, TResult> : ScalarComputing<TResult>
		where TValue : class
	{
		public IReadScalar<TValue> Source => _source;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source});

		public Expression<Func<TValue, TResult>> GetValueExpression => _getValueExpression;
		private readonly IReadScalar<TValue> _source;
		private readonly Expression<Func<TValue, TResult>> _getValueExpression;

		private Computing<TResult> _computing;

		[ObservableComputationsCall]
		public NullPropagating(
			IReadScalar<TValue> source,
			Expression<Func<TValue, TResult>> getValueExpression)
		{
			_source = source;
			_getValueExpression = getValueExpression;

			MemberExpression getSourceValueExpression = Expression.PropertyOrField(Expression.Constant(source), nameof(IReadScalar<TValue>.Value));

			_computing = new Computing<TResult>(
				Expression.Lambda<Func<TResult>>(
					Expression.Condition(
						Expression.NotEqual(
							getSourceValueExpression,
							Expression.Constant(null, typeof(TValue))),
						new ReplaceParameterVisitor(
							new Dictionary<ParameterExpression, Expression>()
							{
								{ getValueExpression.Parameters[0], getSourceValueExpression }
							}).Visit(getValueExpression.Body),
						Expression.Constant(default(TResult), typeof(TResult)))));
		}

		private void handleSourceScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(IReadScalar<TValue>.Value))
				updateValue();
		}

		private void updateValue()
		{
			if (_source.Value != null)
				setValue(_computing.Value);
			else
				setDefaultValue();
		}

		#region Overrides of ScalarComputing<TResult>

		protected override void processSource()
		{
			if (_sourceReadAndSubscribed)
			{
				_source.PropertyChanged -= handleSourceScalarPropertyChanged;
				_sourceReadAndSubscribed = false;
			}

			if (_isActive)
			{
				updateValue();

				_source.PropertyChanged += handleSourceScalarPropertyChanged;
				_sourceReadAndSubscribed = true;
			}
			else
				setDefaultValue();
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
			(_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			((IComputingInternal)_computing).AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			((IComputingInternal)_computing).RemoveDownstreamConsumedComputing(computing);
		}

		#endregion

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			bool conststent = true;

			if (_source.Value != null && !_value.Equals(_getValueExpression.Compile()(_source.Value)))
				throw new ValidateInternalConsistencyException("Consistency violation: NullPropagating.1");

			if (_source.Value == null && !_isDefaulted)
				throw new ValidateInternalConsistencyException("Consistency violation: NullPropagating.1");
		}
	}
}