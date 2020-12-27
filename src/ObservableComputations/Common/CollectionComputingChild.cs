using System;
using System.Collections.Specialized;
using System.Threading;

namespace ObservableComputations
{
	public abstract class CollectionComputingChild<TItem> : ObservableCollectionWithChangeMarker<TItem>, ICollectionComputingChild
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

		public string InstantiatingStackTrace => Parent.InstantiatingStackTrace;
		internal IComputing _userCodeIsCalledFrom;
		public IComputing UserCodeIsCalledFrom => _userCodeIsCalledFrom;
		public object HandledEventSender => Parent.HandledEventSender;
		public EventArgs HandledEventArgs => Parent.HandledEventArgs;
		public bool IsActive => Parent.IsActive;

		protected internal void insertItem(int index, TItem item)
		{
			void perform()
			{
				PreCollectionChanged?.Invoke(this, null);
				base.InsertItem(index, item);
				PostCollectionChanged?.Invoke(this, null);
			}

			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Add;
			_newIndex = index;
			_newItem = item;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
			}
			else
			{
				perform();
			}

			_currentChange = null;
			_newIndex = -1;
			_newItem = default(TItem);
		}

		protected internal void moveItem(int oldIndex, int newIndex)
		{
			void perform()
			{
				PreCollectionChanged?.Invoke(this, null);
				base.MoveItem(oldIndex, newIndex);
				PostCollectionChanged?.Invoke(this, null);
			}

			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Move;
			_oldIndex = oldIndex;
			_newIndex = newIndex;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
			}
			else
			{
				perform();
			}

			_currentChange = null;
			_oldIndex = -1;
			_newIndex = -1;
		}

		
		protected internal void removeItem(int index)
		{
			void perform()
			{
				PreCollectionChanged?.Invoke(this, null);
				base.RemoveItem(index);
				PostCollectionChanged?.Invoke(this, null);
			}

			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Remove;
			_oldIndex = index;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
			}
			else
			{
				perform();
			}

			_currentChange = null;
			_oldIndex = -1;
		}

		
		protected internal void setItem(int index, TItem item)
		{
			void perform()
			{
				PreCollectionChanged?.Invoke(this, null);
				base.SetItem(index, item);
				PostCollectionChanged?.Invoke(this, null);
			}

			ChangeMarkerField = !ChangeMarkerField;
			
			_currentChange = NotifyCollectionChangedAction.Replace;
			_newItem = item;
			_newIndex = index;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
			}
			else
			{
				perform();
			}

			_currentChange = null;
			_newItem = default;
			_newIndex = -1;
		}

		
		protected internal void clearItems()
		{
			void perform()
			{
				PreCollectionChanged?.Invoke(this, null);
				base.ClearItems();
				PostCollectionChanged?.Invoke(this, null);
			}

			ChangeMarkerField = !ChangeMarkerField;

			_currentChange = NotifyCollectionChangedAction.Reset;

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);

				perform();

				Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
			}
			else
			{
				perform();
			}

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

		#region Implementation of IConsistent

		public bool IsConsistent => Parent.IsConsistent;
		public event EventHandler ConsistencyRestored;

		#endregion
	}

}