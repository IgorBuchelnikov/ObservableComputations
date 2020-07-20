using System.Collections.Generic;

namespace ObservableComputations
{
    internal class ExpressionItemInfo : Position
    {
        public ExpressionWatcher ExpressionWatcher;
        public List<IComputingInternal> NestedComputings;
    }
}
