using System.ComponentModel;
using IBCode.ObservableComputations.Common;
using IBCode.ObservableComputations.Common.Interface;

namespace IBCode.ObservableComputations
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
			_count = _countScalar.Value;

			for (int item = 0; item < _count; item++)
			{
				baseInsertItem(item, item);
			}

			_countScalar.PropertyChanged += handleCountChanged;
		}

		private void handleCountChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName != nameof(Computing<int>.Value)) return;

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
			if (Count != count) throw new ObservableComputationsException("Consistency violation: SequenceComputing.1");

			for (int i = 0; i < count; i++)
			{
				if (this[i] != i) throw new ObservableComputationsException("Consistency violation: SequenceComputing.2");
			}
		}
	}
}
