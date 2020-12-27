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