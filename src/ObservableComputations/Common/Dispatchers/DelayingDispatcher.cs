//using System;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;

//namespace ObservableComputations.Common
//{
//	public class DelayingOcDispatcher : IOcDispatcher, IDisposable
//	{
//		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
//		public TimeSpan DueTimeSpan => _dueTimeSpan;
//		public DateTimeOffset DueDateTimeOffset => _dueDateTimeOffset;

//		Subject<Action> _actions;
//		private IDisposable _cleanUp;
//		private readonly IOcDispatcher _destinationOcDispatcher;
//		private readonly TimeSpan _dueTimeSpan;
//		private readonly DateTimeOffset _dueDateTimeOffset;

//		public DelayingOcDispatcher(TimeSpan dueTime, IOcDispatcher destinationOcDispatcher)
//		{
//			_dueTimeSpan = dueTime;
//			_destinationOcDispatcher = destinationOcDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Delay(_dueTimeSpan).Subscribe(action =>
//			{
//				_destinationOcDispatcher.Invoke(action, this);
//			});
//		}

//		public DelayingOcDispatcher(DateTimeOffset dueTime, IOcDispatcher destinationOcDispatcher)
//		{
//			_dueDateTimeOffset = dueTime;
//			_destinationOcDispatcher = destinationOcDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Delay(_dueDateTimeOffset).Subscribe(action =>
//			{
//				_destinationOcDispatcher.Invoke(action, this);
//			});
//		}

//		#region Implementation of IOcDispatcher

//		public void Invoke(Action action, object context)
//		{
//			_actions.OnNext(action);
//		}

//		#endregion

//		#region Implementation of IDisposable

//		public void Dispose()
//		{
//			_cleanUp.Dispose();
//		}

//		#endregion
//	}
//}

