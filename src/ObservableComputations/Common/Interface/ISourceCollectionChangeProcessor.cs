using System.Collections.Specialized;

namespace ObservableComputations
{

    internal interface ISourceCollectionChangeProcessor : ICanInitializeFromSource
    {
        void processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }

}
