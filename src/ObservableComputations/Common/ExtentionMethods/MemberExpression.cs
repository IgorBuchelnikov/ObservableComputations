// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Linq.Expressions;
using System.Reflection;

namespace ObservableComputations
{
	internal static partial class InternalExtensionMethods
	{
		internal static bool IsStatic(this MemberExpression memberExpression)
		{
			FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
			PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
			// ReSharper disable once PossibleNullReferenceException
			return fieldInfo != null ? fieldInfo.IsStatic : propertyInfo.GetMethod.IsStatic;
		}
	}
}