using System;
using System.Linq.Expressions;

namespace IBCode.ObservableComputations
{
	public static class Expr
	{
		public static Expression<Func<TResult>> Is<TResult>(Expression<Func<TResult>> expr)
		{
			return expr;
		}
	}
}
