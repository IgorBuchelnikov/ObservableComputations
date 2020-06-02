using System;

namespace ObservableComputations
{
	public interface IComputing : IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
		IComputing UserCodeIsCalledFrom { get; }
		object ProcessingEventSender { get;  }
		EventArgs ProcessingEventArgs { get;  }
	}
}
