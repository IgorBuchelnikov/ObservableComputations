using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations
{
		internal struct InvolvedMemberChangedArgs
		{
			public InvolvedMember InvolvedMember;
			public bool Created; // false - Removed

			public InvolvedMemberChangedArgs(object source, string memberName, bool created)
			{
				InvolvedMember = new InvolvedMember(source, memberName);
				Created = created;
			}
		}

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
		}

		internal class InvolvedMembersTreeNode
		{
			public IComputingInternal Computing;
			public Action<InvolvedMemberChangedArgs> Handler;
			public Dictionary<InvolvedMember, int> InvolvedMemebers = new Dictionary<InvolvedMember, int>(_comparer);
			private Dictionary<IComputingInternal, int> ChildrenComputings;
			public List<InvolvedMembersTreeNode> Children;
			private static InvolvedMemberEqualityComparer _comparer = new InvolvedMemberEqualityComparer();

			public InvolvedMembersTreeNode(Action<InvolvedMemberChangedArgs> handler)
			{
				Handler = handler;
			}

			public void RegisterInvolvedMember(InvolvedMember involvedMember)
			{
				if (InvolvedMemebers.ContainsKey(involvedMember))
					InvolvedMemebers[involvedMember]++;
				else
					InvolvedMemebers.Add(involvedMember, 1);
			}

			public void UnregisterInvolvedMember(InvolvedMember involvedMember)
			{
				InvolvedMemebers[involvedMember]--;
				if (InvolvedMemebers[involvedMember] == 0)
					InvolvedMemebers.Remove(involvedMember);
			}

			public void AddChild(IComputingInternal computing)
			{
				if (Children == null)
				{
					Children = new List<InvolvedMembersTreeNode>();
					ChildrenComputings = new Dictionary<IComputingInternal, int>();
				}

				if (ChildrenComputings.ContainsKey(computing))
					ChildrenComputings[computing]++;
				else
					ChildrenComputings.Add(computing, 1);

				if (ChildrenComputings[computing] > 1) return;
				InvolvedMembersTreeNode node = new InvolvedMembersTreeNode(args => Handler(args));
				Children.Add(node);
				computing.InitializeInvolvedMembersTreeNode(node);
			}

			public void RemoveChild(IComputingInternal computing)
			{
				ChildrenComputings[computing]--;

				if (ChildrenComputings[computing] == 0)
				{
					ChildrenComputings.Remove(computing);

					int count = Children.Count;
					for (var index = 0; index < count; index++) 
						if (Children[index].Computing == computing)
						{
							Children[index].Clear(Handler);
							break;
						}
				}

				if (ChildrenComputings[computing] == 0)
				{
					ChildrenComputings.Remove(computing);

					if (ChildrenComputings.Count == 0)
					{
						Children = null;
						ChildrenComputings = null;
					}
				}
			}

			public void Clear(Action<InvolvedMemberChangedArgs> handler)
			{
				Computing.RemoveInvolvedMembersTreeNode(this);

				foreach (InvolvedMember involvedMemeber in InvolvedMemebers.Keys)
					handler(new InvolvedMemberChangedArgs(involvedMemeber.Source, involvedMemeber.MemberName, false));

				int count = Children.Count;
				for (var index = 0; index < count; index++) 
					Children[index].Clear(handler);
			}

		}
}
