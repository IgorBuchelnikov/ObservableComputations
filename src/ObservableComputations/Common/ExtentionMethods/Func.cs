// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ObservableComputations
{
	internal static partial class InternalExtensionMethods
	{
		internal static Expression<Func<TResult>> ApplyParameter<TParameter0, TResult>(this Expression<Func<TParameter0, TResult>> func, TParameter0 parameter0Value)
		{
			Expression body = new ReplaceParameterVisitor(new Dictionary<ParameterExpression, Expression>(){{func.Parameters[0], Expression.Constant(parameter0Value, func.Parameters[0].Type)}}).Visit(func.Body);
			// ReSharper disable once AssignNullToNotNullAttribute
			return Expression.Lambda<Func<TResult>>(body);		
		}

		internal static Expression<Func<TResult>> ApplyParameters<TParameter0, TParameter1,TResult>(this Expression<Func<TParameter0, TParameter1, TResult>> func, TParameter0 parameter0Value, TParameter1 parameter1Value)
		{
			Expression body = new ReplaceParameterVisitor(new Dictionary<ParameterExpression, Expression>(){{func.Parameters[0], Expression.Constant(parameter0Value, func.Parameters[0].Type)}, {func.Parameters[1], Expression.Constant(parameter1Value, func.Parameters[1].Type)}}).Visit(func.Body);
			// ReSharper disable once AssignNullToNotNullAttribute
			return Expression.Lambda<Func<TResult>>(body);		
		}

		internal static LambdaExpression ApplyParameters(this LambdaExpression lambdaExpression, object[] parameterValues)
		{
			Dictionary<ParameterExpression, Expression> replacements = new Dictionary<ParameterExpression, Expression>();
			for (int index = 0; index < parameterValues.Length; index++)
				replacements.Add(lambdaExpression.Parameters[index], Expression.Constant(parameterValues[index], lambdaExpression.Parameters[index].Type));

			Expression body = new ReplaceParameterVisitor(replacements).Visit(lambdaExpression.Body);
			return Expression.Lambda(body);		
		}
	}

	internal sealed class ReplaceParameterVisitor : ExpressionVisitor
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
			_replacements = new Dictionary<ParameterExpression, Expression> {{parameterExpression, replacement}};

		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			return _replacements.TryGetValue(node, out Expression replacement) ? replacement : base.VisitParameter(node);
		}
	}

	internal sealed class ReplaceMemberVisitor : ExpressionVisitor
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


