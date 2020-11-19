using System;
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


        private readonly IReadScalar<IEqualityComparer<TResult>> _equalityComparerScalar;
        private Action _changeValueAction;
        private Action _setEqualityComparerAction;

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

            _changeValueAction = () =>
            {
                TResult newValue = _scalar.Value;
                if (!_equalityComparer.Equals(newValue, _value))
                {
                    setValue(newValue);
                }
            };

            _setEqualityComparerAction = () =>
            {
                _equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;
            };
		}

		private void handleScalarPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Utils.processChange(
                sender, 
                e, 
                _changeValueAction,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, this);
        }

		private void handleEqualityComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
        {
            Utils.processChange(
                sender, 
                e, 
                _setEqualityComparerAction,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, this);
        }

        private bool _initializedFromSource;
        #region Overrides of ScalarComputing<TResult>

        protected override void initializeFromSource()
        {
            if (_initializedFromSource)
            {
                if (_equalityComparerScalar != null)
                {
                    _equalityComparerScalar.PropertyChanged -= handleEqualityComparerScalarValueChanged;
                    _equalityComparer = null;      
                }

                _scalar.PropertyChanged -= handleScalarPropertyChanged;
                _initializedFromSource = false;
            }

            if (_isActive)
            {
                if (_equalityComparerScalar != null)
                {
                    _equalityComparerScalar.PropertyChanged += handleEqualityComparerScalarValueChanged;
                    _equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TResult>.Default;      
                }

                _scalar.PropertyChanged += handleScalarPropertyChanged;
                setValue(_scalar.Value);
                _initializedFromSource = true;
            }
            else
            {
                setDefaultValue();
            }
        }

        protected override void initialize()
        {

        }

        protected override void uninitialize()
        {

        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            base.addToUpstreamComputings(computing);
            (_scalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            base.removeFromUpstreamComputings(computing);
            (_scalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        #endregion
    }
}
