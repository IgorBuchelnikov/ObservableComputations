// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ObservableComputations
{
	public class ScalarPausing<TResult> : ScalarComputing<TResult>, IHasSources
	{
		public IReadScalar<TResult> Source => _source;
		private readonly IReadScalar<TResult> _source;
		public IReadScalar<bool> IsPausedScalar => _isPausedScalar;
		public IReadScalar<int?> LastChangesToApplyOnResumeCountScalar => _lastChangesToApplyOnResumeCountScalar;

		public bool IsResuming => _isResuming;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source});


		private readonly IReadScalar<bool> _isPausedScalar;

		private readonly IReadScalar<int?> _lastChangesToApplyOnResumeCountScalar;

		private bool _isResuming;

		private readonly Action _changeValueAction;

		public int? LastChangesToApplyOnResumeCount
		{
			get => _lastChangesToApplyOnResumeCount;
			set
			{
				if (_lastChangesToApplyOnResumeCountScalar != null) throw new ObservableComputationsException(this, "Modifying of LastChangesToApplyOnResumeCount property is controlled by LastChangesToApplyOnResumeCountScalar");

				changeLastChangesToApplyOnResumeCount(value);
			}
		}

		private bool _isPaused;
		public bool IsPaused
		{
			get => _isPaused;
			set
			{
				if (_isPausedScalar != null) throw new ObservableComputationsException(this, "Modifying of IsPaused property is controlled by IsPausedScalar");
				checkConsistent(null, null);

				_isResuming = _isPaused != value && !value;
				_isPaused = value;
				raisePropertyChanged(Utils.IsPausedPropertyChangedEventArgs);

				if (_isResuming) resume();

			}
		}

		private void resume()
		{
			_isConsistent = false;

			int count = _deferredScalarActions.Count;
			int startIndex = count - (_lastChangesToApplyOnResumeCount ?? count);

			for (int i = 0; i < count; i++)
			{
				DeferredScalarAction<TResult> deferredScalarAction = _deferredScalarActions.Dequeue();
				if (i >= startIndex)
				{
					_handledEventSender = deferredScalarAction.EventSender;
					_handledEventArgs = deferredScalarAction.PropertyChangedEventAgs;

					setValue(deferredScalarAction.Value);

					_handledEventSender = null;
					_handledEventArgs = null;
				}
			}
			_isResuming = false;

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		readonly Queue<DeferredScalarAction<TResult>> _deferredScalarActions = new Queue<DeferredScalarAction<TResult>>();
		private int? _lastChangesToApplyOnResumeCount;

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> source,
			bool initialIsPaused = false,
			int? lastChangesToApplyOnResumeCount = 1) : this()
		{
			_isPaused = initialIsPaused;
			_lastChangesToApplyOnResumeCount = lastChangesToApplyOnResumeCount;
			_source = source;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> source,
			IReadScalar<bool> isPausedScalar,
			int? lastChangesToApplyOnResumeCount = null) : this()
		{
			_isPausedScalar = isPausedScalar;
			_lastChangesToApplyOnResumeCount = lastChangesToApplyOnResumeCount;
			_source = source;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> source,
			IReadScalar<bool> isPausedScalar,
			IReadScalar<int?> lastChangesToApplyOnResumeCountScalar) : this()
		{
			_isPausedScalar = isPausedScalar;
			_lastChangesToApplyOnResumeCountScalar = lastChangesToApplyOnResumeCountScalar;
			_source = source;
		}

		[ObservableComputationsCall]
		public ScalarPausing(
			IReadScalar<TResult> source,
			bool initialIsPaused,
			IReadScalar<int?> lastChangesToApplyOnResumeCountScalar) : this()
		{
			_isPaused = initialIsPaused;
			_lastChangesToApplyOnResumeCountScalar = lastChangesToApplyOnResumeCountScalar;
			_source = source;
		}

		private ScalarPausing()
		{
			_changeValueAction = () => setValue(_source.Value);
		}

		private void initializeIsPauserScalar()
		{
			if (_isPausedScalar != null)
			{
				_isPausedScalar.PropertyChanged += handleIsPausedScalarValueChanged;
				_isPaused = _isPausedScalar.Value;
			}
		}

		private void initializeLastChangesCountOnResumeScalar()
		{
			if (_lastChangesToApplyOnResumeCountScalar != null)
			{
				_lastChangesToApplyOnResumeCountScalar.PropertyChanged += handleLastChangesToApplyOnResumeCountScalarValueChanged;
				_lastChangesToApplyOnResumeCount = _lastChangesToApplyOnResumeCountScalar.Value;
			}
		}

		protected override void processSource()
		{
			if (_sourceReadAndSubscribed)
			{
				_source.PropertyChanged -= handleSourceScalarPropertyChanged;

				if (_isPausedScalar != null)
				{
					_isPausedScalar.PropertyChanged -= handleIsPausedScalarValueChanged;			
				}

				if (_lastChangesToApplyOnResumeCountScalar != null)
				{
					_lastChangesToApplyOnResumeCountScalar.PropertyChanged -= handleLastChangesToApplyOnResumeCountScalarValueChanged;			
				}

				_deferredScalarActions.Clear();
				_sourceReadAndSubscribed = false;
			}

			if (_isActive)
			{
				if (_isPaused) _deferredScalarActions.Enqueue(new DeferredScalarAction<TResult>(null, null, _source.Value));
				else setValue(_source.Value);

				_source.PropertyChanged += handleSourceScalarPropertyChanged;

				initializeIsPauserScalar();
				initializeLastChangesCountOnResumeScalar();
				_sourceReadAndSubscribed = true;
			}
			else
				setDefaultValue();
		}

		protected override void initialize()
		{

		}

		protected override void uninitialize()
		{

		}

		protected override void clearCachedScalarArgumentValues()
		{

		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_isPausedScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_lastChangesToApplyOnResumeCountScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_isPausedScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_lastChangesToApplyOnResumeCountScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

  //	  private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		//{
		//	if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;
		//	handleScalarPropertyChanged(sender, e, _scalar.Value);
		//}

		private void handleSourceScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_isPaused)
			{
				_deferredScalarActions.Enqueue(new DeferredScalarAction<TResult>(sender, e, _source.Value));
				if (_lastChangesToApplyOnResumeCount != null && _deferredScalarActions.Count > _lastChangesToApplyOnResumeCount)
					_deferredScalarActions.Dequeue();
			}
			else			 
				Utils.processChange(
					sender, 
					e, 
					_changeValueAction,
					ref _isConsistent, 
					ref _handledEventSender, 
					ref _handledEventArgs, 
					0, _deferredQueuesCount,
					ref _deferredProcessings, this);

		}

		private void handleIsPausedScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			bool newValue = _isPausedScalar.Value;
			_isResuming = _isPaused != newValue && !newValue;
			_isPaused = newValue;

			if (_isResuming) resume();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleLastChangesToApplyOnResumeCountScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			changeLastChangesToApplyOnResumeCount(_lastChangesToApplyOnResumeCountScalar.Value);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void changeLastChangesToApplyOnResumeCount(int? newValue)
		{
			if (_isPaused)
			{
				if ((newValue == null && _lastChangesToApplyOnResumeCount != null) 
					|| (newValue != null && _lastChangesToApplyOnResumeCount != null && newValue > _lastChangesToApplyOnResumeCount))
					throw new ObservableComputationsException(this, "It is impossible to increase LastChangesToApplyOnResumeCount while IsPaused = true");


				if (newValue < _lastChangesToApplyOnResumeCount)
				{
					int count = _deferredScalarActions.Count;
					if (count > newValue)
					{
						for (int index = 0; index < count - newValue; index++)
							_deferredScalarActions.Dequeue();
					}
				}
			}

			_lastChangesToApplyOnResumeCount = newValue;
		}

		internal override void InitializeInvolvedMembersTreeNodeImpl(InvolvedMembersTreeNode involvedMembersTreeNode)
		{
			Utils.AddInvolvedMembersTreeNodeChild(involvedMembersTreeNode, _source);
			Utils.AddInvolvedMembersTreeNodeChild(involvedMembersTreeNode, _isPausedScalar);
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			if (!_isPaused)
			{
				if (!_value.Equals(_source.Value))
				{
					throw new ValidateInternalConsistencyException("Consistency violation: ScalarPausing.1");
				}
			}
		}
	}

	internal struct DeferredScalarAction<TResult>
	{
		public readonly object EventSender;
		public readonly PropertyChangedEventArgs PropertyChangedEventAgs;
		public readonly TResult Value;

		public DeferredScalarAction(object eventSender, PropertyChangedEventArgs eventAgs, TResult value)
		{
			EventSender = eventSender;
			PropertyChangedEventAgs = eventAgs;
			Value = value;
		}
	}
}
