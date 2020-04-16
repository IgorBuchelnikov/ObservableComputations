using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class DifferingTests
	{
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
			Differing<string> computing = new Differing<string>(new Computing<string>(() => order.Num));
			computing.PropertyChanged += (sender, args) => { if (args.PropertyName == "Value") raised = true; };

			order.Num = "1";
			Assert.IsTrue(raised);
			raised = false;

			order.Num = "1";
			Assert.IsFalse(raised);

			order.Num = "2";
			Assert.IsTrue(raised);
		}
	}
}