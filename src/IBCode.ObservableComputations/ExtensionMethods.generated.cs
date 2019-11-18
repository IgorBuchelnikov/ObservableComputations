 
 



using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using IBCode.ObservableComputations.Common;
using IBCode.ObservableComputations.Common.Interface;

namespace IBCode.ObservableComputations
{
	public static partial class ExtensionMethods
	{
		#region Aggregating

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableComputations.Aggregating<TSourceItem, TResult>(
				sourceScalar: sourceScalar,
				funcs: funcs);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableComputations.Aggregating<TSourceItem, TResult>(
				sourceScalar: sourceScalar,
				funcs: funcs);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableComputations.Aggregating<TSourceItem, TResult>(
				source: source,
				funcs: funcs);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 ObservableCollection<TSourceItem> source,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableComputations.Aggregating<TSourceItem, TResult>(
				source: source,
				funcs: funcs);
		}

		#endregion
		#region AllComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AllComputing<TSourceItem> AllComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AllComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AllComputing<TSourceItem> AllComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AllComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AllComputing<TSourceItem> AllComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AllComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AllComputing<TSourceItem> AllComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AllComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		#endregion
		#region AnyComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AnyComputing<TSourceItem> AnyComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AnyComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AnyComputing<TSourceItem> AnyComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AnyComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AnyComputing<TSourceItem> AnyComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AnyComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.AnyComputing<TSourceItem> AnyComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.AnyComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		#endregion
		#region Appending

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		#endregion
		#region Averaging

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.Averaging<TSourceItem, TResult>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.Averaging<TSourceItem, TResult>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.Averaging<TSourceItem, TResult>(
				source: source);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.Averaging<TSourceItem, TResult>(
				source: source);
		}

		#endregion
		#region Computing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Computing<TResult> Computing<TResult>(this
			 System.Linq.Expressions.Expression<System.Func<TResult>> getValueExpression)
		{
			return new IBCode.ObservableComputations.Computing<TResult>(
				getValueExpression: getValueExpression);
		}

		#endregion
		#region Casting

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Casting<TResultItem> Casting<TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.Casting<TResultItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Casting<TResultItem> Casting<TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.Casting<TResultItem>(
				source: source);
		}

		#endregion
		#region Concatenating

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				sourcesScalar: sourcesScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				sourcesScalar: sourcesScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				sources: sources);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				sources: sources);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		#endregion
		#region ContainsComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				item: item,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ContainsComputing<TSourceItem> ContainsComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.ContainsComputing<TSourceItem>(
				source: source,
				item: item,
				equalityComparer: equalityComparer);
		}

		#endregion
		#region Crossing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableComputations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		#endregion
		#region Dictionaring

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		#endregion
		#region Distincting

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region Excepting

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region Filtering

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region FirstComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.FirstComputing<TSourceItem> FirstComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.FirstComputing<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		#endregion
		#region Grouping

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region GroupJoining

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableComputations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		#endregion
		#region Hashing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableComputations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		#endregion
		#region IndicesComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.IndicesComputing<TSourceItem> IndicesComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.IndicesComputing<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region Intersecting

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region ItemComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int index,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int index)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int index,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int index)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int index,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int index)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int index,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int index)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ItemComputing<TSourceItem> ItemComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.ItemComputing<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		#endregion
		#region Joining

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		#endregion
		#region LastComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.LastComputing<TSourceItem> LastComputing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.LastComputing<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		#endregion
		#region Maximazing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		#endregion
		#region Minimazing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableComputations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		#endregion
		#region OfTypeComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.OfTypeComputing<TResultItem> OfTypeComputing<TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.OfTypeComputing<TResultItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.OfTypeComputing<TResultItem> OfTypeComputing<TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.OfTypeComputing<TResultItem>(
				source: source);
		}

		#endregion
		#region Ordering

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		#endregion
		#region PredicateGroupJoining

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableComputations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		#endregion
		#region Prepending

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableComputations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		#endregion
		#region Reversing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.Reversing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.Reversing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.Reversing<TSourceItem>(
				source: source);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.Reversing<TSourceItem>(
				source: source);
		}

		#endregion
		#region Selecting

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableComputations.Selecting<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		#endregion
		#region SelectingMany

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableComputations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		#endregion
		#region SequenceComputing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SequenceComputing SequenceComputing(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.SequenceComputing(
				countScalar: countScalar);
		}

		#endregion
		#region Skipping

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int count)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int count)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int count)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int count)
		{
			return new IBCode.ObservableComputations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: 0);
		}

		#endregion
		#region SkippingWhile

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region StringsConcatenating

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.StringsConcatenating StringsConcatenating(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<string> separatorScalar)
		{
			return new IBCode.ObservableComputations.StringsConcatenating(
				sourceScalar: sourceScalar,
				separatorScalar: separatorScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.StringsConcatenating StringsConcatenating(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.StringsConcatenating(
				sourceScalar: sourceScalar,
				separatorScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.StringsConcatenating StringsConcatenating(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<string> separatorScalar)
		{
			return new IBCode.ObservableComputations.StringsConcatenating(
				source: source,
				separatorScalar: separatorScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.StringsConcatenating StringsConcatenating(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.StringsConcatenating(
				source: source,
				separatorScalar: null);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.StringsConcatenating StringsConcatenating(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 string separator)
		{
			return new IBCode.ObservableComputations.StringsConcatenating(
				source: source,
				separator: separator);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.StringsConcatenating StringsConcatenating(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 string separator)
		{
			return new IBCode.ObservableComputations.StringsConcatenating(
				sourceScalar: sourceScalar,
				separator: separator);
		}

		#endregion
		#region Summarizing

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableComputations.Summarizing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableComputations.Summarizing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableComputations.Summarizing<TSourceItem>(
				source: source);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableComputations.Summarizing<TSourceItem>(
				source: source);
		}

		#endregion
		#region Taking

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				count: count);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				count: count);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int startIndex,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				count: count);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableComputations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				count: count);
		}

		#endregion
		#region TakingWhile

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableComputations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region ThenOrdering

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer,
			 int maxTogetherThenOrderings)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: maxTogetherThenOrderings);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<IBCode.ObservableComputations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableComputations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer,
				maxTogetherThenOrderings: 4);
		}

		#endregion
		#region Uniting

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableComputations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		#endregion
		#region Using

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Using<TArgument, TResult> Using<TArgument, TResult>(this
			 TArgument argument,
			 System.Linq.Expressions.Expression<System.Func<TArgument, TResult>> getValueExpression)
		{
			return new IBCode.ObservableComputations.Using<TArgument, TResult>(
				argument: argument,
				getValueExpression: getValueExpression);
		}

		#endregion
		#region Zipping

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> leftSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> leftSourceScalar,
			 ObservableCollection<TRightSourceItem> rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TLeftSourceItem>> leftSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TLeftSourceItem>> leftSourceScalar,
			 ObservableCollection<TRightSourceItem> rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> leftSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSourceScalar: rightSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> leftSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TRightSourceItem>> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSourceScalar: rightSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TLeftSourceItem>> leftSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSourceScalar: rightSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TLeftSourceItem>> leftSourceScalar,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TRightSourceItem>> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSourceScalar: leftSourceScalar,
				rightSourceScalar: rightSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged leftSource,
			 System.Collections.Specialized.INotifyCollectionChanged rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged leftSource,
			 ObservableCollection<TRightSourceItem> rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 ObservableCollection<TLeftSourceItem> leftSource,
			 System.Collections.Specialized.INotifyCollectionChanged rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 ObservableCollection<TLeftSourceItem> leftSource,
			 ObservableCollection<TRightSourceItem> rightSource)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSource: rightSource);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged leftSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSourceScalar: rightSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged leftSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TRightSourceItem>> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSourceScalar: rightSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 ObservableCollection<TLeftSourceItem> leftSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSourceScalar: rightSourceScalar);
		}

		[ObservableComputationsCall]
		public static IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem> Zipping<TLeftSourceItem, TRightSourceItem>(this
			 ObservableCollection<TLeftSourceItem> leftSource,
			 IBCode.ObservableComputations.Common.Interface.IReadScalar<ObservableCollection<TRightSourceItem>> rightSourceScalar)
		{
			return new IBCode.ObservableComputations.Zipping<TLeftSourceItem, TRightSourceItem>(
				leftSource: leftSource,
				rightSourceScalar: rightSourceScalar);
		}

		#endregion
	}
}
