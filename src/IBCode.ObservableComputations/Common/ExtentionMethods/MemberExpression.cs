using System.Linq.Expressions;
using System.Reflection;

namespace IBCode.ObservableComputations.Common
{
    internal static partial class ExtensionMethods
    {
        public static bool IsStatic(this MemberExpression memberExpression)
        {
            FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
			PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
			// ReSharper disable once PossibleNullReferenceException
			return fieldInfo != null ? fieldInfo.IsStatic : propertyInfo.GetMethod.IsStatic;
        }
    }
}