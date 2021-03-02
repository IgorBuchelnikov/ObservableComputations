using System;
using System.ComponentModel;
using System.Linq.Expressions;
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

		private void test(Binding<string> binding, bool applyOnActivation, Order order, Car car)
		{
			OcConsumer consumer = new OcConsumer();
			binding.For(consumer);
			Assert.AreEqual(binding.ApplyOnActivation, applyOnActivation);

			if (applyOnActivation)
				Assert.AreEqual(order.DeliveryAddress, car.DestinationAddress);
			else
				Assert.AreNotEqual(order.DeliveryAddress, car.DestinationAddress);

			order.DeliveryAddress = "A";
			Assert.AreEqual(order.DeliveryAddress, car.DestinationAddress);

			binding.ApplyOnSourceChanged = false;
			order.DeliveryAddress = "B";
			Assert.AreNotEqual(order.DeliveryAddress, car.DestinationAddress);

			binding.Apply();
			Assert.AreEqual(order.DeliveryAddress, car.DestinationAddress);

			binding.ApplyOnSourceChanged = !applyOnActivation;
			Assert.AreEqual(binding.ApplyOnSourceChanged, !applyOnActivation);

			consumer.Dispose();
		}



		[Test]
		public void Test1()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};

			Action<string, Binding<string>> modifyTargetAction = (da, _) => assignedDeliveryCar.DestinationAddress = da;

			Expression<Func<string>> expression = () => order.DeliveryAddress;
			Binding<string> binding = expression.Binding(modifyTargetAction);
			test(binding, true, order, assignedDeliveryCar);
			Assert.AreEqual(modifyTargetAction, binding.ModifyTargetAction);
		}

		[Test]
		public void Test2()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};

			Action<string, Binding<string>> modifyTargetAction = (da, _) => assignedDeliveryCar.DestinationAddress = da;

			Expression<Func<string>> expression = () => order.DeliveryAddress;
			Binding<string> binding = expression.Binding(modifyTargetAction, false);
			test(binding, false, order, assignedDeliveryCar);
			Assert.AreEqual(modifyTargetAction, binding.ModifyTargetAction);
		}


		[Test]
		public void Test3()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};

			Action<string, Binding<string>> modifyTargetAction = (da, _) => assignedDeliveryCar.DestinationAddress = da;
			Computing<string> computing = new Computing<string>(() => order.DeliveryAddress);
			Binding<string> binding = computing
				.Binding(modifyTargetAction, false);

			test(binding, false, order, assignedDeliveryCar);
			Assert.AreEqual(modifyTargetAction, binding.ModifyTargetAction);
			Assert.AreEqual(computing, binding.Source);
		}

		[Test]
		public void Test4()
		{
			Order order = new Order(){DeliveryAddress = "0"};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};

			Action<string, Binding<string>> modifyTargetAction = (da, _) => assignedDeliveryCar.DestinationAddress = da;

			Computing<string> computing = new Computing<string>(() => order.DeliveryAddress);
			Binding<string> binding = computing
				.Binding(modifyTargetAction);

			test(binding, true, order, assignedDeliveryCar);

			Assert.AreEqual(modifyTargetAction, binding.ModifyTargetAction);
			Assert.AreEqual(computing, binding.Source);
		}

		public BindingTest(bool debug) : base(debug)
		{
		}
	}
}
