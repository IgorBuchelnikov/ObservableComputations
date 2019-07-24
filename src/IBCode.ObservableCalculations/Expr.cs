using System;
using System.Linq.Expressions;

namespace IBCode.ObservableCalculations
{
	public static class Expr
	{
		public static Expression<Func<TResult>> Is<TResult>(Expression<Func<TResult>> expr)
		{
			return expr;
		}
	}
}
