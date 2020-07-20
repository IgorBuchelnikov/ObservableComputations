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
		[ObservableComputationsCall]
		public static TObject Do<TObject>(
			this TObject @object, Action<TObject> action)
		{
			action(@object);
			return @object;
		}

		[ObservableComputationsCall]
		public static TReturnValue Do<TObject, TReturnValue>(
			this TObject @object, Func<TObject, TReturnValue> func)
		{
			return func(@object);
		}

		public static string ToStringSafe(this object @object, Func<Exception, string> getExceptionString = null)
		{
			if (@object == null) return "null";

			try
			{
				return $"({@object.ToString()})";
			}
			catch (Exception exception)
			{
				return getExceptionString != null ? getExceptionString(exception) : "exception";
			}
		}

		public static string ToStringAlt(this PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs == null) return "null";
			return $"({propertyChangedEventArgs.ToString()} (PropertyName = ({propertyChangedEventArgs.PropertyName})))";
		}

		public static string ToStringAlt(this NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
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

		public static string ToStringAlt(this MethodChangedEventArgs methodChangedEventArgs)
		{
			if (methodChangedEventArgs == null) return "null";
			return $"({methodChangedEventArgs.ToString()} (MethodName = ({methodChangedEventArgs.MethodName}), ArgumentsPredicate.GetHashCode() = ({methodChangedEventArgs.ArgumentsPredicate.GetHashCode()})))";
		}

		public static string ToStringAlt(this EventArgs eventArgs)
		{
			if (eventArgs == null) return "null";

			if (eventArgs is PropertyChangedEventArgs propertyChangedEventArgs)
				return propertyChangedEventArgs.ToStringAlt();
			else if  (eventArgs is NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
				return notifyCollectionChangedEventArgs.ToStringAlt();
			else if  (eventArgs is MethodChangedEventArgs notifyMethodChangedEventArgs)
				return notifyMethodChangedEventArgs.ToStringAlt();

			return string.Empty;
		}

        public static TComputing Consume<TComputing>(this TComputing computing, Consumer consumer)
            where TComputing : IComputing
        {
            ((IComputingInternal) computing).AddConsumer(consumer);
            return computing;
        }
	}
}
