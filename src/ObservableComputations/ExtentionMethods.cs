using System;
using System.Collections.Specialized;
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
			System.Func<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, object, EventArgs, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, TReturnValue, object, EventArgs> oldItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, TReturnValue, object, EventArgs> moveItemProcessor = null)
			
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
			System.Func<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, object, EventArgs, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, TReturnValue, object, EventArgs> oldItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, TReturnValue, object, EventArgs> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue>(
				sourceScalar: sourceScalar,
				newItemProcessor: newItemProcessor,
				oldItemProcessor: oldItemProcessor);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.ItemsProcessing<TSourceItem, TReturnValue> ItemsProcessing<TSourceItem, TReturnValue>(this
			Expression<Func<System.Collections.Specialized.INotifyCollectionChanged>> getSourceExpression,
			System.Func<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, object, EventArgs, TReturnValue> newItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, TReturnValue, object, EventArgs> oldItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessing<TSourceItem, TReturnValue>, TReturnValue, object, EventArgs> moveItemProcessor = null)
			
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
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> newItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> oldItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> moveItemProcessor = null)
			
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
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> newItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> oldItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> moveItemProcessor = null)
			
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
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> newItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> oldItemProcessor = null,
			System.Action<TSourceItem, ItemsProcessingVoid<TSourceItem>, object, EventArgs> moveItemProcessor = null)
			
		{
			return new ObservableComputations.ItemsProcessingVoid<TSourceItem>(
				sourceScalar: new Computing<INotifyCollectionChanged>(getSourceExpression),
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
	}
}
