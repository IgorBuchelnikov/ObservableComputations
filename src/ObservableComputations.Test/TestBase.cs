using System;
using System.Collections.Generic;
using System.IO;
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

			File.AppendAllLines("oc_tests.log", new []{$"{DateTime.Now.ToString("O")} {GetType().Name}"});
		}
	}
}
