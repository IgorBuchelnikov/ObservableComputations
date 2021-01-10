using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations.Test
{
	public class TestBase
	{
		public TestBase(bool debug)
		{
			if (debug)
			{
				Configuration.SaveInstantiatingStackTrace = true;
				Configuration.TrackComputingsExecutingUserCode = true;
				Configuration.SaveOcDispatcherInvocationStackTrace = true;
				Configuration.TrackOcDispatcherInvocations = true;
			}
		}
	}
}
