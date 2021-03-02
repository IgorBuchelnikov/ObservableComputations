// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class PropertyAccessingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Order : INotifyPropertyChanged
		{
			private Order _parentOrder;
			public Order ParentOrder
			{
				get { return _parentOrder; }
				set { updatePropertyValue(ref _parentOrder, value); }
			}

			private string _num;
			public string Num
			{
				get { return _num; }
				set { updatePropertyValue(ref _num, value); }
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
				if (EqualityComparer<T>.Default.Equals(field, value)) return false;
				field = value;
				this.onPropertyChanged(propertyName);
				return true;
			}
			#endregion
		}

		[Test]
		public void TestRaiseValueChanged()
		{
			Order order = new Order();
			PropertyAccessing<string> propertyAccessing = order.PropertyAccessing<string>("Num").For(consumer);
			string result = null;
			bool raised = false;

			propertyAccessing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName != nameof(PropertyAccessing<string>.Value)) return;
				string currentResult = propertyAccessing.Value;
				raised = true;
				Assert.IsTrue(currentResult == result);
			};

			Assert.IsTrue(propertyAccessing.Value == null);

			result = "1";
			order.Num = result;
			Assert.IsTrue(raised);
			Assert.IsTrue(propertyAccessing.Value == result);
			raised = false;

			result = "2";
			order.Num = result;
			Assert.IsTrue(raised);
			Assert.IsTrue(propertyAccessing.Value == result);

			result = null;
			consumer.Dispose();
		}

		[Test]
		public void TestRaiseValueChanged2()
		{
			Order order = new Order();
			PropertyAccessing<string> propertyAccessing = new Computing<Order>(() => order.ParentOrder).PropertyAccessing<string>("Num").For(consumer);
			string result = null;
			bool raised = false;

			propertyAccessing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName != nameof(PropertyAccessing<string>.Value)) return;
				string currentResult = propertyAccessing.Value;
				raised = true;
				Assert.AreEqual(currentResult, result);
			};

			Assert.IsTrue(propertyAccessing.Value == null);

			result = null;
			order.ParentOrder = new Order();
			result = "1";
			order.ParentOrder.Num = result;
			Assert.IsTrue(raised);
			Assert.AreEqual(propertyAccessing.Value, result);
			raised = false;

			result = "2";
			order.ParentOrder.Num = result;
			Assert.IsTrue(raised);
			Assert.AreEqual(propertyAccessing.Value, result);

			result = null;
			order.ParentOrder = new Order();
			result = "3";
			order.ParentOrder.Num = result;
			Assert.IsTrue(raised);
			Assert.AreEqual(propertyAccessing.Value, result);
			raised = false;

			result = null;
			order.ParentOrder = null;
			Assert.IsTrue(raised);
			Assert.AreEqual(propertyAccessing.Value, result);
			raised = false;

			result = null;
			order.ParentOrder = new Order();
			result = "4";
			order.ParentOrder.Num = result;
			Assert.IsTrue(raised);
			Assert.AreEqual(propertyAccessing.Value, result);
			raised = false;

			result = null;
			consumer.Dispose();
		}

		private PropertyAccessing<string>[] getPropertyAccessings(Order order)
		{
			List<PropertyAccessing<string>> propertyAccessings = new List<PropertyAccessing<string>>();
			propertyAccessings.Add(order.PropertyAccessing<string>("Num"));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", typeof(string)));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", typeof(string), new Type[0]));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", typeof(string), new Type[0], new ParameterModifier[0]));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", BindingFlags.Instance | BindingFlags.GetProperty));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", BindingFlags.Instance | BindingFlags.GetProperty, null, typeof(string), new Type[0], new ParameterModifier[0]));
			propertyAccessings.Add(order.PropertyAccessing<string>(pi => pi.Name == "Num"));
			propertyAccessings.Add(order.PropertyAccessing<string>(pi => pi.Name == "Num", BindingFlags.Instance | BindingFlags.GetProperty));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", (string)null));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", typeof(string), (string)null));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", typeof(string), new Type[0], (string)null));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", typeof(string), new Type[0], new ParameterModifier[0], (string)null));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", BindingFlags.Instance | BindingFlags.GetProperty, (string)null));
			propertyAccessings.Add(order.PropertyAccessing<string>("Num", BindingFlags.Instance | BindingFlags.GetProperty, null, typeof(string), new Type[0], new ParameterModifier[0], (string)null));
			propertyAccessings.Add(order.PropertyAccessing<string>(pi => pi.Name == "Num", (string)null));
			propertyAccessings.Add(order.PropertyAccessing<string>(pi => pi.Name == "Num", BindingFlags.Instance | BindingFlags.GetProperty, (string)null));

			return propertyAccessings.ToArray();
		}

		public PropertyAccessingTests(bool debug) : base(debug)
		{
		}
	}
}