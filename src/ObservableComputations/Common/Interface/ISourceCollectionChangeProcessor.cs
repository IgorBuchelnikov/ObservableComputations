using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations
{

    internal interface ISourceCollectionChangeProcessor
    {
        void processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }

}
