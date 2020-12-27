using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ObservableComputations
{
	public interface IComputing : IHasTags, IConsistent, IEventHandler
	{
		string InstantiatingStackTrace { get; }
		IComputing UserCodeIsCalledFrom { get; }
		bool IsActive { get; }
	}

	public interface IEventHandler
	{
		object HandledEventSender { get; }
		EventArgs HandledEventArgs { get; }   
	}

	internal interface IComputingInternal : IComputing, ICanInitializeFromSource
	{
		void AddConsumer(OcConsumer addingOcConsumer);
		void RemoveConsumer(OcConsumer removingOcConsumer);
		void AddDownstreamConsumedComputing(IComputingInternal computing);
		void RemoveDownstreamConsumedComputing(IComputingInternal computing);
		IEnumerable<OcConsumer> Consumers { get; }
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
