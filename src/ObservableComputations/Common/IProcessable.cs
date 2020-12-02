using System;
using System.Collections.Generic;

namespace ObservableComputations
{
    internal interface IProcessable
    {
        void Process(Queue<IProcessable>[] deferredProcessings);
        object EventSender { get; }
        EventArgs EventArgs { get; }
    }

}
