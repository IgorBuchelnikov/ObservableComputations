using System;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface INotifyMethodChanged
	{
		event EventHandler<NotifyMethodChangedEventArgs> MethodChanged;
	}

	public class NotifyMethodChangedEventArgs : EventArgs
	{
		public NotifyMethodChangedEventArgs(string methodName, Func<object[], bool> argumentsPredicate)
		{
			MethodName = methodName;
			ArgumentsPredicate = argumentsPredicate;
		}

		public string MethodName { get; }

		public Func<object[], bool> ArgumentsPredicate { get; }
	}
}
