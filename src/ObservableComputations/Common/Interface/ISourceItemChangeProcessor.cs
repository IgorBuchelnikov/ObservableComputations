// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

namespace ObservableComputations
{
	internal interface ISourceItemChangeProcessor : IComputingInternal
	{
		void ProcessSourceItemChange(ExpressionWatcher expressionWatcher);
	}

	internal interface ISourceItemKeyChangeProcessor : ISourceItemChangeProcessor
	{
		new void ProcessSourceItemChange(ExpressionWatcher expressionWatcher);
	}

	internal interface ISourceItemValueChangeProcessor : ISourceItemChangeProcessor
	{
		new void ProcessSourceItemChange(ExpressionWatcher expressionWatcher);
	}
}
