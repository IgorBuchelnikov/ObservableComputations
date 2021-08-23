// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;

namespace ObservableComputations
{
	public interface IScalar<TValue> : IReadScalar<TValue>, IWriteScalar<TValue>
	{

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
