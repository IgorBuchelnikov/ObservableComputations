using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCode.ObservableComputations.Common.Interface
{
	public interface IHasTags
	{
		string DebugTag {get; set;}
		object Tag {get; set;}
	}
}
