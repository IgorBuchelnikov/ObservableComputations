# ObservableCalculations
## What is it? 
This is .NET library for a calculations over INotifyPropertyChanged and INotifyColectionChanged (ObservableCollection) objects. Results of the calculations are INotifyPropertyChanged and INotifyColectionChanged (ObservableCollection) objects. The calculations includes ones similar to LINQ and the calculation of arbitrary expression. 
## Status
ObservableCalculations library is ready to use in production. All essential functions is implemeted. All the bugs found is fixed. Now I work on the readme and nuget package.
## How can I help porject?
If you have positive or negative experience of using ObservableCalculations, please report it.
## Quick start
### LINQ methods analogs
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

			//********************************************
			// We start using ObservableCalculations here!
			Filtering<Order> expensiveOrders = orders.Filtering(o => o.Price > 25); 
			
			checkFiltering(orders, expensiveOrders); // Prints "True"

			expensiveOrders.CollectionChanged += (sender, eventArgs) =>
			{
				// see the changes (add, remove, replace, move, reset) here
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
As you can see Filtering extension method is analog of Where method from LINQ. Filtering extension method returns instance of Filtering class. Filtering class implements INotifyCollectionChanged interface (and derived from ObservableCollection).

ObservavleCalculations library contains analogs of the all LINQ methods. You can combine calls of ObservavleCalculations extention methods including chaining and nesting, as you do for LINQ methods.

### Arbitrary expression observing
```csharp
using System;
using System.ComponentModel;
using System.Linq.Expressions;
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

		private byte _discount;
		public byte Discount
		{
			get => _discount;
			set
			{
				_discount = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Discount)));
			}
		}

		public Order(int num, decimal price, byte discount)
		{
			Num = num;
			_price = price;
			_discount = discount;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Order order = new Order(1, 100, 10);

			Expression<Func<decimal>> discountedPriceExpression =
				() => order.Price - order.Price * order.Discount / 100;

			//********************************************
			// We start using ObservableCalculations here!
			Calculating<decimal> discountedPriceCalculating = 
				discountedPriceExpression.Calculating();

			printDiscountedPrice(discountedPriceCalculating);

			discountedPriceCalculating.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Calculating<decimal>.Value))
				{
					printDiscountedPrice(discountedPriceCalculating);
				}
			};

			// Start the changing...
			order.Price = 200;
			order.Discount = 15;

			Console.ReadLine();
		}

		static void printDiscountedPrice(Calculating<decimal> discountedPriceCalculating)
		{
			Console.WriteLine($"Discounted price is ₽{discountedPriceCalculating.Value}");
		}
	}
}
```
In this code sample we observe value of discounted price expression. Calculating&lt;TResilt&gt; class implements INotifyPropertyChanged interface. Complicity of expression to observe is not limited. The expression can contain results of any ObservavleCalculations methods, including LINQ analogs.
### Use cases and benefits
WPF, Xamarin, Blazor. You can bind UI controls to the instances of ObservableCalculations classes (Filtering, Calculating). If you do it, you do not have to worry about forgetting to call INotifyPropertyChanged and INotifyColectionChanged for the calculated properties when the source properties changes. With ObservableCalculations, you define how the value should be calculated, everything else ObservableCalculations will do. 

This approach facilitates asynchronous programming. You can show the user a UI form and in the background load the source data (from DB or web service). As the source data loads, the UI form will be filled with the calculated data. If the UI form is already visible to the user, you can also refresh the source data in the background, the calculated data on the UI form will be refreshed thanks to ObservableCalculations. You get the following benefits:
* Source data loading code and UI refresh code can be clearly separated.
* The end user will see the UI form faster, since the beginning of the rendering is not tied to the availability of source data.

General benefits:
* Less human error: all data displayed to the user will always be in a consistent state.
* Less boilerplate code
* Calculated data will always correspond to the user input and data loaded from an external source.
* User has no need manually refresh calculated data.
* You do not need refresh calculated data by the timer.

