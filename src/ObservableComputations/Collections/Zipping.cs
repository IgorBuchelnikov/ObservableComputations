// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace ObservableComputations
{
	public class Zipping<TLeftSourceItem, TRightSourceItem> : CollectionComputing<ZipPair<TLeftSourceItem, TRightSourceItem>>, IHasSourceCollections, ILeftSourceIndexerPropertyTracker, IRightSourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> LeftSourceScalar => _leftSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> RightSourceScalar => _rightSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged LeftSource => _leftSource;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged RightSource => _rightSource;

		public virtual ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{LeftSource, RightSource});
		public virtual ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{LeftSourceScalar, RightSourceScalar});

		public Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> SetLeftItemRequestHandler
		{
			get => _setLeftItemRequestHandler;
			set
			{
				if (_setLeftItemRequestHandler != value)
				{
					_setLeftItemRequestHandler = value;
					OnPropertyChanged(Utils.SetLeftItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		public Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> SetRightItemRequestHandler
		{
			get => _setRightItemRequestHandler;
			set
			{
				if (_setRightItemRequestHandler != value)
				{
					_setRightItemRequestHandler = value;
					OnPropertyChanged(Utils.SetRightItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}


		private IList<TLeftSourceItem> _leftSourceAsList;


		private IList<TRightSourceItem> _rightSourceAsList;


		internal Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> _setLeftItemRequestHandler;
		internal Action<ZipPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> _setRightItemRequestHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _leftSourceScalar;
		private readonly IReadScalar<INotifyCollectionChanged> _rightSourceScalar;
		private INotifyCollectionChanged _leftSource;
		private INotifyCollectionChanged _rightSource;
		private List<TLeftSourceItem> _leftSourceCopy;
		private List<TRightSourceItem> _rightSourceCopy;

		private bool _leftSourceIndexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _leftSourceAsINotifyPropertyChanged;

		private bool _rightSourceIndexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _rightSourceAsINotifyPropertyChanged;

		private IHasChangeMarker _leftSourceAsIHasChangeMarker;
		private bool _lastProcessedLeftSourceChangeMarker;

		private IHasChangeMarker _rightSourceAsHasChangeMarker;
		private bool _lastProcessedRightSourceChangeMarker;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		private readonly PropertyChangedEventHandler _leftSourceScalarOnPropertyChanged;
		private readonly PropertyChangedEventHandler _rightSourceScalarOnPropertyChanged;

		[ObservableComputationsCall]
		public Zipping(
			IReadScalar<INotifyCollectionChanged> leftSourceScalar,
			INotifyCollectionChanged rightSource) : this(calculateCapacity(leftSourceScalar, rightSource))
		{
			_leftSourceScalar = leftSourceScalar;
			_rightSource = rightSource;	
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public Zipping(
			IReadScalar<INotifyCollectionChanged> leftSourceScalar,
			IReadScalar<INotifyCollectionChanged> rightSourceScalar) : this(calculateCapacity(leftSourceScalar, rightSourceScalar))
		{
			_leftSourceScalar = leftSourceScalar;
			_rightSourceScalar = rightSourceScalar;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public Zipping(
			INotifyCollectionChanged leftSource,
			INotifyCollectionChanged rightSource) : this(calculateCapacity(leftSource, rightSource))
		{
			_leftSource = leftSource;			
			_rightSource = rightSource;		
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public Zipping(
			INotifyCollectionChanged leftSource,
			IReadScalar<INotifyCollectionChanged> rightSourceScalar) : this(calculateCapacity(leftSource, rightSourceScalar))
		{
			_leftSource = leftSource;			
			_rightSourceScalar = rightSourceScalar;
			_thisAsSourceCollectionChangeProcessor = this;

		}

		private Zipping(int capacity) : base(capacity)
		{
			if (_leftSourceScalar != null)
				_leftSourceScalarOnPropertyChanged = getScalarValueChangedHandler(null, () => processSource(true, false));

			if (_rightSourceScalar != null)
				_rightSourceScalarOnPropertyChanged = getScalarValueChangedHandler(null, () => processSource(false, true));

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

		protected override void processSource()
		{
			processSource(true, true);
		}

		private void processSource(bool replaceLeftSource, bool replaceRightSource)
		{
			int originalCount = _items.Count;

			if (_sourceReadAndSubscribed)
			{
				if (_leftSource != null)
				{
					if (replaceLeftSource)
					{
						_leftSource.CollectionChanged -= handleLeftSourceCollectionChanged;

						if (_leftSourceAsINotifyPropertyChanged != null)
						{
							_leftSourceAsINotifyPropertyChanged.PropertyChanged -=
								((ILeftSourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
							_leftSourceAsINotifyPropertyChanged = null;
						}
					}
				}


				if (_rightSource != null)
				{
					if (replaceRightSource)
					{
						_rightSource.CollectionChanged -= handleLeftSourceCollectionChanged;

						if (_rightSourceAsINotifyPropertyChanged != null)
						{
							_rightSourceAsINotifyPropertyChanged.PropertyChanged -=
								((IRightSourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
							_rightSourceAsINotifyPropertyChanged = null;
						}
					}
				}

				_leftSourceCopy = null;
				_rightSourceCopy = null;

				_sourceReadAndSubscribed = false;
			}

			if (replaceLeftSource)
				Utils.replaceSource(ref _leftSource, _leftSourceScalar, _downstreamConsumedComputings, _consumers, this,
					out _leftSourceAsList, true);

			if (replaceRightSource)
				Utils.replaceSource(ref _rightSource, _rightSourceScalar, _downstreamConsumedComputings, _consumers, this,
					out _rightSourceAsList, true);


			if (_leftSourceAsList != null && _rightSourceAsList != null && _isActive)
			{
				if (replaceLeftSource)
					Utils.subscribeSource(
						out _leftSourceAsIHasChangeMarker, 
						_leftSourceAsList, 
						ref _lastProcessedLeftSourceChangeMarker, 
						ref _leftSourceAsINotifyPropertyChanged,
						(ILeftSourceIndexerPropertyTracker)this,
						_leftSource,
						handleLeftSourceCollectionChanged);

				if (replaceRightSource)
					Utils.subscribeSource(
						out _rightSourceAsHasChangeMarker, 
						_rightSourceAsList, 
						ref _lastProcessedRightSourceChangeMarker, 
						ref _rightSourceAsINotifyPropertyChanged,
						(IRightSourceIndexerPropertyTracker)this,
						_rightSource,
						handleRightSourceCollectionChanged);


				int countLeft = _leftSourceAsList.Count;
				int countRight = _rightSourceAsList.Count;

				_leftSourceCopy = new List<TLeftSourceItem>(_leftSourceAsList);
				_rightSourceCopy = new List<TRightSourceItem>(_rightSourceAsList);

				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < countLeft; sourceIndex++)
				{
					if (sourceIndex < countRight)
					{
						if (originalCount > sourceIndex)
							_items[sourceIndex] = new ZipPair<TLeftSourceItem, TRightSourceItem>(
								this, 
								_leftSourceCopy[sourceIndex], 
								_rightSourceCopy[sourceIndex]);
						else
							_items.Insert(sourceIndex, new ZipPair<TLeftSourceItem, TRightSourceItem>(
								this, 
								_leftSourceCopy[sourceIndex], 
								_rightSourceCopy[sourceIndex]));					
					}
					else
					{
						break;
					}
				}

				for (int index = originalCount - 1; index >= sourceIndex; index--)
					_items.RemoveAt(index);

				_sourceReadAndSubscribed = true;

			}
			else
			{
				_items.Clear();
			}

			reset();
		}

		private void handleLeftSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _leftSourceIndexerPropertyChangedEventRaised, 
				ref _lastProcessedLeftSourceChangeMarker, 
				_leftSourceAsIHasChangeMarker, 
				ref _handledEventSender, 
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, this)) return;

			_thisAsSourceCollectionChangeProcessor.processSourceCollectionChanged(sender, e);

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		void ISourceCollectionChangeProcessor.processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			int newIndex;
			int oldIndex;

			if (ReferenceEquals(sender, _leftSource))
			{
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
						newIndex = e.NewStartingIndex;
						_leftSourceCopy.Insert(newIndex, (TLeftSourceItem) e.NewItems[0]);

						//if (newIndex < this.Count)
						//{
						int sourceLeftCount = _leftSourceCopy.Count;
						int sourceRightCount = _rightSourceCopy.Count;
						int thisCount = Count;
						for (int sourceIndex = newIndex; sourceIndex < sourceLeftCount; sourceIndex++)
						{
							if (sourceIndex < sourceRightCount)
							{
								if (sourceIndex < thisCount)
								{
									this[sourceIndex].setItemLeft(_leftSourceCopy[sourceIndex]);
								}
								else
								{
									baseInsertItem(sourceIndex,
										new ZipPair<TLeftSourceItem, TRightSourceItem>(this, _leftSourceCopy[sourceIndex],
											_rightSourceCopy[sourceIndex]));
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
						//	if (newIndex < _rightSourceCopy.Count)
						//	{
						//		baseInsertItem(this.Count, new ZipPair<TLeftSourceItem, TRightSourceItem>(_leftSourceCopy[newIndex], _rightSourceCopy[newIndex]));
						//	}		
						//}
						break;
					case NotifyCollectionChangedAction.Remove:
						//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
						oldIndex = e.OldStartingIndex;
						_leftSourceCopy.RemoveAt(oldIndex);

						int countLeft = _leftSourceCopy.Count;
						int countRight = _rightSourceCopy.Count;
						for (int sourceIndex = oldIndex; sourceIndex < countLeft; sourceIndex++)
						{
							if (sourceIndex < countRight)
							{
								this[sourceIndex].setItemLeft(_leftSourceCopy[sourceIndex]);
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
						_leftSourceCopy[newIndex] = (TLeftSourceItem) e.NewItems[0];

						if (newIndex < Count)
						{
							this[newIndex].setItemLeft(_leftSourceCopy[newIndex]);
						}

						break;
					case NotifyCollectionChangedAction.Move:
						newIndex = e.NewStartingIndex;
						oldIndex = e.OldStartingIndex;

						TLeftSourceItem leftSourceItem = (TLeftSourceItem) e.NewItems[0];
						_leftSourceCopy.RemoveAt(oldIndex);
						_leftSourceCopy.Insert(newIndex, leftSourceItem);

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
								if (sourceIndex < count)
									this[sourceIndex].setItemLeft(_leftSourceCopy[sourceIndex]);
						}

						break;
					case NotifyCollectionChangedAction.Reset:
						processSource(false, false);
						break;
				}
			}
			else //if (ReferenceEquals(sender, _rightSource))
				switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
					newIndex = e.NewStartingIndex;
					_rightSourceCopy.Insert(newIndex, (TRightSourceItem) e.NewItems[0]);
					int countRight = _rightSourceCopy.Count;
					int countLeft = _leftSourceCopy.Count;
					for (int sourceIndex = newIndex; sourceIndex < countRight; sourceIndex++)
					{
						if (sourceIndex < countLeft)
						{
							if (sourceIndex < Count)
								this[sourceIndex].setItemRight(_rightSourceCopy[sourceIndex]);
							else
								baseInsertItem(sourceIndex,
									new ZipPair<TLeftSourceItem, TRightSourceItem>(this, _leftSourceCopy[sourceIndex],
										_rightSourceCopy[sourceIndex]));
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
					_rightSourceCopy.RemoveAt(oldIndex);
					int sourceRightCount = _rightSourceCopy.Count;
					int sourceLeftCount = _leftSourceCopy.Count;
					for (int sourceIndex = oldIndex; sourceIndex < sourceRightCount; sourceIndex++)
					{

						if (sourceIndex < sourceLeftCount)
						{
							this[sourceIndex].setItemRight(_rightSourceCopy[sourceIndex]);
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
					_rightSourceCopy[newIndex] = (TRightSourceItem) e.NewItems[0];
					if (newIndex < Count)
					{
						this[newIndex].setItemRight(_rightSourceCopy[newIndex]);
					}

					break;
				case NotifyCollectionChangedAction.Move:
					newIndex = e.NewStartingIndex;
					oldIndex = e.OldStartingIndex;

					TRightSourceItem rightSourceItem = (TRightSourceItem) e.NewItems[0];
					_rightSourceCopy.RemoveAt(oldIndex);
					_rightSourceCopy.Insert(newIndex, rightSourceItem);

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
						if (sourceIndex < thisCount)
							this[sourceIndex].setItemRight(_rightSourceCopy[sourceIndex]);

					break;
				case NotifyCollectionChangedAction.Reset:
					processSource(false, false);
					break;
			}
		}

		private void handleRightSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _rightSourceIndexerPropertyChangedEventRaised, 
				ref _lastProcessedRightSourceChangeMarker, 
				_rightSourceAsHasChangeMarker, 
				ref _handledEventSender, 
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, this)) return;

			_thisAsSourceCollectionChangeProcessor.processSourceCollectionChanged(sender, e);

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _leftSourceScalar, _leftSource);
			Utils.AddDownstreamConsumedComputing(computing, _rightSourceScalar, _leftSource);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _leftSourceScalar, _rightSource);
			Utils.RemoveDownstreamConsumedComputing(computing, _rightSourceScalar, _rightSource);
		}

		protected override void initialize()
		{
			Utils.initializeSourceScalar(_leftSourceScalar, ref _leftSource, _leftSourceScalarOnPropertyChanged);
			Utils.initializeSourceScalar(_rightSourceScalar, ref _rightSource, _rightSourceScalarOnPropertyChanged);
		}

		protected override void uninitialize()
		{
			Utils.uninitializeSourceScalar(_leftSourceScalar, _leftSourceScalarOnPropertyChanged, ref _leftSource);
			Utils.uninitializeSourceScalar(_rightSourceScalar, _rightSourceScalarOnPropertyChanged, ref _rightSource);

		}

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
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

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{

		}

		void IRightSourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _rightSourceIndexerPropertyChangedEventRaised);
		}

		void ILeftSourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _leftSourceIndexerPropertyChangedEventRaised);
		}

		#endregion
	}

	public class ZipPair<TLeftSourceItem, TRightSourceItem> : INotifyPropertyChanged
	{
		private TLeftSourceItem _leftItem;

		public TLeftSourceItem LeftItem
		{
			get => _leftItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set
			{
				if (Configuration.TrackComputingsExecutingUserCode)
				{
					int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out  _zipping._userCodeIsCalledFrom, _zipping);
					_zipping._setLeftItemRequestHandler(this, value);
					Utils.endComputingExecutingUserCode(computing, currentThreadId, out  _zipping._userCodeIsCalledFrom);
					return;
				}

				_zipping._setLeftItemRequestHandler(this, value);
			}
		}

		private TRightSourceItem _rightItem;
		public TRightSourceItem RightItem
		{
			get => _rightItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set
			{
				if (Configuration.TrackComputingsExecutingUserCode)
				{
					int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _zipping._userCodeIsCalledFrom, _zipping);
					_zipping._setRightItemRequestHandler(this, value);
					Utils.endComputingExecutingUserCode(computing, currentThreadId, out  _zipping._userCodeIsCalledFrom);
					return;
				}

				_zipping._setRightItemRequestHandler(this, value);
			}
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
			return $"ZipPair: ItemLeft = {(LeftItem != null ? $"{LeftItem.ToString()}" : "null")}	ItemRight = {(RightItem != null ? $"{RightItem.ToString()}" : "null")}";
		}
	}

	public enum ZipPairAction
	{
		SetLeftItem,
		SetRightItem
	}
}