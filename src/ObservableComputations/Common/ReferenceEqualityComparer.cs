using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ObservableComputations.Common
{
	class GenericObjectReferenceEqualityComparer : IEqualityComparer<object>
	{
		#region Implementation of IEqualityComparer<in object>

		public bool Equals(object x, object y)
		{
			return ReferenceEquals(x, y);
		}

		public int GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}

		#endregion

		internal static GenericObjectReferenceEqualityComparer Instance = new GenericObjectReferenceEqualityComparer();
	}
}
