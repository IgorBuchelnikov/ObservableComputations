namespace ObservableComputations
{
    internal interface ISourceItemChangeProcessor : IComputingInternal
    {
        void ProcessSourceItemChange(ExpressionWatcher expressionWatcher);
    }

    internal interface ISourceItemKeyChangeProcessor : ISourceItemChangeProcessor
    {
        void ProcessSourceItemChange(ExpressionWatcher expressionWatcher);
    }

    internal interface ISourceItemValueChangeProcessor : ISourceItemChangeProcessor
    {
        void ProcessSourceItemChange(ExpressionWatcher expressionWatcher);
    }
}
