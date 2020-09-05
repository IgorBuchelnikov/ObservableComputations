using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Process()
        {
            SourceCollectionChangeProcessor.processSourceCollectionChanged(Sender, Args);
        }

        #endregion
    }
}
