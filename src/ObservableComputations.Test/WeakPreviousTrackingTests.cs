using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class WeakPreviousTrackingTests
	{
        Consumer consumer = new Consumer();

		public class Order : INotifyPropertyChanged
		{
			private Order _parentOrder;
			public Order ParentOrder
			{
				get { return _parentOrder; }
				set { updatePropertyValue(ref _parentOrder, value); }
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
			bool raised = false;
			Order order = new Order();
			WeakPreviousTracking<Order> computing = new WeakPreviousTracking<Order>(new Computing<Order>(() => order.ParentOrder)).For(consumer);
			bool
                result = true;
			Order previousOrder = null;
            bool isEverchanged = true;

			computing.PropertyChanged += (sender, eventArgs) =>
			{
				bool currentResult;
				Order currentPreviousOrder;
				if (eventArgs.PropertyName != nameof(WeakPreviousTracking<Order>.Value)) return;
				currentResult = computing.TryGetPreviousValue(out currentPreviousOrder);
				Assert.IsTrue(currentResult == result);
				Assert.IsTrue(currentPreviousOrder == previousOrder);
				Assert.IsTrue(computing.IsEverChanged == isEverchanged);
			};

			Assert.IsFalse(computing.IsEverChanged);
			Assert.IsTrue(computing.Value == null);
			result = computing.TryGetPreviousValue(out previousOrder);
			Assert.IsTrue(previousOrder == null);
			Assert.IsFalse(result);

			result = false;
			previousOrder = null;
			order.ParentOrder = new Order();

			result = true;
			previousOrder = order.ParentOrder;
			order.ParentOrder = new Order();

			result = true;
			previousOrder = order.ParentOrder;
			order.ParentOrder = new Order();

            isEverchanged = false;
            result = true;
            consumer.Dispose();
		}

	}
}