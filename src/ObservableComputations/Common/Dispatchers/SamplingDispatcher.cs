//using System;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;

//namespace ObservableComputations.Common
//{
//	public class SamplingOcDispatcher : IOcDispatcher, IDisposable
//	{
//		public IOcDispatcher DestinationOcDispatcher => _destinationOcDispatcher;
//		public TimeSpan Interval => _interval;

//		Subject<Action> _actions;
//		private IDisposable _cleanUp;
//		private readonly IOcDispatcher _destinationOcDispatcher;
//		private readonly TimeSpan _interval;

//		public SamplingOcDispatcher(TimeSpan interval, IOcDispatcher destinationOcDispatcher)
//		{
//			_interval = interval;
//			_destinationOcDispatcher = destinationOcDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Sample(interval).Subscribe(action =>
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