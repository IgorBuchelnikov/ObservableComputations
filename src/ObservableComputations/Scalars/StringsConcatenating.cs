// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ObservableComputations
{
	public class StringsConcatenating : ScalarComputing<string>, IHasSources, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<string> SeparatorScalar => _separatorScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public string Separator => _separator;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		private IList<string> _sourceAsList;

		private int _separatorLength;

		private readonly StringBuilder _valueStringBuilder = new StringBuilder();
		private RangePositions<RangePosition> _resultRangePositions;

		private bool _moving;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly IReadScalar<string> _separatorScalar;
		private INotifyCollectionChanged _source;
		private string _separator;

		private bool _countPropertyChangedEventRaised;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasTickTackVersion _sourceAsIHasTickTackVersion;
		private bool _lastProcessedSourceTickTackVersion;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
		private int _sourceCount;
		private PropertyChangedEventHandler _handleSeparatorScalarValueChanged;

		private StringsConcatenating(int capacity)
		{
			_resultRangePositions = new RangePositions<RangePosition>(new List<RangePosition>(capacity * 2));
			_thisAsSourceCollectionChangeProcessor = this;
			_deferredQueuesCount = 2;
		}

		[ObservableComputationsCall]
		public StringsConcatenating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<string> separatorScalar) : this(Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_separatorScalar = separatorScalar;
		}

		[ObservableComputationsCall]
		public StringsConcatenating(
			INotifyCollectionChanged source,
			IReadScalar<string> separatorScalar) : this(Utils.getCapacity(source))
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
			{
				_handleSeparatorScalarValueChanged = getScalarValueChangedHandler(
					initializeSeparator, 
					() => processSource(false));
				_separatorScalar.PropertyChanged += _handleSeparatorScalarValueChanged;
			}
		}

		protected override void processSource()
		{
			processSource(true);
		}

		private void processSource(bool replaceSource)
		{
			if (_sourceReadAndSubscribed)
			{
				_valueStringBuilder.Clear();

				int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
				_resultRangePositions = new RangePositions<RangePosition>(new List<RangePosition>(capacity * 2));

				if (replaceSource)
					Utils.unsubscribeSource(
						_source, 
						ref _sourceAsINotifyPropertyChanged, 
						this, 
						handleSourceCollectionChanged);

				_sourceReadAndSubscribed = false;
			}

			if (replaceSource)
				Utils.replaceSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
					out _sourceAsList, true);

			if (_isActive)
			{
				if (_source != null)
				{
					if (replaceSource)
						Utils.subscribeSource(
							out _sourceAsIHasTickTackVersion, 
							_sourceAsList, 
							ref _lastProcessedSourceTickTackVersion, 
							ref _sourceAsINotifyPropertyChanged,
							(ISourceIndexerPropertyTracker)this,
							_source,
							handleSourceCollectionChanged);

					_sourceReadAndSubscribed = true;
					recalculateValue();
				}
				else
					setValue(string.Empty);
			}
			else
				setDefaultValue();
		}

		private void initializeSeparator()
		{
			int oldSeparatorLength = _separatorLength;

			if (_separatorScalar != null) _separator = _separatorScalar.Value;
			if (_separator == null) _separator = string.Empty;

			_separatorLength = _separator.Length;

			int lengthIncrement = _separatorLength - oldSeparatorLength;

			if (_sourceCount > 0)
			{
				int incrementSum = 0;
				for (int sourceIndex = 0; sourceIndex <= _sourceCount - 2; sourceIndex++)
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
						_valueStringBuilder[separatorRangePosition.PlainIndex + incrementSum + separatorCharIndex] =
							_separator[separatorCharIndex];

					if (_separatorLength < oldSeparatorLength)
						_valueStringBuilder.Remove(
							separatorRangePosition.PlainIndex + incrementSum + _separatorLength,
							-lengthIncrement);
					else if (_separatorLength > oldSeparatorLength)
						_valueStringBuilder.Insert(
							separatorRangePosition.PlainIndex + incrementSum + oldSeparatorLength,
							_separator.Substring(oldSeparatorLength, lengthIncrement));

					itemRangePosition.PlainIndex = itemRangePosition.PlainIndex + incrementSum;
					separatorRangePosition.PlainIndex = itemRangePosition.PlainIndex + incrementSum;
					separatorRangePosition.Length = _separatorLength;

					incrementSum = incrementSum + lengthIncrement;
				}

				setValue(_valueStringBuilder.ToString());
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _countPropertyChangedEventRaised,
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourceTickTackVersion, 
				_sourceAsIHasTickTackVersion, 
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
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					IList newItems = e.NewItems;
					_sourceCount++;
					//if (newItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
					string addedSourceItem = (string) newItems[0];
					int addedSourceIndex = e.NewStartingIndex;
					processAddSourceItem(addedSourceItem, addedSourceIndex);
					setValue(_valueStringBuilder.ToString());
					break;
				case NotifyCollectionChangedAction.Remove:
					//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
					_sourceCount--;
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
					int newSourceItemLength = newSourceItem?.Length ?? 0;

					int plainIndex = replacingItemRangePosition.PlainIndex;
					for (
						int charIndex = 0;
						charIndex < replacingSourceItemLength
						&& charIndex < newSourceItemLength;
						charIndex++)
						// ReSharper disable once PossibleNullReferenceException
						_valueStringBuilder[plainIndex + charIndex] = newSourceItem[charIndex];

					if (newSourceItemLength < replacingSourceItemLength)
						_valueStringBuilder.Remove(
							plainIndex + newSourceItemLength,
							replacingSourceItemLength - newSourceItemLength);
					else if (newSourceItemLength > replacingSourceItemLength)
						_valueStringBuilder.Insert(
							plainIndex + replacingSourceItemLength,
							// ReSharper disable once PossibleNullReferenceException
							newSourceItem.Substring(replacingSourceItemLength,
								newSourceItemLength - replacingSourceItemLength));

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
						string newSourceItem1 = (string) e.NewItems[0];
						processAddSourceItem(newSourceItem1, newSourceIndex);
						_moving = false;
						setValue(_valueStringBuilder.ToString());
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					processSource(false);
					break;
			}
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source);
			(_separatorScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _sourceScalar, _source);
			(_separatorScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		protected override void initialize()
		{
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
			initializeSeparatorScalar();
			initializeSeparator();
		}

		protected override void uninitialize()
		{
			Utils.unsubscribeSourceScalar(_sourceScalar, scalarValueChangedHandler);
			if (_separatorScalar != null)
				_separatorScalar.PropertyChanged -= _handleSeparatorScalarValueChanged;
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source);
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _countPropertyChangedEventRaised, ref _indexerPropertyChangedEventRaised);
		}

		#endregion

		private void recalculateValue()
		{
			_resultRangePositions.List.Clear();
			_valueStringBuilder.Clear();
			_sourceCount = _sourceAsList.Count;
			string[] sourceCopy = new string[_sourceCount];
			_sourceAsList.CopyTo(sourceCopy, 0);

			for (int sourceIndex = 0; sourceIndex < _sourceCount; sourceIndex++)
			{
				string sourceItem = sourceCopy[sourceIndex];
				_valueStringBuilder.Append(sourceItem);
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				_resultRangePositions.Add(sourceItem?.Length ?? 0);

				if (sourceIndex != _sourceCount - 1)
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
			if (addedSourceIndex == 0 && _sourceCount == 1)
			{
				_valueStringBuilder.Insert(0, addedSourceItem);
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				_resultRangePositions.Insert(0, addedSourceItemLength);
			}
			else if (addedSourceIndex == _sourceCount - 1)
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

			if (removedSourceIndex < (_moving ? _sourceCount - 1 : _sourceCount))
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

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			_resultRangePositions.ValidateInternalConsistency();
			string separator = _separatorScalar.getValue(_separator);
			IList source = (IList) _sourceScalar.getValue(_source, new ObservableCollection<string>());

			if (source != null)
			{
				string result = string.Join(separator, source.Cast<object>().Select(i => i != null ? i.ToString() : String.Empty).ToArray());

				if (!result.Equals(_valueStringBuilder.ToString()))
					throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.1");

				if (!result.Equals(_value))
					throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.9");

				int plainIndex = 0;
				for (int index = 0; index < source.Count; index++)
				{
					string item = source[index] != null ? source[index].ToString() : String.Empty;
					int itemLength = item.Length;
					if (_resultRangePositions.List[index * 2].Length != itemLength)
						throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.2");

					if (_resultRangePositions.List[index * 2].PlainIndex != plainIndex)
						throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.3");

					if (_resultRangePositions.List[index * 2].Index != index * 2)
						throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.4");

					if (index > 0)
					{
						if (_resultRangePositions.List[index * 2 - 1].Length != separator.Length)
							throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.5");		
						
						if (_resultRangePositions.List[index * 2 - 1].PlainIndex != plainIndex - separator.Length)
							throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.6");

						if (_resultRangePositions.List[index * 2 - 1].Index != index * 2 - 1)
							throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.7");
					}

					plainIndex = plainIndex + itemLength + separator.Length;
				}

				if (_resultRangePositions.List.Count != (source.Count > 0 ? source.Count * 2 - 1 : 0))
					throw new ValidateInternalConsistencyException("Consistency violation: StringsConcatenating.8");
			}
		}
	}
}