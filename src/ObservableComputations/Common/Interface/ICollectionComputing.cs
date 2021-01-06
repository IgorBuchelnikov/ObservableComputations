using System.Collections;

namespace ObservableComputations
{
	public interface ICollectionComputing : INotifyCollectionChangedExtended, IList, IComputing, IHasItemType, IHasInitialCapacity
	{

	}

	public interface ICollectionComputingChild : INotifyCollectionChangedExtended, IList, IComputing, IHasItemType
	{
		ICollectionComputing Parent {get;}
	}
}
