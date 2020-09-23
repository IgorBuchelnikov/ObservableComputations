using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservableComputations
{
    internal class CollectionReset : IProcessable
    {
        private object _eventSender;
        private EventArgs _eventArgs;
        internal Action Action;
        internal ICanInitializeFromSource CanInitializeFromSource;

        public CollectionReset(object eventSender, EventArgs eventArgs, ICanInitializeFromSource canInitializeFromSource, Action action)
        {
            _eventSender = eventSender;
            _eventArgs = eventArgs;
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

        public object EventSender => _eventSender;
        public EventArgs EventArgs => _eventArgs;
    }
}

