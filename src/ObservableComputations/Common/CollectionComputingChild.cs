using System;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public abstract class CollectionComputingChild<TItem> : ObservableCollectionWithChangeMarker<TItem>, ICollectionComputingChild,  IHasChangeMarker
	{
		public string DebugTag {get; set;}
		public object Tag {get; set;}

		public event EventHandler PreCollectionChanged;
		public event EventHandler PostCollectionChanged;

		NotifyCollectionChangedAction? _currentChange;
		TItem _newItem;
		int _oldIndex = -1;
		int _newIndex = -1;

		public NotifyCollectionChangedAction? CurrentChange => _currentChange;
		public TItem NewItem => _newItem;
		public object NewItemObject => _newItem;
		public int OldIndex => _oldIndex;
		public int NewIndex => _newIndex;
	
		protected internal void insertItem(int index, TItem item)
		{
			ChangeMarker = !ChangeMarker;

			_currentChange = NotifyCollectionChangedAction.Add;
			_newIndex = index;
			_newItem = item;

			PreCollectionChanged?.Invoke(this, null);
			base.InsertItem(index, item);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_newIndex = -1;
			_newItem = default(TItem);
		}

		
		protected internal void moveItem(int oldIndex, int newIndex)
		{
			ChangeMarker = !ChangeMarker;

			_currentChange = NotifyCollectionChangedAction.Move;
			_oldIndex = oldIndex;
			_newIndex = newIndex;


			PreCollectionChanged?.Invoke(this, null);
			base.MoveItem(oldIndex, newIndex);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_oldIndex = -1;
			_newIndex = -1;
		}

		
		protected internal void removeItem(int index)
		{
			ChangeMarker = !ChangeMarker;

			_currentChange = NotifyCollectionChangedAction.Remove;
			_oldIndex = index;

			PreCollectionChanged?.Invoke(this, null);
			base.RemoveItem(index);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_oldIndex = -1;
		}

		
		protected internal void setItem(int index, TItem item)
		{
			ChangeMarker = !ChangeMarker;

			_currentChange = NotifyCollectionChangedAction.Replace;
			_newItem = item;
			_newIndex = index;

			PreCollectionChanged?.Invoke(this, null);
			base.SetItem(index, item);
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
			_newItem = default;
			_newIndex = -1;
		}

		
		protected internal void clearItems()
		{
			ChangeMarker = !ChangeMarker;

			_currentChange = NotifyCollectionChangedAction.Reset;

			PreCollectionChanged?.Invoke(this, null);
			base.ClearItems();
			PostCollectionChanged?.Invoke(this, null);

			_currentChange = null;
		}

		protected internal void insertItemNotExtended(int index, TItem item)
		{
			base.InsertItem(index, item);
		}

		
		protected internal void moveItemNotExtended(int oldIndex, int newIndex)
		{
			base.MoveItem(oldIndex, newIndex);
		}

		
		protected internal void removeItemNotExtended(int index)
		{
			base.RemoveItem(index);
		}

		
		protected internal void setItemNotExtended(int index, TItem item)
		{
			base.SetItem(index, item);
		}


		public Type ItemType => typeof(TItem);
		public abstract ICollectionComputing Parent { get; }
	}

}