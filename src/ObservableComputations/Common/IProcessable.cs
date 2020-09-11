using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations
{
    internal  interface IProcessable
    {
        void Process(Queue<IProcessable>[] deferredProcessings);
    }
}
