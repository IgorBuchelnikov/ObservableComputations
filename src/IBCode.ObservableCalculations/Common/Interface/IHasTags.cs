using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface IHasTags
	{
		string DebugTag {get; set;}
		object Tag {get; set;}
	}
}
