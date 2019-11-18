using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace IBCode.ObservableComputations.Common.Interface
{
	public interface ICollectionComputing : INotifyCollectionChangedExtended, IList, IComputing, IHasItemType
	{
	}

	public interface ICollectionComputingChild : INotifyCollectionChangedExtended, IList, IHasTags, IHasItemType
	{
		ICollectionComputing Parent {get;}
	}
}
