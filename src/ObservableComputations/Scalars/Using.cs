using System;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Using<TArgument, TResult> : Computing<TResult>
	{
		public TArgument Argument => _argument;

		// ReSharper disable once ArrangeTypeMemberModifiers
		// ReSharper disable once UnusedMember.Local
		private Expression<Func<TArgument, TResult>> GetValueExpressionUsing => _getValueExpressionUsing;
		private readonly TArgument _argument;
		private readonly Expression<Func<TArgument, TResult>> _getValueExpressionUsing;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Using(
			TArgument argument, Expression<Func<TArgument, TResult>> getValueExpression)
			: base(getValueExpression.ApplyParameter(argument))
		{
			_argument = argument;
			_getValueExpressionUsing = getValueExpression;
		}

		#region Overrides of Computing<TResult>

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			base.addToUpstreamComputings(computing);
			(_argument as IComputingInternal)?.AddDownstreamConsumedComputing(computing);

		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			base.removeFromUpstreamComputings(computing);
			(_argument as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		#endregion
	}
}
