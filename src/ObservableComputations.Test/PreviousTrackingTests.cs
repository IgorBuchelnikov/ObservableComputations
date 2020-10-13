using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class PreviousTrackingTests
	{
        Consumer consumer = new Consumer();

		public class Order : INotifyPropertyChanged
		{
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
			bool raised = false;
			Order order = new Order();
			PreviousTracking<string> computing = new PreviousTracking<string>(new Computing<string>(() => order.Num)).IsNeededFor(consumer);
			Assert.IsFalse(computing.IsEverChanged);
			Assert.IsTrue(computing.PreviousValue == null);
			Assert.IsTrue(computing.Value == null);

			order.Num = "1";
			Assert.IsTrue(computing.IsEverChanged);
			Assert.IsTrue(computing.PreviousValue == null);
			Assert.IsTrue(computing.Value == "1");

			order.Num = "2";
			Assert.IsTrue(computing.IsEverChanged);
			Assert.IsTrue(computing.Value == "2");

            consumer.Dispose();
		}
	}
}