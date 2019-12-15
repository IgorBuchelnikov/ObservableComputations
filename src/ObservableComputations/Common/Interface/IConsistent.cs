using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations.Common.Interface
{
	public interface IConsistent
	{
		bool IsConsistent {get;}
		event EventHandler ConsistencyRestored;

	}
}
