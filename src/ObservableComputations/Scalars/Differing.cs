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

		private TResult _previousValue;

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
			_equalityComparerScalar.PropertyChanged += _equalityComparerScalarWeakPropertyChangedEventHandler.Handle;
			_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;
		}

		private Differing(
			IReadScalar<TResult> scalar)
		{
			_scalar = scalar;
			_previousValue = _value = _scalar.Value;
			_scalarPropertyChangedEventHandler = handleScalarPropertyChanged;
			_scalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_scalarPropertyChangedEventHandler);
			_scalar.PropertyChanged += _scalarWeakPropertyChangedEventHandler.Handle;
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<TResult>.Value)) return;
			TResult newValue = _scalar.Value;
			_previousValue = _value;
			if (!_equalityComparer.Equals(newValue, _previousValue))
			{
				setValue(newValue);
			}
			else
			{
				_value = newValue;
			}
		}

		private void handleEqualityComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;
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
