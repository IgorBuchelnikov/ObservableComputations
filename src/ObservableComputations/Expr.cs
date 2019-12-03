using System;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public static class Expr
	{
		public static Expression<Func<TResult>> Is<TResult>(Expression<Func<TResult>> expr)
		{
			return expr;
		}
	}
}
