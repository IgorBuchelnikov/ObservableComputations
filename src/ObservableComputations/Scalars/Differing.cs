using System.Collections.Generic;
using System.ComponentModel;

namespace ObservableComputations
{
	public class Differing<TResult> : ScalarComputing<TResult>
	{
		public IReadScalar<TResult> Scalar => _scalar;
		public IEqualityComparer<TResult> EqualityComparer =>_equalityComparer;

		private IReadScalar<TResult> _scalar;
		private IEqualityComparer<TResult> _equalityComparer;

		private readonly PropertyChangedEventHandler _scalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _scalarWeakPropertyChangedEventHandler;

		private readonly IReadScalar<IEqualityComparer<TResult>> _equalityComparerScalar;
		private PropertyChangedEventHandler _equalityComparerScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _equalityComparerScalarWeakPropertyChangedEventHandler;

		[ObservableComputationsCall]
		public Differing(
			IReadScalar<TResult> scalar,
			IEqualityComparer<TResult> equalityComparer = null) : this(scalar)
		{
			_equalityComparer = equalityComparer ?? EqualityComparer<TResult>.Default;
		}

		[ObservableComputationsCall]
		public Differing(
			IReadScalar<TResult> scalar,
			IReadScalar<IEqualityComparer<TResult>> equalityComparerScalar) : this(scalar)
		{
			_equalityComparerScalarPropertyChangedEventHandler = handleEqualityComparerScalarValueChanged;
			_equalityComparerScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_equalityComparerScalarPropertyChangedEventHandler);
			_equalityComparerScalar = equalityComparerScalar;
			_equalityComparerScalar.PropertyChanged += _equalityComparerScalarWeakPropertyChangedEventHandler.Handle;
			_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;
		}

		private Differing(
			IReadScalar<TResult> scalar)
		{
			_scalar = scalar;
			_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
			_scalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
			_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			TResult newValue = _scalar.Value;
			if (!_equalityComparer.Equals(newValue, _value))
			{
				setValue(newValue);
			}

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleEqualityComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		~Differing()
		{
			_scalar.PropertyChanged -= _scalarWeakPropertyChangedEventHandler.Handle;

			if (_equalityComparerScalarWeakPropertyChangedEventHandler != null)
			{
				_equalityComparerScalar.PropertyChanged -= _equalityComparerScalarWeakPropertyChangedEventHandler.Handle;			
			}
		}
	}
}
