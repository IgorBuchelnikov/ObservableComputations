// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	public class ToStringTests
	{
		public class Item
		{
			#region Overrides of Object

			public override string ToString()
			{
				throw new Exception("Exception");
			}

			#endregion
		}

		[Test]
		public void ToStringSafe()
		{
			Item item = new Item();
			string str = item.ToStringSafe(e => e.Message);
			Assert.AreEqual(str, "Exception");

			str = item.ToStringSafe();
			Assert.AreEqual(str, "exception");
		}

		//[Test]
		//public void PropertyChangedEventArgs()
		//{
		//	System.ComponentModel.PropertyChangedEventArgs args = new PropertyChangedEventArgs("SomePropety");
		//	string str = args.ToStringAlt();
		//	Assert.AreEqual(str, "(System.ComponentModel.PropertyChangedEventArgs (PropertyName = 'SomePropety'))");
		//}

		//[Test]
		//public void NotifyCollectionChangedEventArgs()
		//{
		//	NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
		//	string str = args.ToStringAlt();
		//	Assert.AreEqual(str, "(System.ComponentModel.PropertyChangedEventArgs (PropertyName = 'SomePropety'))");
		//}
	}
}