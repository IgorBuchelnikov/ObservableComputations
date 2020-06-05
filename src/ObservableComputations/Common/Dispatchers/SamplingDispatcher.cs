//using System;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;

//namespace ObservableComputations.Common
//{
//	public class SamplingDispatcher : IDispatcher, IDisposable
//	{
//		public IDispatcher DestinationDispatcher => _destinationDispatcher;
//		public TimeSpan Interval => _interval;

//		Subject<Action> _actions;
//		private IDisposable _cleanUp;
//		private readonly IDispatcher _destinationDispatcher;
//		private readonly TimeSpan _interval;

//		public SamplingDispatcher(TimeSpan interval, IDispatcher destinationDispatcher)
//		{
//			_interval = interval;
//			_destinationDispatcher = destinationDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Sample(interval).Subscribe(action =>
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