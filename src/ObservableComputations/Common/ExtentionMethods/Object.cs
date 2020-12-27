using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObservableComputations
{
	internal static partial class InternalExtensionMethods
	{
		// ReSharper disable once UnusedMember.Local
		internal static string debugView(this object viewingObject, bool lineBreaks = false, string tabsIndentation = "")
		{
			string lineBreak = lineBreaks ? "\n" : string.Empty;
			switch (viewingObject)
			{
				case Boolean elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case Char elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case DateTime elementary:
					// ReSharper disable once SpecifyACultureInStringConversionExplicitly
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case Decimal elementary:
					// ReSharper disable once SpecifyACultureInStringConversionExplicitly
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case Double elementary:
					// ReSharper disable once SpecifyACultureInStringConversionExplicitly
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case Guid elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case Int32 elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case Int64 elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case IntPtr elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case SByte elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case Single elementary:
					// ReSharper disable once SpecifyACultureInStringConversionExplicitly
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case UInt16 elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case UInt32 elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case UInt64 elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case UIntPtr elementary:
					return $"{viewingObject.GetType().Name}: {elementary.ToString()}";
				case IEnumerable enumerable:
					IEnumerable<object> enumerableObjects = enumerable.Cast<object>();
					int counter = 0;
					IEnumerable<object> objects = enumerableObjects.ToArray();
					return $"{viewingObject.GetType().FullName} Count={objects.Count()}{lineBreak} {string.Join($"{lineBreak}", objects.Select(o => $"{tabsIndentation}\t{counter++}: {o.debugView(lineBreaks, $"{tabsIndentation}\t\t")}"))}";
				default:
					if (viewingObject.GetType().IsEnum)
					{
						return $"enum {viewingObject.GetType().Name}: {viewingObject}";
					}
					else
					{
						List<string> members = new List<string>();
						foreach (PropertyInfo propertyInfo in viewingObject.GetType().GetProperties())
						{
							string member;
							try
							{
								member = $"{tabsIndentation}{propertyInfo.Name}:{lineBreak} {propertyInfo.GetValue(viewingObject).debugView(lineBreaks, $"{tabsIndentation}\t")}";
							}
							catch (Exception e)
							{
								member = $"{tabsIndentation}{propertyInfo.Name}: EXCEPTION: {e.Message} ({e.GetType().FullName})";
							}

							members.Add(member);
						}
						foreach (FieldInfo fieldInfo in viewingObject.GetType().GetFields())
						{
							members.Add( $"{tabsIndentation}{fieldInfo.Name}:{lineBreak} {fieldInfo.GetValue(viewingObject).debugView(lineBreaks, $"{tabsIndentation}\t")}");
						}

						return string.Join($"{lineBreak}", members);						
					}
			}
		}

		internal static bool IsSameAs(this object object1, object object2)
		{
			return 
				object1 == null && object2 == null
					? true
					: (object1 == null && object2 != null) || (object1 != null && object2 == null)
						? false
						: object1.Equals(object2);
		}

		internal static TResult GetValueAs<TArgument, TResult>(this TArgument argument, Func<TArgument, TResult> getValueFunc)
		{
			return getValueFunc(argument);
		}
	}
}