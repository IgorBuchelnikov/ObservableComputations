using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IBCode.ObservableCalculations.Common
{
	internal sealed class CallToConstantConverter : ExpressionVisitor
	{
		private readonly IEnumerable<ParameterExpression> _parameterExpressions;

		public bool ContainsParametrizedObservableCalculationCalls;

		public CallToConstantConverter(IEnumerable<ParameterExpression> parameterExpressions = null)
		{
			_parameterExpressions = parameterExpressions;
		}

		#region Overrides of ExpressionVisitor

		protected override Expression VisitNew(NewExpression node)
		{
			if (node.Constructor.GetCustomAttribute<ObservableCalculationsCallAttribute>() != null)
			{
				ConstantExpression constantExpression = getConstantExpression(node);
				if (constantExpression != null) return constantExpression;
			}

			return base.VisitNew(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.GetCustomAttribute<ObservableCalculationsCallAttribute>() != null)
			{
				if (!node.Method.IsStatic)
					throw new ObservableCalculationsException(
						"ObservableCalculationsCallAttribute is applicable for static methods only");

				ConstantExpression constantExpression = getConstantExpression(node);
				if (constantExpression != null) return constantExpression;
			}

			return base.VisitMethodCall(node);
		}

		private ConstantExpression getConstantExpression(Expression node)
		{
			ConstantExpression getConstantExpressionLocal()
			{
				return Expression.Constant(Expression.Lambda(node).Compile().DynamicInvoke(), node.Type);
			}

			if (_parameterExpressions != null)
			{
				ParametersFinder parametersFinder = new ParametersFinder(_parameterExpressions);
				parametersFinder.Visit(node);

				if (!parametersFinder.ParametersFound)
				{
					return getConstantExpressionLocal();
				}
				else
				{
					ContainsParametrizedObservableCalculationCalls = parametersFinder.ParametersFound;
				}
			}
			else
			{
				return getConstantExpressionLocal();
			}

			return null;
		}

		#endregion
	}

	internal sealed class ParametersFinder : ExpressionVisitor
	{
		public bool ParametersFound;

		private readonly IEnumerable<ParameterExpression> _parameterExpressions;

		public ParametersFinder(IEnumerable<ParameterExpression> parameterExpressions)
		{
			_parameterExpressions = parameterExpressions;
		}

		#region Overrides of ExpressionVisitor

		protected override Expression VisitParameter(ParameterExpression node)
		{
			if (_parameterExpressions.Contains(node)) ParametersFound = true;
			return base.VisitParameter(node);
		}

		#endregion
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
	public sealed class ObservableCalculationsCallAttribute : Attribute
	{
	}
}