// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ObservableComputations
{
	public interface IComputing : IHasTags, IConsistent, IEventHandler
	{
		string InstantiationStackTrace { get; }
		IComputing UserCodeIsCalledFrom { get; }
		bool IsActive { get; }
		bool ActivationInProgress {get; }
		bool InactivationInProgress {get; }
		IEnumerable<IComputing> UpstreamComputingsDirect { get; }
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
		void ClearCachedScalarArgumentValues();
		void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs);
		void SetIsActive(bool value);
		void SetInactivationInProgress(bool value);
		void SetActivationInProgress(bool value);
		void RegisterInvolvedMembersAccumulator(InvolvedMembersAccumulator involvedMembersAccumulator);
		void UnregisterInvolvedMembersAccumulator(InvolvedMembersAccumulator involvedMembersAccumulator);
		List<InvolvedMembersAccumulator> InvolvedMembersAccumulators {get;}
	}

	internal interface ICanInitializeFromSource
	{
		void ProcessSource();
	}
}
