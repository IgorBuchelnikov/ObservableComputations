//using System;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;

//namespace ObservableComputations.Common
//{
//	public class BufferingDispatcher : IDispatcher, IDisposable
//	{
//		public IDispatcher DestinationDispatcher => _destinationDispatcher;
//		public TimeSpan TimeSpan => _timeSpan;

//		Subject<Action> _actions;
//		private IDisposable _cleanUp;
//		private readonly IDispatcher _destinationDispatcher;
//		private readonly TimeSpan _timeSpan;

//		public BufferingDispatcher(TimeSpan timeSpan, IDispatcher destinationDispatcher)
//		{
//			_timeSpan = timeSpan;
//			_destinationDispatcher = destinationDispatcher;

//			_actions = new Subject<Action>();
//			_cleanUp = _actions.Buffer(timeSpan).Subscribe(actions =>
//			{
//				_destinationDispatcher.Invoke(() =>
//				{
//					for (var index = 0; index < actions.Count; index++)
//					{
//						actions[index]();
//					}
//				}, this);

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