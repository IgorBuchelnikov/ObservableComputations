# ObservableCalculations
## What is it? 
This is .NET library for a calculations over INotifyPropertyChanged and INotifyColectionChanged (ObservableCollection) objects. Results of the calculations are INotifyPropertyChanged and INotifyColectionChanged (ObservableCollection) objects. The calculations includes ones similar to LINQ and the calculation of variant expression. 
## Status
ObservableCalculations library is ready to use in production. All essential functions is implemeted. All the bugs found is fixed.
## How can I help porject?
If you have positive or negative experience of using ObservableCalculations, please report it.
## Quick start
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using IBCode.ObservableCalculations;

namespace ObservableCalculationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public int Num {get; set;}

		private decimal _price;
		public decimal Price
		{
			get => _price;
			set
			{
				_price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}

		public Order(int num, decimal price)
		{
			Num = num;
			_price = price;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			ObservableCollection<Order> orders = 
				new ObservableCollection<Order>(new []
				{
					new Order(1, 15),
					new Order(2, 15),
					new Order(3, 25),
					new Order(4, 27),
					new Order(5, 30),
					new Order(6, 75),
					new Order(7, 80),
				});

			Filtering<Order> expensiveOrders = orders.Filtering(o => o.Price > 25);
			checkFiltering(orders, expensiveOrders); // Prints "True"

			expensiveOrders.CollectionChanged += (sender, eventArgs) =>
			{
				// see the changes here
			};

			// Start the changing...
			orders.Add(new Order(8, 30));
			orders.Add(new Order(9, 10));
			orders[0].Price = 60;
			orders[4].Price = 10;
			orders.Move(5, 1);
			orders[1] = new Order(10, 17);

			checkFiltering(orders, expensiveOrders); // Prints "True"

			Console.ReadLine();
		}

		static void checkFiltering(
		    ObservableCollection<Order> orders, 
		    Filtering<Order> expensiveOrders)
		{
			Console.WriteLine(expensiveOrders.SequenceEqual(
				orders.Where(o => o.Price > 25)));
		}
	}
}
```
As you can see Filtering method is analog of Where method from LINQ. ObservavleCalculations labrary contains analogs of LINQ methods.

