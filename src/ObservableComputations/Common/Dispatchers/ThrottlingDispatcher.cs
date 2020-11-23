//using System;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;

//namespace ObservableComputations.Common
//{
//	public class ThrottlingOcDispatcher : IOcDispatcher, IDisposable
//	{
//		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
//		public TimeSpan TimeSpan => _timeSpan;

//		Subject<Action> _actions;
//		private IDisposable _cleanUp;
//		private readonly IOcDispatcher _destinationOcDispatcher;
//		private readonly TimeSpan _timeSpan;

//		public ThrottlingOcDispatcher(TimeSpan timeSpan, IOcDispatcher destinationOcDispatcher)
//		{
//			_timeSpan = timeSpan;
//			_destinationOcDispatcher = destinationOcDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Throttle(timeSpan).Subscribe(action =>
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