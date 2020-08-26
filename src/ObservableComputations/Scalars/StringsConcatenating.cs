using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ObservableComputations
{
	public class StringsConcatenating : ScalarComputing<string>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<string> SeparatorScalar => _separatorScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public string Separator => _separator;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private IList<string> _sourceAsList;

		private int _separatorLength;

		private readonly StringBuilder _valueStringBuilder = new StringBuilder();
		private RangePositions<RangePosition> _resultRangePositions;

		private bool _sourceInitialized;

		private bool _moving;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly IReadScalar<string> _separatorScalar;
		private INotifyCollectionChanged _source;
		private string _separator;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

        private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private StringsConcatenating(int capacity)
		{
			_resultRangePositions = new RangePositions<RangePosition>(new List<RangePosition>(capacity * 2));
		}

		[ObservableComputationsCall]
		public StringsConcatenating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<string> separatorScalar = null) : this(Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_separatorScalar = separatorScalar;
		}

		[ObservableComputationsCall]
		public StringsConcatenating(
			INotifyCollectionChanged source,
			IReadScalar<string> separatorScalar = null) : this(Utils.getCapacity(source))
		{
			_source = source;
			_separatorScalar = separatorScalar;
		}

		[ObservableComputationsCall]
		public StringsConcatenating(
			INotifyCollectionChanged source,
			string separator = "") : this(Utils.getCapacity(source))
		{
			_source = source;
			_separator = separator;
		}

		[ObservableComputationsCall]
		public StringsConcatenating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			string separator = "") : this(Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_separator = separator;
		}

		private void initializeSeparatorScalar()
		{
			if (_separatorScalar != null)
				_separatorScalar.PropertyChanged += handleSeparatorScalarValueChanged;
		}

        protected override void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				_valueStringBuilder.Clear();

				int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
				_resultRangePositions = new RangePositions<RangePosition>(new List<RangePosition>(capacity * 2));

				_source.CollectionChanged -= handleSourceCollectionChanged;

                if (_sourceAsINotifyPropertyChanged != null)
                {
                    _sourceAsINotifyPropertyChanged.PropertyChanged -=
                        ((ISourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
                    _sourceAsINotifyPropertyChanged = null;
                }

                _sourceInitialized = false;
            }


            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _sourceAsList, true);

			if (_source != null && _isActive)
			{
                Utils.initializeFromHasChangeMarker(
                    out _sourceAsIHasChangeMarker, 
                    _sourceAsList, 
                    ref _lastProcessedSourceChangeMarker, 
                    ref _sourceAsINotifyPropertyChanged,
                    (ISourceIndexerPropertyTracker)this);

				_source.CollectionChanged += handleSourceCollectionChanged;
                _sourceInitialized = true;
				recalculateValue();
			}
			else
			{
				_valueStringBuilder.Clear();
				setValue(String.Empty);
			}
		}

		private void initializeSeparator()
		{
			int oldSeparatorLength = _separatorLength;

			if (_separatorScalar != null) _separator = _separatorScalar.Value;
			if (_separator == null) _separator = string.Empty;

			_separatorLength = _separator.Length;

			int lengthIncrement = _separatorLength - oldSeparatorLength;

			if (_sourceAsList != null && _sourceAsList.Count > 0)
			{
				int incrementSum = 0;
				int count = _sourceAsList.Count;
				for (int sourceIndex = 0; sourceIndex <= count - 2; sourceIndex++)
				{
					int itemIndex = sourceIndex * 2;

					RangePosition itemRangePosition =
						_resultRangePositions.List[itemIndex];

					RangePosition separatorRangePosition =
						_resultRangePositions.List[itemIndex + 1];

					for (
						int separatorCharIndex = 0; 
						separatorCharIndex < oldSeparatorLength 
							&& separatorCharIndex  < _separatorLength; 
						separatorCharIndex++)
					{
						_valueStringBuilder[separatorRangePosition.PlainIndex + incrementSum + separatorCharIndex] = _separator[separatorCharIndex];
					}

					if (_separatorLength < oldSeparatorLength)
					{
						_valueStringBuilder.Remove(
							separatorRangePosition.PlainIndex + incrementSum + _separatorLength,
							-lengthIncrement);
					}
					else if (_separatorLength > oldSeparatorLength)
					{
						_valueStringBuilder.Insert(
							separatorRangePosition.PlainIndex + incrementSum + oldSeparatorLength,
							_separator.Substring(oldSeparatorLength, lengthIncrement));
					}

					itemRangePosition.PlainIndex = itemRangePosition.PlainIndex + incrementSum;
					separatorRangePosition.PlainIndex = itemRangePosition.PlainIndex + incrementSum;
					separatorRangePosition.Length = _separatorLength;

					incrementSum = incrementSum + lengthIncrement;
				}

				setValue(_valueStringBuilder.ToString());
			}
		}

		private void handleSeparatorScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			initializeSeparator();
			initializeFromSource();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			initializeFromSource();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
            if (!Utils.preHandleSourceCollectionChanged(
                sender, 
                e, 
                _isConsistent, 
                this, 
                ref _indexerPropertyChangedEventRaised, 
                ref _lastProcessedSourceChangeMarker, 
                _sourceAsIHasChangeMarker, 
                ref _handledEventSender, 
                ref _handledEventArgs)) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    IList newItems = e.NewItems;
                    //if (newItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
                    string addedSourceItem = (string) newItems[0];
                    int addedSourceIndex = e.NewStartingIndex;
                    processAddSourceItem(addedSourceItem, addedSourceIndex);
                    setValue(_valueStringBuilder.ToString());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    //if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
                    int removedSourceIndex = e.OldStartingIndex;
                    processRemoveSourceItem(removedSourceIndex);
                    setValue(_valueStringBuilder.ToString());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    IList newItems1 = e.NewItems;
                    //if (newItems1.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
                    RangePosition replacingItemRangePosition =
                        _resultRangePositions.List[e.NewStartingIndex * 2];
                    string newSourceItem = (string) newItems1[0];
                    int replacingSourceItemLength = replacingItemRangePosition.Length;
                    int newSourceItemLength = newSourceItem != null ? newSourceItem.Length : 0;

                    int plainIndex = replacingItemRangePosition.PlainIndex;
                    for (
                        int charIndex = 0;
                        charIndex < replacingSourceItemLength
                        && charIndex < newSourceItemLength;
                        charIndex++)
                    {

                        // ReSharper disable once PossibleNullReferenceException
                        _valueStringBuilder[plainIndex + charIndex] = newSourceItem[charIndex];
                    }

                    if (newSourceItemLength < replacingSourceItemLength)
                    {
                        _valueStringBuilder.Remove(
                            plainIndex + newSourceItemLength,
                            replacingSourceItemLength - newSourceItemLength);
                    }
                    else if (newSourceItemLength > replacingSourceItemLength)
                    {
                        _valueStringBuilder.Insert(
                            plainIndex + replacingSourceItemLength,
                            // ReSharper disable once PossibleNullReferenceException
                            newSourceItem.Substring(replacingSourceItemLength,
                                newSourceItemLength - replacingSourceItemLength));
                    }

                    _resultRangePositions.ModifyLength(replacingItemRangePosition.Index,
                        newSourceItemLength - replacingSourceItemLength);

                    setValue(_valueStringBuilder.ToString());
                    break;
                case NotifyCollectionChangedAction.Move:
                    int newSourceIndex = e.NewStartingIndex;
                    int oldSourceIndex = e.OldStartingIndex;
                    if (newSourceIndex != oldSourceIndex)
                    {
                        _moving = true;
                        processRemoveSourceItem(oldSourceIndex);
                        string newSourceItem1 = _sourceAsList[newSourceIndex];
                        processAddSourceItem(newSourceItem1, newSourceIndex);
                        _moving = false;
                        setValue(_valueStringBuilder.ToString());
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    initializeFromSource();
                    break;
            }

            Utils.postHandleSourceCollectionChanged(
                out _handledEventSender,
                out _handledEventArgs);
		}

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)        
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourceScalar, ref _source, handleSourceScalarValueChanged);
            initializeSeparatorScalar();
            initializeSeparator();
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, handleSourceScalarValueChanged, ref _source);
            if (_separatorScalar != null)
                _separatorScalar.PropertyChanged -= handleSeparatorScalarValueChanged;
        }

        #region Implementation of ISourceIndexerPropertyTracker

        void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Utils.HandleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
        }

        #endregion

		private void recalculateValue()
		{
			_resultRangePositions.List.Clear();
			_valueStringBuilder.Clear();
			int count = _sourceAsList.Count;
			for (int sourceIndex = 0; sourceIndex < count; sourceIndex++)
			{
				string sourceItem = _sourceAsList[sourceIndex];
				_valueStringBuilder.Append(sourceItem);
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				_resultRangePositions.Add(sourceItem != null ? sourceItem.Length : 0);

				if (sourceIndex != count - 1)
				{
					_valueStringBuilder.Append(_separator);
					// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
					_resultRangePositions.Add(_separatorLength);
				}
			}

			setValue(_valueStringBuilder.ToString());
		}

		private void processAddSourceItem(string addedSourceItem, int addedSourceIndex)
		{
			int addedSourceItemLength = addedSourceItem?.Length ?? 0;
			int count = _sourceAsList.Count;
			if (addedSourceIndex == 0 &&  count == 1)
			{
				_valueStringBuilder.Insert(0, addedSourceItem);
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				_resultRangePositions.Insert(0, addedSourceItemLength);
			}
			else if (addedSourceIndex == count - 1)
			{
				_valueStringBuilder.Append(_separator);
				_valueStringBuilder.Append(addedSourceItem);
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				_resultRangePositions.Add(_separatorLength);
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				_resultRangePositions.Add(addedSourceItemLength);						
			}
			else //if (addedSourceIndex < _sourceAsList.Count - 1)
			{
				RangePosition addedItemRangePosition =
					_resultRangePositions.List[addedSourceIndex*2];
				_valueStringBuilder.Insert(addedItemRangePosition.PlainIndex, addedSourceItem);
				_valueStringBuilder.Insert(addedItemRangePosition.PlainIndex + addedSourceItemLength, _separator);
				
				_resultRangePositions.InsertRange(
					addedItemRangePosition.Index,
					new [] {addedSourceItemLength, _separatorLength});
			}
		}

		private void processRemoveSourceItem(int removedSourceIndex)
		{
			RangePosition oldItemRangePosition =
				_resultRangePositions.List[removedSourceIndex * 2];
			int count = _sourceAsList.Count;
			if (removedSourceIndex < (_moving ? count - 1 : count))
			{
				_valueStringBuilder.Remove(oldItemRangePosition.PlainIndex, oldItemRangePosition.Length + _separatorLength);
				_resultRangePositions.RemoveRange(oldItemRangePosition.Index, 2);
			}
			else
			{
				if (oldItemRangePosition.Index > 0)
				{
					_valueStringBuilder.Remove(oldItemRangePosition.PlainIndex - _separatorLength,
						_separatorLength + oldItemRangePosition.Length);
					_resultRangePositions.RemoveRange(oldItemRangePosition.Index - 1, 2);					
				}
				else
				{
					_valueStringBuilder.Remove(oldItemRangePosition.PlainIndex, oldItemRangePosition.Length);		
					_resultRangePositions.Remove(oldItemRangePosition.Index);					
				}
			}
		}

		public void ValidateConsistency()
		{
			_resultRangePositions.ValidateConsistency();
			string separator = _separatorScalar.getValue(_separator);
			IList source = (IList) _sourceScalar.getValue(_source, new ObservableCollection<string>());

			if (source != null)
			{
				string result = string.Join(separator, source.Cast<object>().Select(i => i != null ? i.ToString() : String.Empty).ToArray());

				if (!result.Equals(_valueStringBuilder.ToString()))
					throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.1");

				if (!result.Equals(_value))
					throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.9");

				int plainIndex = 0;
				for (int index = 0; index < source.Count; index++)
				{
					string item = source[index] != null ? source[index].ToString() : String.Empty;
					int itemLength = item.Length;
					if (_resultRangePositions.List[index * 2].Length != itemLength)
						throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.2");

					if (_resultRangePositions.List[index * 2].PlainIndex != plainIndex)
						throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.3");

					if (_resultRangePositions.List[index * 2].Index != index * 2)
						throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.4");

					if (index > 0)
					{
						if (_resultRangePositions.List[index * 2 - 1].Length != separator.Length)
							throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.5");		
						
						if (_resultRangePositions.List[index * 2 - 1].PlainIndex != plainIndex - separator.Length)
							throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.6");

						if (_resultRangePositions.List[index * 2 - 1].Index != index * 2 - 1)
							throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.7");
					}

					plainIndex = plainIndex + itemLength + separator.Length;
				}

				if (_resultRangePositions.List.Count != (source.Count > 0 ? source.Count * 2 - 1 : 0))
					throw new ObservableComputationsException(this, "Consistency violation: StringsConcatenating.8");
			}
		}
	}
}