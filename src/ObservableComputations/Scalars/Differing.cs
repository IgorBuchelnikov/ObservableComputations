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

        private readonly IReadScalar<IEqualityComparer<TResult>> _equalityComparerScalar;

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
			_equalityComparerScalar = equalityComparerScalar;
		}

		private Differing(
			IReadScalar<TResult> scalar)
		{
			_scalar = scalar;
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


        #region Overrides of ScalarComputing<TResult>
        protected override void initialize()
        {
            if (_equalityComparerScalar != null)
            {
                _equalityComparerScalar.PropertyChanged += handleEqualityComparerScalarValueChanged;
                _equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;      
            }

            _scalar.PropertyChanged += handleScalarPropertyChanged;
            setValue(_scalar.Value);
        }

        protected override void uninitialize()
        {
            if (_equalityComparerScalar != null)
            {
                _equalityComparerScalar.PropertyChanged -= handleEqualityComparerScalarValueChanged;
                _equalityComparer = null;      
            }

            _scalar.PropertyChanged -= handleScalarPropertyChanged;
            setValue(default);
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_scalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        #endregion
    }
}
