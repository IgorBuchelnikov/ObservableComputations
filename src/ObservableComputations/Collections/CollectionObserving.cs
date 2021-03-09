// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;

//namespace ObservableComputations
//{
//	public class CollectionObserving<TSourceItem> : CollectionComputing<TSourceItem>, IHasSources
//	{
//		public virtual INotifyCollectionChanged Source => _source;
//		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

//		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
//		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

//		private INotifyCollectionChanged _source;
//		private IList<TSourceItem> _sourceAsList;
//		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
//		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
//		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
//		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

//		private bool _sourceInitialized;
//		private Weakbool _sourceInitialized;

//		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
//		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
//		private bool _indexerPropertyChangedEventRaised;
//		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;


//		private IHasChangeMarker _sourceAsIHasChangeMarker;
//		private bool _lastProcessedSourceChangeMarker;


//		[ObservableComputationsCall]
//		public CollectionObserving(
//			IReadScalar<INotifyCollectionChanged> sourceScalar) 
//		{
//			_sourceScalar = sourceScalar;
//			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
//			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
//			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

//			initializeFromSource();
//		}

//		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
//		{
//			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
//			checkConsistent(sender, e);

//			_handledEventSender = sender;
//			_handledEventArgs = e;

//			initializeFromSource();

//			_handledEventSender = null;
//			_handledEventArgs = null;
//		}

//		private void initializeFromSource()
//		{
//			int originalSourceCount = 0;
//			if (_sourceNotifyCollectionChangedEventHandler != null)
//			{
//				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
//				_sourceNotifyCollectionChangedEventHandler = null;
//				_sourceWeakNotifyCollectionChangedEventHandler = null;
//				originalSourceCount = _sourceAsList.Count;
//			}

//			if (_sourceAsINotifyPropertyChanged != null)
//			{
//				_sourceAsINotifyPropertyChanged.PropertyChanged -=
//					_sourceWeakPropertyChangedEventHandler.Handle;

//				_sourceAsINotifyPropertyChanged = null;
//				_sourcePropertyChangedEventHandler = null;
//				_sourceWeakPropertyChangedEventHandler = null;
//			}

//			if (_sourceScalar != null) _source = _sourceScalar.Value;
//			_sourceAsList = _source as IList<TSourceItem>;

//			if (_sourceAsList != null)
//			{
//				_sourceAsIHasChangeMarker = _sourceAsList as IHasChangeMarker;

//				if (_sourceAsIHasChangeMarker != null)
//				{
//					_lastProcessedSourceChangeMarker = _sourceAsIHasChangeMarker.ChangeMarker;
//				}
//				else
//				{
//					_sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourceAsList;

//					_sourcePropertyChangedEventHandler = (sender1, args1) =>
//					{
//						if (args1.PropertyName == "Item[]")
//							_indexerPropertyChangedEventRaised =
//								true; // ObservableCollection raises this before CollectionChanged event raising
//					};

//					_sourceWeakPropertyChangedEventHandler =
//						new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

//					_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;
//				}

//				int count = _sourceAsList.Count;
//				int sourceIndex;

//				for (sourceIndex = 0; sourceIndex < count && sourceIndex < originalSourceCount; sourceIndex++)
//				{
//					TSourceItem sourceItem = _sourceAsList[sourceIndex];
//					baseSetItem(sourceIndex, sourceItem);
//				}

//				for (sourceIndex = originalSourceCount; sourceIndex < count; sourceIndex++)
//				{
//					TSourceItem sourceItem = _sourceAsList[sourceIndex];
//					baseInsertItem(sourceIndex, sourceItem);
//				}

//				for (sourceIndex = count; sourceIndex < originalSourceCount; sourceIndex++)
//				{
//					baseRemoveItem(count);
//				}

//				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
//				_sourceWeakNotifyCollectionChangedEventHandler =
//					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);
//				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
//			}
//		}

//		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//		{
//			checkConsistent(sender, e);

//			_handledEventSender = sender;
//			_handledEventArgs = e;

//			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsIHasChangeMarker.ChangeMarker)
//			{
//				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;
//				_indexerPropertyChangedEventRaised = false;

//				switch (e.Action)
//				{
//					case NotifyCollectionChangedAction.Add:
//						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
//						_isConsistent = false;
//						baseInsertItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
//						_isConsistent = true;
//						raiseConsistencyRestored();
//						break;
//					case NotifyCollectionChangedAction.Remove:
//						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
//						_isConsistent = false;

//						baseRemoveItem(e.OldStartingIndex);
//						_isConsistent = true;
//						raiseConsistencyRestored();
//						break;
//					case NotifyCollectionChangedAction.Replace:
//						_isConsistent = false;
//						baseSetItem(e.NewStartingIndex,  (TSourceItem) e.NewItems[0]);
//						_isConsistent = true;
//						raiseConsistencyRestored();						
//						break;
//					case NotifyCollectionChangedAction.Move:
//						int oldStartingIndex1 = e.OldStartingIndex;
//						int newStartingIndex1 = e.NewStartingIndex;

//						if (oldStartingIndex1 == newStartingIndex1) return;

//						_isConsistent = false;
//						baseMoveItem(oldStartingIndex1, newStartingIndex1);
//						_isConsistent = true;
//						raiseConsistencyRestored();

//						break;
//					case NotifyCollectionChangedAction.Reset:
//						_isConsistent = false;
//						initializeFromSource();
//						_isConsistent = true;
//						raiseConsistencyRestored();

//						break;
//				}
//			}

//			_handledEventSender = null;
//			_handledEventArgs = null;
//		}

//		~CollectionObserving()
//		{
//			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
//			{
//				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
//			}

//			if (_sourceScalarWeakPropertyChangedEventHandler != null)
//			{
//				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
//			}
//		}

//		public void ValidateInternalConsistency()
//		{

//		}
//	}
//}
