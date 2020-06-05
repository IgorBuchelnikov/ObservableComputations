using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	internal static class Utils
	{
		internal static TScalarType getValue<TScalarType>(this IReadScalar<TScalarType> scalar, TScalarType defaultValue)
		{
			return scalar != null ? scalar.Value : defaultValue;
		}

		internal static TScalarType getValue<TScalarType>(this IReadScalar<TScalarType> scalar, TScalarType defaultValue, TScalarType defaultDefaultValue)
		{
			return scalar != null 
				? scalar.Value == null
					? defaultDefaultValue
					: scalar.Value
				: defaultValue == null
					? defaultDefaultValue
					: defaultValue;
		}

		internal static Expression<Func<TSourceItem, int, bool>> getIndexedPredicate<TSourceItem>(this Expression<Func<TSourceItem, bool>> predicate)
		{
			Expression<Func<TSourceItem, int, bool>> predicate1 =
				Expression.Lambda<Func<TSourceItem, int, bool>>(predicate.Body,
					new[] {predicate.Parameters[0], Expression.Parameter(typeof(int), "index")});
			return predicate1;
		}

		
		internal static int getCapacity(INotifyCollectionChanged source)
		{
			//return 0;
			return source is IHasCapacity sourceCapacity ? sourceCapacity.Capacity : ((IList) source)?.Count ?? 0;
		}

		
		internal static int getCapacity(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			//return 0;
			return sourceScalar.Value is IHasCapacity sourceCapacity ? sourceCapacity.Capacity : ((IList) sourceScalar.Value)?.Count ?? 0;
		}

		internal static readonly PropertyChangedEventArgs InsertItemIntoGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("InsertItemIntoGroupAction");
		internal static readonly PropertyChangedEventArgs RemoveItemFromGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemFromGroupAction");
		internal static readonly PropertyChangedEventArgs SetGroupItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("SetGroupItemAction");
		internal static readonly PropertyChangedEventArgs MoveItemInGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("MoveItemInGroupAction");
		internal static readonly PropertyChangedEventArgs ClearGroupItemsActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ClearGroupItemsAction");
		internal static readonly PropertyChangedEventArgs OuterItemPropertyChangedEventArgs = new PropertyChangedEventArgs("OuterItem");
		internal static readonly PropertyChangedEventArgs KeyPropertyChangedEventArgs = new PropertyChangedEventArgs("Key");
		internal static readonly PropertyChangedEventArgs JoinPairSetOuterItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("JoinPairSetOuterItemAction");
		internal static readonly PropertyChangedEventArgs JoinPairSetInnerItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("JoinPairSetInnerItemAction");
		internal static readonly PropertyChangedEventArgs InnerItemPropertyChangedEventArgs = new PropertyChangedEventArgs("InnerItem");
		internal static readonly PropertyChangedEventArgs ZipPairSetLeftItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ZipPairSetLeftItemAction");
		internal static readonly PropertyChangedEventArgs ZipPairSetRightItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ZipPairSetRightItemAction");
		internal static readonly PropertyChangedEventArgs ItemLeftPropertyChangedEventArgs = new PropertyChangedEventArgs("ItemLeft");
		internal static readonly PropertyChangedEventArgs ItemRightPropertyChangedEventArgs = new PropertyChangedEventArgs("ItemRight");
		internal static readonly PropertyChangedEventArgs InsertItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("InsertItemAction");
		internal static readonly PropertyChangedEventArgs AddItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("AddItemAction");
		internal static readonly PropertyChangedEventArgs RemoveItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemAction");
		internal static readonly PropertyChangedEventArgs RemoveItemFuncPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemFunc");
		internal static readonly PropertyChangedEventArgs SetItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("SetItemAction");
		internal static readonly PropertyChangedEventArgs MoveItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("MoveItemAction");
		internal static readonly PropertyChangedEventArgs ClearItemsActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ClearItemsAction");
		internal static readonly PropertyChangedEventArgs ValuePropertyChangedEventArgs = new PropertyChangedEventArgs("Value");
		internal static readonly PropertyChangedEventArgs ResultPropertyChangedEventArgs = new PropertyChangedEventArgs("Result");
		internal static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs("Count");
		internal static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
		internal static readonly PropertyChangedEventArgs ValueObjectPropertyChangedEventArgs = new PropertyChangedEventArgs("ValueObject");
		internal static readonly PropertyChangedEventArgs SetValueActionPropertyChangedEventArgs = new PropertyChangedEventArgs("SetValueAction");
		internal static readonly PropertyChangedEventArgs IsDisabledPropertyChangedEventArgs = new PropertyChangedEventArgs("IsDisabled");
		internal static readonly PropertyChangedEventArgs PreviousValuePropertyChangedEventArgs = new PropertyChangedEventArgs("PreviousValue");
		internal static readonly PropertyChangedEventArgs IsEverChangedPropertyChangedEventArgs = new PropertyChangedEventArgs("IsEverChanged");
		internal static readonly PropertyChangedEventArgs CurrentPagePropertyChangedEventArgs = new PropertyChangedEventArgs("CurrentPage");
		internal static readonly PropertyChangedEventArgs PageCountPropertyChangedEventArgs = new PropertyChangedEventArgs("PageCount");
		internal static readonly PropertyChangedEventArgs PageSizePropertyChangedEventArgs = new PropertyChangedEventArgs("PageSize");
		internal static readonly PropertyChangedEventArgs PausedPropertyChangedEventArgs = new PropertyChangedEventArgs("Paused");

	}
}
