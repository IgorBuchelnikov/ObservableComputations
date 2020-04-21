using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class Crossing<TOuterSourceItem, TInnerSourceItem> : CollectionComputing<JoinPair<TOuterSourceItem, TInnerSourceItem>>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedMember.Global
		public IReadScalar<INotifyCollectionChanged> OuterSourceScalar => _outerSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedMember.Global
		public IReadScalar<INotifyCollectionChanged> InnerSourceScalar => _innerSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedMember.Global
		public INotifyCollectionChanged OuterSource => _outerSource;

		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedMember.Global
		public INotifyCollectionChanged InnerSource => _innerSource;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{OuterSource, InnerSource});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{OuterSourceScalar, InnerSourceScalar});

		private PropertyChangedEventHandler _outerSourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _outerSourceScalarWeakPropertyChangedEventHandler;

		private IList<TOuterSourceItem> _outerSourceAsList;

		private PropertyChangedEventHandler _innerSourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _innerSourceScalarWeakPropertyChangedEventHandler;

		private IList<TInnerSourceItem> _innerSourceAsList;

		private NotifyCollectionChangedEventHandler _outerSourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _outerSourceWeakNotifyCollectionChangedEventHandler;

		private NotifyCollectionChangedEventHandler _innerSourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _innerSourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _outerSourceScalar;
		private readonly IReadScalar<INotifyCollectionChanged> _innerSourceScalar;
		private INotifyCollectionChanged _outerSource;
		private INotifyCollectionChanged _innerSource;

		private PropertyChangedEventHandler _innerSourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _innerSourceWeakPropertyChangedEventHandler;
		private bool _innerSourceIndexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _innerSourceAsINotifyPropertyChanged;

		private PropertyChangedEventHandler _outerSourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _outerSourceWeakPropertyChangedEventHandler;
		private bool _outerSourceIndexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _outerSourceAsINotifyPropertyChanged;

		private ObservableCollectionWithChangeMarker<TOuterSourceItem> _outerSourceAsObservableCollectionWithChangeMarker;
		private bool _lastProcessedOuterSourceChangeMarker;

		private ObservableCollectionWithChangeMarker<TInnerSourceItem> _innerSourceAsObservableCollectionWithChangeMarker;
		private bool _lastProcessedInnerSourceChangeMarker;


		[ObservableComputationsCall]
		public Crossing(
			INotifyCollectionChanged outerSource,
			INotifyCollectionChanged innerSource) : base(Utils.getCapacity(outerSource) * Utils.getCapacity(innerSource))
		{
			_outerSource = outerSource;			
			_innerSource = innerSource;					
			initializeFromSources();
		}

		[ObservableComputationsCall]
		public Crossing(
			INotifyCollectionChanged outerSource,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar) : base(Utils.getCapacity(outerSource) * Utils.getCapacity(innerSourceScalar))
		{
			_outerSource = outerSource;
			_innerSourceScalar = innerSourceScalar;
			initializeInnerSourceScalar();		
					
			initializeFromSources();
		}

		[ObservableComputationsCall]
		public Crossing(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			INotifyCollectionChanged innerSource) : base(Utils.getCapacity(outerSourceScalar) * Utils.getCapacity(innerSource))
		{
			_outerSourceScalar = outerSourceScalar;
			initializeOuterSourceScalar();
			
			_innerSource = innerSource;	
			
			initializeFromSources();
		}

		[ObservableComputationsCall]
		public Crossing(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar) : base(Utils.getCapacity(outerSourceScalar) * Utils.getCapacity(innerSourceScalar))
		{
			_outerSourceScalar = outerSourceScalar;
			initializeOuterSourceScalar();

			_innerSourceScalar = innerSourceScalar;
			initializeInnerSourceScalar();

			initializeFromSources();
		}

		private void initializeInnerSourceScalar()
		{
			_innerSourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_innerSourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_innerSourceScalarPropertyChangedEventHandler);
			_innerSourceScalar.PropertyChanged += _innerSourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		private void initializeOuterSourceScalar()
		{
			_outerSourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_outerSourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_outerSourceScalarPropertyChangedEventHandler);
			_outerSourceScalar.PropertyChanged += _outerSourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		private void initializeFromSources()
		{
			if (_outerSourceNotifyCollectionChangedEventHandler != null)
			{
				_outerSource.CollectionChanged -= _outerSourceWeakNotifyCollectionChangedEventHandler.Handle;
				_outerSourceNotifyCollectionChangedEventHandler = null;
				_outerSourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_innerSourceNotifyCollectionChangedEventHandler != null)
			{
				_innerSource.CollectionChanged -= _innerSourceWeakNotifyCollectionChangedEventHandler.Handle;
				_innerSourceNotifyCollectionChangedEventHandler = null;
				_innerSourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_innerSourceAsINotifyPropertyChanged != null)
			{
				_innerSourceAsINotifyPropertyChanged.PropertyChanged -=
					_innerSourceWeakPropertyChangedEventHandler.Handle;

				_innerSourceAsINotifyPropertyChanged = null;
				_innerSourcePropertyChangedEventHandler = null;
				_innerSourceWeakPropertyChangedEventHandler = null;
			}

			if (_outerSourceAsINotifyPropertyChanged != null)
			{
				_outerSourceAsINotifyPropertyChanged.PropertyChanged -=
					_outerSourceWeakPropertyChangedEventHandler.Handle;

				_outerSourceAsINotifyPropertyChanged = null;
				_outerSourcePropertyChangedEventHandler = null;
				_outerSourceWeakPropertyChangedEventHandler = null;
			}


			if (_outerSource != null || _innerSource != null)
			{
				baseClearItems();				
			}

			if (_outerSourceScalar != null) _outerSource = _outerSourceScalar.Value;
			_outerSourceAsList = (IList<TOuterSourceItem>) _outerSource;

			if (_innerSourceScalar != null) _innerSource = _innerSourceScalar.Value;
			_innerSourceAsList = (IList<TInnerSourceItem>) _innerSource;

			if (_outerSource != null && _innerSource != null)
			{
				_outerSourceAsObservableCollectionWithChangeMarker = _outerSourceAsList as ObservableCollectionWithChangeMarker<TOuterSourceItem>;

				if (_outerSourceAsObservableCollectionWithChangeMarker != null)
				{
					_lastProcessedOuterSourceChangeMarker = _outerSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField;
				}
				else
				{
					_outerSourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _outerSource;

					_outerSourcePropertyChangedEventHandler = (sender, args) =>
					{
						if (args.PropertyName == "Item[]") _outerSourceIndexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_outerSourceWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_outerSourcePropertyChangedEventHandler);

					_outerSourceAsINotifyPropertyChanged.PropertyChanged +=
						_outerSourceWeakPropertyChangedEventHandler.Handle;
				}


				_innerSourceAsObservableCollectionWithChangeMarker = _innerSourceAsList as ObservableCollectionWithChangeMarker<TInnerSourceItem>;

				if (_innerSourceAsObservableCollectionWithChangeMarker != null)
				{
					_lastProcessedInnerSourceChangeMarker = _innerSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField;
				}
				else
				{
					_innerSourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _innerSource;

					_innerSourcePropertyChangedEventHandler = (sender, args) =>
					{
						if (args.PropertyName == "Item[]") _innerSourceIndexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_innerSourceWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_innerSourcePropertyChangedEventHandler);

					_innerSourceAsINotifyPropertyChanged.PropertyChanged +=
						_innerSourceWeakPropertyChangedEventHandler.Handle;
				}


				// ReSharper disable once PossibleNullReferenceException
				int outerSourceCount = _outerSourceAsList.Count;
				// ReSharper disable once PossibleNullReferenceException
				int innerSourceCount = _innerSourceAsList.Count;
				int baseIndex = 0;
				for (int outerSourceIndex = 0; outerSourceIndex < outerSourceCount; outerSourceIndex++)
				{
					TOuterSourceItem sourceOuterItem = _outerSourceAsList[outerSourceIndex];
					for (int innerSourceIndex = 0; innerSourceIndex < innerSourceCount; innerSourceIndex++)
					{
						TInnerSourceItem sourceInnerItem = _innerSourceAsList[innerSourceIndex];
						JoinPair<TOuterSourceItem, TInnerSourceItem> joinPair = new JoinPair<TOuterSourceItem, TInnerSourceItem>(
							sourceOuterItem, sourceInnerItem);
						baseInsertItem(baseIndex + innerSourceIndex, joinPair);
					}

					baseIndex = baseIndex + innerSourceCount;
				}

				_outerSourceNotifyCollectionChangedEventHandler = handleOuterSourceCollectionChanged;
				_outerSourceWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_outerSourceNotifyCollectionChangedEventHandler);

				_outerSource.CollectionChanged += _outerSourceWeakNotifyCollectionChangedEventHandler.Handle;				

				_innerSourceNotifyCollectionChangedEventHandler = handleInnerSourceCollectionChanged;
				_innerSourceWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_innerSourceNotifyCollectionChangedEventHandler);

				_innerSource.CollectionChanged += _innerSourceWeakNotifyCollectionChangedEventHandler.Handle;				
			}
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent();
		
			_isConsistent = false;

			initializeFromSources();

			_isConsistent = true;
			raiseConsistencyRestored();
		}

		private void handleOuterSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			if (_outerSourceIndexerPropertyChangedEventRaised || _outerSourceAsObservableCollectionWithChangeMarker != null && _lastProcessedOuterSourceChangeMarker != _outerSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField)
			{
				_outerSourceIndexerPropertyChangedEventRaised = false;
				_lastProcessedOuterSourceChangeMarker = !_lastProcessedOuterSourceChangeMarker;


				_isConsistent = false;

				int newIndex;
				int oldIndex;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
						newIndex = e.NewStartingIndex;
						TOuterSourceItem sourceOuterItem = _outerSourceAsList[newIndex];


						int count = _innerSourceAsList.Count;
						int baseIndex = newIndex * count;

						for (int innerSourceIndex = 0; innerSourceIndex < count; innerSourceIndex++)
						{
							TInnerSourceItem sourceInnerItem = _innerSourceAsList[innerSourceIndex];
							JoinPair<TOuterSourceItem, TInnerSourceItem> joinPair = new JoinPair<TOuterSourceItem, TInnerSourceItem>(
								sourceOuterItem, sourceInnerItem);

							baseInsertItem(baseIndex + innerSourceIndex, joinPair);												
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
						oldIndex = e.OldStartingIndex;

						int count1 = _innerSourceAsList.Count;
						int baseIndex1 = oldIndex * count1;
						for (int innerSourceIndex = count1 - 1; innerSourceIndex >= 0; innerSourceIndex--)
						{
							baseRemoveItem(baseIndex1 + innerSourceIndex);												
						}
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
						newIndex = e.NewStartingIndex;
						TOuterSourceItem sourceItem3 = _outerSourceAsList[newIndex];

						int count2 = _innerSourceAsList.Count;
						int baseIndex2 = newIndex * count2;
						for (int innerSourceIndex = 0; innerSourceIndex < count2; innerSourceIndex++)
						{
							this[baseIndex2 + innerSourceIndex].setOuterItem(sourceItem3);											
						}
						break;
					case NotifyCollectionChangedAction.Move:
						newIndex = e.NewStartingIndex;
						oldIndex = e.OldStartingIndex;
						if (newIndex != oldIndex)
						{
							int count3 = _innerSourceAsList.Count;
							int oldResultIndex = oldIndex * count3;
							int newBaseIndex = newIndex * count3;
							if (oldIndex < newIndex)
							{
								for (int index = 0; index < count3; index++)
								{
									baseMoveItem(oldResultIndex, newBaseIndex + count3 - 1);
								}						
							}
							else
							{
								for (int index = 0; index < count3; index++)
								{
									baseMoveItem(oldResultIndex + index, newBaseIndex + index);
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
		}

		private void handleInnerSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			if (_innerSourceIndexerPropertyChangedEventRaised || _lastProcessedInnerSourceChangeMarker != _innerSourceAsObservableCollectionWithChangeMarker.ChangeMarkerField)
			{
				_innerSourceIndexerPropertyChangedEventRaised = false;
				_lastProcessedInnerSourceChangeMarker = !_lastProcessedInnerSourceChangeMarker;


				_isConsistent = false;

				int newIndex;
				int oldIndex;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
						newIndex = e.NewStartingIndex;
						TInnerSourceItem sourceInnerItem = _innerSourceAsList[newIndex];
		
						int index1 = newIndex;
						int outerSourceCount1 = _outerSourceAsList.Count;
						int innerSourceCount1 = _innerSourceAsList.Count;
						for (int outerSourceIndex = 0; outerSourceIndex < outerSourceCount1; outerSourceIndex++)
						{
							TOuterSourceItem sourceOuterItem = _outerSourceAsList[outerSourceIndex];
							JoinPair<TOuterSourceItem, TInnerSourceItem> joinPair = new JoinPair<TOuterSourceItem, TInnerSourceItem>(
								sourceOuterItem, sourceInnerItem);

							baseInsertItem(index1, joinPair);	
						
							index1 = index1 + innerSourceCount1;
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
						oldIndex = e.OldStartingIndex;
						int outerSourceCount3 = _outerSourceAsList.Count;
						int oldInnerSourceCount = _innerSourceAsList.Count + 1;
						int baseIndex = (outerSourceCount3 - 1) * oldInnerSourceCount;
						for (int outerSourceIndex =  outerSourceCount3 - 1; outerSourceIndex >= 0; outerSourceIndex--)
						{
							baseRemoveItem(baseIndex + oldIndex);

							baseIndex = baseIndex - oldInnerSourceCount;
						}	
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
						newIndex = e.NewStartingIndex;
						TInnerSourceItem sourceItem3 = _innerSourceAsList[newIndex];	
						int index2 = newIndex;
						int outerSourceCount2 = _outerSourceAsList.Count;
						int innerSourceCount2 = _innerSourceAsList.Count;
						for (int outerSourceIndex = 0; outerSourceIndex < outerSourceCount2; outerSourceIndex++)
						{
					
							this[index2].setInnerItem(sourceItem3);
							index2 = index2 + innerSourceCount2;
						}
						break;
					case NotifyCollectionChangedAction.Move:
						newIndex = e.NewStartingIndex;
						oldIndex = e.OldStartingIndex;

						if (newIndex != oldIndex)
						{
							int index = 0;
							int outerSourceCount = _outerSourceAsList.Count;
							int innerSourceCount = _innerSourceAsList.Count;
							for (int outerSourceIndex = 0; outerSourceIndex < outerSourceCount; outerSourceIndex++)
							{
					
								baseMoveItem(index + oldIndex, index + newIndex);
								index = index + innerSourceCount;
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
		}

		~Crossing()
		{
			if (_outerSourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_outerSource.CollectionChanged -= _outerSourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_innerSourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_innerSource.CollectionChanged -= _innerSourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_innerSourceScalarWeakPropertyChangedEventHandler != null)
			{
				_innerSourceScalar.PropertyChanged -= _innerSourceScalarWeakPropertyChangedEventHandler.Handle;
			}

			if (_outerSourceScalarWeakPropertyChangedEventHandler != null)
			{
				_outerSourceScalar.PropertyChanged -= _outerSourceScalarWeakPropertyChangedEventHandler.Handle;
			}


			if (_outerSourceAsINotifyPropertyChanged != null)
				_outerSourceAsINotifyPropertyChanged.PropertyChanged -=
					_outerSourceWeakPropertyChangedEventHandler.Handle;

			if (_innerSourceAsINotifyPropertyChanged != null)
				_innerSourceAsINotifyPropertyChanged.PropertyChanged -=
					_innerSourceWeakPropertyChangedEventHandler.Handle;
		} 

		public void ValidateConsistency()
		{
			IList<TOuterSourceItem> outerSource = _outerSourceScalar.getValue(_outerSource, new ObservableCollection<TOuterSourceItem>()) as IList<TOuterSourceItem>;
			IList<TInnerSourceItem> innerSource = _innerSourceScalar.getValue(_innerSource, new ObservableCollection<TInnerSourceItem>()) as IList<TInnerSourceItem>;

			int index = 0;
			// ReSharper disable once PossibleNullReferenceException
			for (int index1 = 0; index1 < outerSource.Count; index1++)
			{
				TOuterSourceItem sourceOuterItem = outerSource[index1];

				// ReSharper disable once PossibleNullReferenceException
				for (int index2 = 0; index2 < innerSource.Count; index2++)
				{
					TInnerSourceItem sourceInnerItem = innerSource[index2];

					JoinPair<TOuterSourceItem, TInnerSourceItem> joinPair = this[index];

					if (!EqualityComparer<TOuterSourceItem>.Default.Equals(joinPair.OuterItem, sourceOuterItem))
						throw new ObservableComputationsException(this, "Consistency violation: Crossing.1");

					if (!EqualityComparer<TInnerSourceItem>.Default.Equals(joinPair.InnerItem, sourceInnerItem))
						throw new ObservableComputationsException(this, "Consistency violation: Crossing.2");

					index++;
				}
			}

			// ReSharper disable once PossibleNullReferenceException
			if (Count != outerSource.Count * innerSource.Count)
				throw new ObservableComputationsException(this, "Consistency violation: Crossing.3");
		}

	}
}
