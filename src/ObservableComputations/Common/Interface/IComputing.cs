using System;

namespace ObservableComputations
{
	public interface IComputing : IHasTags, IConsistent
	{
		string InstantiatingStackTrace { get; }
		IComputing UserCodeIsCalledFrom { get; }
		object HandledEventSender { get;  }
		EventArgs HandledEventArgs { get;  }
	}
}
