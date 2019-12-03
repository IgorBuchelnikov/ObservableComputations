using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ObservableComputations.Common
{
    internal static partial class ExtensionMethods
    {
		public static Expression<Func<TResult>> ApplyParameter<TParameter0, TResult>(this Expression<Func<TParameter0, TResult>> func, TParameter0 parameter0Value)
		{
			var body = new ReplaceParameterVisitor(new Dictionary<ParameterExpression, Expression>(){{func.Parameters[0], Expression.Constant(parameter0Value, func.Parameters[0].Type)}}).Visit(func.Body);
			// ReSharper disable once AssignNullToNotNullAttribute
			return Expression.Lambda<Func<TResult>>(body);		
		}

		public static Expression<Func<TResult>> ApplyParameters<TParameter0, TParameter1,TResult>(this Expression<Func<TParameter0, TParameter1, TResult>> func, TParameter0 parameter0Value, TParameter1 parameter1Value)
		{
			var body = new ReplaceParameterVisitor(new Dictionary<ParameterExpression, Expression>(){{func.Parameters[0], Expression.Constant(parameter0Value, func.Parameters[0].Type)}, {func.Parameters[1], Expression.Constant(parameter1Value, func.Parameters[1].Type)}}).Visit(func.Body);
			// ReSharper disable once AssignNullToNotNullAttribute
			return Expression.Lambda<Func<TResult>>(body);		
		}

		public static LambdaExpression ApplyParameters(this LambdaExpression lambdaExpression, object[] parameterValues)
		{
			Dictionary<ParameterExpression, Expression> replacements = new Dictionary<ParameterExpression, Expression>();
			for (var index = 0; index < parameterValues.Length; index++)
			{
				object parameterValue = parameterValues[index];
				replacements.Add(lambdaExpression.Parameters[index], Expression.Constant(parameterValue, lambdaExpression.Parameters[index].Type));
			}

			var body = new ReplaceParameterVisitor(replacements).Visit(lambdaExpression.Body);
			return Expression.Lambda(body);		
		}
    }

	public sealed class ReplaceParameterVisitor : ExpressionVisitor
	{
		private readonly Dictionary<ParameterExpression, Expression> _replacements;

		public ReplaceParameterVisitor(Dictionary<ParameterExpression, Expression> replacements)
		{
			_replacements = replacements;
		}  
   
		public ReplaceParameterVisitor(IEnumerable<ParameterExpression> parameterExpressions, Expression[] replacements)
		{
			_replacements = new Dictionary<ParameterExpression, Expression>();
			int counter = 0;
			foreach (ParameterExpression parameterExpression in parameterExpressions)
			{
				_replacements.Add(parameterExpression, replacements[counter]);
				counter++;
			}
		}

		public ReplaceParameterVisitor(ParameterExpression parameterExpression, Expression replacement)
		{
			_replacements = new Dictionary<ParameterExpression, Expression>();
			_replacements.Add(parameterExpression, replacement);		
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			Expression replacement;
			return _replacements.TryGetValue(node, out replacement) ? replacement : base.VisitParameter(node);
		}
	}

	public sealed class ReplaceMemberVisitor : ExpressionVisitor
	{
		private readonly Func<MemberExpression, Expression> _replaceFunc;

		public ReplaceMemberVisitor(Func<MemberExpression, Expression> replaceFunc)
		{
			_replaceFunc = replaceFunc;
		}  
  
		protected override Expression VisitMember(MemberExpression node)
		{
			Expression replacement = _replaceFunc(node);
			return replacement ?? base.VisitMember(node);
		}
	}

}


