// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public static partial class ExtensionMethods
	{
		#region ScalarProcessing
		[ObservableComputationsCall]
		public static ScalarProcessing<TValue, TReturnValue> ScalarProcessing<TValue, TReturnValue>(this
			IReadScalar<TValue> source,
			Func<TValue, IScalarComputing, TReturnValue> newValueProcessor,
			Action<TValue, IScalarComputing, TReturnValue> oldValueProcessor = null)
			
		{
			return new ScalarProcessing<TValue, TReturnValue>(
				source: source,
				newValueProcessor: newValueProcessor,
				oldValueProcessor : oldValueProcessor);
		}

		[ObservableComputationsCall]
		public static ScalarProcessingVoid<TValue> ScalarProcessing<TValue>(this
			 IReadScalar<TValue> source,
			Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor = null,
			Action<TValue, ScalarProcessingVoid<TValue>> oldValueProcessor = null)
			
		{
			return new ScalarProcessingVoid<TValue>(
				source: source,
				newValueProcessor: newValueProcessor,
				oldValueProcessor : oldValueProcessor);
		}

		[ObservableComputationsCall]
		public static ScalarDisposing<TValue> ScalarDisposing<TValue>(this
				IReadScalar<TValue> source) where TValue : IDisposable

		{
			return new ScalarDisposing<TValue>(
				source: source);
		}



		[ObservableComputationsCall]
		public static ScalarProcessing<TValue, TReturnValue> ScalarProcessing<TValue, TReturnValue>(this
			Expression<Func<TValue>> getValueExpression,
			Func<TValue, IScalarComputing, TReturnValue> newValueProcessor,
			Action<TValue, IScalarComputing, TReturnValue> oldValueProcessor = null)
			
		{
			return new ScalarProcessing<TValue, TReturnValue>(
				source: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				oldValueProcessor : oldValueProcessor);
		}

		[ObservableComputationsCall]
		public static ScalarProcessingVoid<TValue> ScalarProcessing<TValue>(this
			Expression<Func<TValue>> getValueExpression,
			Action<TValue, ScalarProcessingVoid<TValue>> newValueProcessor = null,
			Action<TValue, ScalarProcessingVoid<TValue>> oldValueProcessor = null)

		{
			return new ScalarProcessingVoid<TValue>(
				source: new Computing<TValue>(getValueExpression),
				newValueProcessor: newValueProcessor,
				oldValueProcessor : oldValueProcessor);
		}
		[ObservableComputationsCall]
		public static ScalarDisposing<TValue> ScalarDisposing<TValue>(this
				Expression<Func<TValue>> getValueExpression) where TValue : IDisposable

		{
			return new ScalarDisposing<TValue>(
				source: new Computing<TValue>(getValueExpression));
		}

		#endregion

		#region CollectionProcessing
		[ObservableComputationsCall]
		public static CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			INotifyCollectionChanged source,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new CollectionProcessing<TSourceItem, TReturnValue>(
				source: source,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: sourceScalar,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			Expression<Func<INotifyCollectionChanged>> getSourceExpression,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression),
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			INotifyCollectionChanged source,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new CollectionProcessingVoid<TSourceItem>(
				source: source,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new CollectionProcessingVoid<TSourceItem>(
				sourceScalar: sourceScalar,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			Expression<Func<INotifyCollectionChanged>> getSourceExpression,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new CollectionProcessingVoid<TSourceItem>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression),
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionDisposing<TSourceItem> CollectionDisposing<TSourceItem>(this
			INotifyCollectionChanged source) where TSourceItem : IDisposable

		{
			return new CollectionDisposing<TSourceItem>(
				source: source);
		}

		[ObservableComputationsCall]
		public static CollectionDisposing<TSourceItem> CollectionDisposing<TSourceItem>(this
			IReadScalar<INotifyCollectionChanged> sourceScalar) where TSourceItem : IDisposable

		{
			return new CollectionDisposing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static CollectionDisposing<TSourceItem> CollectionDisposing<TSourceItem>(this
			Expression<Func<INotifyCollectionChanged>> getSourceExpression) where TSourceItem : IDisposable

		{
			return new CollectionDisposing<TSourceItem>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression));
		}

		[ObservableComputationsCall]
		public static CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			ObservableCollection<TSourceItem> source,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new CollectionProcessing<TSourceItem, TReturnValue>(
				source: source,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: sourceScalar,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessing<TSourceItem, TReturnValue> CollectionProcessing<TSourceItem, TReturnValue>(this
			Expression<Func<ObservableCollection<TSourceItem>>> getSourceExpression,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
		{
			return new CollectionProcessing<TSourceItem, TReturnValue>(
				sourceScalar: new Computing<ObservableCollection<TSourceItem>>(getSourceExpression),
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			ObservableCollection<TSourceItem> source,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new CollectionProcessingVoid<TSourceItem>(
				source: source,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new CollectionProcessingVoid<TSourceItem>(
				sourceScalar: sourceScalar,
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionProcessingVoid<TSourceItem> CollectionProcessing<TSourceItem>(this
			Expression<Func<ObservableCollection<TSourceItem>>> getSourceExpression,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null)
			
		{
			return new CollectionProcessingVoid<TSourceItem>(
				sourceScalar: new Computing<ObservableCollection<TSourceItem>>(getSourceExpression),
				newItemsProcessor: newItemProcessor,
				oldItemsProcessor: oldItemProcessor,
				moveItemProcessor: moveItemProcessor);
		}

		[ObservableComputationsCall]
		public static CollectionDisposing<TSourceItem> CollectionDisposing<TSourceItem>(this
			ObservableCollection<TSourceItem> source) where TSourceItem : IDisposable

		{
			return new CollectionDisposing<TSourceItem>(
				source: source);
		}

		[ObservableComputationsCall]
		public static CollectionDisposing<TSourceItem> CollectionDisposing<TSourceItem>(this
			IReadScalar<ObservableCollection<TSourceItem>> sourceScalar) where TSourceItem : IDisposable

		{
			return new CollectionDisposing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static CollectionDisposing<TSourceItem> CollectionDisposing<TSourceItem>(this
			Expression<Func<ObservableCollection<TSourceItem>>> getSourceExpression) where TSourceItem : IDisposable

		{
			return new CollectionDisposing<TSourceItem>(
				sourceScalar: new Computing<ObservableCollection<TSourceItem>>(getSourceExpression));
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
				return $"({@object})";
			}
			catch (Exception exception)
			{
				return getExceptionString != null ? getExceptionString(exception) : "exception";
			}
		}

		private static string ToStringAlt(this PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs == null) return "null";
			return $"({propertyChangedEventArgs} (PropertyName = '{propertyChangedEventArgs.PropertyName}'))";
		}

		public static string ToStringAlt(this NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			if (notifyCollectionChangedEventArgs == null) return "null";

			switch (notifyCollectionChangedEventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					return $"({notifyCollectionChangedEventArgs} (Action = '{notifyCollectionChangedEventArgs.Action.ToString()}' notifyCollectionChangedEventArgs.NewItems[0] = '{notifyCollectionChangedEventArgs.NewItems[0].ToStringSafe(e => "e.ToString() in notifyCollectionChangedEventArgs.NewItems[0].ToString()")}', notifyCollectionChangedEventArgs.NewStartingIndex = {notifyCollectionChangedEventArgs.NewStartingIndex.ToString()}))";
				case NotifyCollectionChangedAction.Remove:
					return $"({notifyCollectionChangedEventArgs} (Action = '{notifyCollectionChangedEventArgs.Action.ToString()}' notifyCollectionChangedEventArgs.OldItems[0] = '{notifyCollectionChangedEventArgs.OldItems[0].ToStringSafe(e => "e.ToString() in notifyCollectionChangedEventArgs.OldItems[0].ToString()")}', notifyCollectionChangedEventArgs.OldStartingIndex = {notifyCollectionChangedEventArgs.OldStartingIndex.ToString()}))";
				case NotifyCollectionChangedAction.Replace:
					return $"({notifyCollectionChangedEventArgs} (Action = '{notifyCollectionChangedEventArgs.Action.ToString()}' notifyCollectionChangedEventArgs.NewItems[0] = '{notifyCollectionChangedEventArgs.NewItems[0].ToStringSafe(e => "e.ToString() in notifyCollectionChangedEventArgs.NewItems[0].ToString()")}', notifyCollectionChangedEventArgs.NewStartingIndex = {notifyCollectionChangedEventArgs.NewStartingIndex.ToString()}, notifyCollectionChangedEventArgs.OldItems[0] = '{notifyCollectionChangedEventArgs.OldItems[0].ToStringSafe(e => "e.ToString() in notifyCollectionChangedEventArgs.OldItems[0].ToString()")}', notifyCollectionChangedEventArgs.OldStartingIndex = {notifyCollectionChangedEventArgs.OldStartingIndex.ToString()}))";					
				case NotifyCollectionChangedAction.Move:
					return $"({notifyCollectionChangedEventArgs} (Action = '{notifyCollectionChangedEventArgs.Action.ToString()}' notifyCollectionChangedEventArgs.OldStartingIndex = {notifyCollectionChangedEventArgs.OldStartingIndex.ToString()}, notifyCollectionChangedEventArgs.NewStartingIndex = {notifyCollectionChangedEventArgs.NewStartingIndex.ToString()}))";					
				case NotifyCollectionChangedAction.Reset:
					return $"({notifyCollectionChangedEventArgs} (Action = '{notifyCollectionChangedEventArgs.Action.ToString()}'))";					
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static string ToStringAlt(this MethodChangedEventArgs methodChangedEventArgs)
		{
			if (methodChangedEventArgs == null) return "null";
			return $"({methodChangedEventArgs} (MethodName = '{methodChangedEventArgs.MethodName}', ArgumentsPredicate.GetHashCode() = {methodChangedEventArgs.ArgumentsPredicate.GetHashCode()}))";
		}

		public static string ToStringAlt(this EventArgs eventArgs)
		{
			if (eventArgs == null) return "null";

			if (eventArgs is PropertyChangedEventArgs propertyChangedEventArgs)
				return propertyChangedEventArgs.ToStringAlt();
			if  (eventArgs is NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
				return notifyCollectionChangedEventArgs.ToStringAlt();
			if  (eventArgs is MethodChangedEventArgs notifyMethodChangedEventArgs)
				return notifyMethodChangedEventArgs.ToStringAlt();

			return string.Empty;
		}

		[ObservableComputationsCall]
		public static TComputing For<TComputing>(this TComputing computing, OcConsumer ocConsumer)
			where TComputing : IComputing
		{
			((IComputingInternal) computing).AddConsumer(ocConsumer);
			return computing;
		}

		public static void Invoke(this IOcDispatcher ocDispatcher, Action action, int priority = 0, object parameter = null, object context = null)
		{
			ocDispatcher.Invoke(action, priority, parameter, context);
		}

		[ObservableComputationsCall]
		public static TComputing SetDebugTag<TComputing>(this TComputing computing, string debugTag)
			where TComputing : IHasTags
		{
			computing.DebugTag = debugTag;
			return computing;
		}
	}
}
