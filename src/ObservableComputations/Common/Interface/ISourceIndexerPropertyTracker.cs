// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.ComponentModel;

namespace ObservableComputations
{
	internal interface ISourceIndexerPropertyTracker
	{
		void HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs);
	}

	internal interface ILeftSourceIndexerPropertyTracker : ISourceIndexerPropertyTracker
	{
		void HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs);
	}

	internal interface IRightSourceIndexerPropertyTracker : ISourceIndexerPropertyTracker
	{
		void HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs);
	}


}
