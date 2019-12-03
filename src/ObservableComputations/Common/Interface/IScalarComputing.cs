using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations.Common.Interface
{
	public interface IScalarComputing : IScalar, IComputing
	{	
		event EventHandler PreValueChanged;
		event EventHandler PostValueChanged;

		object NewValueObject {get;}
	}
}
