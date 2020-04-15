//using System.ComponentModel;

//namespace ObservableComputations
//{
//	public class ScalarSynchronizing<TResult> : ScalarComputing<TResult>
//	{
//		public INotifyScalarChanged<TResult> Scalar => _scalar;
//		public IPostingSynchronizer PostingSynchronizer => _postingSynchronizer;
//		public ISendingSynchronizer SendingSynchronizer => _sendingSynchronizer;

//		private IPostingSynchronizer _postingSynchronizer;
//		private ISendingSynchronizer _sendingSynchronizer;

//		private INotifyScalarChanged<TResult> _scalar;
//		private TResult _previousValue;
//		private bool _isEverChanged;

//		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
//		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

//		[ObservableComputationsCall]
//		public ScalarSynchronizing(
//			INotifyScalarChanged<TResult> scalar)
//		{
//			_scalar = scalar;
//			_value = _scalar.Value;
//			_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
//			_scalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
//			_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
//		}


//		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
//		{
//			if (e.PropertyName != nameof(INotifyScalarChanged<TResult>.Value)) return;
//			TResult newValue = _scalar.Value;
//			_previousValue = _value;
//			_isConsistent = false;

//			if (!_isEverChanged)
//			{
//				_isEverChanged = true;
//				raisePropertyChanged(Utils.IsEverChangedPropertyChangedEventArgs);
//			}

//			raisePropertyChanged(Utils.PreviousValuePropertyChangedEventArgs);
//			setValue(newValue);
//			raiseConsistencyRestored();
//		}

//		~PreviousTracking()
//		{
//			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;
//		}
//	}
//}