using System.Collections;
using System.Collections.Generic;

namespace ObservableComputations
{
	public interface ICollectionProcessing : ICollectionComputing
	{
        bool Initializing { get; }
	}

}
