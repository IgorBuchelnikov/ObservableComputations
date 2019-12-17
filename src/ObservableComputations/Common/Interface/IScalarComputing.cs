using System;

namespace ObservableComputations.Common.Interface
{
	public interface IScalarComputing : IScalar, IComputing
	{	
		event EventHandler PreValueChanged;
		event EventHandler PostValueChanged;

		object NewValueObject {get;}
	}
}
