using System;

namespace ObservableComputations
{
	public interface IScalarComputing : IScalar, IComputing
	{	
		event EventHandler PreValueChanged;
		event EventHandler PostValueChanged;

		object NewValueObject {get;}
	}
}
