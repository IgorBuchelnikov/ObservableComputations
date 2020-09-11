using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservableComputations
{
    internal class CollectionChangedEventRaise : IProcessable
    {
        internal object Sender;
        internal NotifyCollectionChangedEventArgs Args;
        internal ISourceCollectionChangeProcessor SourceCollectionChangeProcessor;

        public CollectionChangedEventRaise(object sender, NotifyCollectionChangedEventArgs args, ISourceCollectionChangeProcessor sourceCollectionChangeProcessor)
        {
            Sender = sender;
            Args = args;
            SourceCollectionChangeProcessor = sourceCollectionChangeProcessor;

        }

        #region Implementation of IProcessable

        public void Process(Queue<IProcessable>[] deferredProcessings)
        {
            SourceCollectionChangeProcessor.processSourceCollectionChanged(Sender, Args);
        }

        #endregion
    }
}
