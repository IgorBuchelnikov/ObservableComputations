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

		internal class InvolvedMembersTreeNode
		{
			public IComputingInternal Computing;
			public Action<InvolvedMemberChangedArgs> Handler;
			public Dictionary<InvolvedMember, int> InvolvedMemebers = new Dictionary<InvolvedMember, int>();
			private Dictionary<IComputingInternal, int> ChildrenComputings;
			public List<InvolvedMembersTreeNode> Children;

			public InvolvedMembersTreeNode(Action<InvolvedMemberChangedArgs> handler)
			{
				Handler = handler;
			}

			public void AddChild(IComputingInternal computing)
			{
				if (Children == null)
				{
					Children = new List<InvolvedMembersTreeNode>();
					ChildrenComputings = new Dictionary<IComputingInternal, int>();
				}

				ChildrenComputings[computing]++;
				if (ChildrenComputings[computing] == 1)
				{
					InvolvedMembersTreeNode node = new InvolvedMembersTreeNode(args => Handler(args));
					Children.Add(node);
					computing.InitializeInvolvedMembersTreeNode(node);
				}
			}

			public void RemoveChild(IComputingInternal computing)
			{

				ChildrenComputings[computing]--;

				if (ChildrenComputings[computing] == 0)
				{
					int count = Children.Count;
					for (var index = 0; index < count; index++) 
						if (Children[index].Computing == computing)
						{
							Children[index].Dispose(Handler);
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

			private void Dispose(Action<InvolvedMemberChangedArgs> handler)
			{
				Computing.RemoveInvolvedMembersTreeNode(this);

				foreach (InvolvedMember involvedMemeber in InvolvedMemebers.Keys)
					handler(new InvolvedMemberChangedArgs(involvedMemeber.Source, involvedMemeber.MemberName, false));

				int count = Children.Count;
				for (var index = 0; index < count; index++)
				{
					Children[index].Dispose(handler);		
				}	
			}

		}
}
