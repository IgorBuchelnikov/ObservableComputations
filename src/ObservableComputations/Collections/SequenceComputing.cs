// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

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

			void action()
			{
				if (_count < newCount)
				{
					for (int item = _count; item < newCount; item++)
						baseInsertItem(item, item);

					_count = newCount;
				}
				else if (_count > newCount)
				{
					for (int itemIndex = _count - 1; itemIndex > newCount - 1; itemIndex--)
						baseRemoveItem(itemIndex);

					_count = newCount;
				}
			}

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
		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			int count =  _countScalar.Value;
			if (Count != count) throw new ValidateInternalConsistencyException("Consistency violation: SequenceComputing.1");

			for (int i = 0; i < count; i++)
			{
				if (this[i] != i) throw new ValidateInternalConsistencyException("Consistency violation: SequenceComputing.2");
			}
		}

		#region Overrides of CollectionComputing<int>

		protected override void processSource()
		{
			if (_isActive)
			{
				_count = _countScalar.Value;

				for (int item = 0; item < _count; item++)
					baseInsertItem(item, item);

				_countScalar.PropertyChanged += handleCountChanged;
			}
			else
			{
				_countScalar.PropertyChanged -= handleCountChanged;
				baseClearItems();				
			}
		}

		protected override void initialize()
		{

		}

		protected override void uninitialize()
		{

		}

		protected override void clearCachedScalarArgumentValues()
		{

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
