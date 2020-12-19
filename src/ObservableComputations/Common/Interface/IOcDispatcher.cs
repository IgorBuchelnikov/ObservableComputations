using System;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface IOcDispatcher
	{
		void Invoke(Action action, int priority, object parameter, object context);
	}

	public interface ICollectionDestinationOcDispatcher
	{
		void InvokeCollectionChange(
			Action action, 
			int priority, 
			object parameter,
			ICollectionComputing context,
			NotifyCollectionChangedAction notifyCollectionChangedAction,
			object newItem,
			object oldItem,
			int newIndex,
			int oldIndex);

		void InvokeInitialization(
			Action action,
			int priority, 
			object parameter,
			ICollectionComputing context);
	}

	public interface IPropertySourceOcDispatcher
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="propertyDispatching"></param>
		/// <param name="initializing">false if setter of Value property is called</param>
		/// <param name="newValue">new value if setter of Value property is called </param>
		void Invoke(
			Action action, 
			int priority, 
			object parameter,
			IComputing propertyDispatching,
			bool initializing,
			object newValue);
	}
}
