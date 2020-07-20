 
 
 



using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using ObservableComputations;

namespace ObservableComputations
{
	public static partial class ExtensionMethods
	{
		#region Selecting

		[ObservableComputationsCall]
		public static ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 ObservableComputations.IReadScalar<System.Collections.Specialized.INotifyCollectionChanged> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
			
		{
			return new ObservableComputations.Selecting<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 ObservableComputations.IReadScalar<ObservableCollection<TSourceItem>> sourceScalar,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
			
		{
			return new ObservableComputations.Selecting<TSourceItem, TResultItem>(
				sourceScalar: sourceScalar,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 System.Collections.Specialized.INotifyCollectionChanged source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
			
		{
			return new ObservableComputations.Selecting<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.Selecting<TSourceItem, TResultItem> Selecting<TSourceItem, TResultItem>(this
			 ObservableCollection<TSourceItem> source,
			 System.Linq.Expressions.Expression<System.Func<TSourceItem, TResultItem>> selectorExpression)
			
		{
			return new ObservableComputations.Selecting<TSourceItem, TResultItem>(
				source: source,
				selectorExpression: selectorExpression);
		}

		#endregion
	}
}
