using System;
using System.Collections.Generic;
using System.ComponentModel;

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

    internal interface IComputingInternal : IComputing, ICanInitializeFromSource
    {
        void AddConsumer(Consumer addingConsumer);
        void RemoveConsumer(Consumer removingConsumer);
        void AddDownstreamConsumedComputing(IComputingInternal computing);
        void RemoveDownstreamConsumedComputing(IComputingInternal computing);
        IEnumerable<Consumer> Consumers { get; }
        void RaiseConsistencyRestored();
        void AddToUpstreamComputings(IComputingInternal computing);
        void RemoveFromUpstreamComputings(IComputingInternal computing);
        void Initialize();
        void Uninitialize();
        void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs);
        void SetIsActive(bool value);
    }

    internal interface ICanInitializeFromSource
    {
        void InitializeFromSource();
    }
}
