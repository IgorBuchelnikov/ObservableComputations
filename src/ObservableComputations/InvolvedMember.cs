using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations
{
		internal struct InvolvedMember
		{
			public object Source;
			public string MemberName;

			public InvolvedMember(object source, string memberName)
			{
				Source = source;
				MemberName = memberName;
			}
		}

		internal class InvolvedMemberEqualityComparer : IEqualityComparer<InvolvedMember>
		{
			#region Implementation of IEqualityComparer<in InvolvedMember>

			public bool Equals(InvolvedMember x, InvolvedMember y)
			{
				return ReferenceEquals(x.Source, y.Source) && string.Equals(x.MemberName, y.MemberName);
			}

			public int GetHashCode(InvolvedMember obj)
			{
				unchecked
				{
					return obj.Source.GetHashCode() + obj.MemberName.GetHashCode();					
				}
			}

			#endregion

			internal static InvolvedMemberEqualityComparer Instance = new InvolvedMemberEqualityComparer();
		}
}
