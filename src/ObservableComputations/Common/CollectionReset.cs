using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservableComputations
{
    internal class CollectionReset : IProcessable
    {
        internal object Sender;
        internal EventArgs Args;
        internal Action Action;
        internal ICanInitializeFromSource CanInitializeFromSource;

        public CollectionReset(object sender, EventArgs args, ICanInitializeFromSource canInitializeFromSource, Action action)
        {
            Sender = sender;
            Args = args;
            CanInitializeFromSource = canInitializeFromSource;
            Action = action;
        }

        #region Implementation of IProcessable
        public void Process(Queue<IProcessable>[] deferredProcessings)
        {
            Utils.ClearDefferedProcessings(deferredProcessings);

            Action?.Invoke();
            CanInitializeFromSource.InitializeFromSource();
        }

        #endregion
    }
}

