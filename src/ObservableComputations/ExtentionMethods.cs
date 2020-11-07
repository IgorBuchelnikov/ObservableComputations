using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

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

		#region ScalarProcessing

		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessing<TValue, TReturnValue> ScalarProcessing<TValue, TReturnValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Func<TValue, IScalarComputing, TReturnValue, TReturnValue> newValueProcessor,
			 bool processNow)
			
		{
			return new ObservableComputations.ScalarProcessing<TValue, TReturnValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessing<TValue, TReturnValue> ScalarProcessing<TValue, TReturnValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Func<TValue, IScalarComputing, TReturnValue, TReturnValue> newValueProcessor)
			
		{
			return new ObservableComputations.ScalarProcessing<TValue, TReturnValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: true);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessingVoid<TValue> ScalarProcessing<TValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor,
			 bool processNow)
			
		{
			return new ObservableComputations.ScalarProcessingVoid<TValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessingVoid<TValue> ScalarProcessing<TValue>(this
			 ObservableComputations.IReadScalar<TValue> scalar,
			 System.Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor)
			
		{
			return new ObservableComputations.ScalarProcessingVoid<TValue>(
				scalar: scalar,
				newValueProcessor: newValueProcessor,
				processNow: true);
		}



		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessing<TValue, TReturnValue> ScalarProcessing<TValue, TReturnValue>(this
			 Expression<Func<TValue>> getValueExpression,
			System.Func<TValue, IScalarComputing, TReturnValue, TReturnValue> newValueProcessor,
			 bool processNow)
			
		{
			return new ObservableComputations.ScalarProcessing<TValue, TReturnValue>(
				scalar: new Computing<TValue>(getValueExpression), 
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessing<TValue, TReturnValue> ScalarProcessing<TValue, TReturnValue>(this
			Expression<Func<TValue>> getValueExpression,
			System.Func<TValue, IScalarComputing, TReturnValue, TReturnValue> newValueProcessor)
			
		{
			return new ObservableComputations.ScalarProcessing<TValue, TReturnValue>(
				scalar: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				processNow: true);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessingVoid<TValue> ScalarProcessing<TValue>(this
			Expression<Func<TValue>> getValueExpression,
			System.Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor,
			bool processNow)
			
		{
			return new ObservableComputations.ScalarProcessingVoid<TValue>(
				scalar: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				processNow: processNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ScalarProcessingVoid<TValue> ScalarProcessing<TValue>(this
			Expression<Func<TValue>> getValueExpression,
			System.Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor)

		{
			return new ObservableComputations.ScalarProcessingVoid<TValue>(
				scalar: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				processNow: true);
		}

		#endregion

		#region CollectionProcessing
		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			System.Collections.Specialized.INotifyCollectionChanged source,
			System.Func<TSourceItem[], ICollectionProcessing, TReturnValue[]> newItemProcessor = null,
			System.Action<TSourceItem[], ICollectionProcessing, TReturnValue[]> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionProcessing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			ObservableComputations.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			System.Func<TSourceItem[], ICollectionProcessing, TReturnValue[]> newItemProcessor = null,
			System.Action<TSourceItem[], ICollectionProcessing, TReturnValue[]> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionProcessing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			Expression<Func<System.Collections.Specialized.INotifyCollectionChanged>> getSourceExpression,
			System.Func<TSourceItem[], ICollectionProcessing, TReturnValue[]> newItemProcessor = null,
			System.Action<TSourceItem[], ICollectionProcessing, TReturnValue[]> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionProcessing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			System.Collections.Specialized.INotifyCollectionChanged source,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			System.Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessingVoid<TSourceItem>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			ObservableComputations.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			System.Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessingVoid<TSourceItem>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			Expression<Func<System.Collections.Specialized.INotifyCollectionChanged>> getSourceExpression,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			System.Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessingVoid<TSourceItem>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}



		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			ObservableCollection<TSourceItem> source,
			System.Func<TSourceItem[], ICollectionProcessing, TReturnValue[]> newItemProcessor = null,
			System.Action<TSourceItem[], ICollectionProcessing, TReturnValue[]> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionProcessing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			ObservableComputations.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			System.Func<TSourceItem[], ICollectionProcessing, TReturnValue[]> newItemProcessor = null,
			System.Action<TSourceItem[], ICollectionProcessing, TReturnValue[]> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionProcessing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			Expression<Func<ObservableCollection<TSourceItem>>> getSourceExpression,
			System.Func<TSourceItem[], ICollectionProcessing, TReturnValue[]> newItemProcessor = null,
			System.Action<TSourceItem[], ICollectionProcessing, TReturnValue[]> oldItemProcessor = null,
			System.Action<TSourceItem, ICollectionProcessing, TReturnValue> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: new Computing<ObservableCollection<TSourceItem>>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			ObservableCollection<TSourceItem> source,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			System.Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessingVoid<TSourceItem>(
				source: source,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			ObservableComputations.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			System.Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessingVoid<TSourceItem>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			Expression<Func<ObservableCollection<TSourceItem>>> getSourceExpression,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			System.Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			System.Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new ObservableComputations.CollectionProcessingVoid<TSourceItem>(
				sourceScalar: new Computing<ObservableCollection<TSourceItem>>(getSourceExpression),
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}


		#endregion

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

        [ObservableComputationsCall]
        public static TComputing For<TComputing>(this TComputing computing, Consumer consumer)
            where TComputing : IComputing
        {
            ((IComputingInternal) computing).AddConsumer(consumer);
            return computing;
        }

        [ObservableComputationsCall]
        public static TComputing SetDebugTag<TComputing>(this TComputing computing, string debugTag)
            where TComputing : IHasTags
        {
            computing.DebugTag = debugTag;
            return computing;
        }

        [ObservableComputationsCall]
        public static TScalarComputing SetDefaultValue<TScalarComputing, TValue>(this TScalarComputing scalarComputing, TValue defaultValue)
            where TScalarComputing : ScalarComputing<TValue>
        {
            scalarComputing.SetDefaultValue(defaultValue);
            return scalarComputing;
        }

        [ObservableComputationsCall]
        public static TScalarComputing SetDefaultValue<TScalarComputing, TValue>(this TScalarComputing scalarComputing, IReadScalar<TValue> defaultValueScalar)
            where TScalarComputing : ScalarComputing<TValue>
        {
            scalarComputing.SetDefaultValue(defaultValueScalar);
            return scalarComputing;
        }

        [ObservableComputationsCall]
        public static TScalarComputing SetDefaultValue<TScalarComputing, TValue>(this TScalarComputing scalarComputing, Expression<Func<TValue>> getValueExpression)
            where TScalarComputing : ScalarComputing<TValue>
        {
            scalarComputing.SetDefaultValue(new Computing<TValue>(getValueExpression));
            return scalarComputing;
        }
	}
}
