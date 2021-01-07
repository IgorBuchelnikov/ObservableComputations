// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.ObjectModel;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class OfTypeComputingTests
	{
		OcConsumer consumer = new OcConsumer();

		class BaseItem{}
		class DerivedItem : BaseItem{}

		[Test]
		public void OfTypeComputing_Initialization_01()
		{
			ObservableCollection<DerivedItem> items = new ObservableCollection<DerivedItem>();

			OfTypeComputing<BaseItem> ofTypeComputing = items.OfTypeComputing<BaseItem>();
			ofTypeComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void OfTypeComputing_Set(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 1, 1)] int index,
			[Range(-1, 0, 1)] int newItem)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (index >= items.Count) return;

			OfTypeComputing<DerivedItem> ofTypeComputing = items.OfTypeComputing<DerivedItem>().For(consumer);
			ofTypeComputing.ValidateConsistency();
			if (index < items.Count) items[index] = newItem >= 0 ? (newItem == 1 ? new DerivedItem() : new BaseItem()) : null;
			ofTypeComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void OfTypeComputing_Remove(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 1, 1)] int index)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (index >= items.Count) return;

			OfTypeComputing<DerivedItem> ofTypeComputing = items.OfTypeComputing<DerivedItem>().For(consumer);
			ofTypeComputing.ValidateConsistency();
			items.RemoveAt(index);
			ofTypeComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void OfTypeComputing_Insert(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 2, 1)] int index,
			[Range(-1, 0, 1)] int newItem)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (index > items.Count) return;

			OfTypeComputing<DerivedItem> ofTypeComputing = items.OfTypeComputing<DerivedItem>().For(consumer);
			ofTypeComputing.ValidateConsistency();
			items.Insert(index, newItem >= 0 ? (newItem == 1 ? new DerivedItem() : new BaseItem()) : null);
			ofTypeComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void OfTypeComputing_Move(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 4, 1)] int oldIndex,
			[Range(0, 4, 1)] int newIndex)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (oldIndex >= items.Count || newIndex >= items.Count) return;

			OfTypeComputing<DerivedItem> ofTypeComputing = items.OfTypeComputing<DerivedItem>().For(consumer);
			ofTypeComputing.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			ofTypeComputing.ValidateConsistency();			
			consumer.Dispose();
		}	
	}
}