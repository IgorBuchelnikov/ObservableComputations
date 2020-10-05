using System;
using System.ComponentModel;

namespace ObservableComputations
{
	public class SequenceComputing : CollectionComputing<int>
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> CountScalar => _countScalar;

		int _count;
		private readonly IReadScalar<int> _countScalar;

		[ObservableComputationsCall]
		public SequenceComputing(IReadScalar<int> countScalar)
		{
			_countScalar = countScalar;
            _deferredQueuesCount = 1;
        }

		private void handleCountChanged(object sender, PropertyChangedEventArgs e)
		{
			int newCount = _countScalar.Value;

            Action action = () =>
            {
			    if (_count < newCount)
			    {
				    for (int item = _count; item < newCount; item++)
				    {
					    baseInsertItem(item, item);
				    }	
				    
				    _count = newCount;
			    }
			    else if (_count > newCount)
			    {
				    for (int itemIndex = _count - 1; itemIndex > newCount - 1; itemIndex--)
				    {
					    baseRemoveItem(itemIndex);
				    }	
				    
				    _count = newCount;
			    }
            };

            Utils.processChange(
                sender, 
                e, 
                action,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, _deferredQueuesCount,
                ref _deferredProcessings, this);
		}

		// ReSharper disable once InconsistentNaming
		public void ValidateConsistency()
		{
			int count =  _countScalar.Value;
			if (Count != count) throw new ObservableComputationsException(this, "Consistency violation: SequenceComputing.1");

			for (int i = 0; i < count; i++)
			{
				if (this[i] != i) throw new ObservableComputationsException(this, "Consistency violation: SequenceComputing.2");
			}
		}

        #region Overrides of CollectionComputing<int>

        protected override void initializeFromSource()
        {
        }

        protected override void initialize()
        {
            _count = _countScalar.Value;

            for (int item = 0; item < _count; item++)
            {
                baseInsertItem(item, item);
            }

            _countScalar.PropertyChanged += handleCountChanged;
        }

        protected override void uninitialize()
        {
            _countScalar.PropertyChanged -= handleCountChanged;
            baseClearItems();
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_countScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_countScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        #endregion
    }
}
