using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class PropertyAccessingTests
	{
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
			var propertyAccessing = order.PropertyAccessing<string>("Num");
			string result = null;
			bool raised = false;

			propertyAccessing.PropertyChanged += (sender, eventArgs) =>
			{
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
		}

		[Test]
		public void TestRaiseValueChanged2()
		{
			Order order = new Order();
			var propertyAccessing = new Computing<Order>(() => order.ParentOrder).PropertyAccessing<string>("Num");
			string result = null;
			bool raised = false;

			propertyAccessing.PropertyChanged += (sender, eventArgs) =>
			{
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
		}

	}
}