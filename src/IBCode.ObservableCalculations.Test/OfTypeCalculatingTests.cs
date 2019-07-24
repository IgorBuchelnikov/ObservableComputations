using System.Collections.ObjectModel;
using NUnit.Framework;

namespace IBCode.ObservableCalculations.Test
{
	[TestFixture]
	public class OfTypeCalculatingTests
	{
		class BaseItem{}
		class DerivedItem : BaseItem{}

		[Test]
		public void OfTypeCalculating_Initialization_01()
		{
			ObservableCollection<DerivedItem> items = new ObservableCollection<DerivedItem>();

			OfTypeCalculating<BaseItem> ofTypeCalculating = items.OfTypeCalculating<BaseItem>();
			ofTypeCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void OfTypeCalculating_Set(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 1, 1)] int index,
			[Range(-1, 0, 1)] int newItem)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (index >= items.Count) return;

			OfTypeCalculating<DerivedItem> ofTypeCalculating = items.OfTypeCalculating<DerivedItem>();
			ofTypeCalculating.ValidateConsistency();
			if (index < items.Count) items[index] = newItem >= 0 ? (newItem == 1 ? new DerivedItem() : new BaseItem()) : null;
			ofTypeCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void OfTypeCalculating_Remove(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 1, 1)] int index)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (index >= items.Count) return;

			OfTypeCalculating<DerivedItem> ofTypeCalculating = items.OfTypeCalculating<DerivedItem>();
			ofTypeCalculating.ValidateConsistency();
			items.RemoveAt(index);
			ofTypeCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void OfTypeCalculating_Insert(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 2, 1)] int index,
			[Range(-1, 0, 1)] int newItem)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (index > items.Count) return;

			OfTypeCalculating<DerivedItem> ofTypeCalculating = items.OfTypeCalculating<DerivedItem>();
			ofTypeCalculating.ValidateConsistency();
			items.Insert(index, newItem >= 0 ? (newItem == 1 ? new DerivedItem() : new BaseItem()) : null);
			ofTypeCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void OfTypeCalculating_Move(
			[Range(-2, 0, 1)] int item1,
			[Range(-2, 0, 1)] int item2,
			[Range(0, 4, 1)] int oldIndex,
			[Range(0, 4, 1)] int newIndex)
		{
			ObservableCollection<BaseItem> items = new ObservableCollection<BaseItem>();
			if (item1 >= -1) items.Add(item1 >= 0 ? (item1 == 1 ? new DerivedItem() : new BaseItem()) : null);
			if (item2 >= -1) items.Add(item2 >= 0 ? (item2 == 1 ? new DerivedItem() : new BaseItem()) : null);

			if (oldIndex >= items.Count || newIndex >= items.Count) return;

			OfTypeCalculating<DerivedItem> ofTypeCalculating = items.OfTypeCalculating<DerivedItem>();
			ofTypeCalculating.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			ofTypeCalculating.ValidateConsistency();
		}	
	}
}