using System;
using System.Collections.Generic;

namespace ObservableComputations
{
	public interface IComputing : IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
		IComputing UserCodeIsCalledFrom { get; }
		object HandledEventSender { get;  }
		EventArgs HandledEventArgs { get;  }
        bool IsActive { get; }
	}

    internal interface IComputingInternal : IComputing
    {
        void AddConsumer(Consumer addingConsumer);
        void RemoveConsumer(Consumer removingConsumer);
        void AddDownstreamConsumedComputing(IComputingInternal computing);
        void RemoveDownstreamConsumedComputing(IComputingInternal computing);
        IEnumerable<Consumer> Consumers { get; }
        void RaiseConsistencyRestored();
    }
}
