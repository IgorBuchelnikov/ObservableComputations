// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text.RegularExpressions;
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

		[Test]
		public void PropertyChangedEventArgs()
		{
			EventArgs args = new PropertyChangedEventArgs("SomePropety");
			string str = args.ToStringAlt();
			Assert.AreEqual(str, "(PropertyChangedEventArgs (PropertyName = 'SomePropety'))");
		}

		[Test]
		public void PropertyChangedEventArgsNull()
		{
			EventArgs args = null;
			string str = args.ToStringAlt();
			Assert.AreEqual(str, "(null)");
		}

		[Test]
		public void NotifyCollectionChangedEventArgs()
		{
			EventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			string str = args.ToStringAlt();
			Assert.AreEqual(str, "(NotifyCollectionChangedEventArgs (Action = 'Reset'))");

			args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new []{new Item()}, 1);
			str = args.ToStringAlt();
			Assert.AreEqual(str, "(NotifyCollectionChangedEventArgs (Action = 'Add' NewItems[0] = 'Exception: Exception', NewStartingIndex = 1))");

			args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new []{new Item()}, 1);
			str = args.ToStringAlt();
			Assert.AreEqual(str, "(NotifyCollectionChangedEventArgs (Action = 'Remove' OldItems[0] = 'Exception: Exception', OldStartingIndex = 1))");

			args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new []{new Item()}, new []{new Item()}, 1);
			str = args.ToStringAlt();
			Assert.AreEqual(str, "(NotifyCollectionChangedEventArgs (Action = 'Replace' NewItems[0] = 'Exception: Exception', NewStartingIndex = 1, OldItems[0] = 'Exception: Exception', OldStartingIndex = 1))");

			args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, new []{new Item()}, 2, 1);
			str = args.ToStringAlt();
			Assert.AreEqual(str, "(NotifyCollectionChangedEventArgs (Action = 'Move' OldStartingIndex = 1, NewStartingIndex = 2))");
		}

		[Test]
		public void NotifyCollectionChangedEventArgsNull()
		{
			NotifyCollectionChangedEventArgs args = null;
			string str = args.ToStringAlt();
			Assert.AreEqual(str, "(null)");
		}

		[Test]
		public void MethodChangedEventArgs()
		{
			EventArgs args = new MethodChangedEventArgs("SomeMethod", objects => true);
			string str = args.ToStringAlt();
			Regex regex = new Regex(@"\(MethodChangedEventArgs \(MethodName = 'SomeMethod', ArgumentsPredicate.GetHashCode\(\) = \d+\)");
			Assert.IsTrue(regex.Match(str).Success);
		}

		[Test]
		public void MethodChangedEventArgsNull()
		{
			MethodChangedEventArgs args = null;
			string str = args.ToStringAlt();
			Assert.AreEqual(str, "(null)");
		}

		[Test]
		public void Null()
		{
			EventArgs args = null;
			string str = args.ToStringAlt();
			Assert.AreEqual(str, "(null)");
		}


	}
}