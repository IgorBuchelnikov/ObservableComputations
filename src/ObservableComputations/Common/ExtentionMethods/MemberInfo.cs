// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Reflection;

namespace ObservableComputations
{
	internal static partial class InternalExtensionMethods
	{
		internal static bool IsReadOnly(this MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return !propertyInfo.CanWrite;
			}
			else
			{
				FieldInfo fieldInfo = memberInfo as FieldInfo;

				if (fieldInfo != null)
				{
					return fieldInfo.IsInitOnly;
				}
				else
				{
					throw new Exception("Unknown MemberInfo");
				}
			}
		}

	}
}