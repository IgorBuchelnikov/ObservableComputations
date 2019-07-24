 
 



using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public static partial class ExtensionMethods
	{
		#region Aggregating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult>(
				sourceScalar: sourceScalar,
				funcs: funcs);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult>(
				sourceScalar: sourceScalar,
				funcs: funcs);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult>(
				source: source,
				funcs: funcs);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult> Aggregating<TSourceItem, TResult>(this
			 ObservableCollection<TSourceItem> source,
			 (System.Func<TSourceItem, TResult, TResult> aggregateFunc, System.Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			return new IBCode.ObservableCalculations.Aggregating<TSourceItem, TResult>(
				source: source,
				funcs: funcs);
		}

		#endregion
		#region AllCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AllCalculating<TSourceItem> AllCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AllCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AllCalculating<TSourceItem> AllCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AllCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AllCalculating<TSourceItem> AllCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AllCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AllCalculating<TSourceItem> AllCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AllCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		#endregion
		#region AnyCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AnyCalculating<TSourceItem> AnyCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AnyCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AnyCalculating<TSourceItem> AnyCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AnyCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AnyCalculating<TSourceItem> AnyCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AnyCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.AnyCalculating<TSourceItem> AnyCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.AnyCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression);
		}

		#endregion
		#region Appending

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Appending<TSourceItem> Appending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Appending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		#endregion
		#region Averaging

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Averaging<TSourceItem, TResult>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Averaging<TSourceItem, TResult>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.Averaging<TSourceItem, TResult>(
				source: source);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Averaging<TSourceItem, TResult> Averaging<TSourceItem, TResult>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.Averaging<TSourceItem, TResult>(
				source: source);
		}

		#endregion
		#region Calculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Calculating<TResult> Calculating<TResult>(this
			 System.Linq.Expressions.Expression<System.Func<TResult>> getValueExpression)
		{
			return new IBCode.ObservableCalculations.Calculating<TResult>(
				getValueExpression: getValueExpression);
		}

		#endregion
		#region Casting

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Casting<TResultItem> Casting<TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Casting<TResultItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Casting<TResultItem> Casting<TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.Casting<TResultItem>(
				source: source);
		}

		#endregion
		#region Concatenating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				sourcesScalar: sourcesScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				sourcesScalar: sourcesScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				sources: sources);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				sources: sources);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Concatenating<TSourceItem> Concatenating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Concatenating<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar);
		}

		#endregion
		#region ContainsCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				item: item,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				itemScalar: itemScalar,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				item: item,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ContainsCalculating<TSourceItem> ContainsCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.ContainsCalculating<TSourceItem>(
				source: source,
				item: item,
				equalityComparer: equalityComparer);
		}

		#endregion
		#region Crossing

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem> Crossing<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar)
		{
			return new IBCode.ObservableCalculations.Crossing<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar);
		}

		#endregion
		#region Dictionaring

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue> Dictionaring<TSourceItem, TKey, TValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TValue>> valueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Dictionaring<TSourceItem, TKey, TValue>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				valueSelectorExpression: valueSelectorExpression,
				comparer: null,
				capacity: 0);
		}

		#endregion
		#region Distincting

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				source: source,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Distincting<TSourceItem> Distincting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Distincting<TSourceItem>(
				sourceScalar: sourceScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region Excepting

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Excepting<TSourceItem> Excepting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Excepting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region Filtering

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Filtering<TSourceItem> Filtering<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.Filtering<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region FirstCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.FirstCalculating<TSourceItem> FirstCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.FirstCalculating<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		#endregion
		#region Grouping

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Grouping<TSourceItem, TKey> Grouping<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Grouping<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region GroupJoining

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TKey>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: equalityComparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSource: innerSource,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TKey>> outerKeySelector,
			 System.Linq.Expressions.Expression<System.Func<TInnerSourceItem, TKey>> innerKeySelector,
			 System.Collections.Generic.IEqualityComparer<TKey> equalityComparer)
		{
			return new IBCode.ObservableCalculations.GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				outerKeySelector: outerKeySelector,
				innerKeySelector: innerKeySelector,
				equalityComparer: equalityComparer);
		}

		#endregion
		#region Hashing

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				sourceScalar: sourceScalar,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: comparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Hashing<TSourceItem, TKey> Hashing<TSourceItem, TKey>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TKey>> keySelectorExpression)
		{
			return new IBCode.ObservableCalculations.Hashing<TSourceItem, TKey>(
				source: source,
				keySelectorExpression: keySelectorExpression,
				comparer: null,
				capacity: 0);
		}

		#endregion
		#region IndicesCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.IndicesCalculating<TSourceItem> IndicesCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.IndicesCalculating<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region Intersecting

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Intersecting<TSourceItem> Intersecting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Intersecting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		#endregion
		#region ItemCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int index,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int index)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int index,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int index)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int index,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int index)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int index,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int index)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				index: index,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int index,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				index: index,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ItemCalculating<TSourceItem> ItemCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> indexScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.ItemCalculating<TSourceItem>(
				source: source,
				indexScalar: indexScalar,
				defaultValue: defaultValue);
		}

		#endregion
		#region Joining

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem> Joining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.Joining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression,
				capacity: 0);
		}

		#endregion
		#region LastCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				source: source,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				sourceScalar: sourceScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.LastCalculating<TSourceItem> LastCalculating<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.LastCalculating<TSourceItem>(
				source: source,
				defaultValue: defaultValue);
		}

		#endregion
		#region Maximazing

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Maximazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Maximazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		#endregion
		#region Minimazing

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				sourceScalar: sourceScalar,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: null,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> defaultValueScalar)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: defaultValueScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValueScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Collections.Generic.IComparer<TSourceItem> comparer,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: comparer,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparer: null,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem> Minimazing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TSourceItem>> comparerScalar,
			 TSourceItem defaultValue)
		{
			return new IBCode.ObservableCalculations.MinimazingOrMaximazing<TSourceItem>(
				source: source,
				mode: MinimazingOrMaximazingMode.Minimazing,
				comparerScalar: comparerScalar,
				defaultValue: defaultValue);
		}

		#endregion
		#region OfTypeCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.OfTypeCalculating<TResultItem> OfTypeCalculating<TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.OfTypeCalculating<TResultItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.OfTypeCalculating<TResultItem> OfTypeCalculating<TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.OfTypeCalculating<TResultItem>(
				source: source);
		}

		#endregion
		#region Ordering

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue> Ordering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.Ordering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer);
		}

		#endregion
		#region PredicateGroupJoining

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TOuterSourceItem>> outerSourceScalar,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSourceScalar: outerSourceScalar,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TInnerSourceItem>> innerSourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSourceScalar: innerSourceScalar,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 System.Collections.Specialized.INotifyCollectionChanged innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(this
			 ObservableCollection<TOuterSourceItem> outerSource,
			 ObservableCollection<TInnerSourceItem> innerSource,
			 System.Linq.Expressions.Expression<System.Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			return new IBCode.ObservableCalculations.PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem>(
				outerSource: outerSource,
				innerSource: innerSource,
				joinPredicateExpression: joinPredicateExpression);
		}

		#endregion
		#region Prepending

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				itemScalar: itemScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<TSourceItem> itemScalar)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				source: source,
				itemScalar: itemScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				source: source,
				item: item);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Prepending<TSourceItem> Prepending<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 TSourceItem item)
		{
			return new IBCode.ObservableCalculations.Prepending<TSourceItem>(
				sourceScalar: sourceScalar,
				item: item);
		}

		#endregion
		#region Reversing

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Reversing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Reversing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.Reversing<TSourceItem>(
				source: source);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Reversing<TSourceItem> Reversing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.Reversing<TSourceItem>(
				source: source);
		}

		#endregion
		#region Selecting

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.Selecting<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		#endregion
		#region SelectingMany

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem> SelectingMany<TSourceItem, TResultItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, System.Collections.Specialized.INotifyCollectionChanged>> selectorExpression)
		{
			return new IBCode.ObservableCalculations.SelectingMany<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		#endregion
		#region SequenceCalculating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SequenceCalculating SequenceCalculating(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.SequenceCalculating(
				countScalar: countScalar);
		}

		#endregion
		#region Skipping

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int count)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int count)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				sourceScalar: sourceScalar,
				count: count,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int count)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int count,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Skipping<TSourceItem> Skipping<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int count)
		{
			return new IBCode.ObservableCalculations.Skipping<TSourceItem>(
				source: source,
				count: count,
				capacity: 0);
		}

		#endregion
		#region SkippingWhile

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.SkippingWhile<TSourceItem> SkippingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.SkippingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region StringsConcatenating

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.StringsConcatenating StringsConcatenating(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<string> separatorScalar)
		{
			return new IBCode.ObservableCalculations.StringsConcatenating(
				sourceScalar: sourceScalar,
				separatorScalar: separatorScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.StringsConcatenating StringsConcatenating(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.StringsConcatenating(
				sourceScalar: sourceScalar,
				separatorScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.StringsConcatenating StringsConcatenating(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<string> separatorScalar)
		{
			return new IBCode.ObservableCalculations.StringsConcatenating(
				source: source,
				separatorScalar: separatorScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.StringsConcatenating StringsConcatenating(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.StringsConcatenating(
				source: source,
				separatorScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.StringsConcatenating StringsConcatenating(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 string separator)
		{
			return new IBCode.ObservableCalculations.StringsConcatenating(
				source: source,
				separator: separator);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.StringsConcatenating StringsConcatenating(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 string separator)
		{
			return new IBCode.ObservableCalculations.StringsConcatenating(
				sourceScalar: sourceScalar,
				separator: separator);
		}

		#endregion
		#region Summarizing

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Summarizing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar)
		{
			return new IBCode.ObservableCalculations.Summarizing<TSourceItem>(
				sourceScalar: sourceScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source)
		{
			return new IBCode.ObservableCalculations.Summarizing<TSourceItem>(
				source: source);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Summarizing<TSourceItem> Summarizing<TSourceItem>(this
			 ObservableCollection<TSourceItem> source)
		{
			return new IBCode.ObservableCalculations.Summarizing<TSourceItem>(
				source: source);
		}

		#endregion
		#region Taking

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				count: count);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				sourceScalar: sourceScalar,
				startIndex: startIndex,
				count: count);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> startIndexScalar,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndexScalar: startIndexScalar,
				count: count);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int startIndex,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<int> countScalar)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				countScalar: countScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				count: count);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Taking<TSourceItem> Taking<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 int startIndex,
			 int count)
		{
			return new IBCode.ObservableCalculations.Taking<TSourceItem>(
				source: source,
				startIndex: startIndex,
				count: count);
		}

		#endregion
		#region TakingWhile

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, int, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				sourceScalar: sourceScalar,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.TakingWhile<TSourceItem> TakingWhile<TSourceItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, bool>> predicateExpression)
		{
			return new IBCode.ObservableCalculations.TakingWhile<TSourceItem>(
				source: source,
				predicateExpression: predicateExpression,
				capacity: 0);
		}

		#endregion
		#region ThenOrdering

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: ListSortDirection.Ascending,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IComparer<TOrderingValue>> comparerScalar)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: comparerScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparerScalar: null);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: null,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.ComponentModel.ListSortDirection> sortDirectionScalar,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				source: source,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirectionScalar: sortDirectionScalar,
				comparer: comparer);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue> ThenOrdering<TSourceItem, TOrderingValue>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<IBCode.ObservableCalculations.Common.Interface.IOrdering<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			 System.ComponentModel.ListSortDirection sortDirection,
			 System.Collections.Generic.IComparer<TOrderingValue> comparer)
		{
			return new IBCode.ObservableCalculations.ThenOrdering<TSourceItem, TOrderingValue>(
				sourceScalar: sourceScalar,
				orderingValueSelectorExpression: orderingValueSelectorExpression,
				sortDirection: sortDirection,
				comparer: comparer);
		}

		#endregion
		#region Uniting

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparerScalar: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<ObservableCollection<TSourceItem>> sources,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sources: sources,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<ObservableCollection<TSourceItem>>> sourcesScalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				sourcesScalar: sourcesScalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 System.Collections.Generic.IEqualityComparer<TSourceItem> equalityComparer)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: equalityComparer,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparer: null,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 System.Collections.Specialized.INotifyCollectionChanged source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 ObservableCollection<TSourceItem> source2,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2: source2,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source1Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1Scalar: source1Scalar,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar,
			 int capacity)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: capacity);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Uniting<TSourceItem> Uniting<TSourceItem>(this
			 ObservableCollection<TSourceItem> source1,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItem>> source2Scalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Generic.IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return new IBCode.ObservableCalculations.Uniting<TSourceItem>(
				source1: source1,
				source2Scalar: source2Scalar,
				equalityComparerScalar: equalityComparerScalar,
				capacity: 0);
		}

		#endregion
		#region Using

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Using<TArgument, TResult> Using<TArgument, TResult>(this
			 TArgument argument,
			 System.Linq.Expressions.Expression<System.Func<TArgument, TResult>> getValueExpression)
		{
			return new IBCode.ObservableCalculations.Using<TArgument, TResult>(
				argument: argument,
				getValueExpression: getValueExpression);
		}

		#endregion
		#region Zipping

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceLeftScalar,
			 System.Collections.Specialized.INotifyCollectionChanged sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceLeftScalar,
			 ObservableCollection<TSourceItemRight> sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemLeft>> sourceLeftScalar,
			 System.Collections.Specialized.INotifyCollectionChanged sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemLeft>> sourceLeftScalar,
			 ObservableCollection<TSourceItemRight> sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceLeftScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRightScalar: sourceRightScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceLeftScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemRight>> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRightScalar: sourceRightScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemLeft>> sourceLeftScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRightScalar: sourceRightScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemLeft>> sourceLeftScalar,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemRight>> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeftScalar: sourceLeftScalar,
				sourceRightScalar: sourceRightScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 System.Collections.Specialized.INotifyCollectionChanged sourceLeft,
			 System.Collections.Specialized.INotifyCollectionChanged sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 System.Collections.Specialized.INotifyCollectionChanged sourceLeft,
			 ObservableCollection<TSourceItemRight> sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 ObservableCollection<TSourceItemLeft> sourceLeft,
			 System.Collections.Specialized.INotifyCollectionChanged sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 ObservableCollection<TSourceItemLeft> sourceLeft,
			 ObservableCollection<TSourceItemRight> sourceRight)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRight: sourceRight);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 System.Collections.Specialized.INotifyCollectionChanged sourceLeft,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRightScalar: sourceRightScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 System.Collections.Specialized.INotifyCollectionChanged sourceLeft,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemRight>> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRightScalar: sourceRightScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 ObservableCollection<TSourceItemLeft> sourceLeft,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRightScalar: sourceRightScalar);
		}

		[ObservableCalculationsCall]
		public static IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight> Zipping<TSourceItemLeft, TSourceItemRight>(this
			 ObservableCollection<TSourceItemLeft> sourceLeft,
			 IBCode.ObservableCalculations.Common.Interface.IReadScalar<ObservableCollection<TSourceItemRight>> sourceRightScalar)
		{
			return new IBCode.ObservableCalculations.Zipping<TSourceItemLeft, TSourceItemRight>(
				sourceLeft: sourceLeft,
				sourceRightScalar: sourceRightScalar);
		}

		#endregion
	}
}
