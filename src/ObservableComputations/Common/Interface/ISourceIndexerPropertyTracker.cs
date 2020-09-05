using System.ComponentModel;

namespace ObservableComputations
{
    internal interface ISourceIndexerPropertyTracker
    {
        void HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs);
    }

    internal interface ILeftSourceIndexerPropertyTracker : ISourceIndexerPropertyTracker
    {
        void HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs);
    }

    internal interface IRightSourceIndexerPropertyTracker : ISourceIndexerPropertyTracker
    {
        void HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs);
    }


}
