using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace ObservableComputations
{
	public class Zipping<TLeftSourceItem, TRightSourceItem> : CollectionComputing<ZipPair<TLeftSourceItem, TRightSourceItem>>, IHasSourceCollections
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> LeftSourceScalar => _leftSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> RightSourceScalar => _rightSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged LeftSource => _leftSource;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged RightSource => _rightSource;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{LeftSource, RightSource});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{LeftSourceScalar, RightSourceScalar});

		public Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> ZipPairSetLeftItemAction
		{
			get => _zipPairSetLeftItemAction;
			set
			{
				if (_zipPairSetLeftItemAction != value)
				{
					checkLockModifyZipPairChangeAction(ZipPairAction.SetLeftItem);

					_zipPairSetLeftItemAction = value;
					OnPropertyChanged(Utils.ZipPairSetLeftItemActionPropertyChangedEventArgs);
				}
			}
		}

		public Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> ZipPairSetRightItemAction
		{
			get => _zipPairSetRightItemAction;
			set
			{
				if (_zipPairSetRightItemAction != value)
				{
					checkLockModifyZipPairChangeAction(ZipPairAction.SetRightItem);

					_zipPairSetRightItemAction = value;
					OnPropertyChanged(Utils.ZipPairSetRightItemActionPropertyChangedEventArgs);
				}
			}
		}


		Dictionary<ZipPairAction, object> _lockModifyZipPairChangeActionsKeys;
		private Dictionary<ZipPairAction, object> lockModifyZipPairChangeActionsKeys => _lockModifyZipPairChangeActionsKeys = 
			_lockModifyZipPairChangeActionsKeys ?? new Dictionary<ZipPairAction, object>();

		public void LockModifyZipPairChangeAction(ZipPairAction collectionChangeAction, object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (!lockModifyZipPairChangeActionsKeys.ContainsKey(collectionChangeAction))
				lockModifyZipPairChangeActionsKeys[collectionChangeAction] = key;
			else
				throw new ObservableComputationsException(this,
					$"Modifying of '{collectionChangeAction.ToString()}' zipPair change action is already locked. Unlock first.");
		}

		public void UnlockModifyZipPairChangeAction(ZipPairAction collectionChangeAction, object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (!lockModifyZipPairChangeActionsKeys.ContainsKey(collectionChangeAction))
				throw new ObservableComputationsException(this,
					"Modifying of '{collectionChangeAction.ToString()}' zipPair change action is not locked. Lock first.");

			if (ReferenceEquals(lockModifyZipPairChangeActionsKeys[collectionChangeAction], key))
				lockModifyZipPairChangeActionsKeys.Remove(collectionChangeAction);
			else
				throw new ObservableComputationsException(this,
					"Wrong key to unlock modifying of '{collectionChangeAction.ToString()}' zipPair change action.");
		}

		public bool IsModifyZipPairChangeActionLocked(ZipPairAction collectionChangeAction)
		{
			return lockModifyZipPairChangeActionsKeys.ContainsKey(collectionChangeAction);
		}

		private void checkLockModifyZipPairChangeAction(ZipPairAction collectionChangeAction)
		{
			if (lockModifyZipPairChangeActionsKeys.ContainsKey(collectionChangeAction))
				throw new ObservableComputationsException(this,
					"Modifying of '{collectionChangeAction.ToString()}' zipPair change action is locked. Unlock first.");
		}

		private PropertyChangedEventHandler _leftSourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _leftSourceScalarWeakPropertyChangedEventHandler;

		private IList<TLeftSourceItem> _leftSourceAsList;

		private PropertyChangedEventHandler _rightSourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _rightSourceScalarWeakPropertyChangedEventHandler;

		private IList<TRightSourceItem> _rightSourceAsList;

		private NotifyCollectionChangedEventHandler _leftSourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _leftSourceWeakNotifyCollectionChangedEventHandler;

		private NotifyCollectionChangedEventHandler _rightSourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _rightSourceWeakNotifyCollectionChangedEventHandler;

		internal Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> _zipPairSetLeftItemAction;
		internal Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> _zipPairSetRightItemAction;
		private readonly IReadScalar<INotifyCollectionChanged> _leftSourceScalar;
		private readonly IReadScalar<INotifyCollectionChanged> _rightSourceScalar;
		private INotifyCollectionChanged _leftSource;
		private INotifyCollectionChanged _rightSource;

		private PropertyChangedEventHandler _leftSourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _leftSourceWeakPropertyChangedEventHandler;
		private bool _leftSourceIndexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _leftSourceAsINotifyPropertyChanged;

		private PropertyChangedEventHandler _rigthSourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _rigthSourceWeakPropertyChangedEventHandler;
		private bool _rigthtSourceIndexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _rigthSourceAsINotifyPropertyChanged;

		private ObservableCollectionWithChangeMarker<TLeftSourceItem> _leftSourceAsObservableCollectionWithChangeMarker;
		private bool _lastProcessedLeftSourceChangeMarker;

		private ObservableCollectionWithChangeMarker<TRightSourceItem> _rightSourceAsObservableCollectionWithChangeMarker;
		private bool _lastProcessedRightSourceChangeMarker;

		[ObservableComputationsCall]
		public Zipping(
			IReadScalar<INotifyCollectionChanged> leftSourceScalar,
			INotifyCollectionChanged rightSource) : base(calculateCapacity(leftSourceScalar, rightSource))
		{
			_leftSourceScalar = leftSourceScalar;
			initializeSourceLeftScalar();
			
			_rightSource = rightSource;	
			
			initializeFromSources();
		}

		[ObservableComputationsCall]
		public Zipping(
			IReadScalar<INotifyCollectionChanged> leftSourceScalar,
			IReadScalar<INotifyCollectionChanged> rightSourceScalar) : base(calculateCapacity(leftSourceScalar, rightSourceScalar))
		{
			_leftSourceScalar = leftSourceScalar;
			initializeSourceLeftScalar();

			_rightSourceScalar = rightSourceScalar;
			initializeSourceRightScalar();

			initializeFromSources();
		}

		[ObservableComputationsCall]
		public Zipping(
			INotifyCollectionChanged leftSource,
			INotifyCollectionChanged rightSource) : base(calculateCapacity(leftSource, rightSource))
		{
			_leftSource = leftSource;			
			_rightSource = rightSource;					
			initializeFromSources();
		}

		[ObservableComputationsCall]
		public Zipping(
			INotifyCollectionChanged leftSource,
			IReadScalar<INotifyCollectionChanged> rightSourceScalar) : base(calculateCapacity(leftSource, rightSourceScalar))
		{
			_leftSource = leftSource;
			
			_rightSourceScalar = rightSourceScalar;
			initializeSourceRightScalar();		
					
			initializeFromSources();
		}

		private void initializeSourceRightScalar()
		{
			_rightSourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_rightSourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_rightSourceScalarPropertyChangedEventHandler);
			_rightSourceScalar.PropertyChanged += _rightSourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		private void initializeSourceLeftScalar()
		{
			_leftSourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_leftSourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_leftSourceScalarPropertyChangedEventHandler);
			_leftSourceScalar.PropertyChanged += _leftSourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		private static int calculateCapacity(INotifyCollectionChanged sourceLeft, INotifyCollectionChanged sourceRight)
		{
			int capacityLeft = Utils.getCapacity(sourceLeft);
			int capacityRight = Utils.getCapacity(sourceRight);
			return capacityLeft > capacityRight ? capacityLeft : capacityRight;
		}

		private static int calculateCapacity(INotifyCollectionChanged sourceLeft, IReadScalar<INotifyCollectionChanged> sourceRightScalar)
		{
			int capacityLeft = Utils.getCapacity(sourceLeft);
			int capacityRight = Utils.getCapacity(sourceRightScalar);
			return capacityLeft > capacityRight ? capacityLeft : capacityRight;
		}

		private static int calculateCapacity(IReadScalar<INotifyCollectionChanged> sourceLeftScalar, INotifyCollectionChanged sourceRight)
		{
			int capacityLeft = Utils.getCapacity(sourceLeftScalar);
			int capacityRight = Utils.getCapacity(sourceRight);
			return capacityLeft > capacityRight ? capacityLeft : capacityRight;
		}

		private static int calculateCapacity(IReadScalar<INotifyCollectionChanged> sourceLeftScalar,
			IReadScalar<INotifyCollectionChanged> sourceRightScalar)
		{
			int capacityLeft = Utils.getCapacity(sourceLeftScalar);
			int capacityRight = Utils.getCapacity(sourceRightScalar);
			return capacityLeft > capacityRight ? capacityLeft : capacityRight;
		}

		private void initializeFromSources()
		{
			int originalCount = _items.Count;

			if (_leftSourceNotifyCollectionChangedEventHandler != null)
			{
				_leftSource.CollectionChanged -= _leftSourceWeakNotifyCollectionChangedEventHandler.Handle;
				_leftSourceNotifyCollectionChangedEventHandler = null;
				_leftSourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_rightSourceNotifyCollectionChangedEventHandler != null)
			{
				_rightSource.CollectionChanged -= _rightSourceWeakNotifyCollectionChangedEventHandler.Handle;
				_rightSourceNotifyCollectionChangedEventHandler = null;
				_rightSourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_leftSourceAsINotifyPropertyChanged != null)
			{
				_leftSourceAsINotifyPropertyChanged.PropertyChanged -=
					_leftSourceWeakPropertyChangedEventHandler.Handle;

				_leftSourceAsINotifyPropertyChanged = null;
				_leftSourcePropertyChangedEventHandler = null;
				_leftSourceWeakPropertyChangedEventHandler = null;
			}

			if (_rigthSourceAsINotifyPropertyChanged != null)
			{
				_rigthSourceAsINotifyPropertyChanged.PropertyChanged -=
					_rigthSourceWeakPropertyChangedEventHandler.Handle;

				_rigthSourceAsINotifyPropertyChanged = null;
				_rigthSourcePropertyChangedEventHandler = null;
				_rigthSourceWeakPropertyChangedEventHandler = null;
			}

			if (_leftSourceScalar != null) _leftSource = _leftSourceScalar.Value;
			_leftSourceAsList = (IList<TLeftSourceItem>) _leftSource;

			if (_rightSourceScalar != null) _rightSource = _rightSourceScalar.Value;
			_rightSourceAsList = (IList<TRightSourceItem>) _rightSource;

			if (_leftSourceAsList != null && _rightSourceAsList != null)
			{
				_leftSourceAsObservableCollectionWithChangeMarker = _leftSourceAsList as ObservableCollectionWithChangeMarker<TLeftSourceItem>;

				if (_leftSourceAsObservableCollectionWithChangeMarker != null)
				{
					_lastProcessedLeftSourceChangeMarker = _leftSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField;
				}
				else
				{
					_leftSourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _leftSourceAsList;

					_leftSourcePropertyChangedEventHandler = (sender, args) =>
					{
						if (args.PropertyName == "Item[]") _leftSourceIndexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_leftSourceWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_leftSourcePropertyChangedEventHandler);

					_leftSourceAsINotifyPropertyChanged.PropertyChanged +=
						_leftSourceWeakPropertyChangedEventHandler.Handle;
				}

				_rightSourceAsObservableCollectionWithChangeMarker = _rightSourceAsList as ObservableCollectionWithChangeMarker<TRightSourceItem>;

				if (_rightSourceAsObservableCollectionWithChangeMarker != null)
				{
					_lastProcessedRightSourceChangeMarker = _rightSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField;
				}
				else
				{
					_rigthSourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _rightSourceAsList;

					_rigthSourcePropertyChangedEventHandler = (sender, args) =>
					{
						if (args.PropertyName == "Item[]") _rigthtSourceIndexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_rigthSourceWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_rigthSourcePropertyChangedEventHandler);

					_rigthSourceAsINotifyPropertyChanged.PropertyChanged +=
						_rigthSourceWeakPropertyChangedEventHandler.Handle;
				}

				int countLeft = _leftSourceAsList.Count;
				int countRight = _rightSourceAsList.Count;
				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < countLeft; sourceIndex++)
				{
					if (sourceIndex < countRight)
					{								
						ZipPair<TLeftSourceItem, TRightSourceItem> zipPair = new ZipPair<TLeftSourceItem, TRightSourceItem>(this, _leftSourceAsList[sourceIndex], _rightSourceAsList[sourceIndex]);
						
						if (originalCount > sourceIndex)
							_items[sourceIndex] = zipPair;
						else
							_items.Insert(sourceIndex, zipPair);					
					}
					else
					{
						break;
					}
				}

				for (int index = originalCount - 1; index >= sourceIndex; index--)
				{
					_items.RemoveAt(index);
				}
			}

			if (_leftSource != null && _rightSource != null)
			{
				_leftSourceNotifyCollectionChangedEventHandler = handleLeftSourceCollectionChanged;
				_leftSourceWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_leftSourceNotifyCollectionChangedEventHandler);

				_leftSource.CollectionChanged += _leftSourceWeakNotifyCollectionChangedEventHandler.Handle;				

				_rightSourceNotifyCollectionChangedEventHandler = handleRightSourceCollectionChanged;
				_rightSourceWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_rightSourceNotifyCollectionChangedEventHandler);

				_rightSource.CollectionChanged += _rightSourceWeakNotifyCollectionChangedEventHandler.Handle;				
			}
			else
			{
				_items.Clear();
			}

			reset();
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			_isConsistent = false;

			initializeFromSources();

			_isConsistent = true;
			raiseConsistencyRestored();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleLeftSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			if (_leftSourceIndexerPropertyChangedEventRaised || _leftSourceAsObservableCollectionWithChangeMarker != null && _lastProcessedLeftSourceChangeMarker != _leftSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField)
			{
				_leftSourceIndexerPropertyChangedEventRaised = false;
				_lastProcessedLeftSourceChangeMarker = !_lastProcessedLeftSourceChangeMarker;

				_isConsistent = false;

				int newIndex;
				int oldIndex;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
						newIndex = e.NewStartingIndex;
		
						//if (newIndex < this.Count)
						//{
						int sourceLeftCount = _leftSourceAsList.Count;
						int sourceRightCount = _rightSourceAsList.Count;
						int thisCount = Count;
						for (int sourceIndex = newIndex; sourceIndex < sourceLeftCount; sourceIndex++)
						{
							if (sourceIndex < sourceRightCount)
							{

								if (sourceIndex < thisCount)
								{
									this[sourceIndex].setItemLeft(_leftSourceAsList[sourceIndex]);									
								}
								else
								{
									baseInsertItem(sourceIndex, new ZipPair<TLeftSourceItem, TRightSourceItem>(this, _leftSourceAsList[sourceIndex], _rightSourceAsList[sourceIndex]));
								}
							}
							else
							{
								break;
							}
						}						
						//}
						//else
						//{
						//	if (newIndex < _rightSourceAsList.Count)
						//	{
						//		baseInsertItem(this.Count, new ZipPair<TLeftSourceItem, TRightSourceItem>(_leftSourceAsList[newIndex], _rightSourceAsList[newIndex]));
						//	}		
						//}
						break;
					case NotifyCollectionChangedAction.Remove:
						//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
						oldIndex = e.OldStartingIndex;
		
						int countLeft = _leftSourceAsList.Count;
						int countRight = _rightSourceAsList.Count;
						for (int sourceIndex = oldIndex; sourceIndex < countLeft; sourceIndex++)
						{
							if (sourceIndex < countRight)
							{
								this[sourceIndex].setItemLeft(_leftSourceAsList[sourceIndex]);
							}
							else
							{
								break;
							}					
						}

						int thisCountLeft = Count;
						if (thisCountLeft > countLeft)
						{
							baseRemoveItem(thisCountLeft - 1);
						}
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
						newIndex = e.NewStartingIndex;

						if (newIndex < Count)
						{
							this[newIndex].setItemLeft(_leftSourceAsList[newIndex]);
						}
						break;
					case NotifyCollectionChangedAction.Move:
						newIndex = e.NewStartingIndex;
						oldIndex = e.OldStartingIndex;
						if (newIndex != oldIndex)
						{
							int lowerIndex;
							int upperIndex;
							int count = Count;
							if (newIndex < oldIndex)
							{
								lowerIndex = newIndex;

								upperIndex = oldIndex >= count ? count - 1 : oldIndex; 
							}
							else
							{
								lowerIndex = oldIndex;
								upperIndex = newIndex >= count ? count - 1 : newIndex; 						
							}

							for (int sourceIndex = lowerIndex; sourceIndex <= upperIndex; sourceIndex++)
							{
								if (sourceIndex < count)
								{
									this[sourceIndex].setItemLeft(_leftSourceAsList[sourceIndex]);
								}					
							}
						}

						break;
					case NotifyCollectionChangedAction.Reset:
						initializeFromSources();
						break;
				}

				_isConsistent = true;
				raiseConsistencyRestored();
			}

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleRightSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			if (_rigthtSourceIndexerPropertyChangedEventRaised || _rightSourceAsObservableCollectionWithChangeMarker != null && _lastProcessedRightSourceChangeMarker != _rightSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField)
			{
				_rigthtSourceIndexerPropertyChangedEventRaised = false;
				_lastProcessedRightSourceChangeMarker = !_lastProcessedRightSourceChangeMarker;

				_isConsistent = false;

				int newIndex;
				int oldIndex;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
						newIndex = e.NewStartingIndex;

						int countRight = _rightSourceAsList.Count;
						int countLeft = _leftSourceAsList.Count;
						for (int sourceIndex = newIndex; sourceIndex < countRight; sourceIndex++)
						{
							if (sourceIndex < countLeft)
							{
								TRightSourceItem sourceItemRight = _rightSourceAsList[sourceIndex];
								if (sourceIndex < Count)
								{
									this[sourceIndex].setItemRight(sourceItemRight);
								}
								else
								{
									baseInsertItem(sourceIndex, new ZipPair<TLeftSourceItem, TRightSourceItem>(this, _leftSourceAsList[sourceIndex], sourceItemRight));
								}
							}
							else
							{
								break;
							}					
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
						oldIndex = e.OldStartingIndex;
						int sourceRightCount = _rightSourceAsList.Count;
						int sourceLeftCount = _leftSourceAsList.Count;
						for (int sourceIndex = oldIndex; sourceIndex < sourceRightCount; sourceIndex++)
						{

							if (sourceIndex < sourceLeftCount)
							{
								this[sourceIndex].setItemRight(_rightSourceAsList[sourceIndex]);
							}
							else
							{
								break;
							}
						}


						int thisCountLeft = Count;
						if (thisCountLeft > sourceRightCount)
						{
							baseRemoveItem(thisCountLeft - 1);
						}
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
						newIndex = e.NewStartingIndex;
						if (newIndex < Count)
						{
							this[newIndex].setItemRight(_rightSourceAsList[newIndex]);
						}
						break;
					case NotifyCollectionChangedAction.Move:
						newIndex = e.NewStartingIndex;
						oldIndex = e.OldStartingIndex;

						int lowerIndex;
						int upperIndex;
						int thisCount = Count;
						if (newIndex < oldIndex)
						{
							lowerIndex = newIndex;

							upperIndex = oldIndex >= thisCount ? thisCount - 1 : oldIndex; 
						}
						else
						{
							lowerIndex = oldIndex;
							upperIndex = newIndex >= thisCount ? thisCount - 1 : newIndex; 						
						}

						for (int sourceIndex = lowerIndex; sourceIndex <= upperIndex; sourceIndex++)
						{
							if (sourceIndex < thisCount)
							{
								this[sourceIndex].setItemRight(_rightSourceAsList[sourceIndex]);
							}					
						}
						break;
					case NotifyCollectionChangedAction.Reset:
						initializeFromSources();
						break;
				}

				_isConsistent = true;
				raiseConsistencyRestored();
			}

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		~Zipping()
		{
			if (_leftSourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_leftSource.CollectionChanged -= _leftSourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_rightSourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_rightSource.CollectionChanged -= _rightSourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_leftSourceScalarWeakPropertyChangedEventHandler != null)
			{
				_leftSourceScalar.PropertyChanged -= _leftSourceScalarWeakPropertyChangedEventHandler.Handle;
			}

			if (_rightSourceScalarWeakPropertyChangedEventHandler != null)
			{
				_rightSourceScalar.PropertyChanged -= _rightSourceScalarWeakPropertyChangedEventHandler.Handle;
			}

			if (_rigthSourceAsINotifyPropertyChanged != null)
				_rigthSourceAsINotifyPropertyChanged.PropertyChanged -=
					_rigthSourceWeakPropertyChangedEventHandler.Handle;

			if (_leftSourceAsINotifyPropertyChanged != null)
				_leftSourceAsINotifyPropertyChanged.PropertyChanged -=
					_leftSourceWeakPropertyChangedEventHandler.Handle;
		} 

		public void ValidateConsistency()
		{
			IList<TLeftSourceItem> sourceLeft = _leftSourceScalar.getValue(_leftSource, new ObservableCollection<TLeftSourceItem>()) as IList<TLeftSourceItem>;
			IList<TRightSourceItem> sourceRight = _rightSourceScalar.getValue(_rightSource, new ObservableCollection<TRightSourceItem>()) as IList<TRightSourceItem>;

			// ReSharper disable once PossibleNullReferenceException
			for (int index = 0; index < sourceLeft.Count; index++)
			{
				// ReSharper disable once PossibleNullReferenceException
				if (index < sourceRight.Count)
				{
					if (!EqualityComparer<TLeftSourceItem>.Default.Equals(this[index].LeftItem, sourceLeft[index]))
						throw new ObservableComputationsException(this, "Consistency violation: Zipping.Left");

					if (!EqualityComparer<TRightSourceItem>.Default.Equals(this[index].RightItem, sourceRight[index]))
						throw new ObservableComputationsException(this, "Consistency violation: Zipping.Right");
				}
				else
				{
					break;
				}
			}
		}
	}

	public class ZipPair<TLeftSourceItem, TRightSourceItem> : INotifyPropertyChanged
	{
		private TLeftSourceItem _leftItem;

		public TLeftSourceItem LeftItem
		{
			get => _leftItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _zipping._zipPairSetLeftItemAction(this, value);
		}

		private TRightSourceItem _rightItem;
		public TRightSourceItem RightItem
		{
			get => _rightItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set =>  _zipping._zipPairSetRightItemAction(this, value);
		}

		internal void setItemLeft(TLeftSourceItem itemLeft)
		{
			_leftItem = itemLeft;
			PropertyChanged?.Invoke(this, Utils.LeftItemPropertyChangedEventArgs);
		}

		internal void setItemRight(TRightSourceItem itemRight)
		{
			_rightItem = itemRight;
			PropertyChanged?.Invoke(this, Utils.RightItemPropertyChangedEventArgs);
		}

		private readonly Zipping<TLeftSourceItem, TRightSourceItem> _zipping;

		public ZipPair(Zipping<TLeftSourceItem, TRightSourceItem> zipping, TLeftSourceItem leftItem,
			TRightSourceItem rightItem)
		{
			_leftItem = leftItem;
			_rightItem = rightItem;
			_zipping = zipping;
		}

		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion


		public override string ToString()
		{
			return $"ZipPair: ItemLeft = {(LeftItem != null ? $"{LeftItem.ToString()}" : "null")}    ItemRight = {(RightItem != null ? $"{RightItem.ToString()}" : "null")}";
		}
	}

	public enum ZipPairAction
	{
		SetLeftItem,
		SetRightItem
	}
}