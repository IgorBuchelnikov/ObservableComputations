using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ObservableComputations
{
	public static partial class ExtensionMethods
	{
		#region Binding

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				IReadScalar<TValue> sourceScalar,
			Action<TValue> modifyTargetAction,
			bool applyNow)
			
		{
			return new Binding<TValue>(
				sourceScalar: sourceScalar,
				modifyTargetAction: modifyTargetAction,
				applyNow : applyNow);
		}

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				IReadScalar<TValue> sourceScalar,
			Action<TValue> modifyTargetAction)
			
		{
			return new Binding<TValue>(
				sourceScalar: sourceScalar,
				modifyTargetAction: modifyTargetAction,
				applyNow : true);
		}

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				Expression<Func<TValue>> getSourceExpression,
			Action<TValue> modifyTargetAction,
			bool applyNow)
			
		{
			return new Binding<TValue>(
				getSourceExpression: getSourceExpression,
				modifyTargetAction: modifyTargetAction,
				applyNow : applyNow);
		}

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				Expression<Func<TValue>> getSourceExpression,
			Action<TValue> modifyTargetAction)
			
		{
			return new Binding<TValue>(
				getSourceExpression: getSourceExpression,
				modifyTargetAction: modifyTargetAction,
				applyNow : true);
		}

		#endregion

		#region ValuesProcessing

		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessing<TValue, TReturnValue> ValuesProcessing<TValue, TReturnValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Func<TValue, ValuesProcessing<TValue, TReturnValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs, TReturnValue> newValueProcessor,
			 bool processNow)
			
		{
			return new ObservableComputations.ValuesProcessing<TValue, TReturnValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessing<TValue, TReturnValue> ValuesProcessing<TValue, TReturnValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Func<TValue, ValuesProcessing<TValue, TReturnValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs, TReturnValue> newValueProcessor)
			
		{
			return new ObservableComputations.ValuesProcessing<TValue, TReturnValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: true);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessingVoid<TValue> ValuesProcessing<TValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Action<TValue, ValuesProcessingVoid<TValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs> newValueProcessor,
			 bool processNow)
			
		{
			return new ObservableComputations.ValuesProcessingVoid<TValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessingVoid<TValue> ValuesProcessing<TValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Action<TValue, ValuesProcessingVoid<TValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs> newValueProcessor)
			
		{
			return new ObservableComputations.ValuesProcessingVoid<TValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: true);
		}



		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessing<TValue, TReturnValue> ValuesProcessing<TValue, TReturnValue>(this
			 Expression<Func<TValue>> getValueExpression,
			 System.Func<TValue, ValuesProcessing<TValue, TReturnValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs, TReturnValue> newValueProcessor,
			 bool processNow)
			
		{
			return new ObservableComputations.ValuesProcessing<TValue, TReturnValue>(
				scalar: new Computing<TValue>(getValueExpression), 
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessing<TValue, TReturnValue> ValuesProcessing<TValue, TReturnValue>(this
			Expression<Func<TValue>> getValueExpression,
			System.Func<TValue, ValuesProcessing<TValue, TReturnValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs, TReturnValue> newValueProcessor)
			
		{
			return new ObservableComputations.ValuesProcessing<TValue, TReturnValue>(
				scalar: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				processNow: true);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessingVoid<TValue> ValuesProcessing<TValue>(this
			Expression<Func<TValue>> getValueExpression,
			System.Action<TValue, ValuesProcessingVoid<TValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs> newValueProcessor,
			bool processNow)
			
		{
			return new ObservableComputations.ValuesProcessingVoid<TValue>(
				scalar: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ValuesProcessingVoid<TValue> ValuesProcessing<TValue>(this
			Expression<Func<TValue>> getValueExpression,
			System.Action<TValue, ValuesProcessingVoid<TValue>, ObservableComputations.IReadScalar<TValue>, System.EventArgs> newValueProcessor)

		{
			return new ObservableComputations.ValuesProcessingVoid<TValue>(
				scalar: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				processNow: true);
		}

		#endregion

		#region ItemsProcessing
		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue> ItemsProcessing<TSourceItem, TReturnValue>(this
			System.Collections.Specialized.INotifyCollectionChanged source,
			System.Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue> ItemsProcessing<TSourceItem, TReturnValue>(this
			ObservableComputations.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			System.Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue> ItemsProcessing<TSourceItem, TReturnValue>(this
			Expression<Func<System.Collections.Specialized.INotifyCollectionChanged>> getSourceExpression,
			System.Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessingVoid<TSourceItem> ItemsProcessing<TSourceItem>(this
			System.Collections.Specialized.INotifyCollectionChanged source,
			System.Action<TSourceItem, ICollectionComputing> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessingVoid<TSourceItem>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessingVoid<TSourceItem> ItemsProcessing<TSourceItem>(this
			ObservableComputations.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			System.Action<TSourceItem, ICollectionComputing> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessingVoid<TSourceItem>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessingVoid<TSourceItem> ItemsProcessing<TSourceItem>(this
			Expression<Func<System.Collections.Specialized.INotifyCollectionChanged>> getSourceExpression,
			System.Action<TSourceItem, ICollectionComputing> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessingVoid<TSourceItem>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}



		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue> ItemsProcessing<TSourceItem, TReturnValue>(this
			ObservableCollection<TSourceItem> source,
			System.Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue> ItemsProcessing<TSourceItem, TReturnValue>(this
			ObservableComputations.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			System.Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue> ItemsProcessing<TSourceItem, TReturnValue>(this
			Expression<Func<ObservableCollection<TSourceItem>>> getSourceExpression,
			System.Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue>(
				sourceScalar: new Computing<ObservableCollection<TSourceItem>>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessingVoid<TSourceItem> ItemsProcessing<TSourceItem>(this
			ObservableCollection<TSourceItem> source,
			System.Action<TSourceItem, ICollectionComputing> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessingVoid<TSourceItem>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessingVoid<TSourceItem> ItemsProcessing<TSourceItem>(this
			ObservableComputations.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			System.Action<TSourceItem, ICollectionComputing> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessingVoid<TSourceItem>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessingVoid<TSourceItem> ItemsProcessing<TSourceItem>(this
			Expression<Func<ObservableCollection<TSourceItem>>> getSourceExpression,
			System.Action<TSourceItem, ICollectionComputing> newItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionComputing> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessingVoid<TSourceItem>(
				sourceScalar: new Computing<ObservableCollection<TSourceItem>>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}


		#endregion

		public static TObject Do<TObject>(
			this TObject @object, Action<TObject> action)
		{
			action(@object);
			return @object;
		}

		internal static string ToStringSafe(this object @object, Func<Exception, string> getExceptionString)
		{
			if (@object == null) return "null";

			try
			{
				return $"({@object.ToString()})";
			}
			catch (Exception exception)
			{
				return getExceptionString(exception);
			}
		}

		internal static string ToStringAlt(this PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs == null) return "null";
			return $"({propertyChangedEventArgs.ToString()} (PropertyName = ({propertyChangedEventArgs.PropertyName})))";
		}

		internal static string ToStringAlt(this NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			if (notifyCollectionChangedEventArgs == null) return "null";

			switch (notifyCollectionChangedEventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					return $"({notifyCollectionChangedEventArgs.ToString()} (Action = ({notifyCollectionChangedEventArgs.Action.ToString()}') notifyCollectionChangedEventArgs.NewItems[0] = {notifyCollectionChangedEventArgs.NewItems[0].ToStringSafe(e => $"e.ToString() in notifyCollectionChangedEventArgs.NewItems[0].ToString()")}, notifyCollectionChangedEventArgs.NewStartingIndex = ({notifyCollectionChangedEventArgs.NewStartingIndex.ToString()}))))";
				case NotifyCollectionChangedAction.Remove:
					return $"({notifyCollectionChangedEventArgs.ToString()} (Action = ({notifyCollectionChangedEventArgs.Action.ToString()}') notifyCollectionChangedEventArgs.OldItems[0] = {notifyCollectionChangedEventArgs.OldItems[0].ToStringSafe(e => $"e.ToString() in notifyCollectionChangedEventArgs.OldItems[0].ToString()")}, notifyCollectionChangedEventArgs.OldStartingIndex = ({notifyCollectionChangedEventArgs.OldStartingIndex.ToString()}))))";
				case NotifyCollectionChangedAction.Replace:
					return $"({notifyCollectionChangedEventArgs.ToString()} (Action = ({notifyCollectionChangedEventArgs.Action.ToString()}') notifyCollectionChangedEventArgs.NewItems[0] = {notifyCollectionChangedEventArgs.NewItems[0].ToStringSafe(e => $"e.ToString() in notifyCollectionChangedEventArgs.NewItems[0].ToString()")}, notifyCollectionChangedEventArgs.NewStartingIndex = ({notifyCollectionChangedEventArgs.NewStartingIndex.ToString()}), notifyCollectionChangedEventArgs.OldItems[0] = {notifyCollectionChangedEventArgs.OldItems[0].ToStringSafe(e => $"e.ToString() in notifyCollectionChangedEventArgs.OldItems[0].ToString()")}, notifyCollectionChangedEventArgs.OldStartingIndex = ({notifyCollectionChangedEventArgs.OldStartingIndex.ToString()}))))";					
				case NotifyCollectionChangedAction.Move:
					return $"({notifyCollectionChangedEventArgs.ToString()} (Action = ({notifyCollectionChangedEventArgs.Action.ToString()}') notifyCollectionChangedEventArgs.OldStartingIndex = {notifyCollectionChangedEventArgs.OldStartingIndex.ToString()}, notifyCollectionChangedEventArgs.NewStartingIndex = ({notifyCollectionChangedEventArgs.NewStartingIndex.ToString()})))";					
				case NotifyCollectionChangedAction.Reset:
					return $"({notifyCollectionChangedEventArgs.ToString()} (Action = ({notifyCollectionChangedEventArgs.Action.ToString()})))";					
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		internal static string ToStringAlt(this NotifyMethodChangedEventArgs notifyMethodChangedEventArgs)
		{
			if (notifyMethodChangedEventArgs == null) return "null";
			return $"({notifyMethodChangedEventArgs.ToString()} (MethodName = ({notifyMethodChangedEventArgs.MethodName}), ArgumentsPredicate.GetHashCode() = ({notifyMethodChangedEventArgs.ArgumentsPredicate.GetHashCode()})))";
		}

		internal static string ToStringAlt(this EventArgs eventArgs)
		{
			if (eventArgs == null) return "null";

			if (eventArgs is PropertyChangedEventArgs propertyChangedEventArgs)
				return propertyChangedEventArgs.ToStringAlt();
			else if  (eventArgs is NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
				return notifyCollectionChangedEventArgs.ToStringAlt();
			else if  (eventArgs is NotifyMethodChangedEventArgs notifyMethodChangedEventArgs)
				return notifyMethodChangedEventArgs.ToStringAlt();

			return string.Empty;
		}
	}
}
