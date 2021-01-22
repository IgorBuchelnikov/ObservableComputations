// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.IO;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[SetUpFixture]
	public class SetUpFixture
	{
	    [OneTimeSetUp]
	    public void RunBeforeAnyTests()
	    {
#if GeneratingMinimalTestsToCover
			File.Delete(TestBase.MinimalTestsToCoverFileName);
			File.WriteAllText(TestBase.MinimalTestsToCoverFileName,
@"// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE


using NUnit.Framework;

namespace ObservableComputations.Test
{"			);


#endif	
	    }

	    [OneTimeTearDown]
	    public void RunAfterAnyTests()
	    {
#if GeneratingMinimalTestsToCover
			File.AppendAllText(TestBase.MinimalTestsToCoverFileName,
@"
		}
	}
}"			);
#endif
	    }
	}

}