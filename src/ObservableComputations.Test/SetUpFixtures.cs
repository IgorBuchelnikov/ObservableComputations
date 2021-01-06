using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[NonParallelizable]
	[SetUpFixture]
	public class SetUpFixture
	{
		public SetUpFixture()
		{
			Configuration.SaveInstantiatingStackTrace = false;
			Configuration.TrackComputingsExecutingUserCode = false;
			Configuration.SaveOcDispatcherInvocationStackTrace = false;
			Configuration.TrackOcDispatcherInvocations = false;
		}
	}

	[NonParallelizable]
	[SetUpFixture]
	public class SetUpFixtureDebug
	{
		public SetUpFixtureDebug()
		{
			Configuration.SaveInstantiatingStackTrace = true;
			Configuration.TrackComputingsExecutingUserCode = true;
			Configuration.SaveOcDispatcherInvocationStackTrace = true;
			Configuration.TrackOcDispatcherInvocations = true;
		}
	}
}
