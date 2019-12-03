using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations.Common;
using ObservableComputations.Common.Base;
using ObservableComputations.Common.Interface;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace ObservableComputations
{
	public class Zipping<TLeftSourceItem, TRightSourceItem> : CollectionComputing<ZipPair<TLeftSourceItem, TRightSourceItem>>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> LeftSourceScalar => _leftSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> RightSourceScalar => _rightSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged LeftSource => _leftSource;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged RightSource => _rightSource;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{LeftSource, RightSource});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{LeftSourceScalar, RightSourceScalar});

		public Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> ZipPairSetItemLeftAction
		{
			get => _zipPairSetItemLeftAction;
			set
			{
				if (_zipPairSetItemLeftAction != value)
				{
					_zipPairSetItemLeftAction = value;
					OnPropertyChanged(Utils.ZipPairSetItemLeftActionPropertyChangedEventArgs);
				}
			}
		}

		public Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> ZipPairSetItemRightAction
		{
			get => _zipPairSetItemRightAction;
			set
			{
				if (_zipPairSetItemRightAction != value)
				{
					_zipPairSetItemRightAction = value;
					OnPropertyChanged(Utils.ZipPairSetItemRightActionPropertyChangedEventArgs);
				}
			}
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

		private Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> _zipPairSetItemLeftAction;

		private Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> _zipPairSetItemRightAction;
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

			if (_leftSource != null || _rightSource != null)
			{
				baseClearItems();				
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
					_lastProcessedLeftSourceChangeMarker = _leftSourceAsObservableCollectionWithChangeMarker.ChangeMarker;
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
					_lastProcessedRightSourceChangeMarker = _rightSourceAsObservableCollectionWithChangeMarker.ChangeMarker;
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

				for (int index = 0; index < countLeft; index++)
				{
					TLeftSourceItem sourceItemLeft = _leftSourceAsList[index];

					if (index < countRight)
					{
						TRightSourceItem sourceItemRight = _rightSourceAsList[index];
						ZipPair<TLeftSourceItem, TRightSourceItem> zipPair = new ZipPair<TLeftSourceItem, TRightSourceItem>(this, sourceItemLeft, sourceItemRight);
						baseInsertItem(index, zipPair);
					}
					else
					{
						break;
					}
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

		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent();
			_consistent = false;

			initializeFromSources();

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void handleLeftSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_leftSourceIndexerPropertyChangedEventRaised || _leftSourceAsObservableCollectionWithChangeMarker != null && _lastProcessedLeftSourceChangeMarker != _leftSourceAsObservableCollectionWithChangeMarker.ChangeMarker)
			{
				_leftSourceIndexerPropertyChangedEventRaised = false;
				_lastProcessedLeftSourceChangeMarker = !_lastProcessedLeftSourceChangeMarker;

				checkConsistent();
				_consistent = false;

				int newIndex;
				int oldIndex;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
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
						if (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
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
						if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
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

				_consistent = true;
				raiseConsistencyRestored();
			}
		}

		private void handleRightSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_rigthtSourceIndexerPropertyChangedEventRaised || _rightSourceAsObservableCollectionWithChangeMarker != null && _lastProcessedRightSourceChangeMarker != _rightSourceAsObservableCollectionWithChangeMarker.ChangeMarker)
			{
				_rigthtSourceIndexerPropertyChangedEventRaised = false;
				_lastProcessedRightSourceChangeMarker = !_lastProcessedRightSourceChangeMarker;

				checkConsistent();
				_consistent = false;

				int newIndex;
				int oldIndex;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
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
						if (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
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
						if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
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

				_consistent = true;
				raiseConsistencyRestored();
			}
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
					if (!EqualityComparer<TLeftSourceItem>.Default.Equals(this[index].ItemLeft, sourceLeft[index]))
						throw new ObservableComputationsException("Consistency violation: Zipping.Left");

					if (!EqualityComparer<TRightSourceItem>.Default.Equals(this[index].ItemRight, sourceRight[index]))
						throw new ObservableComputationsException("Consistency violation: Zipping.Right");
				}
				else
				{
					break;
				}
			}
		}

	}

	public class ZipPair<TLeftSourceItem, TRightSourceItem> : INotifyPropertyChanged, IEquatable<ZipPair<TLeftSourceItem, TRightSourceItem>>
	{
		readonly EqualityComparer<TLeftSourceItem> _sourceItemLeftEqualityComparer = EqualityComparer<TLeftSourceItem>.Default;
		readonly EqualityComparer<TRightSourceItem> _sourceItemRightEqualityComparer = EqualityComparer<TRightSourceItem>.Default;

		private TLeftSourceItem _itemLeft;

		public TLeftSourceItem ItemLeft
		{
			get => _itemLeft;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _zipping.ZipPairSetItemLeftAction(this, value);
		}

		private TRightSourceItem _itemRight;
		public TRightSourceItem ItemRight
		{
			get => _itemRight;
			// ReSharper disable once MemberCanBePrivate.Global
			set =>  _zipping.ZipPairSetItemRightAction(this, value);
		}

		internal void setItemLeft(TLeftSourceItem itemLeft)
		{
			_itemLeft = itemLeft;
			PropertyChanged?.Invoke(this, Utils.ItemLeftPropertyChangedEventArgs);
		}

		internal void setItemRight(TRightSourceItem itemRight)
		{
			_itemRight = itemRight;
			PropertyChanged?.Invoke(this, Utils.ItemRightPropertyChangedEventArgs);
		}

		readonly Zipping<TLeftSourceItem, TRightSourceItem> _zipping;

		public ZipPair(Zipping<TLeftSourceItem, TRightSourceItem> zipping, TLeftSourceItem itemLeft, TRightSourceItem itemRight)
		{
			_itemLeft = itemLeft;
			_itemRight = itemRight;
			_zipping = zipping;
		}

		#region INotifyPropertyChanged imlementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public override int GetHashCode()
		{
			return _sourceItemLeftEqualityComparer.GetHashCode(ItemLeft) +_sourceItemRightEqualityComparer.GetHashCode(ItemRight);
		}

		public override bool Equals(object obj)
		{
			return obj is ZipPair<TLeftSourceItem, TRightSourceItem> other && Equals(other);
		}

		public bool Equals(ZipPair<TLeftSourceItem, TRightSourceItem> other)
		{
			return other != null && (_sourceItemLeftEqualityComparer.Equals(ItemLeft, other.ItemLeft) && _sourceItemRightEqualityComparer.Equals(ItemRight, other.ItemRight));
		}

		public override string ToString()
		{
			return $"ZipPair: ItemLeft = {(ItemLeft != null ? $"{ItemLeft.ToString()}" : "null")}    ItemRight = {(ItemRight != null ? $"{ItemRight.ToString()}" : "null")}";
		}
	}


}