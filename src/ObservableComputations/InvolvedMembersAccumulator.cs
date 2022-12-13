using System;
using System.Collections.Generic;
using ObservableComputations.Common;

namespace ObservableComputations
{
	internal class InvolvedMembersAccumulator
	{
		internal Dictionary<IComputingInternal, int> Computings = new Dictionary<IComputingInternal, int>(GenericObjectReferenceEqualityComparer.Instance);
		internal Dictionary<InvolvedMember, int> InvolvedMembers = new Dictionary<InvolvedMember, int>(InvolvedMemberEqualityComparer.Instance);
		internal IComputing[] ExcludedComputings;

		public InvolvedMembersAccumulator(IComputing[] excludedComputings)
		{
			ExcludedComputings = excludedComputings;
		}

		internal bool RegisterComputing(IComputingInternal computing)
		{
			if (Computings.ContainsKey(computing))
			{
				Computings[computing]++;
				return false;
			}
			else
			{
				Computings.Add(computing, 1);
				return true;
			}
		}

		internal bool UnregisterComputing(IComputingInternal computing)
		{
			if (Computings.ContainsKey(computing))
			{
				Computings[computing]--;

				if (Computings[computing] == 0) return Computings.Remove(computing);

				return true;
			}
			else
				return false;
		}

		internal void RegisterInvolvedMember(InvolvedMember involvedMember)
		{
			if (InvolvedMembers.ContainsKey(involvedMember))
			{
				InvolvedMembers[involvedMember]++;
			}
			else
			{
				InvolvedMembers.Add(involvedMember, 1);
			}
		}

		internal void UnregisterInvolvedMember(InvolvedMember involvedMember)
		{
#if DEBUG
			if (InvolvedMembers.ContainsKey(involvedMember))
			{
#endif
				InvolvedMembers[involvedMember]--;

				if (InvolvedMembers[involvedMember] == 0)
				{
					InvolvedMembers.Remove(involvedMember);
				}

#if DEBUG
			}

			else
				throw new Exception("Cannot unregister the computing. It hasn't been registered yet.");
#endif
		}
	}
}
