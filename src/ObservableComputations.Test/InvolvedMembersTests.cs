// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class InvolvedMembersTests : TestBase
	{
		public class Item : INotifyPropertyChanged
		{
			private string _num;
			public string Num
			{
				get { return _num; }
				set
				{
					updatePropertyValue(ref _num, value);
				}
			}

			#region INotifyPropertyChanged imlementation
			public event PropertyChangedEventHandler PropertyChanged;

			protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChangedEventHandler onPropertyChanged = PropertyChanged;
				if (onPropertyChanged != null) onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}

			protected bool updatePropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
			{
				field = value;
				this.onPropertyChanged(propertyName);
				return true;
			}
			#endregion

			public Delegate[] GetPropertyChangedInvocationList => PropertyChanged.GetInvocationList();
		}


		[Test]
		public void TestSelecting()
		{
			OcConsumer consumer = new OcConsumer();
			Item item1 = new Item();

			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					item1
				}
			);

			Selecting<Item, string> selecting = items.Selecting(i => i.Num).For(consumer);

			InvolvedMembersTreeNode node = new InvolvedMembersTreeNode(args => { });

			((IComputingInternal)selecting).InitializeInvolvedMembersTreeNode(node);
			items.Add(new Item());
			Assert.IsTrue(node.InvolvedMembersWithDescedants.SequenceEqual(new []{new InvolvedMember(item1, nameof(Item.Num))}));

		}

		public InvolvedMembersTests(bool debug) : base(debug)
		{
		}
	}
}
