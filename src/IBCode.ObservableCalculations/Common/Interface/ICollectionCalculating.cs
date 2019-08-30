using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface ICollectionCalculating : INotifyCollectionChangedExtended, IList, ICalculating, IHasItemType
	{
	}

	public interface ICollectionCalculatingChild : INotifyCollectionChangedExtended, IList, IHasTags, IHasItemType
	{
		ICollectionCalculating Parent {get;}
	}
}
