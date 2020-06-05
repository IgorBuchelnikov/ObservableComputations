//using System;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;

//namespace ObservableComputations.Common
//{
//	public class DelayingDispatcher : IDispatcher, IDisposable
//	{
//		public IDispatcher DestinationDispatcher => _destinationDispatcher;
//		public TimeSpan DueTimeSpan => _dueTimeSpan;
//		public DateTimeOffset DueDateTimeOffset => _dueDateTimeOffset;

//		Subject<Action> _actions;
//		private IDisposable _cleanUp;
//		private readonly IDispatcher _destinationDispatcher;
//		private readonly TimeSpan _dueTimeSpan;
//		private readonly DateTimeOffset _dueDateTimeOffset;

//		public DelayingDispatcher(TimeSpan dueTime, IDispatcher destinationDispatcher)
//		{
//			_dueTimeSpan = dueTime;
//			_destinationDispatcher = destinationDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Delay(_dueTimeSpan).Subscribe(action =>
//			{
//				_destinationDispatcher.Invoke(action, this);
//			});
//		}

//		public DelayingDispatcher(DateTimeOffset dueTime, IDispatcher destinationDispatcher)
//		{
//			_dueDateTimeOffset = dueTime;
//			_destinationDispatcher = destinationDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Delay(_dueDateTimeOffset).Subscribe(action =>
//			{
//				_destinationDispatcher.Invoke(action, this);
//			});
//		}

//		#region Implementation of IDispatcher

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

