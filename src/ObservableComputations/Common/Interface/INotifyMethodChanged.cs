using System;

namespace ObservableComputations
{
	public interface INotifyMethodChanged
	{
		event EventHandler<MethodChangedEventArgs> MethodChanged;
	}

	public class MethodChangedEventArgs : EventArgs
	{
		public MethodChangedEventArgs(string methodName, Func<object[], bool> argumentsPredicate)
		{
			MethodName = methodName;
			ArgumentsPredicate = argumentsPredicate;
		}

		public string MethodName { get; }

		public Func<object[], bool> ArgumentsPredicate { get; }
	}
}
