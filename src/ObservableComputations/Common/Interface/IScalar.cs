using System;

namespace ObservableComputations.Interface
{
	public interface IScalar<TValue> : IScalar
	{
		TValue Value { get; set;}
	}

	public interface IScalar : System.ComponentModel.INotifyPropertyChanged
	{
		object ValueObject { get; set;}
		Type ValueType {get;}
	}

	public interface IReadScalar<out TValue> : System.ComponentModel.INotifyPropertyChanged
	{
		TValue Value { get;}
	}

	public interface IWriteScalar<in TValue> : System.ComponentModel.INotifyPropertyChanged
	{
		TValue Value { set;}
	}
}
