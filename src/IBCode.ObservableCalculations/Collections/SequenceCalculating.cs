using System.ComponentModel;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class SequenceCalculating : CollectionCalculating<int>
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> CountScalar => _countScalar;

		int _count;
		private readonly IReadScalar<int> _countScalar;

		[ObservableCalculationsCall]
		public SequenceCalculating(IReadScalar<int> countScalar)
		{
			_countScalar = countScalar;
			_count = _countScalar.Value;

			for (int item = 0; item < _count; item++)
			{
				baseInsertItem(item, item);
			}

			_countScalar.PropertyChanged += handleCountChanged;
		}

		private void handleCountChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName != nameof(Calculating<int>.Value)) return;

			int newCount = _countScalar.Value;

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
		}

		// ReSharper disable once InconsistentNaming
		public void ValidateConsistency()
		{
			int count =  _countScalar.Value;
			if (Count != count) throw new ObservableCalculationsException("Consistency violation: SequenceCalculating.1");

			for (int i = 0; i < count; i++)
			{
				if (this[i] != i) throw new ObservableCalculationsException("Consistency violation: SequenceCalculating.2");
			}
		}
	}
}
