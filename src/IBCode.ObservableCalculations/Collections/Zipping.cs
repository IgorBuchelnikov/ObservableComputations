using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace IBCode.ObservableCalculations
{
	public class Zipping<TSourceItemLeft, TSourceItemRight> : CollectionCalculating<ZipPair<TSourceItemLeft, TSourceItemRight>>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceLeftScalar => _sourceLeftScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceRightScalar => _sourceRightScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged SourceLeft => _sourceLeft;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged SourceRight => _sourceRight;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{SourceLeft, SourceRight});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceLeftScalar, SourceRightScalar});

		public Action<ZipPair<TSourceItemLeft, TSourceItemRight>, TSourceItemLeft> ZipPairSetItemLeftAction
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

		public Action<ZipPair<TSourceItemLeft, TSourceItemRight>, TSourceItemRight> ZipPairSetItemRightAction
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

		private PropertyChangedEventHandler _sourceLeftScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceLeftScalarWeakPropertyChangedEventHandler;

		private IList<TSourceItemLeft> _sourceLeftAsList;

		private PropertyChangedEventHandler _sourceRightScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceRightScalarWeakPropertyChangedEventHandler;

		private IList<TSourceItemRight> _sourceRightAsList;

		private NotifyCollectionChangedEventHandler _sourceLeftNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceLeftWeakNotifyCollectionChangedEventHandler;

		private NotifyCollectionChangedEventHandler _sourceRightNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceRightWeakNotifyCollectionChangedEventHandler;

		private Action<ZipPair<TSourceItemLeft, TSourceItemRight>, TSourceItemLeft> _zipPairSetItemLeftAction;

		private Action<ZipPair<TSourceItemLeft, TSourceItemRight>, TSourceItemRight> _zipPairSetItemRightAction;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceLeftScalar;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceRightScalar;
		private INotifyCollectionChanged _sourceLeft;
		private INotifyCollectionChanged _sourceRight;

		[ObservableCalculationsCall]
		public Zipping(
			IReadScalar<INotifyCollectionChanged> sourceLeftScalar,
			INotifyCollectionChanged sourceRight) : base(calculateCapacity(sourceLeftScalar, sourceRight))
		{
			_sourceLeftScalar = sourceLeftScalar;
			initializeSourceLeftScalar();
			
			_sourceRight = sourceRight;	
			
			initializeFromSources();
		}

		[ObservableCalculationsCall]
		public Zipping(
			IReadScalar<INotifyCollectionChanged> sourceLeftScalar,
			IReadScalar<INotifyCollectionChanged> sourceRightScalar) : base(calculateCapacity(sourceLeftScalar, sourceRightScalar))
		{
			_sourceLeftScalar = sourceLeftScalar;
			initializeSourceLeftScalar();

			_sourceRightScalar = sourceRightScalar;
			initializeSourceRightScalar();

			initializeFromSources();
		}

		[ObservableCalculationsCall]
		public Zipping(
			INotifyCollectionChanged sourceLeft,
			INotifyCollectionChanged sourceRight) : base(calculateCapacity(sourceLeft, sourceRight))
		{
			_sourceLeft = sourceLeft;			
			_sourceRight = sourceRight;					
			initializeFromSources();
		}

		[ObservableCalculationsCall]
		public Zipping(
			INotifyCollectionChanged sourceLeft,
			IReadScalar<INotifyCollectionChanged> sourceRightScalar) : base(calculateCapacity(sourceLeft, sourceRightScalar))
		{
			_sourceLeft = sourceLeft;
			
			_sourceRightScalar = sourceRightScalar;
			initializeSourceRightScalar();		
					
			initializeFromSources();
		}

		private void initializeSourceRightScalar()
		{
			_sourceRightScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceRightScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceRightScalarPropertyChangedEventHandler);
			_sourceRightScalar.PropertyChanged += _sourceRightScalarWeakPropertyChangedEventHandler.Handle;
		}

		private void initializeSourceLeftScalar()
		{
			_sourceLeftScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceLeftScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceLeftScalarPropertyChangedEventHandler);
			_sourceLeftScalar.PropertyChanged += _sourceLeftScalarWeakPropertyChangedEventHandler.Handle;
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
			if (_sourceLeftNotifyCollectionChangedEventHandler != null)
			{
				_sourceLeft.CollectionChanged -= _sourceLeftWeakNotifyCollectionChangedEventHandler.Handle;
				_sourceLeftNotifyCollectionChangedEventHandler = null;
				_sourceLeftWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceRightNotifyCollectionChangedEventHandler != null)
			{
				_sourceRight.CollectionChanged -= _sourceRightWeakNotifyCollectionChangedEventHandler.Handle;
				_sourceRightNotifyCollectionChangedEventHandler = null;
				_sourceRightWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceLeft != null || _sourceRight != null)
			{
				baseClearItems();				
			}

			if (_sourceLeftScalar != null) _sourceLeft = _sourceLeftScalar.Value;
			_sourceLeftAsList = (IList<TSourceItemLeft>) _sourceLeft;

			if (_sourceRightScalar != null) _sourceRight = _sourceRightScalar.Value;
			_sourceRightAsList = (IList<TSourceItemRight>) _sourceRight;

			if (_sourceLeftAsList != null && _sourceRightAsList != null)
			{
				int countLeft = _sourceLeftAsList.Count;
				int countRight = _sourceRightAsList.Count;

				for (int index = 0; index < countLeft; index++)
				{
					TSourceItemLeft sourceItemLeft = _sourceLeftAsList[index];

					if (index < countRight)
					{
						TSourceItemRight sourceItemRight = _sourceRightAsList[index];
						ZipPair<TSourceItemLeft, TSourceItemRight> zipPair = new ZipPair<TSourceItemLeft, TSourceItemRight>(this, sourceItemLeft, sourceItemRight);
						baseInsertItem(index, zipPair);
					}
					else
					{
						break;
					}
				}
			}

			if (_sourceLeft != null && _sourceRight != null)
			{
				_sourceLeftNotifyCollectionChangedEventHandler = handleSourceLeftCollectionChanged;
				_sourceLeftWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_sourceLeftNotifyCollectionChangedEventHandler);

				_sourceLeft.CollectionChanged += _sourceLeftWeakNotifyCollectionChangedEventHandler.Handle;				

				_sourceRightNotifyCollectionChangedEventHandler = handleSourceRightCollectionChanged;
				_sourceRightWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_sourceRightNotifyCollectionChangedEventHandler);

				_sourceRight.CollectionChanged += _sourceRightWeakNotifyCollectionChangedEventHandler.Handle;				
			}

		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent();
			_consistent = false;
			OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

			initializeFromSources();

			_consistent = true;
			OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
		}

		private void handleSourceLeftCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			int newIndex;
			int oldIndex;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems.Count > 1) throw new ObservableCalculationsException("Adding of multiple items is not supported");
					newIndex = e.NewStartingIndex;
		
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					//if (newIndex < this.Count)
					//{
					int sourceLeftCount = _sourceLeftAsList.Count;
					int sourceRightCount = _sourceRightAsList.Count;
					int thisCount = Count;
					for (int sourceIndex = newIndex; sourceIndex < sourceLeftCount; sourceIndex++)
					{
						if (sourceIndex < sourceRightCount)
						{

							if (sourceIndex < thisCount)
							{
								this[sourceIndex].setItemLeft(_sourceLeftAsList[sourceIndex]);									
							}
							else
							{
								baseInsertItem(sourceIndex, new ZipPair<TSourceItemLeft, TSourceItemRight>(this, _sourceLeftAsList[sourceIndex], _sourceRightAsList[sourceIndex]));
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
					//	if (newIndex < _sourceRightAsList.Count)
					//	{
					//		baseInsertItem(this.Count, new ZipPair<TSourceItemLeft, TSourceItemRight>(_sourceLeftAsList[newIndex], _sourceRightAsList[newIndex]));
					//	}		
					//}

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems.Count > 1) throw new ObservableCalculationsException("Removing of multiple items is not supported");
					oldIndex = e.OldStartingIndex;
		
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					int countLeft = _sourceLeftAsList.Count;
					int countRight = _sourceRightAsList.Count;
					for (int sourceIndex = oldIndex; sourceIndex < countLeft; sourceIndex++)
					{
						if (sourceIndex < countRight)
						{
							this[sourceIndex].setItemLeft(_sourceLeftAsList[sourceIndex]);
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

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
				case NotifyCollectionChangedAction.Replace:
					if (e.NewItems.Count > 1) throw new ObservableCalculationsException("Replacing of multiple items is not supported");
					newIndex = e.NewStartingIndex;

					if (newIndex < Count)
					{
						this[newIndex].setItemLeft(_sourceLeftAsList[newIndex]);
					}
					break;
				case NotifyCollectionChangedAction.Move:
					newIndex = e.NewStartingIndex;
					oldIndex = e.OldStartingIndex;
					if (newIndex == oldIndex) return;
		
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

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
							this[sourceIndex].setItemLeft(_sourceLeftAsList[sourceIndex]);
						}					
					}

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
				case NotifyCollectionChangedAction.Reset:
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					initializeFromSources();

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
			}
		}

		private void handleSourceRightCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			int newIndex;
			int oldIndex;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems.Count > 1) throw new ObservableCalculationsException("Adding of multiple items is not supported");
					newIndex = e.NewStartingIndex;
		
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					int countRight = _sourceRightAsList.Count;
					int countLeft = _sourceLeftAsList.Count;
					for (int sourceIndex = newIndex; sourceIndex < countRight; sourceIndex++)
					{
						if (sourceIndex < countLeft)
						{
							TSourceItemRight sourceItemRight = _sourceRightAsList[sourceIndex];
							if (sourceIndex < Count)
							{
								this[sourceIndex].setItemRight(sourceItemRight);
							}
							else
							{
								baseInsertItem(sourceIndex, new ZipPair<TSourceItemLeft, TSourceItemRight>(this, _sourceLeftAsList[sourceIndex], sourceItemRight));
							}
						}
						else
						{
							break;
						}					
					}

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
				case NotifyCollectionChangedAction.Remove:
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					if (e.OldItems.Count > 1) throw new ObservableCalculationsException("Removing of multiple items is not supported");
					oldIndex = e.OldStartingIndex;
					int sourceRightCount = _sourceRightAsList.Count;
					int sourceLeftCount = _sourceLeftAsList.Count;
					for (int sourceIndex = oldIndex; sourceIndex < sourceRightCount; sourceIndex++)
					{

						if (sourceIndex < sourceLeftCount)
						{
							this[sourceIndex].setItemRight(_sourceRightAsList[sourceIndex]);
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

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);				
					break;
				case NotifyCollectionChangedAction.Replace:
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					if (e.NewItems.Count > 1) throw new ObservableCalculationsException("Replacing of multiple items is not supported");
					newIndex = e.NewStartingIndex;
					if (newIndex < Count)
					{
						this[newIndex].setItemRight(_sourceRightAsList[newIndex]);
					}

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);	
					break;
				case NotifyCollectionChangedAction.Move:
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

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
							this[sourceIndex].setItemRight(_sourceRightAsList[sourceIndex]);
						}					
					}

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
				case NotifyCollectionChangedAction.Reset:
					_consistent = false;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);

					initializeFromSources();

					_consistent = true;
					OnPropertyChanged(Utils.ConsistentPropertyChangedEventArgs);
					break;
			}
		}

		~Zipping()
		{
			if (_sourceLeftWeakNotifyCollectionChangedEventHandler != null)
			{
				_sourceLeft.CollectionChanged -= _sourceLeftWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceRightWeakNotifyCollectionChangedEventHandler != null)
			{
				_sourceRight.CollectionChanged -= _sourceRightWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceLeftScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceLeftScalar.PropertyChanged -= _sourceLeftScalarWeakPropertyChangedEventHandler.Handle;
			}

			if (_sourceRightScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceRightScalar.PropertyChanged -= _sourceRightScalarWeakPropertyChangedEventHandler.Handle;
			}
		} 

		public void ValidateConsistency()
		{
			IList<TSourceItemLeft> sourceLeft = _sourceLeftScalar.getValue(_sourceLeft, new ObservableCollection<TSourceItemLeft>()) as IList<TSourceItemLeft>;
			IList<TSourceItemRight> sourceRight = _sourceRightScalar.getValue(_sourceRight, new ObservableCollection<TSourceItemRight>()) as IList<TSourceItemRight>;

			// ReSharper disable once PossibleNullReferenceException
			for (int index = 0; index < sourceLeft.Count; index++)
			{
				// ReSharper disable once PossibleNullReferenceException
				if (index < sourceRight.Count)
				{
					if (!EqualityComparer<TSourceItemLeft>.Default.Equals(this[index].ItemLeft, sourceLeft[index]))
						throw new ObservableCalculationsException("Consistency violation: Zipping.Left");

					if (!EqualityComparer<TSourceItemRight>.Default.Equals(this[index].ItemRight, sourceRight[index]))
						throw new ObservableCalculationsException("Consistency violation: Zipping.Right");
				}
				else
				{
					break;
				}
			}
		}

	}

	public class ZipPair<TSourceItemLeft, TSourceItemRight> : INotifyPropertyChanged, IEquatable<ZipPair<TSourceItemLeft, TSourceItemRight>>
	{
		readonly EqualityComparer<TSourceItemLeft> _sourceItemLeftEqualityComparer = EqualityComparer<TSourceItemLeft>.Default;
		readonly EqualityComparer<TSourceItemRight> _sourceItemRightEqualityComparer = EqualityComparer<TSourceItemRight>.Default;

		private TSourceItemLeft _itemLeft;

		public TSourceItemLeft ItemLeft
		{
			get => _itemLeft;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _zipping.ZipPairSetItemLeftAction(this, value);
		}

		private TSourceItemRight _itemRight;
		public TSourceItemRight ItemRight
		{
			get => _itemRight;
			// ReSharper disable once MemberCanBePrivate.Global
			set =>  _zipping.ZipPairSetItemRightAction(this, value);
		}

		internal void setItemLeft(TSourceItemLeft itemLeft)
		{
			_itemLeft = itemLeft;
			PropertyChanged?.Invoke(this, Utils.ItemLeftPropertyChangedEventArgs);
		}

		internal void setItemRight(TSourceItemRight itemRight)
		{
			_itemRight = itemRight;
			PropertyChanged?.Invoke(this, Utils.ItemRightPropertyChangedEventArgs);
		}

		readonly Zipping<TSourceItemLeft, TSourceItemRight> _zipping;

		public ZipPair(Zipping<TSourceItemLeft, TSourceItemRight> zipping, TSourceItemLeft itemLeft, TSourceItemRight itemRight)
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
			return obj is ZipPair<TSourceItemLeft, TSourceItemRight> other && Equals(other);
		}

		public bool Equals(ZipPair<TSourceItemLeft, TSourceItemRight> other)
		{
			return other != null && (_sourceItemLeftEqualityComparer.Equals(ItemLeft, other.ItemLeft) && _sourceItemRightEqualityComparer.Equals(ItemRight, other.ItemRight));
		}

		public override string ToString()
		{
			return $"ZipPair: ItemLeft = {(ItemLeft != null ? $"{ItemLeft.ToString()}" : "null")}    ItemRight = {(ItemRight != null ? $"{ItemRight.ToString()}" : "null")}";
		}
	}


}