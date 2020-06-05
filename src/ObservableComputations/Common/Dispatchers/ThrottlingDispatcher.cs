//using System;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;

//namespace ObservableComputations.Common
//{
//	public class ThrottlingDispatcher : IDispatcher, IDisposable
//	{
//		public IDispatcher DestinationDispatcher => _destinationDispatcher;
//		public TimeSpan TimeSpan => _timeSpan;

//		Subject<Action> _actions;
//		private IDisposable _cleanUp;
//		private readonly IDispatcher _destinationDispatcher;
//		private readonly TimeSpan _timeSpan;

//		public ThrottlingDispatcher(TimeSpan timeSpan, IDispatcher destinationDispatcher)
//		{
//			_timeSpan = timeSpan;
//			_destinationDispatcher = destinationDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Throttle(timeSpan).Subscribe(action =>
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