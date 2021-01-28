using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public class BindingTest : TestBase
	{
		public class Order : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			private string _deliveryAddress;
			public string DeliveryAddress
			{
				get => _deliveryAddress;
				set
				{
					_deliveryAddress = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeliveryAddress)));
				}
			}
		}

		public class Car : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			private string _destinationAddress;
			public string DestinationAddress
			{
				get => _destinationAddress;
				set
				{
					_destinationAddress = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DestinationAddress)));
				}
			}
		}

		private void test(Binding<string> binding, bool applyNow, Order order, Car car)
		{
			if (applyNow)
				Assert.AreEqual(order.DeliveryAddress, car.DestinationAddress);
			else
				Assert.AreNotEqual(order.DeliveryAddress, car.DestinationAddress);

			order.DeliveryAddress = "A";
			Assert.AreEqual(order.DeliveryAddress, car.DestinationAddress);

			binding.BindOnSourceChanged = false;
			order.DeliveryAddress = "B";
			Assert.AreNotEqual(order.DeliveryAddress, car.DestinationAddress);

			binding.Bind();
			Assert.AreEqual(order.DeliveryAddress, car.DestinationAddress);

			Exception e = null;

			binding.BindOnDemand = false;
			order.DeliveryAddress = "C";
			Assert.AreNotEqual(order.DeliveryAddress, car.DestinationAddress);

			try
			{
				binding.Bind();
			}
			catch (Exception exception)
			{
				e = exception;
			}

			Assert.IsTrue(e != null);

			binding.Dispose();


			e = null;

			try
			{
				binding.BindOnDemand = true;
			}
			catch (Exception exception)
			{
				e = exception;
			}

			Assert.IsTrue(e != null);
			e = null;

			try
			{
				binding.BindOnSourceChanged = true;
			}
			catch (Exception exception)
			{
				e = exception;
			}

			Assert.IsTrue(e != null);
			e = null;

			try
			{
				binding.Bind();
			}
			catch (Exception exception)
			{
				e = exception;
			}

			Assert.IsTrue(e != null);
		}

		[Test]
		public void Test1()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};

			test(
				Expr.Is(() => order.DeliveryAddress).Binding(da => assignedDeliveryCar.DestinationAddress = da),
					true, order, assignedDeliveryCar);
		}

		[Test]
		public void Test2()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};

			test(
				Expr.Is(() => order.DeliveryAddress).Binding(da => assignedDeliveryCar.DestinationAddress = da, false),
					false, order, assignedDeliveryCar);
		}


		[Test]
		public void Test3()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};
			OcConsumer consumer = new OcConsumer();

			test(
				new Computing<string>(() => order.DeliveryAddress).For(consumer).Binding(da => assignedDeliveryCar.DestinationAddress = da, false),
					false, order, assignedDeliveryCar);
		}

		[Test]
		public void Test4()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};
			OcConsumer consumer = new OcConsumer();

			test(
				new Computing<string>(() => order.DeliveryAddress).For(consumer).Binding(da => assignedDeliveryCar.DestinationAddress = da),
					true, order, assignedDeliveryCar);
		}

		public BindingTest(bool debug) : base(debug)
		{
		}
	}
}
