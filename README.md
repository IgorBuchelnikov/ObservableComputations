# ObservableComputations
## What I should  know to read this paper?
To understand written here you should know: basic programming and OOP concepts, C# syntax (including events and extension methods, lambda expressions), [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/), [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) and [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interfaces. 

It is advisable to know differences between [delegates](https://docs.microsoft.com/en-us/dotnet/api/system.delegate?view=netframework-4.8) and [expression trees](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/).

To imagine [benefits of using ObservableComputations](#use-cases-and-benefits) you should know about [binding in WPF](https://docs.microsoft.com/en-us/dotnet/desktop-wpf/data/data-binding-overview) (or in other UI platforms: [Xamarin](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/data-binding/basic-bindings), [Blazor](https://demos.telerik.com/blazor-ui/grid/observable-data)), especially in relation with [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) and [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interfaces, Entity framework`s [DbSet.Local](https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbset.local?view=entity-framework-6.2.0) property ([local data](https://docs.microsoft.com/en-us/ef/ef6/querying/local-data)), [asynchronous querying in Entity framework](https://www.entityframeworktutorial.net/entityframework6/async-query-and-save.aspx).  

## What is ObservableComputations? 
This is a cross-platform .NET library for computations whose arguments and results are objects that implement  [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) and [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) ([ObservableCollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8)) interfaces. The computations includes ones similar to [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/), the computation of arbitrary expression and additional features. The computations are implemented as [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), like [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) ones. You can combine calls of ObservableComputations extension methods including chaining and nesting, as you do for [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) methods. Computations in background threads, including parallel ones, as well as time related processing of [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) and [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) events, are supported. ObservableComputations is easy to use and powerful implementation of [reactive programming paradigm](https://en.wikipedia.org/wiki/Reactive_programming). With ObservableComputations, your code will fit more to the functional style than with [standard LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).

## Analogs
The closest analogs of ObservableComputations are following libraries: [Obtics](https://archive.codeplex.com/?p=obtics), [OLinq](https://github.com/wasabii/OLinq), [NFM.Expressions](https://github.com/NMFCode/NMF), [BindableLinq](https://github.com/svn2github/bindablelinq), [ContinuousLinq](https://github.com/ismell/Continuous-LINQ).

### [Reactive Extensions](https://github.com/dotnet/reactive)

ObservableComputations is not analog of [Reactive Extensions](https://github.com/dotnet/reactive). The main distinguish ObservableComputations from [Reactive Extensions](https://github.com/dotnet/reactive) is the following: 
* [Reactive Extensions](https://github.com/dotnet/reactive) is abstracted from event specific and event semantics: it is framework for processing all possible events. Reactive Extensions handles all events in the same way and all specifics are only in user code. ObservableComputations is focused to [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) and [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) events only and brings great benefit processing these events. 
* [Reactive Extensions](https://github.com/dotnet/reactive) library provides the stream of events. ObservableComputations library provides not only the stream of data change events, but currently computed data.

Some of the tasks that you solved using [Reactive Extensions](https://github.com/dotnet/reactive) are now easier and more efficient to solve using ObservableComputations. You can use ObservableComputations separately or in cooperation with [Reactive Extensions](https://github.com/dotnet/reactive). Observable Computations will not replace [Reactive Extensions](https://github.com/dotnet/reactive):
* when time related processing of events (Throttle, Buffer) needed. ObservableComputation allows you to implement time-related handling of [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) and [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) events by cooperation with [Reactive Extentions](https://github.com/dotnet/reactive) (see the example [here](#variants-of-the-implementation-of-the-idispatcher-interface-and-other-similar-interfaces));
* when processing events not related to data (for example, keystrokes), especially when combining these events. Example of cooperation ObservableComputations with combination [Reactive Extensions](https://github.com/dotnet/reactive) operators see [here](#isconsistent-property-and-inconsistency-exception);
* when working with asynchronous operations ([Observable.FromAsyncPattern method](https://docs.microsoft.com/en-us/previous-versions/dotnet/reactive-extensions/hh229052(v%3Dvs.103))).



### [ReactiveUI](https://github.com/reactiveui/ReactiveUI) and [DynamicData](https://github.com/reactiveui/DynamicData)

The [ReactiveUI](https://github.com/reactiveui/ReactiveUI) library (and its [DynamicData](https://github.com/reactiveui/DynamicData) sub-library) are not abstracted from the [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) and [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interfaces and when working with these interfaces allows you to do much the same things as ObservableComputations, but ObservableComputations are less verbose, easier to use, more declarative, less touches the source data. Why?

* Reactivity of ObservableComputations is based on two events only: [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) and [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8). This reactivity is native to ObservableComputations. Reactivity of [ReactiveUI](https://github.com/reactiveui/ReactiveUI) is based on interfaces inherited from [Reactive Extentions](https://github.com/dotnet/reactive): [IObserver&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iobserver-1?view=netframework-4.8), [IObservable&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iobservable-1?view=netframework-4.8,), as well as additional interfaces for working with collections (included in [DynamicData](https://github.com/reactiveui/DynamicData)): [IChangeSet](https://github.com/reactiveui/DynamicData/blob/master/src/DynamicData/IChangeSet.cs) and [IChangeSet&lt;TObject&gt;](https://github.com/reactiveui/DynamicData/blob/master/src/DynamicData/List/IChangeSet.cs). [ReactiveUI](https://github.com/reactiveui/ReactiveUI) performs bidirectional conversion between these interfaces and  [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) and [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interfaces. Even with this conversion, [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) and [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interfaces look alien in [ReactiveUI](https://github.com/reactiveui/ReactiveUI). (Offtopic: If you need [ReactiveUI](https://github.com/reactiveui/ReactiveUI) features that are not included in ObservableComputations, you can get them using this conversion. You can also use [ReactiveList](https://reactiveui.net/docs/handbook/obsolete/collections/reactive-list) as a source collection for ObservableComputations.)
* ObservableComputations uses weak events, so there is no need for [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.8). 
* [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) and [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interfaces are tightly integrated into Microsoft's UI platforms ([WPF](https://docs.microsoft.com/en-us/dotnet/desktop-wpf/data/data-binding-overview), [Xamarin](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/data-binding/basic-bindings), [Blazor](https://demos.telerik.com/blazor-ui/grid/observable-data)).

You can compare these library and ObservableComputations in action, see [ObservableComputations.Samples](https://github.com/IgorBuchelnikov/ObservableComputations.Samples).

## Status
ObservableComputations library is ready to use in production. All essential functions is implemented and covered by unit-tests. All the bugs found is fixed.

## How to install?
ObservableComputations is available on [NuGet](https://www.nuget.org/packages/ObservableComputations/).

## How can I get help?
You can [create issue](https://github.com/IgorBuchelnikov/ObservableComputations/issues/new) or [contact me via e-mail](mailto:igor_buchelnikov_github@mail.ru).

## How can I help the project?
Documentation comments and corrections are welcome. Demo projects, blog posts and tutorials are needed.

## Quick start
### Sample application  
[ObservableComputations.Samples](https://github.com/IgorBuchelnikov/ObservableComputations.Samples)

### [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) methods analogs
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using ObservableComputations;

namespace ObservableComputationsExamples
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
	}

	class Program
	{
		static void Main(string[] args)
		{
			ObservableCollection<Order> orders = 
				new ObservableCollection<Order>(new []
				{
					new Order{Num = 1, Price = 15},
					new Order{Num = 2, Price = 15},
					new Order{Num = 3, Price = 25},
					new Order{Num = 4, Price = 27},
					new Order{Num = 5, Price = 30},
					new Order{Num = 6, Price = 75},
					new Order{Num = 7, Price = 80}
				});

			// We start using ObservableComputations here!
			Filtering<Order> expensiveOrders = orders.Filtering(o => o.Price > 25); 
			
			Debug.Assert(expensiveOrders is ObservableCollection<Order>);
			
			checkFiltering(orders, expensiveOrders); // Prints "True"

			expensiveOrders.CollectionChanged += (sender, eventArgs) =>
			{
				// see the changes (add, remove, replace, move, reset) here			
				checkFiltering(orders, expensiveOrders); // Prints "True"
			};

			// Start the changing...
			orders.Add(new Order{Num = 8, Price = 30});
			orders.Add(new Order{Num = 9, Price = 10});
			orders[0].Price = 60;
			orders[4].Price = 10;
			orders.Move(5, 1);
			orders[1] = new Order{Num = 10, Price = 17};

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
As you can see *Filtering* extension method is analog of *Where* method from [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/). *Filtering* extension method returns instance of *Filtering&lt;Order&gt;* class. *Filtering&lt;TSourceItem&gt;* class implements [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interface and derived from [ObservableCollection&lt;TSourceItem&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8). Examining code above you can see *expensiveOrders* is not recomputed from scratch every time when the *orders* collection change or  *Price* property of some order changed, in the *expensiveOrders* collection occurs only that changes, that relevant to particular change in the *orders* collection or *Price* property of some order. [Referring reactive programming terminology, this behavior defines change propagation algorithm as "push"](https://en.wikipedia.org/wiki/Reactive_programming#Change_propagation_algorithms).


In the code above, during the execution of *Filtering* extension method  (during the creation of an instance of *Filtering&lt;Order&gt;* class), following events are subscribed: the  [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) event of *orders* collection and [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) event of every instance of the *Order* class. ObservableComputations performs weak subscriptions only (**weak event pattern**), so the *expensiveOrders* can be garbage collected, while the *orders* will remain alive.

Complexity of predicate expression passed to *Filtering* method (*o => o.Price > 25*) is not limited. The expression can contain results of any ObservableComputations methods, including [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) analogs.

### Arbitrary expression observing
```csharp
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using ObservableComputations;

namespace ObservableComputationsExamples
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
				PropertyChanged?.Invoke(this, 
					new PropertyChangedEventArgs(nameof(Price)));
			}
		}

		private byte _discount;
		public byte Discount
		{
			get => _discount;
			set
			{
				_discount = value;
				PropertyChanged?.Invoke(this, 
					new PropertyChangedEventArgs(nameof(Discount)));
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Order order = new Order{Num = 1, Price = 100, Discount = 10};

			// We start using ObservableComputations here!
			Computing<decimal> discountedPriceComputing = new Computing(
				() => order.Price - order.Price * order.Discount / 100);
				
			Debug.Assert(discountedPriceComputing is INotifyPropertyChanged);

			printDiscountedPrice(discountedPriceComputing);

			discountedPriceComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Computing<decimal>.Value))
				{
					// see the changes here
					printDiscountedPrice(discountedPriceComputing);
				}
			};

			// Start the changing...
			order.Price = 200;
			order.Discount = 15;

			Console.ReadLine();
		}

		static void printDiscountedPrice(Computing<decimal> discountedPriceComputing)
		{
			Console.WriteLine($"Discounted price is ₽{discountedPriceComputing.Value}");
		}
	}
}
```
In this code sample we observe value of discounted price expression. *Computing&lt;TResult&gt;* class implements [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) interface. Complexity of expression to observe is not limited. The expression can contain results of any ObservableComputations methods, including [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) analogs.

Same as in the previous example [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) event of *Order* class instance is subscribed weakly (**weak event pattern**).

If you want *() => order.Price - order.Price * order.Discount / 100* to be a pure function, no problem:  
  
```csharp
Expression<Func<Order, decimal>> discountedPriceExpression = 
	o => o.Price - o.Price * o.Discount / 100;
	
// We start using ObservableComputations here!
Computing<decimal> discountedPriceComputing = 
	order.Using(discountedPriceExpression);
```
Now *discountedPriceExpression* can be reused for other instances of *Order* class.


## Use cases and benefits

### UI binding
WPF, Xamarin, Blazor. You can bind UI controls to the instances of ObservableComputations classes (*Filtering*, *Computing* etc.). If you do it, you do not have to worry about forgetting to call [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) for the computed properties or manually process change in some collection. With ObservableComputations, you define how the value should be computed, everything else ObservableComputations will do. 

### Asynchronous programming
This approach facilitates **asynchronous programming**. You can show the user the UI form and in the background begin load the source data (from DB or web service). As the source data loads, the UI form will be filled with the computed data. The end user will see the UI form faster (while the source data is loaded in background, you can start rendering). If the UI form is already shown to the user, you can also refresh the source data in the background, the computed data on the UI form will be refreshed thanks to ObservableComputations. ObservableComputations also include features for multi-threaded computing. See [here](#multithreading) for details .

### Increased performance
If you have complex computations, over frequently changing source data and\or data is large, you can get increased performance with ObservableComputations, since you do not need recompute value from scratch every time when source data gets some little change. Every little change in source data causes a little change in the data computed by ObservableComputations.
UI performance is increased, as the need for re-rendering is reduced (only data that has changed is rendered) and data from external sources (DB, web service) is loaded in background (see [previous section](#asynchronous-programming)).

### Clean and durable code
* Less boilerplate imperative code. More clear declarative (functional style) code. Total code is reduced.
* Less human error: computed data shown to the user will always correspond to the user input and the data loaded from an external sources (DB, web service)
* Source data loading code and UI data computation code can be clearly separated.
* You do not need to worry about the fact that you forgot to update the calculated data. All calculated data will be updated automatically.

### Friendly UI
ObservableComputations facilitates design of friendly UI.
* User has no need manually refresh computed data.
* User can see computed data always, not only by request.
* You do not need refresh computed data by the timer.
* No need to block UI during the computation and rendering of a large amount of data (while showing a busy indicator). Data can be updated in small pieces, while the user can continue to work.

## Full list of operators
Before examine the table bellow, please take into account

* *CollectionComputing&lt;TSourceItem&gt;* derived from [ObservableCollection&lt;TSourceItem&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8). That class implements [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) interface.


* *ScalarComputing&lt;TValue&gt;* implements *IReadScalar&lt;TValue&gt;* interface;
```csharp
public interface IReadScalar<out TValue> : System.ComponentModel.INotifyPropertyChanged
{
	TValue Value { get;}
}
```
*Value* property allows you to get current result of a computation. From code above you can see: *ScalarComputation&lt;TValue&gt;* allows you to observe the changes of the *Value* property through [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) event of [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) interface.

<html>
<body>
<table cellspacing="0" border="0">
	<colgroup width="189"></colgroup>
	<colgroup width="147"></colgroup>
	<colgroup width="144"></colgroup>
	<colgroup width="395"></colgroup>
	<tr>
		<td colspan=4 height="45" align="center" valign=bottom><b><font size=4 color="#000000">MS LINQ analogs</font></b></td>
		</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="center" valign=top><b><font face="Segoe UI" color="#24292E">ObservableComputations <br>overloaded <br>methods group </font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">MS LINQ overloaded <br>methods group</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Returned instance<br> class derived from</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Note</font></b></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Appending</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Append</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Aggregating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Aggregate</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">AllCalcuating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">All</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">AnyCalcuating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Any</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Averaging</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Average</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Casting</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Cast</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="47" align="left" valign=bottom><font color="#000000">Concatenating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Concat</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Element of the source collection <br>may be INotifyCollectionChanged <br>or IReadScalar&lt;INotifyCollectionChanged&gt;</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ContainsComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Contains</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="32" align="left" valign=bottom><font color="#000000">ObservableCollection<br>    .Count property</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Count</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not implemented</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">DefaultIfEmpty</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Distincting</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Distinct</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="left" valign=bottom><font color="#000000">ItemComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ElementAtOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">If index requested is out of source collection<br>range ScalarComputing&lt;TSourceItem&gt;.Value <br>property returns default of TSourceItem <br>or value passed in defaultValue paremeter</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Excepting</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Except</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="41" align="left" valign=bottom><font color="#000000">FirstComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">FirstOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">If source collection length is zero <br>ScalarComputing&lt;TSourceItem&gt;.Value property <br>returns default of TSourceItem <br>or value passed in defaultValue paremeter</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Grouping</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Group</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Can contain a group with null key</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">GroupJoining</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" rowspan=2 align="left" valign=middle><font color="#000000">GroupJoin</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PredicateGroupJoining</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Intersecting</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Intersect</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Joing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Join</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="left" valign=bottom><font color="#000000">LastComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">LastOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">If source collection length is zero <br>ScalarComputing&lt;TSourceItem&gt;.Value property <br>returns default of TSourceItem <br>or value passed in defaultValue paremeter</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="left" valign=bottom><font color="#000000">Maximazing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Max</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">If source collection length is zero <br>ScalarComputing&lt;TSourceItem&gt;.Value property<br> returns default of TSourceItem <br>or value passed in defaultValue paremeter</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="left" valign=bottom><font color="#000000">Minimazing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Min</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">If source collection length is zero <br>ScalarComputing&lt;TSourceItem&gt;.Value property <br>returns default of TSourceItem <br>or value passed in defaultValue paremeter</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">OfTypeComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">OfType</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Ordering</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Order</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Ordering</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">OrderByDescending</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Prepending</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Prepend</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">SequenceComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Range</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Reversing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Reverse</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Selecting</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Select</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">SelectingMany</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">SelectMany</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Skiping</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Skip</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">SkipingWhile</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">SkipWhile</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">StringsConcatenating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">string.Join</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Summarizing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Sum</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Taking</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Take</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">TakingWhile</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">TakeWhile</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ThenOrdering</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ThenBy</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ThenOrdering</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ThenByDescending</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Dictionaring</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ToDictionary</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Dictionary</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Hashing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ToHashSet</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">HashSet</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Uniting</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Union</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Filtering</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Where</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Zipping</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Zip</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td colspan=4 height="59" align="center" valign=bottom><b><font size=4 color="#000000">Other features</font></b></td>
		</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="42" align="center" valign=top><b><font face="Segoe UI" color="#24292E">ObservableComputations overloaded methods group </font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="center" valign=middle><b><font face="Segoe UI" color="#24292E">Returned instance class derived from</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Note</font></b></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Binding class</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#binding">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">CollectionDispatching</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#multithreading">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="121" align="left" valign=bottom><font color="#000000">CollectionPausing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">When setting the value true <br>for the IsPaused property <br>does not apply changes <br>of source collection <br>and accumulates them, <br>further, when setting value false<br> for property IsPaused <br>applies all accumulated changes</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">CollectionProcessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#change-handling-in-observablecollectiont">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">CollectionProcessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#change-handling-in-observablecollectiont">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Computing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#arbitrary-expression-observing">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Differing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#differingtresult-extension-method">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Paging</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PreviousTracking</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#tracking-previous-value-of-ireadscalartvalue">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PropertyAccessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#accessing-a-property-via-reflection">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="left" valign=bottom><font color="#000000">PropertyDispatching</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">IReadScalar<br>IWriteScalar<br>IScalar<br>IComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#multithreading">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ScalarDispatching</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#multithreading">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="121" align="left" valign=bottom><font color="#000000">ScalarPausing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">When setting the value true <br>for the IsPaused property <br>does not apply changes <br>of source scalar<br>and accumulates them, <br>further, when setting value false<br> for property IsPaused <br>applies accumulated changes</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ScalarProcessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#handling-changes-in-ireadscalartvalue">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ScalarProcessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#handling-changes-in-ireadscalartvalue">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Using</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#arbitrary-expression-observing">here</a> and <a href="#applications-of-usingtresult-extension-method">here</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">WeakPreviousTracking</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">see more <a href="#tracking-previous-value-of-ireadscalartvalue">here</a></font></td>
	</tr>
</table>
<!-- ************************************************************************** -->
</body>
</html>

For the all computations having parameter of type [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8): null value of the parameter is treated as empty collection.

For the all computations having parameter of type  *IReadScalar*&lt;[INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8)&gt;: null value of *IReadScalar*&lt;[INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8)&gt;.*Value* property is treated as empty collection.


## Passing arguments as non-observables and observables
ObservableComputations extension method arguments can be passed by two ways: as non-observables and observables.

### Passing arguments as non-observables
```csharp
using System;
using System.Collections.ObjectModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Person
	{
		public  string Name { get; set; }
	}

	public class LoginManager
	{
		 public Person LoggedInPerson { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			Person[] allPersons = 
				new []
				{
					new Person(){Name = "Vasiliy"},
					new Person(){Name = "Nikolay"},
					new Person(){Name = "Igor"},
					new Person(){Name = "Aleksandr"},
					new Person(){Name = "Ivan"}
				};

			ObservableCollection<Person> hockeyTeam = 
				new ObservableCollection<Person>(new []
				{
					allPersons[0],
					allPersons[2],
					allPersons[3]
				});

			LoginManager loginManager = new LoginManager();
			loginManager.LoggedInPerson = allPersons[0];

			// We start using ObservableComputations here!
			ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
			    hockeyTeam.ContainsComputing(loginManager.LoggedInPerson);

			isLoggedInPersonHockeyPlayer.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(ContainsComputing<Person>.Value))
				{
					// see the changes here
				}
			};

			// Start the changing...
			hockeyTeam.RemoveAt(0);           // 🙂
			hockeyTeam.Add(allPersons[0]);    // 🙂
			loginManager.LoggedInPerson = allPersons[4];  // 🙁!
            
			Console.ReadLine();
		}
	}
}
```
In the code above we compute whether the logged in person is a hockey player. Expression "*loginManager.LoggedInPerson*" passed to *ContainsComputing* method is evaluated by ObservableComputations only once: when *ContainsComputing&lt;Person&gt;* class is instantiated (when *ContainsComputing* is called). If *LoggedInPerson* property changes, that change is **not** reflected in *isLoggedInPersonHockeyPlayer*. 

Of course, you can use more complex expression than "*loginManager.LoggedInPerson* for passing as an argument to any ObservableComputations extension method. As you see passing argument as non-observable of type *T* is ordinary way to pass argument of type *T*.

### Passing argument as observable

In the previous section, we assumed that our application does not support logging out (and subsequent logging in). In other words the application doesn't treat changes of *LoginManager.LoggedInPerson* property. Let us add logging out to our application:  
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.System.Linq.Expressions;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Person
	{
		public  string Name { get; set; }
	}

	public class LoginManager : INotifyPropertyChanged
	{
		private Person _loggedInPerson;

		public Person LoggedInPerson
		{
			get => _loggedInPerson;
			set
			{
				_loggedInPerson = value;
				PropertyChanged?.Invoke(this, 
					new PropertyChangedEventArgs(nameof(LoggedInPerson)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Person[] allPersons = 
				new []
				{
					new Person(){Name = "Vasiliy"},
					new Person(){Name = "Nikolay"},
					new Person(){Name = "Igor"},
					new Person(){Name = "Aleksandr"},
					new Person(){Name = "Ivan"}
				};

			ObservableCollection<Person> hockeyTeam = 
				new ObservableCollection<Person>(new []
				{
					allPersons[0],
					allPersons[2],
					allPersons[3]
				});

			LoginManager loginManager = new LoginManager();
			loginManager.LoggedInPerson = allPersons[0];

			//********************************************
			// We start using ObservableComputations here!			    
			ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
			    hockeyTeam.ContainsComputing<Person>(new Computing(
			    	() => loginManager.LoggedInPerson));

			isLoggedInPersonHockeyPlayer.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(ContainsComputing<Person>.Value))
				{
					// see the changes here
				}
			};

			// Start the changing...
			hockeyTeam.RemoveAt(0);           // 🙂
			hockeyTeam.Add(allPersons[0]);    // 🙂
			loginManager.LoggedInPerson = allPersons[4];  // 🙂!!!

			Console.ReadLine();
		}
	}
}
```

In the code above we pass the argument to the *ContainsComputing* method as *IReadScalar&lt;Person&gt;* (not as *Person* as in the code in the previous section). *Computing&lt;Person&gt;* implements *IReadScalar&lt;Person&gt;*. *IReadScalar&lt;TValue&gt;* was originally mentioned in the ["Full list of methods and classes" section](#full-list-of-operators). As you see if you want to pass argument of type T as observable you should perform ordinary argument passing of type *IReadScalar&lt;T&gt;*. In that case another overloaded version of *ContainsComputing* method is used than one in [the previous section](#Passing-arguments-as-non-observables). It gives us the opportunity to track changes of *LoginManager.LoggedInPerson* property. Now changes in the *LoginManager.LoggedInPerson* is reflected in *isLoggedInPersonHockeyPlayer*. Note than *LoginManager* class implements [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) now.  

Сode above can be shortened:  
  ```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
    hockeyTeam.ContainsComputing(() => loginManager.LoggedInPerson);
```
Using this overloaded version of *ContainsComputing* method variable *loggedInPersonExpression* is no longer needed. This overloaded version of *ContainsComputing* method creates *Computing&lt;Person&gt;* behind the scene.

Other shortened variant:

```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
    hockeyTeam.ContainsComputing<Person>(
        Expr.Is(() => loginManager.LoggedInPerson).Computing());
```

Original variant can be useful if you want reuse *new Computing(() => loginManager.LoggedInPerson)* for other computations than *isLoggedInPersonHockeyPlayer*. First shortened variant do not allow that. Shortened variants can be useful for the [expression-bodied properties and methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members).

Of course, you can use more complex expression than "*() => loginManager.LoggedInPerson* for passing as an argument to any ObservableComputations extension method.

### Passing source collection argument as observable
As you see all calls of [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) like extension methods generically can be presented as
```csharp
sourceCollection.ExtensionMethodName(arg1, arg2, ...);
```
*sourceCollection* is the first argument in the extension method declaration. So like other arguments that argument can also be passed as [non-observable](#Passing-arguments-as-non-observables) and as [observables](#passing-argument-as-observable). Before now we passed the source collections as non-observables (it was the simplest expression consisting of a single variable, of course we was able to use more complex expressions, but the essence is the same). Now let us try pass some source collection argument as observable:  

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.System.Linq.Expressions;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Person
	{
		public  string Name { get; set; }
	}

	public class LoginManager : INotifyPropertyChanged
	{
		private Person _loggedInPerson;

		public Person LoggedInPerson
		{
			get => _loggedInPerson;
			set
			{
				_loggedInPerson = value;
				PropertyChanged?.Invoke(this, 
					new PropertyChangedEventArgs(nameof(LoggedInPerson)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class HockeyTeamManager : INotifyPropertyChanged
	{
		private ObservableCollection<Person> _hockeyTeamInterested;

		public ObservableCollection<Person> HockeyTeamInterested
		{
			get => _hockeyTeamInterested;
			set
			{
				_hockeyTeamInterested = value;
				PropertyChanged?.Invoke(this, 
					new PropertyChangedEventArgs(nameof(HockeyTeamInterested)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Person[] allPersons = 
				new []
				{
					new Person(){Name = "Vasiliy"},
					new Person(){Name = "Nikolay"},
					new Person(){Name = "Igor"},
					new Person(){Name = "Aleksandr"},
					new Person(){Name = "Ivan"}
				};

			ObservableCollection<Person> hockeyTeam1 = 
				new ObservableCollection<Person>(new []
				{
					allPersons[0],
					allPersons[2],
					allPersons[3]
				});

			ObservableCollection<Person> hockeyTeam2 = 
				new ObservableCollection<Person>(new []
				{
					allPersons[1],
					allPersons[4]
				});

			LoginManager loginManager = new LoginManager();
			loginManager.LoggedInPerson = allPersons[0];

			HockeyTeamManager hockeyTeamManager = new HockeyTeamManager();
	    
			Expression<Func<ObservableCollection<Person>>> hockeyTeamInterestedExpression =
			    () => hockeyTeamManager.HockeyTeamInterested;

			//********************************************
			// We start using ObservableComputations here!	
			Computing<ObservableCollection<Person>> hockeyTeamInterestedComputing =
			    hockeyTeamInterestedExpression.Computing();

			ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
				hockeyTeamInterestedComputing.ContainsComputing(
				    () => loginManager.LoggedInPerson);

			isLoggedInPersonHockeyPlayer.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(ContainsComputing<Person>.Value))
				{
					// see the changes here
				}
			};

			// Start the changing...
			hockeyTeamManager.HockeyTeamInterested = hockeyTeam1;
			hockeyTeamManager.HockeyTeamInterested.RemoveAt(0);           
			hockeyTeamManager.HockeyTeamInterested.Add(allPersons[0]);  
			loginManager.LoggedInPerson = allPersons[4]; 
			loginManager.LoggedInPerson = allPersons[2];
			hockeyTeamManager.HockeyTeamInterested = hockeyTeam2;         
			hockeyTeamManager.HockeyTeamInterested.Add(allPersons[2]);  

			Console.ReadLine();
		}
	}
}
```

As in previous section code above can be shortened:
```csharp
Expression<Func<ObservableCollection<Person>>> hockeyTeamInterestedExpression =
    () => hockeyTeamManager.HockeyTeamInterested;

ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
   hockeyTeamInterestedExpression
      .ContainsComputing(() => loginManager.LoggedInPerson);
```

Or:
```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
   Expr.Is(() => hockeyTeamManager.HockeyTeamInterested)
      .ContainsComputing(() => loginManager.LoggedInPerson);
```

Or:  
```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
	new Computing<ObservableCollection<Person>>(
	    () => hockeyTeamManager.HockeyTeamInterested)
	.ContainsComputing<Person>(
		() => loginManager.LoggedInPerson);
```

Or:
```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
	Expr.Is(() => hockeyTeamManager.HockeyTeamInterested).Computing()
	.ContainsComputing(
	    () => loginManager.LoggedInPerson);
```

Of course, you can use more complex expression than "*() => hockeyTeamManager.HockeyTeamInterested* for passing as an argument to any ObservableComputations extension method.  

### Non-observable and observable arguments in nested calls
We continue to consider the example from the [previous section](#passing-source-collection-argument-as-observable). We used following code to track changes in  *hockeyTeamManager.HockeyTeamInterested*:
```csharp
new Computing<ObservableCollection<Person>>(
    () => hockeyTeamManager.HockeyTeamInterested)
```

It might seem at first glance that the following code will work and *isLoggedInPersonHockeyPlayer* will reflect changes of *hockeyTeamManager.HockeyTeamInterested*:

```csharp
Computing<bool> isLoggedInPersonHockeyPlayer = new Computing<bool>(() => 
   hockeyTeamManager.HockeyTeamInterested.ContainsComputing(
      () => loginManager.LoggedInPerson).Value);
```
 
In that code *"hockeyTeamManager.HockeyTeamInterested"* is passed to *ContainsComputing* method as [non-observable](#Passing-arguments-as-non-observables), and it does not matter that *"hockeyTeamManager.HockeyTeamInterested"* is part of expression passed to *Computing&lt;bool&gt;* class constructor, changes of *"hockeyTeamManager.HockeyTeamInterested"* is not reflected in *isLoggedInPersonHockeyPlayer*. Non-observable and observable arguments rule is applied in one-way detection: from nested (wrapped) calls to the outer (wrapper) calls. In other words, non-observable and observable arguments rule is always valid, regardless of whether the computation is root or nested.

Here is another example:

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public int Num {get; set;}

		private string _type;
		public string Type
		{
			get => _type;
			set
			{
				_type = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			ObservableCollection<Order> orders = 
				new ObservableCollection<Order>(new []
				{
					new Order{Num = 1, Type = "VIP"},
					new Order{Num = 2, Type = "Regular"},
					new Order{Num = 3, Type = "VIP"},
					new Order{Num = 4, Type = "VIP"},
					new Order{Num = 5, Type = "NotSpecified"},
					new Order{Num = 6, Type = "Regular"},
					new Order{Num = 7, Type = "Regular"}
				});

			ObservableCollection<string> selectedOrderTypes = new ObservableCollection<string>(new []
				{
					"VIP", "NotSpecified"
				});

			ObservableCollection<Order> filteredByTypeOrders =  orders.Filtering(o => 
				selectedOrderTypes.ContainsComputing(() => o.Type).Value);
			

			filteredByTypeOrders.CollectionChanged += (sender, eventArgs) =>
			{
				// see the changes (add, remove, replace, move, reset) here			
			};

			// Start the changing...
			orders.Add(new Order{Num = 8, Type = "VIP"});
			orders.Add(new Order{Num = 9, Type = "NotSpecified"});
			orders[4].Type = "Regular";
			orders.Move(4, 1);
			orders[0] = new Order{Num = 10, Type = "Regular"};
			selectedOrderTypes.Remove("NotSpecified");

			Console.ReadLine();
		}
	}
}
```
In the code above we have created *"filteredByTypeOrders"* computation that reflects changes in *orders*, *selectedOrderTypes* collections and in the *Order.Type* property. Take attention on argument passed to *ContainsComputing*. Following code will not reflect changes in the *Order.Type* property:

```csharp
ObservableCollection<Order> filteredByTypeOrders =  orders.Filtering(o => 
   selectedOrderTypes.ContainsComputing(o.Type).Value);
```

## Сomputation result change request handlers
The only way to modify result of computation is to modify source data. Неre is the code:

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		public int Num {get; set;}

		private string _manager;
		public string Manager
		{
			get => _manager;
			set
			{
				_manager = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Manager)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

	}

	class Program
	{
		static void Main(string[] args)
		{
			ObservableCollection<Order> orders = 
				new ObservableCollection<Order>(new []
				{
					new Order{Num = 1, Manager = "Stepan"},
					new Order{Num = 2, Manager = "Aleksey"},
					new Order{Num = 3, Manager = "Aleksey"},
					new Order{Num = 4, Manager = "Oleg"},
					new Order{Num = 5, Manager = "Stepan"},
					new Order{Num = 6, Manager = "Oleg"},
					new Order{Num = 7, Manager = "Aleksey"}
				});

			Filtering<Order> stepansOrders =  orders.Filtering(o => 
				o.Manager == "Stepan");
			
			stepansOrders.InsertItemAction = (i, order) =>
			{
				orders.Add(order);
				order.Manager = "Stepan";
			};

			Order newOrder = new Order(){Num = 8};
			stepansOrders.Add(newOrder);
			Debug.Assert(stepansOrders.Contains(newOrder));

			Console.ReadLine();
		}
	}
}
```

In the code above we created *stepansOrders* (Stepan's orders) computation. We set delegate to *stepansOrders.InsertItemAction* property to define how to modify *orders* collection and *order* to be inserted so what one is included in *stepansOrders* computation.

Note that [Add method](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.add?view=netframework-4.8#System_Collections_Generic_ICollection_1_Add__0_) is member of [ICollection&lt;T&gt; interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1?view=netframework-4.8).

This feature can be used if  you pass *stepansOrders* to the code abstracted from what is *stepansOrders*: computation or ordinary collection. That code only knows *stepansOrders* implements [ICollection&lt;T&gt; interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1?view=netframework-4.8) and sometimes wants add orders to *stepansOrders*. Such a code may be for example [binding in WPF](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.bindingmode?view=netframework-4.8#System_Windows_Data_BindingMode_TwoWay).

Properties similar to *InsertItemAction* exist for all other operations: 
* [*CollectionComputing&lt;TValue&gt;.Value*](#полный-список-операторов) :
   * [Add](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.add?view=netcore-3.1) and [Insert](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.insert?view=netcore-3.1): *InsertItemAction*
   * [Remove](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.remove?view=netframework-4.8): *RemoveItemAction*, 
   * [Set (Replace)](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.collection-1.item?view=netframework-4.8#System_Collections_ObjectModel_Collection_1_Item_System_Int32_): *SetItemAction*,
   * [Move](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1.move?view=netframework-4.8): *MoveItemAction*,
   * [Clear](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.clear?view=netframework-4.8): *ClearItemsAction*.
* [*ScalarComputing&lt;TValue&gt;.Value*](#полный-список-операторов) : 
  * *SetValueAction*.
* *Dictionaring* и *ConcurentDictionaring* :
  * [Add](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.add?view=netcore-3.1): *AddItemAction*,
  * [Remove](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.remove?view=netcore-3.1): *RemoveItemAction*,
  * [Set](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.item?view=netcore-3.1): *SetItemAction*
  * [Clear](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.clear?view=netframework-4.8): *ClearItemsAction*.
* HashSetting :
   * [Add](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.add?view=netcore-3.1): *AddItemAction*,
   * [Remove](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.remove?view=netframework-4.8): *RemoveItemAction*, 
   * [Clear](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.clear?view=netframework-4.8): *ClearItemsAction*. 

### Lock setting properties of computation result change request handlers

Properties of the computation change request handlers are public. By default, any code that has a reference to the computation can set or overwrite the value of this property. It is possible to control the ability to set the values of these properties using 

* methods of [*CollectionComputing&lt;TSourceItem&gt;*](#full-list-of-operators):
  * void LockModifyChangeAction(CollectionChangeAction collectionChangeAction, object key)
  * void UnlockModifyChangeAction(CollectionChangeAction collectionChangeAction, object key)
  * bool IsModifyChangeActionLocked(CollectionChangeAction collectionChangeAction)
* methods of [*ScalarComputing&lt;TValue&gt;*](#full-list-of-operators):
  * void LockModifySetValueAction(object key)
  * void UnlockModifySetValueAction(object key)
  * bool IsModifySetValueActionLocked()
* methods of [*Grouping&lt;TSourceItem, TKeygt;*](#full-list-of-operators):
  * void LockModifyGroupChangeAction(CollectionChangeAction collectionChangeAction, object key)
  * void UnlockModifyGroupChangeAction(CollectionChangeAction collectionChangeAction, object key)
  * bool IsModifyGroupChangeActionLocked(CollectionChangeAction collectionChangeAction) 
* methods of *Dictionaring* и *ConcurentDictionaring*:
  * void LockModifyChangeAction(DictionaryChangeAction dictionaryChangeAction, object key)
  * void UnlockModifyChangeAction(DictionaryChangeAction dictionaryChangeAction, object key)
  * bool IsModifyChangeActionLocked(DictionaryChangeAction dictionaryChangeAction)
* methods of *HashSetting*:
  * void LockModifyChangeAction(HashSetChangeAction hashSetChangeAction, object key)
  * void UnlockModifyChangeAction(HashSetChangeAction hashSetChangeAction, object key)
  * bool IsModifyChangeActionLocked(HashSetChangeAction hashSetChangeAction) 

## Processing changes of computation results
### Change handling in ObservableCollection&lt;T&gt;

Sometimes it becomes necessary to perform some actions
* with elements added to the collection
* with items to be removed from the collection
* elements moved within the collection

Of course, you can process all the current elements in the collection, then subscribe to the [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) event, but the ObservableComputations library contains a simpler and more effective tool.

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Client : INotifyPropertyChanged
	{
		public string Name { get; set; }

		private bool _online;

		public bool Online
		{
			get => _online;
			set
			{
				_online = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Online)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class NetworkChannel : IDisposable
	{
		public NetworkChannel(string clientName)
		{
			ClientName = clientName;
			Console.WriteLine($"NetworkChannel to {ClientName} has been created");
		}

		public string ClientName { get; set; }

		public void Dispose()
		{
			Console.WriteLine($"NetworkChannel to {ClientName} has been disposed");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			ObservableCollection<Client> clients = new ObservableCollection<Client>(new Client[]
			{
				new Client(){Name  = "Sergey", Online = false},
				new Client(){Name  = "Evgeney", Online = true},
				new Client(){Name  = "Anatoley", Online = false}
			});

			Filtering<Client> onlineClients = clients.Filtering(c => c.Online);

			var processing = 
				onlineClients.CollectionProcessing(
					(client, @this) =>
					{
						var networkChannel  = new NetworkChannel(client.Name);
						return networkChannel;
					},
					(client, @this, networkChannel) =>
					{
						networkChannel.Dispose();
					});
					

			clients[2].Online = true;
			clients.RemoveAt(1);

			Console.ReadLine();
		}
	}
}
```

Delegate passed to the *newItemProcessor* parameter is called

* when instantiating *CollectionProcessing&lt;TSourceItem, TReturnValue&gt;* class (if the source collection (*clients*) contains elements at the time of instantiation),
* when adding items to the source collection,
* when replacing an item in the source collection (setting the collection item by index),
* when [source collection is passed as scalar](#passing-source-collection-argument-as-observable) (*IReadScalar&lt;TValue&gt;*), and its value changes to the collection that contains the elements.

The delegate passed to the * oldItemProcessor * parameter is called

* when removing items in the source collection,
* when replacing an item in the source collection (setting the collection item by index),
* when cleaning the source collection (Clear() method),
* when [source collection is passed as scalar](#passing-source-collection-argument-as-observable) (*IReadScalar&lt;TValue&gt;*), and its value changes.

It is also possible to pass *moveItemProcessor* delegate to handle event of element move in the source collection.

In order to avoid unloading from memory an instance of *CollectionProcessing&lt;TSourceItem, TReturnValue&gt;* class by the garbage collector, save reference to it in an object that has a suitable lifetime.

The value returned by the delegate passed to *newItemProcessor* parameter can also be used to save references in order to avoid garbage collection from memory, for example, if you create instances of [Binding](#Binding), [CollectionProcessing](#change-handling-in-observablecollectiont) or [ScalarProcessing](#handling-changes-in-ireadscalartvalue) classes when adding items to the source collection.

There is also an overloaded version of the *CollectionProcessing* method, which accepts *newItemProcessor* delegate that returns an empty value (void).

### Handling changes in IReadScalar&lt;TValue&gt;

*IReadScalar&lt;TValue&gt;* is mentioned for the first time here. You can handle changes to the Value property by subscribing to the [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) event, but similar to processing changes in ObservableCollection&lt;T&gt; ObservableComputations allows you to process changes in *IReadScalar&lt;TValue&gt;* easier and more efficiently:

```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Client : INotifyPropertyChanged
	{
		private NetworkChannel _networkChannel;

		public NetworkChannel NetworkChannel
		{
			get => _networkChannel;
			set
			{
				_networkChannel = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NetworkChannel)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class NetworkChannel : IDisposable
	{
		public NetworkChannel(int num)
		{
			Num = num;
			
		}

		public int Num { get; set; }

		public void Open()
		{
			Console.WriteLine($"NetworkChannel #{Num} has been opened");
		}

		public void Dispose()
		{
			Console.WriteLine($"NetworkChannel #{Num} has been disposed");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var networkChannel  = new NetworkChannel(1);
			Client client = new Client() {NetworkChannel = networkChannel};

			Computing<NetworkChannel> networkChannelComputing 
				= new Computing<NetworkChannel>(() => client.NetworkChannel);

			networkChannelComputing.ScalarProcessing(
				(networkChannel1, @this) =>
				{
					// this.Value is old NetworkChannel
					@this.Value?.Dispose();

					// networkChannel1 is new NetworkChannel
					networkChannel1.Open();
				});

			client.NetworkChannel = new NetworkChannel(2);
			client.NetworkChannel = new NetworkChannel(3);

			Console.ReadLine();
		}
	}
}
}
```  

In order to avoid unloading an instance of the *ScalarProcessing&lt;TValue&gt;* class from the memory by the garbage collector, save refrence to it in object that has a suitable lifetime.

There is also an overloaded version of the *ScalarProcessing* method that accepts a *newValueProcessor* delegate that returns a non-void value. This value can be used to free resources ([IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.8)) or to save references to avoid garbage collection from memory, for example, if instances of [Binding](#Binding), [CollectionProcessing](#change-handling-in-observablecollectiont) or [ScalarProcessing](#handling-changes-in-ireadscalartvalue) classes are created in *newValueProcessor* delegate.

## IsConsistent property and inconsistency exception
Scenario described in this section is very specific. May be you will never meet it. However if want to be fully prepared read it. Consider following code:
```csharp
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public enum RelationType { Parent, Child }

	public struct Relation
	{
		public string From {get; set;}
		public string To {get; set;}
		public RelationType Type {get; set;}
	}

	class Program
	{
		static void Main(string[] args)
		{
			RelationType invertRelationType(RelationType relationType)
			{
				return relationType == RelationType.Child ? RelationType.Parent : RelationType.Child;
			}

			ObservableCollection<Relation> relations = 
				new ObservableCollection<Relation>(new []
				{
					new Relation{From = "Valentin", To = "Filipp", Type = RelationType.Child},
					new Relation{From = "Filipp", To = "Valentin", Type = RelationType.Parent},

					new Relation{From = "Olga", To = "Evgeny", Type = RelationType.Child},
					new Relation{From = "Evgeny", To = "Olga", Type = RelationType.Parent}
				});

			var orderedRelations = relations.Ordering(r => r.From);

			orderedRelations.CollectionChanged += (sender, eventArgs) =>
			{
				switch (eventArgs.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//...
						break;
					case NotifyCollectionChangedAction.Remove:
						//...
						break;
					case NotifyCollectionChangedAction.Replace:
						Relation oldItem = (Relation) eventArgs.OldItems[0];
						relations.Remove(new Relation{From = oldItem.To, To = oldItem.From, Type = invertRelationType(oldItem.Type)}); 
						// ObservableComputationsException is thrown !!!

						Relation newItem = (Relation) eventArgs.NewItems[0];
						relations.Add(new Relation{From = newItem.To, To = newItem.From, Type = invertRelationType(newItem.Type)});
						break;
				}
			};

			relations[0] = new Relation{From = "Arseny", To = "Dmitry", Type = RelationType.Parent};

			Console.ReadLine();
		}
	}
}
```

In the code above we have collection of relations: *relations*. That collection has redundancy: if the collection contains relation A to B as parent, it must contain corresponding relation: B to A as child, and vise versa. Also we have computed collection of ordered relations: *orderedRelations*. Our task is to support integrity of relations collection: if someone changes it we have to react so the collection restores integrity. Imagine that the only way to do it is to subscribe to CollectionChanged event of *orderedRelations* collection (for some reason we cannot subscribe to CollectionChanged event of *relations* collection). In the code above we consider only one type of change: Replace.
Code above does not work: line "*relations.Remove(new Relation{From = oldItem.To, To = oldItem.From, Type = invertRelationType(oldItem.Type)});*" throws:
> ObservableComputations.Common.ObservableComputationsException: 'The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after Consistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.'

Why? When an item is replaced in *relations* collection, *orderedRelations* collection produces not only a replacement, but also an additional subsequent movement of the item to maintain order. After replacement and prior to movement, *orderedRelations* is in an inconsistent state and so cannot process any other source collection change. Here is fixed code:  
  
```csharp
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public enum RelationType { Parent, Child }

	public struct Relation
	{
		public string From {get; set;}
		public string To {get; set;}
		public RelationType Type {get; set;}
	}

	class Program
	{
		static void Main(string[] args)
		{
			RelationType invertRelationType(RelationType relationType)
			{
				return relationType == RelationType.Child ? RelationType.Parent : RelationType.Child;
			}

			ObservableCollection<Relation> relations = 
				new ObservableCollection<Relation>(new []
				{
					new Relation{From = "Valentin", To = "Filipp", Type = RelationType.Child},
					new Relation{From = "Filipp", To = "Valentin", Type = RelationType.Parent},

					new Relation{From = "Olga", To = "Evgeny", Type = RelationType.Child},
					new Relation{From = "Evgeny", To = "Olga", Type = RelationType.Parent}
				});

			var orderedRelations = relations.Ordering(r => r.From);

			orderedRelations.CollectionChanged += (sender, eventArgs) =>
			{
				switch (eventArgs.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//...
						break;
					case NotifyCollectionChangedAction.Remove:
						//...
						break;
					case NotifyCollectionChangedAction.Replace:
                        Debug.Assert(orderedRelations.IsConsistent == false);					
					    // HERE IS THE FIX !!!
						orderedRelations.ConsistencyRestored += (o, args1) =>
						{
							Relation oldItem = (Relation) eventArgs.OldItems[0];
							relations.Remove(new Relation{From = oldItem.To, To = oldItem.From, Type = invertRelationType(oldItem.Type)});

							Relation newItem = (Relation) eventArgs.NewItems[0];
							relations.Add(new Relation{From = newItem.To, To = newItem.From, Type = invertRelationType(newItem.Type)});
						};

						break;
				}
			};

			relations[0] = new Relation{From = "Arseny", To = "Dmitry", Type = RelationType.Parent};

			Console.ReadLine();
		}
	}
}
```
In the fixed code, we defer restoration of integrity of *relations* collection until ConsistencyRestored event of *orderedRelations* collection occurs. 
For the sake of simplification we don't unsubscribe from ConsistencyRestored event, so we accumulate ConsistencyRestored event handlers. To fix it we can do unsubscribe from ConsistencyRestored event manually or use [Reactive Extensions](https://github.com/dotnet/reactive):

```csharp
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public enum RelationType { Parent, Child }

	public struct Relation
	{
		public string From {get; set;}
		public string To {get; set;}
		public RelationType Type {get; set;}
	}

	class Program
	{
		static void Main(string[] args)
		{
			RelationType invertRelationType(RelationType relationType)
			{
				return relationType == RelationType.Child ? RelationType.Parent : RelationType.Child;
			}

			ObservableCollection<Relation> relations = 
				new ObservableCollection<Relation>(new []
				{
					new Relation{From = "Valentin", To = "Filipp", Type = RelationType.Child},
					new Relation{From = "Filipp", To = "Valentin", Type = RelationType.Parent},

					new Relation{From = "Olga", To = "Evgeny", Type = RelationType.Child},
					new Relation{From = "Evgeny", To = "Olga", Type = RelationType.Parent}
				});

			Ordering<Relation, string> orderedRelations = relations.Ordering(r => r.From);

			Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>( 
					h => orderedRelations.CollectionChanged += h,
					h => orderedRelations.CollectionChanged -= h)
				.Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Replace)
				.Zip(Observable.FromEventPattern<EventHandler, EventArgs>(               
					h => orderedRelations.ConsistencyRestored += h,
					h => orderedRelations.ConsistencyRestored -= h), 
					(collectionChangedEventPattern, consistencyRestoredEventPattern) =>
						collectionChangedEventPattern.EventArgs)
				.Subscribe(collectionChangedEventArgs => {
					Relation oldItem = (Relation) collectionChangedEventArgs.OldItems[0];
					relations.Remove(new Relation{From = oldItem.To, To = oldItem.From, Type = invertRelationType(oldItem.Type)});

					Relation newItem = (Relation) collectionChangedEventArgs.NewItems[0];
					relations.Add(new Relation{From = newItem.To, To = newItem.From, Type = invertRelationType(newItem.Type)});
				});

			relations[0] = new Relation{From = "Arseny", To = "Dmitry", Type = RelationType.Parent};

			Console.ReadLine();
		}
	}
}
```

Debuging of inconsistency exception described [here](#inconsistency-exception).

## Debugging

### User code

Use code includes:

* Selectors are expressions that are passed as an argument to the following [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods): *Selecting*, *SelectingMany*, *Grouping*, *GroupJoining*, *Dictionaring*, *Hashing*, *Ordering*, *ThenOrdering*, *PredicateGroupJoining*. 

* Predicates are expressions that are passed as an argument to *Filtering* [extension method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). 

* Aggregation functions are delegates that are passed as an argument to *Aggregating* [extension method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). 

* Arbitrary expressions are expressions that are passed as argument to *Computing* and *Using* [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). 

* Сomputation change request handlers was described in the ["Modifying computations"](#modifying-computations). 

* Handlers of changes of computation results was described in the ["Modifying computations"](#processing-changes-of-computation-results). 

* Code called using the methods [*Dispatcher.Invoke* and *Dispatcher.BeginInvoke*](#using-the-dispatcher-class).

Here is the code illustrating debugging of arbitrary expressions (other types of code can be debugged by the same way):

```csharp
using System;
using System.ComponentModel;
using System.Threading;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class ValueProvider : INotifyPropertyChanged
	{
		private int _value;

		public int Value
		{
			get => _value;
			set
			{
				_value = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
	class Program
	{
		static void Main(string[] args)
		{
			Configuration.SaveInstantiatingStackTrace = true;
			Configuration.TrackComputingsExecutingUserCode = true;

			ValueProvider valueProvider = new ValueProvider(){Value = 2};

			Computing<decimal> computing1 = new Computing<decimal>(() => 1 / valueProvider.Value);
			Computing<decimal> computing2 = new Computing<decimal>(() => 1 / (valueProvider.Value - 1));

			try
			{
				valueProvider.Value = new Random().Next(0, 1);
			}
			catch (DivideByZeroException exception)
			{
				Console.WriteLine($"Exception stacktrace:\n{exception.StackTrace}");
				Console.WriteLine($"\nComputing which caused the exception has been instantiated by the following stacktrace :\n{DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].InstantiatingStackTrace}");
			}

			Console.ReadLine();
		}
	}
}
```

As you see *exception.StackTrace* points to line caused the exception: *valueProvider.Value = new Random().Next(0, 1);*. That line doesn't point us to computation which caused the exception: *computing1* or *computing2*. To determine computation which caused the exception we should look at *DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].InstantiatingStackTrace* property. That property contains stack trace of instantiating of the computation. By default ObservableComputations doesn't save stack traces of instantiating of computations for performance reasons. To save that stack traces use *Configuration.SaveInstantiatingStackTrace* property. By default ObservableComputations doesn't track computations executing user code for performance reasons. To track computations executing user code use *Configuration.TrackComputingsExecutingUserCode* property.
If the user code was called from the user code of another computation, then *DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].UserCodeIsCalledFrom* will point to that computation.

All unhanded exceptions thrown in the user code are fatal, as the internal state of the computations becomes damaged. Pay attention to null checks.

### User code in background threads
Work with computations in background threads is described [here](#multithreading).
```csharp
using System;
using System.ComponentModel;
using System.Threading;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class ValueProvider : IReadScalar<int>
	{
		private int _value;

		public int Value
		{
			get => _value;
			set
			{
				_value = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			Configuration.SaveInstantiatingStackTrace = true;
			Configuration.TrackComputingsExecutingUserCode = true;
			Configuration.SaveDispatcherInvocationStackTrace = true;
			Configuration.TrackDispatcherInvocations = true;

			ValueProvider valueProvider = new ValueProvider(){Value = 2};
			
			Dispatcher dispatcher = new Dispatcher();

			System.AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				Console.WriteLine($"Exception stacktrace:\n{DebugInfo.ExecutingDispatcherInvocations[dispatcher.Thread].Peek().CallStackTrace}");
				Console.WriteLine($"\nComputing which caused the exception has been instantiated by the following stacktrace :\n{DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].InstantiatingStackTrace}");
				Console.WriteLine($"\nDispatch computing which caused the exception has been instantiated by the following stacktrace :\n{((IComputing) DebugInfo.ExecutingDispatcherInvocations[dispatcher.Thread].Peek().Context).InstantiatingStackTrace}");

				Thread.CurrentThread.IsBackground = true;

				while (true)
					Thread.Sleep(TimeSpan.FromHours(1));
			};

			ScalarDispatching<int> valueProviderDispatching = valueProvider.ScalarDispatching(dispatcher);

			Computing<decimal> computing1 = new Computing<decimal>(() => 1 / valueProviderDispatching.Value);
			Computing<decimal> computing2 = new Computing<decimal>(() => 1 / (valueProviderDispatching.Value - 1));


			try
			{
				valueProvider.Value = new Random().Next(0, 2);
			}
			catch (DivideByZeroException exception)
			{
				Console.WriteLine($"Exception stacktrace:\n{exception.StackTrace}");
				Console.WriteLine($"\nComputing which caused the exception has been instantiated by the following stacktrace :\n{DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].InstantiatingStackTrace}");
			}

			Console.ReadLine();
		}
	}
}
```
This example is similar to the previous one, except
* Properties that contain exception information
* Setting configuration parameters *Configuration.SaveDispatcherInvocationStackTrace* and *Configuration.TrackDispatcherInvocations*

*DebugInfo.ExecutingDispatcherInvocations [dispatcher.Thread]* is of type Stack&lt;Invocation&gt;. The stack will contain more than one element if you called the *Dispatcher.DoOthers* method.

### Inconsistency exception

Inconsistency exception was described in the ["IsConsistent property and inconsistency exception"](#IsConsistent-property-and-inconsistency-exception) section.
In the following example, we are trying to make discount on expensive orders: we truncate order price to the lowest value, a multiple of one hundred:

```csharp
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

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
	}

	class Program
	{
		static void Main(string[] args)
		{
			Configuration.SaveInstantiatingStackTrace = true;

			ObservableCollection<Order> orders = 
				new ObservableCollection<Order>();

			Filtering<Order> ordinaryOrders = orders.Filtering(o => o.Price <= 25000);

			Filtering<Order> expensiveOrders = orders.Filtering(o => o.Price > 25000); 

			expensiveOrders.CollectionChanged += (sender, eventArgs) =>
			{
				switch (eventArgs.Action)
				{
					case NotifyCollectionChangedAction.Add:
						Order addedOrder = (Order) eventArgs.NewItems[0];
						addedOrder.Price = Math.Truncate(addedOrder.Price / 100) * 100;
						break;
				}
			};

			try
			{
				orders.Add(new Order(){Price = 35397});
			}
			catch (ObservableComputationsInconsistencyException exception)
			{
				Console.WriteLine($"Exception stacktrace:\n{exception.StackTrace}");
				Console.WriteLine($"\nComputing which caused the exception has been instantiated by the following stacktrace :\n{exception.Computing.InstantiatingStackTrace}");
				Console.WriteLine($"\nSender for the event that cannot be processed is :\n{exception.EventSender.ToStringSafe()}");
				Console.WriteLine($"\nArgs for the event that cannot be processed is :\n{exception.EventArgs.ToStringAlt()}");
				Console.WriteLine($"\nSender of event now processing is :\n{exception.Computing.HandledEventSender.ToStringSafe()}");
				Console.WriteLine($"\nArgs for the event that is currently being processed is :\n{exception.Computing.HandledEventArgs.ToStringAlt()}");

			}

			Console.ReadLine();
		}
	}
}
```

As you see *exception.StackTrace* points to line caused the exception: *orders.Add(new Order(){Price = 35397});*. That line doesn't point us to computation which caused the exception: *ordinaryOrders* or *expensiveOrders*. To determine computation which caused the exception we should look at *exception.Computing.InstantiatingStackTrace* property. That property contains stack trace of instantiating of the computation. By default ObservableComputations doesn't save stack traces of instantiating of computation for performance reasons. To save that stack traces use *Configuration.SaveInstantiatingStackTrace* property.


## Additional events for changes handling: PreCollectionChanged, PreValueChanged, PostCollectionChanged, PostValueChanged
```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		private double _price;
		public double Price
		{
			get => _price;
			set
			{
				_price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}

		private bool _discount;
		public bool Discount
		{
			get => _discount;
			set
			{
				_discount = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Discount)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Order order = new Order(){Price = 100};

			Computing<string> messageForUser = null;

			Computing<double> priceDiscounted 
				= new Computing<double>(() => order.Discount 
				    ? order.Price - order.Price * 0.1 
				    : order.Price);

			priceDiscounted.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Computing<double>.Value))
					Console.WriteLine(messageForUser.Value);
			};

			messageForUser 
				= new Computing<string>(() => order.Price > priceDiscounted.Value
					? $"Your order price is ₽{order.Price}. You have a discount! Therefore your price is ₽{priceDiscounted.Value}!"
					: $"Your order price is ₽{order.Price}");

			order.Discount = true;

			Console.ReadLine();
		}
	}
}
```

Code above has following output:
> Your order price is ₽100

Although we could expect:  
> Your order price is ₽100. You have a discount! Therefore your price is ₽90!

Why? We subscribe to *priceDiscounted.PropertyChanged* before *messageForUser* does it. Event handlers are invoked in the order of subscriptions (it is implementation detail of .NET). So we read *messageForUser.Value* before *messageForUser* handles change of *order.Discount*.

Here is the fixed code:
```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		private double _price;
		public double Price
		{
			get => _price;
			set
			{
				_price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}

		private bool _discount;
		public bool Discount
		{
			get => _discount;
			set
			{
				_discount = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Discount)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Order order = new Order(){Price = 100};

			Computing<string> messageForUser = null;

			Computing<double> priceDiscounted 
				= new Computing<double>(() => order.Discount 
				    ? order.Price - order.Price * 0.1 
				    : order.Price);

                        // HERE IS THE FIX!
			priceDiscounted.PostValueChanged += (sender, eventArgs) =>
			{
				Console.WriteLine(messageForUser.Value);
			};

			messageForUser 
				= new Computing<string>(() => order.Price > priceDiscounted.Value
					? $"Your order price is ₽{order.Price}. You have a discount! Therefore your price is ₽{priceDiscounted.Value}!"
					: $"Your order price is ₽{order.Price}");

			order.Discount = true;

			Console.ReadLine();
		}
	}
}
```

Instead of  *priceDiscounted.PropertyChanged* we subscribe to *priceDiscounted.PostValueChanged*. That event is raised after *PropertyChanged*, so we can sure: all the dependent computations have refreshed their values. *PostValueChanged* is declared in *ScalarComputing&lt;TValue&gt;*. *Computing&lt;string&gt;* inherits *ScalarComputing&lt;TValue&gt;*. *ScalarComputing&lt;TValue&gt;* is mentioned [here](#full-list-of-operators) for the first time. *ScalarComputing&lt;TValue&gt;* contains *PreValueChanged* event. That event allow you see state of the all computations before a change.

*CollectionComputing&lt;TItem&gt;* contains *PreCollectionChanged* and *PostCollectionChanged* events. *CollectionComputing&lt;TItem&gt;* is mentioned [here](#full-list-of-operators) for the first time. If you want handle collection change of your collection that implements [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) (not of computed collection (for example [*ObservableCollection&lt;TItem&gt;*](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8)) and that handle reads dependent computations you may use *ObservableCollectionExtended&lt;TItem&gt;*. That class inherits [*ObservableCollection&lt;TItem&gt;*](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8) and contains *PreCollectionChanged* and *PostCollectionChanged* events. Also you can use *Extending* extension method. That method creates *ObservableCollectionExtended&lt;TItem&gt;* from [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8).

## Multithreading
### Thread safety
[*CollectionComputing&lt;TSourceItem&gt;*](#full-list-of-operators) and [*ScalarComputing&lt;TSourceItem&gt;*](#full-list-of-operators)
* supports multiple reader threads simultaneously, as long there are no modifications made by the writer thread. Exclusion: *ConcurrentDictionaring* computation, which supports simultaneous reading and modification.
* do not support simultaneous modfications by multiple writer threads. 

The computations are modified by writer thread while they handle [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) and [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) events of source objects.

### Loading source data in a background thread
Code of the window of the WPF application:
```xml
<Window
	x:Class="ObservableComputationsExample.MainWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
	xmlns:local="clr-namespace:ObservableComputationsExample"
	xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	x:Name="uc_this"
	Title="ObservableComputationsExample"
	Width="800"
	Height="450"
	mc:Ignorable="d">
	<Grid>
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width="*" />
			<ColumnDefinition Width="*" />
		</Grid.ColumnDefinitions>
		<Grid.RowDefinitions>
			<RowDefinition Height="Auto" />
			<RowDefinition Height="Auto" />
			<RowDefinition Height="*" />
		</Grid.RowDefinitions>

		<Label
			x:Name="uc_LoadingIndicator"
			Grid.Row="0"
			Grid.Column="0"
			Grid.ColumnSpan="2"
			HorizontalAlignment="Center">
			Loading source data...
		</Label>

		<Label
			Grid.Row="1"
			Grid.Column="0"
			FontWeight="Bold">
			Unpaid orders
		</Label>
		<ListBox
			Grid.Row="2"
			Grid.Column="0"
			DisplayMemberPath="Num"
			ItemsSource="{Binding UnpaidOrders, ElementName=uc_this}" />

		<Label
			Grid.Row="1"
			Grid.Column="1"
			FontWeight="Bold">
			Paid orders
		</Label>
		<ListBox
			Grid.Row="2"
			Grid.Column="1"
			DisplayMemberPath="Num"
			ItemsSource="{Binding PaidOrders, ElementName=uc_this}" />
	</Grid>
</Window>
```

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ObservableComputations;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		public MainWindow()
		{
			Orders = new ObservableCollection<Order>();
			fillOrdersFromDb();
			
			PaidOrders = Orders.Filtering(o => o.Paid);
			UnpaidOrders = Orders.Filtering(o => !o.Paid);

			InitializeComponent();
		}

		private void fillOrdersFromDb()
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(1000); // accessing DB
				Random random = new Random();
				for (int i = 0; i < 5000; i++)
				{
					Order order = new Order(i);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					this.Dispatcher.Invoke(() => Orders.Add(order), DispatcherPriority.Background);
				}

				this.Dispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					DispatcherPriority.Background);
			});

			thread.Start();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num)
		{
			Num = num;
		}

		public int Num { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
```
In this example, we show the user the form without waiting for the data to load from the database to finish. While loading, the form is rendered and the user gets acquainted with its contents. Note that the source code loading code is abstracted from computations over them (*PaidOrders* and *UnpaidOrders*).

### Performing computations in a background thread
In the previous example, only data from the database was loaded in the background thread. The computations (*PaidOrders* and *UnpaidOrders*) were performed in the main thread (UI thread). Sometimes it is necessary to perform a computations in a background thread, and in the main thread to get only the final computation results:
```xml
<Window
	x:Class="ObservableComputationsExample.MainWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
	xmlns:local="clr-namespace:ObservableComputationsExample"
	xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	x:Name="uc_this"
	Title="ObservableComputationsExample"
	Width="800"
	Height="450"
	mc:Ignorable="d"
	Closed="mainWindow_OnClosed">
	<Grid>
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width="*" />
			<ColumnDefinition Width="*" />
		</Grid.ColumnDefinitions>
		<Grid.RowDefinitions>
			<RowDefinition Height="Auto" />
			<RowDefinition Height="*" />
		</Grid.RowDefinitions>

		<Label
			Grid.Row="0"
			Grid.Column="0"
			FontWeight="Bold">
			Unpaid orders
		</Label>
		<ListBox
			Grid.Row="1"
			Grid.Column="0"
			DisplayMemberPath="Num"
			ItemsSource="{Binding UnpaidOrders, ElementName=uc_this}" />

		<Label
			Grid.Row="0"
			Grid.Column="1"
			FontWeight="Bold">
			Paid orders
		</Label>
		<ListBox
			Grid.Row="1"
			Grid.Column="1"
			DisplayMemberPath="Num"
			ItemsSource="{Binding PaidOrders, ElementName=uc_this}" />
	</Grid>
</Window>
```

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ObservableComputations;
using Dispatcher = System.Windows.Threading.Dispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		// Dispatcher for computations in the backgroung thread
		ObservableComputations.Dispatcher _ocDispatcher = new ObservableComputations.Dispatcher();

		public MainWindow()
		{
			Orders = new ObservableCollection<Order>();

			WpfOcDispatcher wpfOcDispatcher = new WpfOcDispatcher(this.Dispatcher);

			fillOrdersFromDb();

			PaidOrders = 
				Orders.CollectionDispatching(_ocDispatcher) // direct the computation to the background thread
				.Filtering(o => o.Paid)
				.CollectionDispatching(wpfOcDispatcher, _ocDispatcher); // return the computation to the main thread from the background one

			UnpaidOrders = Orders.Filtering(o => !o.Paid);

			InitializeComponent();
		}

		private void fillOrdersFromDb()
		{
			Thread.Sleep(1000); // accessing DB
			Random random = new Random();
			for (int i = 0; i < 10000; i++)
			{
				Order order = new Order(i);
				order.Paid = Convert.ToBoolean(random.Next(0, 3));
				this.Dispatcher.Invoke(() => Orders.Add(order), DispatcherPriority.Background);
			}
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocDispatcher.Dispose();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num)
		{
			Num = num;
		}

		public int Num { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class WpfOcDispatcher : IDispatcher
	{
		private Dispatcher _dispatcher;

		public WpfOcDispatcher(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region Implementation of IDispatcher

		public void Invoke(Action action, object context)
		{
			_dispatcher.BeginInvoke(action, DispatcherPriority.Background);
		}

		#endregion
	}
}
```
In this example, we load data from the database in the main thread, but filtering the source collection *Orders* to receive paid orders (*PaidOrders*) is performed in the background thread.
*ObservableComputations.Dispatcher* class is very similar to the class [System.Windows.Threading.Dispatcher](https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher?view=netcore- 3.1). *ObservableComputations.Dispatcher* class is associated with a single thread. In this thread, you can execute delegates by calling *ObservableComputations.Dispatcher.Invoke* and *ObservableComputations.Dispatcher.BeginInvoke* methods.
The *CollectionDispatching* method redirects all changes of the source collection to the target dispatcher thread (*distinationDispatcher* parameter).
When the *CollectionDispatching* method is called, the source collection is enumerated (*Orders* or *Orders.CollectionDispatching(_ocDispatcher) .Filtering (o => o.Paid)*) and its event is subscribed to [CollectionChanged](https: // docs. microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netcore-3.1). However, the source collection should not be changed. When calling *.CollectionDispatching (_ocDispatcher)*, the collection *Orders* does not change. When calling *.CollectionDispatching (wpfOcDispatcher, _ocDispatcher)* collection *Orders.CollectionDispatching (_ocDispatcher) .Filtering (o => o.Paid)* may change in the *_ocDispatcher* thread, but since we pass *_ocDispatcher* to the *sourceDispatcher* parameter, then the enumeration of the collection of the source and subscription to its  [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netcore-3.1) event occurs in the thread of *_ocDispatcher*, which guarantees that there are no changes to the source collection during enumeration. Since when calling *.CollectionDispatching(_ocDispatcher)*, the *Orders* collection does not change, then passing *wpfOcDispatcher* to the *sourceDispatcher* parameter makes no sense, especially since at the time of calling *.CollectionDispatching (_ocDispatcher)* we are in the thread of *wpfOcDispatcher*. In most cases, unnecessarily passing the *sourceDispatcher* parameter will not result in a loss of workability, unless performance is slightly affected.
Note the need to call *_ocDispatcher.Dispose ()*.
The above example is not the only design option. Here is another option (XAML is the same as in the previous example):
 ```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ObservableComputations;
using Dispatcher = System.Windows.Threading.Dispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		WpfOcDispatcher _wpfOcDispatcher;
		
		// Dispatcher for computations in the backgroung thread
		ObservableComputations.Dispatcher _ocDispatcher = new ObservableComputations.Dispatcher();

		public MainWindow()
		{
			_wpfOcDispatcher = new WpfOcDispatcher(this.Dispatcher);
			
			Orders = new ObservableCollection<Order>();

			fillOrdersFromDb();

			PaidOrders = 
				Orders
				.Filtering(o => o.Paid)
				.CollectionDispatching(_wpfOcDispatcher, _ocDispatcher); // return the computation to the main thread from the background one

			UnpaidOrders = 
				Orders
				.Filtering(o => !o.Paid)
				.CollectionDispatching(_wpfOcDispatcher, _ocDispatcher); // return the computation to the main thread from the background one

			InitializeComponent();
		}

		private void fillOrdersFromDb()
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(1000); // accessing DB
				Random random = new Random();
				for (int i = 0; i < 5000; i++)
				{
					Order order = new Order(i);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					_ocDispatcher.Invoke(() => Orders.Add(order));
				}

				this.Dispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					DispatcherPriority.Background);
			});

			thread.Start();
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocDispatcher.Dispose();
		}		
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num)
		{
			Num = num;
		}

		public int Num { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class WpfOcDispatcher : IDispatcher
	{
		private Dispatcher _dispatcher;

		public WpfOcDispatcher(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region Implementation of IDispatcher

		public void Invoke(Action action, object context)
		{
			_dispatcher.BeginInvoke(action, DispatcherPriority.Background);
		}

		#endregion
	}
}
```

And one more:
```xml
<Window
	x:Class="ObservableComputationsExample.MainWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
	xmlns:local="clr-namespace:ObservableComputationsExample"
	xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	x:Name="uc_this"
	Title="ObservableComputationsExample"
	Width="800"
	Height="450"
	mc:Ignorable="d">
	<Grid>
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width="*" />
			<ColumnDefinition Width="*" />
		</Grid.ColumnDefinitions>
		<Grid.RowDefinitions>
			<RowDefinition Height="Auto" />
			<RowDefinition Height="Auto" />
			<RowDefinition Height="*" />
		</Grid.RowDefinitions>

		<Label
			x:Name="uc_LoadingIndicator"
			Grid.Row="0"
			Grid.Column="0"
			Grid.ColumnSpan="2"
			HorizontalAlignment="Center">
			Loading source data...
		</Label>

		<Label
			Grid.Row="1"
			Grid.Column="0"
			FontWeight="Bold">
			Unpaid orders
		</Label>
		<ListBox
			Grid.Row="2"
			Grid.Column="0"
			x:Name="uc_UnpaidOrderList"
			DisplayMemberPath="Num"
			ItemsSource="{Binding UnpaidOrders, ElementName=uc_this}"/>

		<Label
			Grid.Row="1"
			Grid.Column="1"
			FontWeight="Bold">
			Paid orders
		</Label>
		<ListBox
			Grid.Row="2"
			Grid.Column="1"
			DisplayMemberPath="Num"
			ItemsSource="{Binding PaidOrders, ElementName=uc_this}" />
	</Grid>
</Window>
```

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ObservableComputations;
using Dispatcher = System.Windows.Threading.Dispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		WpfOcDispatcher _wpfOcDispatcher;

		public MainWindow()
		{
			_wpfOcDispatcher = new WpfOcDispatcher(this.Dispatcher);
			
			Orders = new ObservableCollection<Order>();

			PaidOrders = 
				Orders
				.Filtering(o => o.Paid)
				.CollectionDispatching(_wpfOcDispatcher); // direct the computation to the main thread

			UnpaidOrders = 
				Orders
				.Filtering(o => !o.Paid)
				.CollectionDispatching(_wpfOcDispatcher); // direct the computation to the main thread

			InitializeComponent();

			fillOrdersFromDb();
		}

		private void fillOrdersFromDb()
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(1000); // accessing DB
				Random random = new Random();
				for (int i = 0; i < 5000; i++)
				{
					Order order = new Order(i);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					Orders.Add(order);
				}

				this.Dispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					DispatcherPriority.Background);
			});

			thread.Start();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num)
		{
			Num = num;
		}

		public int Num { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class WpfOcDispatcher : IDispatcher
	{
		private Dispatcher _dispatcher;

		public WpfOcDispatcher(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region Implementation of IDispatcher

		public void Invoke(Action action, object context)
		{
			_dispatcher.BeginInvoke(action, DispatcherPriority.Background);
		}

		#endregion
	}
}
```

### Property dispatching
In the previous examples, we saw how collections are dispatched using the *CollectionDispatching* method. But you may also need to dispatch properties:

```xml
<Window
	x:Class="ObservableComputationsExample.MainWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
	xmlns:local="clr-namespace:ObservableComputationsExample"
	xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	x:Name="uc_this"
	Title="ObservableComputationsExample"
	Width="800"
	Height="450"
	mc:Ignorable="d"
	Closed="mainWindow_OnClosed">
	<Grid>
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width="*" />
			<ColumnDefinition Width="*" />
		</Grid.ColumnDefinitions>
		<Grid.RowDefinitions>
			<RowDefinition Height="Auto" />
			<RowDefinition Height="Auto" />
			<RowDefinition Height="*" />
		</Grid.RowDefinitions>

		<Label
			x:Name="uc_LoadingIndicator"
			Grid.Row="0"
			Grid.Column="0"
			Grid.ColumnSpan="2"
			HorizontalAlignment="Center">
			Loading source data...
		</Label>

		<Label
			Grid.Row="1"
			Grid.Column="0"
			FontWeight="Bold">
			Unpaid orders
		</Label>
		<ListBox
			Grid.Row="2"
			Grid.Column="0"
			x:Name="uc_UnpaidOrderList"
			DisplayMemberPath="Num"
			ItemsSource="{Binding UnpaidOrders, ElementName=uc_this}"
			MouseDoubleClick="unpaidOrdersList_OnMouseDoubleClick" />

		<Label
			Grid.Row="1"
			Grid.Column="1"
			FontWeight="Bold">
			Paid orders
		</Label>
		<ListBox
			Grid.Row="2"
			Grid.Column="1"
			DisplayMemberPath="Num"
			ItemsSource="{Binding PaidOrders, ElementName=uc_this}" />
	</Grid>
</Window>
```

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ObservableComputations;
using Dispatcher = System.Windows.Threading.Dispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		// Dispatcher for computations in the backgroung thread
		ObservableComputations.Dispatcher _ocDispatcher = new ObservableComputations.Dispatcher();
		
		WpfOcDispatcher _wpfOcDispatcher;

		public MainWindow()
		{
			_wpfOcDispatcher = new WpfOcDispatcher(this.Dispatcher);
			
			Orders = new ObservableCollection<Order>();
			
			fillOrdersFromDb();			

			PaidOrders = 
				Orders.CollectionDispatching(_ocDispatcher) // direct the computation to the background thread
				.Filtering(o => o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcDispatcher); // return the computation to the main thread

			UnpaidOrders = Orders.Filtering(o => !o.Paid);

			InitializeComponent();
		}

		private void fillOrdersFromDb()
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(1000); // accessing DB
				Random random = new Random();
				for (int i = 0; i < 5000; i++)
				{
					Order order = new Order(i, _ocDispatcher, _wpfOcDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					this.Dispatcher.Invoke(() => Orders.Add(order), DispatcherPriority.Background);
				}

				this.Dispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					DispatcherPriority.Background);
			});

			thread.Start();
		}

		private void unpaidOrdersList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			((Order) uc_UnpaidOrderList.SelectedItem).Paid = true;
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocDispatcher.Dispose();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IDispatcher backgroundDispatcher, IDispatcher wpfDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundDispatcher, wpfDispatcher);

		}

		public int Num { get; }

		public PropertyDispatching<Order, bool> PaidPropertyDispatching { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class WpfOcDispatcher : IDispatcher
	{
		private Dispatcher _dispatcher;

		public WpfOcDispatcher(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region Implementation of IDispatcher

		public void Invoke(Action action, object context)
		{
			_dispatcher.BeginInvoke(action, DispatcherPriority.Background);
		}

		#endregion
	}
}
```
In this example, when we double-click on an unpaid order, we make it paid. Since the *Paid* property in this case changes in the main thread, we cannot read it in the background thread of *_ocDispatcher*. In order to read this property in the background thread of *_ocDispatcher*, it is necessary to dispatch changes of that property into that thread. This is done using the  *PropertyDispatching&lt;THolder, TResult&gt;* class. Similar to the *CollectionDispatching* method, the constructor of the *PropertyDispatching&lt;THolder, TResult&gt;* class has the required parameter *destinationDispatcher* and the optional parameter *sourceDispatcher*. The difference is that instead of enumerating the source collection and subscribing to the [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netcore- 3.1) event, the property value is read and the [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netcore-3.1) event is subscribed. Another difference is that the value passed to the *sourceDispatcher* parameter is used to dispatch the property value change (setter of *PropertyDispatching&lt;THolder, TResult&gt;.Value*) to the *sourceDispatcher* thread, if this change is made in another thread.
The above example is not the only design option. Here is another option (XAML has not changed):  
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ObservableComputations;
using Dispatcher = System.Windows.Threading.Dispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		// Dispatcher for computations in the backgroung thread
		ObservableComputations.Dispatcher _ocDispatcher = new ObservableComputations.Dispatcher();
		
		WpfOcDispatcher _wpfOcDispatcher;

		public MainWindow()
		{
			_wpfOcDispatcher = new WpfOcDispatcher(this.Dispatcher);
			
			Orders = new ObservableCollection<Order>();
			
			fillOrdersFromDb();			

			PaidOrders = 
				Orders
				.Filtering(o => o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcDispatcher, _ocDispatcher); // direct the computation to the main thread from the background one

			UnpaidOrders = 
				Orders
				.Filtering(o => !o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcDispatcher, _ocDispatcher); // direct the computation to the main thread from the background one

			InitializeComponent();
		}

		private void fillOrdersFromDb()
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(1000); // accessing DB
				Random random = new Random();
				for (int i = 0; i < 5000; i++)
				{
					Order order = new Order(i, _ocDispatcher, _wpfOcDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					_ocDispatcher.Invoke(() => Orders.Add(order));
				}

				this.Dispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					DispatcherPriority.Background);
			});

			thread.Start();
		}

		private void unpaidOrdersList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			((Order) uc_UnpaidOrderList.SelectedItem).Paid = true;
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocDispatcher.Dispose();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IDispatcher backgroundDispatcher, IDispatcher wpfDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundDispatcher, wpfDispatcher);

		}

		public int Num { get; }

		public PropertyDispatching<Order, bool> PaidPropertyDispatching { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class WpfOcDispatcher : IDispatcher
	{
		private Dispatcher _dispatcher;

		public WpfOcDispatcher(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region Implementation of IDispatcher

		public void Invoke(Action action, object context)
		{
			_dispatcher.Invoke(action, DispatcherPriority.Background);
		}

		#endregion
	}
}
```

And one more (XAML has not changed):
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ObservableComputations;
using Dispatcher = System.Windows.Threading.Dispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		// Dispatcher for computations in the backgroung thread
		ObservableComputations.Dispatcher _ocDispatcher = new ObservableComputations.Dispatcher();
		
		WpfOcDispatcher _wpfOcDispatcher;

		public MainWindow()
		{
			_wpfOcDispatcher = new WpfOcDispatcher(this.Dispatcher);
			
			Orders = new ObservableCollection<Order>();

			PaidOrders = 
				Orders
				.Filtering(o => o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcDispatcher); // direct the computation to the main thread

			UnpaidOrders = 
				Orders
				.Filtering(o => !o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcDispatcher); // direct the computation to the main thread

			InitializeComponent();

			fillOrdersFromDb();
		}

		private void fillOrdersFromDb()
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(1000); // accessing DB
				Random random = new Random();
				for (int i = 0; i < 5000; i++)
				{
					Order order = new Order(i, _ocDispatcher, _wpfOcDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					_ocDispatcher.Invoke(() => Orders.Add(order));
				}

				this.Dispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					DispatcherPriority.Background);
			});

			thread.Start();
		}

		private void unpaidOrdersList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			((Order) uc_UnpaidOrderList.SelectedItem).Paid = true;
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocDispatcher.Dispose();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IDispatcher backgroundDispatcher, IDispatcher wpfDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundDispatcher, wpfDispatcher);

		}

		public int Num { get; }

		public PropertyDispatching<Order, bool> PaidPropertyDispatching { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class WpfOcDispatcher : IDispatcher
	{
		private Dispatcher _dispatcher;

		public WpfOcDispatcher(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region Implementation of IDispatcher

		public void Invoke(Action action, object context)
		{
			_dispatcher.Invoke(action, DispatcherPriority.Background);
		}

		#endregion
	}
}
```

### Dispatching *IReadScalar&lt;TValue&gt;*
*IReadScalar&lt;TValue&gt;* was first mentioned [here](#full-list-of-operators). In addition to the *CollectionDispatching* method, ObservableComputations contains the *ScalarDispatching* method. Its use is completely analogous to the use of *CollectionDispatching*. Using *ScalarDispatching* you can implement [property dispatching](#property-dispatching), but using the *PropertyDispatching&lt;THolder, TResult&gt;* class it is simpler and faster.

### Parallel computations in background threads
In the previous examples, we saw how the computation is performed in one background thread. Using the dispatch methods described above, it is possible to organize computations in several background threads, the results of which are concurrently combined in another thread (main or background).

### Using the *Dispatcher* class
The class *Dispatcher* has methods that you can call if necessary
* *Invoke* and *BeginInvoke* - for synchronous and asynchronous execution of a delegate in the thread of an instance of *Dispatcher* class, for example, for changing the source data for computations performed in the thread of an instance of *Dispatcher* class. After calling *Dispose* method, these methods return control without executing the delegate passed and without throwing an exception.
* *DoOthers* - if the delegate passed to the *Invoke* or *BeginInvoke* methods takes a long time, when *DoOthers* is called, other delegates are called. It is possible to set the maximum number of delegates that should be executed or the approximate maximum time for their execution.

### Variants of the implementation of the IDispatcher interface and other similar interfaces
So far, we have used a very simple implementation of the *IDispatcher* interface. For example, this:
```csharp
public class WpfOcDispatcher : IDispatcher
{
   private Dispatcher _dispatcher;

   public WpfOcDispatcher(System.Windows.Threading.Dispatcher dispatcher)
   {
      _dispatcher = dispatcher;
   }

   #region Implementation of IDispatcher

   public void Invoke(Action action, object context)
   {
      _dispatcher.Invoke(action, DispatcherPriority.Background);
   }

   #endregion
}
```
In this implementation, the [System.Windows.Threading.Dispatcher.Invoke](https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher.invoke?view=netcore-3.1) method is called. In other implementations, we called [System.Windows.Threading.Dispatcher.BeginInvoke]([System.Windows.Threading.Dispatcher.Invoke](https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher.invoke?view=netcore-3.1)). The implementation options are not limited to this, for example, you can use an implementation that will buffer a collection changes using [Reactive Extensions](https://github.com/dotnet/reactive):
```csharp
public class WpfOcDispatcher : IDispatcher, IDisposable
{
	Subject<Action> _actions;

	private System.Windows.Dispatcher _dispatcher;

	public WpfOcDispatcher(System.Windows.Dispatcher dispatcher)
	{
		_dispatcher = dispatcher;

		_actions = new Subject<Action>();
		_actions.Buffer(TimeSpan.FromMilliseconds(300)).Subscribe(actions =>
		{
			_dispatcher.Invoke(() =>
			{
				for (var index = 0; index < actions.Count; index++)
				{
					actions[index]();
				}
			}, DispatcherPriority.Background);
		});
	}

	#region Implementation of IDispatcher

	public void Invoke(Action action, object context)
	{
		_actions.OnNext(action);
	}

	#endregion

	#region Implementation of IDisposable

	public void Dispose()
	{
		_actions.Dispose();
	}

	#endregion
}
```

When dispatching properties (*PropertyDispatching*) and *IReadScalar&lt;TValue&gt;* (*ScalarDispatching*), ThrottlingDispatcher can be useful:
```
public class ThrottlingDispatcher : IDispatcher, IDisposable
{
	Subject<Action> _actions;

	private System.Windows.Dispatcher _dispatcher;

	public WpfOcDispatcher(System.Windows.Dispatcher dispatcher)
	{
		_dispatcher = dispatcher;

		_actions = new Subject<Action>();
		_actions.Throttle(TimeSpan.FromMilliseconds(300)).Subscribe(action =>
		{
			_dispatcher.Invoke(action, DispatcherPriority.Background);
		});
	}

	#region Implementation of IDispatcher

	public void Invoke(Action action, object context)
	{
		_actions.OnNext(action);
	}

	#endregion

	#region Implementation of IDisposable

	public void Dispose()
	{
		_actions.Dispose();
	}

	#endregion
}
```

It may be necessary to implement depending on what changes are made by the *action* delegate passed to the *Invoke* method. To do this, you can analyze the *context* parameter passed to the *IDispatcher.Invoke* method or use the following interfaces instead of *IDispatcher* interface:

```csharp
	public interface ICollectionDestinationDispatcher
	{
		void Invoke(
			Action action, 
			ICollectionComputing collectionDispatching,
			NotifyCollectionChangedAction notifyCollectionChangedAction,
			object newItem,
			object oldItem,
			int newIndex,
			int oldIndex);
	}

	public interface IPropertySourceDispatcher
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="propertyDispatching"></param>
		/// <param name="initializing">false if setter of Value property is called</param>
		/// <param name="newValue">new value if setter of Value property is called </param>
		void Invoke(
			Action action, 
			IComputing propertyDispatching,
			bool initializing,
			object newValue);
	}
```


### Launch in a console application
The previous examples were WPF application examples. Similar examples can be run in a console application. This may be needed for unit tests.

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	class Program
	{
		static ObservableComputations.Dispatcher _backgroundDispatcher = new ObservableComputations.Dispatcher();
		
		static ObservableComputations.Dispatcher _mainDispatcher = new ObservableComputations.Dispatcher();
		static ObservableCollection<Order> Orders;

		static void Main(string[] args)
		{
			_mainDispatcher.Invoke(() =>
			{
				ObservableCollection<Order> paidOrders;
				ObservableCollection<Order> unpaidOrders;

				Orders = new ObservableCollection<Order>();

				paidOrders =
					Orders.CollectionDispatching(_backgroundDispatcher)  // direct the computation to the background thread
					.Filtering(o => o.PaidPropertyDispatching.Value)
					.CollectionDispatching(_mainDispatcher,
						_backgroundDispatcher); // return the computation to the main thread from the background one

				unpaidOrders = Orders.Filtering(o => !o.Paid);

				paidOrders.CollectionChanged += (sender, eventArgs) =>
				{
					Console.WriteLine($"Paid order: {((Order) eventArgs.NewItems[0]).Num}" );
				};

				unpaidOrders.CollectionChanged += (sender, eventArgs) =>
				{
					Console.WriteLine($"Unpaid order: {((Order) eventArgs.NewItems[0]).Num}");
				};

				fillOrdersFromDb();
			});

			Console.ReadLine();
		}

		private static void fillOrdersFromDb()
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(1000); // accessing DB
				Random random = new Random();
				for (int i = 0; i < 5000; i++)
				{
					Order order = new Order(i, _backgroundDispatcher, _mainDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					_mainDispatcher.Invoke(() => Orders.Add(order));
				}
			});

			thread.Start();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IDispatcher backgroundDispatcher, IDispatcher mainDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundDispatcher, mainDispatcher);

		}

		public int Num { get; }

		public PropertyDispatching<Order, bool> PaidPropertyDispatching { get; }

		private bool _paid;
		public bool Paid
		{
			get => _paid;
			set
			{
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
```

### Debugging user code

Is described [here](#user-code-in-background-threads).

## Tracking changes in a method return value
Before now we saw how ObservableComputations tracks changes in property values and collections via [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) and [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) events. ObservableComputations introduces new interface and event for tracking changes in a method return value: *INotifyMethodChanged* interface and *MethodChanged* event. Here is example:

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class RoomReservation
	{
		public string RoomId { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}

	public class RoomReservationManager : INotifyMethodChanged
	{
		private List<RoomReservation> _roomReservations = new List<RoomReservation>();

		public void AddReservation(RoomReservation roomReservation)
		{
			_roomReservations.Add(roomReservation);
			MethodChanged?.Invoke(this, new NotifyMethodChangedEventArgs(
				nameof(IsRoomReserved),
				args =>
				{
					string roomId = (string) args[0];
					DateTime dateTime = (DateTime) args[1];
					return
						roomId == roomReservation.RoomId
						&& roomReservation.From < dateTime && dateTime < roomReservation.To;
				}));
		}

		public bool IsRoomReserved(string roomId, DateTime dateTime)
		{
			return _roomReservations.Any(rr => 
				rr.RoomId == roomId 
				&& rr.From < dateTime && dateTime < rr.To);
		}

		public event EventHandler<NotifyMethodChangedEventArgs> MethodChanged;
	}

	public class Meeting : INotifyPropertyChanged
	{
		private string _roomNeeded;
		public string RoomNeeded
		{
			get => _roomNeeded;
			set
			{
				_roomNeeded = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomNeeded)));
			}
		}

		private DateTime _dateTimeNeeded;
		public DateTime DateTimeNeeded
		{
			get => _dateTimeNeeded;
			set
			{
				_dateTimeNeeded = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateTimeNeeded)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			RoomReservationManager roomReservationManager = new RoomReservationManager();
			Meeting planingMeeting = new Meeting()
			{
				RoomNeeded = "ConferenceHall", 
				DateTimeNeeded = new DateTime(2020, 02, 07, 15, 45, 00)
			};

			Computing<bool> isRoomReservedComputing = new Computing<bool>(() =>
				roomReservationManager.IsRoomReserved(
					planingMeeting.RoomNeeded, 
					planingMeeting.DateTimeNeeded));

			isRoomReservedComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Computing<bool>.Value))
				{
					// see changes here
				}
			};

			roomReservationManager.AddReservation(new RoomReservation()
			{
				RoomId = "ConferenceHall",
				From =  new DateTime(2020, 02, 07, 15, 00, 00),
				To =  new DateTime(2020, 02, 07, 16, 00, 00)
			});

			planingMeeting.DateTimeNeeded = new DateTime(2020, 02, 07, 16, 30, 00);
				
			Console.ReadLine();
		}
	}
}
```
As you see *NotifyMethodChangedEventArgs* contains *ArgumentsPredicate* property. Following value is passed to that property:
```csharp
args =>
{
   string roomId = (string) args[0];
   DateTime dateTime = (DateTime) args[1];
   return
      roomId == roomReservation.RoomId
      && roomReservation.From < dateTime && dateTime < roomReservation.To;
}
```
That property defines what values should have arguments in a method call so that return value of that call changes.

ATTENTION: Code example given in this section is not a disign standard, it is rather an antipattern: it contains code duplication and changes of properties of *RoomReservation* class is not tracked.  That code is given only for demonstration of tracking changes in a method return value. See fixed code [here](#use-public-readonly-structures-instead-of-encapsulated-private-members).

### Computations implementing *INotifyMethodChanged*
*INotifyMethodChanged* is implemented by the following computations:
* *Dictionaring* (methods: ContainsKey, Indexer ([]), GetValueOrDefault).
* *ConcurrentDictionaring* (methods: ContainsKey, Indexer ([]), GetValueOrDefault).
* *HashSetting* (method: Contains).

## Performance tips
### Avoid nested parameter dependent computations on big data
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public int Num {get; set;}

		private string _type;
		public string Type
		{
			get => _type;
			set
			{
				_type = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			ObservableCollection<Order> orders = 
				new ObservableCollection<Order>(new []
				{
					new Order{Num = 1, Type = "VIP"},
					new Order{Num = 2, Type = "Regular"},
					new Order{Num = 3, Type = "VIP"},
					new Order{Num = 4, Type = "VIP"},
					new Order{Num = 5, Type = "NotSpecified"},
					new Order{Num = 6, Type = "Regular"},
					new Order{Num = 7, Type = "Regular"}
				});

			ObservableCollection<string> selectedOrderTypes = new ObservableCollection<string>(new []
				{
					"VIP", "NotSpecified"
				});

			ObservableCollection<Order> filteredByTypeOrders =  orders.Filtering(o => 
				selectedOrderTypes.ContainsComputing(() => o.Type).Value);
			

			filteredByTypeOrders.CollectionChanged += (sender, eventArgs) =>
			{
				// see the changes (add, remove, replace, move, reset) here			
			};

			// Start the changing...
			orders.Add(new Order{Num = 8, Type = "VIP"});
			orders.Add(new Order{Num = 9, Type = "NotSpecified"});
			orders[4].Type = "Regular";
			orders.Move(4, 1);
			orders[0] = new Order{Num = 10, Type = "Regular"};
			selectedOrderTypes.Remove("NotSpecified");

			Console.ReadLine();
		}
	}
}
```
In the code above *selectedOrderTypes.ContainsComputing(() => o.Type)* is nested computation which is defendant on outer parameter *o*. These two circumstances lead to the fact that instance of *ContainsComputing* class will be created for each order in the *orders* collection. This may impact performance and memory consumption if you have many of orders. Fortunately, *filteredByTypeOrders* calculation can be made "flat":

```csharp
ObservableCollection<Order> filteredByTypeOrders =  orders
    .Joining(selectedOrderTypes, (o, ot) => o.Type == ot)
    .Selecting(oot => oot.OuterItem);
```

This computation has performance and memory consumption advantage. 

### Cache property (method) values
Suppose we have long-computed property and we want increase performance of getting it's value:

```csharp
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class ValueHolder : INotifyPropertyChanged
	{
		private string _value;

		public string Value
		{
			get
			{
				Thread.Sleep(100);
				return _value;
			}
			set
			{
				_value = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}

		private Computing<string> _valueComputing;
		public Computing<string> ValueComputing => _valueComputing = 
			_valueComputing ?? new Computing<string>(() => Value);

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			ValueHolder valueHolder = new ValueHolder();

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = 0; i < 20; i++)
			{
				string value = valueHolder.Value;
			}
			stopwatch.Stop();
			Console.WriteLine($"Direct access to property: {stopwatch.ElapsedMilliseconds}");

			stopwatch.Restart();
			for (int i = 0; i < 20; i++)
			{
				string value = valueHolder.ValueComputing.Value;
			}
			stopwatch.Stop();
			Console.WriteLine($"Access to property via computing: {stopwatch.ElapsedMilliseconds}");
				
			Console.ReadLine();
		}
	}
}
```

Code above has following output:

> Direct access to property: 2155<br>
Access to property via computing: 626

### Differing&lt;TResult&gt; extension method
That extension method allows you to suppress extra raisings of [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) event (when value of a property in not changed). 

```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Angle : INotifyPropertyChanged
	{
		private double _rads;
		public double Rads
		{
			get
			{
				return _rads;
			}
			set
			{
				_rads = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rads)));
			}
		}

		public static double DegreesToRads(double degrees) => degrees * (Math.PI / 180);

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Angle angle = new Angle(){Rads = Angle.DegreesToRads(0)};
			Computing<double> sinComputing = new Computing<double>(
				() => Math.Round(Math.Sin(angle.Rads), 3)); // 0
			Console.WriteLine($"sinComputing: {sinComputing.Value}");

			sinComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Computing<double>.Value))
				{
					Console.WriteLine($"sinComputing: {sinComputing.Value}");
				}
			};

			Differing<double> differingSinComputing = sinComputing.Differing();
			Console.WriteLine($"differingSinComputing: {sinComputing.Value}");
			differingSinComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Computing<double>.Value))
				{
					Console.WriteLine($"differingSinComputing: {differingSinComputing.Value}");
				}
			};


			angle.Rads = Angle.DegreesToRads(30); // 0,5
			angle.Rads = Angle.DegreesToRads(180) - angle.Rads; // 0,5	
			angle.Rads = Angle.DegreesToRads(360 + 180) - angle.Rads; // 0,5
			angle.Rads = Angle.DegreesToRads(360) - angle.Rads; // -0,5
			
				
			Console.ReadLine();
		}
	}
}
```

Code above has following output:
> sinComputing: 0 <br>
differingSinComputing: 0 <br>
sinComputing: 0,5 <br>
differingSinComputing: 0,5 <br>
sinComputing: 0,5 <br>
sinComputing: 0,5 <br>
sinComputing: -0,5 <br>
differingSinComputing: -0,5 <br>

Sometimes handling of every [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) events is long-time and may freeze UI (rerendering, recomputing). Use *Differing* extension method to decrease that effect.

#### Use *capacity* argument
If, after instantiating the collection computation class class (e.g. *Filtering*), it is expected that the collection will grow significantly, it makes sense to pass the *capacity* argument to the constructor to reserve memory for the collection.

### Use computations in background threads
See details [here](#multithreading).

## Design tips

### Lazy initialized computation
If some computation is needed only for particular scenarios or you want delay initialization until the computation becomes needed, lazy initialized computation is advisable. Here is an example:  
  
```csharp
private Computing<string> _valueComputing;
public Computing<string> ValueComputing => _valueComputing = 
   _valueComputing ?? new Computing<string>(() => Value);
```

### Use public readonly structures instead of encapsulated private members
Code example given in ["Tracking changes in a method return value" section ](#tracking-changes-in-a-method-return-value) is not a design standard, it is rather an antipattern: it contains code duplication and changes of properties of RoomReservation class is not tracked. That code is given only for demonstration of tracking changes in a method return value. Here is the fixed design:

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class RoomReservation : INotifyPropertyChanged
	{
		private string _roomId;
		public string RoomId
		{
			get => _roomId;
			set
			{
				_roomId = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomId)));
			}
		}


		private DateTime _from;
		public DateTime From
		{
			get => _from;
			set
			{
				_from = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(From)));
			}
		}

		private DateTime _to;
		public DateTime To
		{
			get => _to;
			set
			{
				_to = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(To)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class RoomReservationManager 
	{
		private ObservableCollection<RoomReservation> _roomReservations = new ObservableCollection<RoomReservation>();
		private ReadOnlyObservableCollection<RoomReservation> _roomReservationsReadOnly;

		public RoomReservationManager()
		{
			_roomReservationsReadOnly = new ReadOnlyObservableCollection<RoomReservation>(_roomReservations);
		}

		public void AddReservation(RoomReservation roomReservation)
		{
			_roomReservations.Add(roomReservation);;
		}

		public ReadOnlyObservableCollection<RoomReservation> RoomReservations =>
			_roomReservationsReadOnly;
	}

	public class Meeting : INotifyPropertyChanged
	{
		private string _roomNeeded;
		public string RoomNeeded
		{
			get => _roomNeeded;
			set
			{
				_roomNeeded = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomNeeded)));
			}
		}

		private DateTime _dateTimeNeeded;
		public DateTime DateTimeNeeded
		{
			get => _dateTimeNeeded;
			set
			{
				_dateTimeNeeded = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateTimeNeeded)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			RoomReservationManager roomReservationManager = new RoomReservationManager();
			Meeting planingMeeting = new Meeting()
			{
				RoomNeeded = "ConferenceHall", 
				DateTimeNeeded = new DateTime(2020, 02, 07, 15, 45, 00)
			};

			AnyComputing<RoomReservation> isRoomReservedComputing = 
				roomReservationManager.RoomReservations.AnyComputing<RoomReservation>(rr => 
					rr.RoomId == planingMeeting.RoomNeeded
					&& rr.From < planingMeeting.DateTimeNeeded 
					&& planingMeeting.DateTimeNeeded < rr.To);

			isRoomReservedComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Computing<bool>.Value))
				{
					// see changes here
				}
			};

			roomReservationManager.AddReservation(new RoomReservation()
			{
				RoomId = "ConferenceHall",
				From =  new DateTime(2020, 02, 07, 15, 00, 00),
				To =  new DateTime(2020, 02, 07, 16, 00, 00)
			});

			planingMeeting.DateTimeNeeded = new DateTime(2020, 02, 07, 16, 30, 00);
				
			Console.ReadLine();
		}
	}
}
```

Note that type of *RoomReservationManager._roomReservations* is changed to *ObservableCollection&lt;RoomReservation&gt;* and *RoomReservationManager.RoomReservations* member of type *System.Collections.ObjectModel.ReadOnlyObservableCollection&lt;RoomReservation&gt;* has been added.

### Short your code
See [here](#passing-argument-as-observable) and [here](#passing-source-collection-argument-as-observable).

### Do not create extra variables
See [here](#variable-declaration-in-a-computations-chain)


## Applications of Using&lt;TResult&gt; extension method
### Clear expressions
See the end lines of [Arbitrary expression observing](#arbitrary-expression-observing).

### Variable declaration in a computations chain
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class OrderLine : INotifyPropertyChanged
	{
		private decimal _price;
		public decimal Price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class Order : INotifyPropertyChanged
	{
		public ObservableCollection<OrderLine> Lines = new ObservableCollection<OrderLine>();

		private decimal _discount;
		public decimal Discount
		{
			get
			{
				return _discount;
			}
			set
			{
				_discount = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Discount)));
			}
		}

		private Computing<decimal> _priceWithDiscount;
		public Computing<decimal> PriceWithDiscount
		{
			get
			{
				if (_priceWithDiscount == null)
				{
					// first step
					Summarizing<decimal> totalPrice 
						= Lines.Selecting(l => l.Price).Summarizing(); 
					    
					// second step
					_priceWithDiscount = new Computing<decimal>(
						() => totalPrice.Value - totalPrice.Value * Discount);
				}

				return _priceWithDiscount;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Order order = new Order(){Discount = 0.25m};
			order.Lines.Add(new OrderLine(){Price = 100});
			order.Lines.Add(new OrderLine(){Price = 150});
			order.Lines.Add(new OrderLine(){Price = 50});

			Console.WriteLine(order.PriceWithDiscount.Value);

			order.Lines[1].Price = 130;

			Console.WriteLine(order.PriceWithDiscount.Value);
				
			Console.ReadLine();
		}
	}
}
```
Pay attention on *PriceWithDiscount* property. In the body of that property we construct *_priceWithDiscount* computation in two steps. Can we refactor *PriceWithDiscount* property to [expression body](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members)?  Yes:

```csharp
public Computing<decimal> PriceWithDiscount => _priceWithDiscount = _priceWithDiscount ?? 
   Lines.Selecting(l => l.Price).Summarizing().Using(p => p.Value - p.Value * Discount);
```

In the code above *p* parameter is the result of *Lines.Selecting(l => l.Price).Summarizing()*. So *p* parameter is kind of variable. 
Following code is incorrect as changes in *OrderLine.Price* property and *Order.Lines* collection is not reflected in the result computation:

```csharp
public Computing<decimal> PriceWithDiscount => _priceWithDiscount = _priceWithDiscount ?? 
   Lines.Selecting(l => l.Price).Summarizing().Value.Using(p => p - p * Discount);
```
In this code *p* parameter has type decimal, not *Summarizing&lt;decimal&gt*; as in correct variant. See [here](#passing-argument-as-observable) for details.

## Tracking previous value of IReadScalar&lt;TValue&gt;
*IReadScalar&lt;TValue&gt;* is mentioned [here](#full-list-of-operators) for the first time.
There is not built-in facilities to get previous value of a property while handling [PropertyChanged event](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8). ObservableComputation helps you and provides *PreviousTracking&lt;TResult&gt;* and *WeakPreviousTracking&lt;TResult&gt;* extension methods.

```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		private string _deliveryDispatchCenter;
		public string DeliveryDispatchCenter
		{
			get
			{
				return _deliveryDispatchCenter;
			}
			set
			{
				_deliveryDispatchCenter = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeliveryDispatchCenter)));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Order order = new Order()
			{
				DeliveryDispatchCenter = "A"
			};

			PreviousTracking<string> previousTracking = new Computing<string>(() => order.DeliveryDispatchCenter).PreviousTracking();

			previousTracking.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(Computing<double>.Value))
				{
					Console.WriteLine($"Current dispatch center: {previousTracking.Value}; Previous dispatch center: {previousTracking.PreviousValue};");
				}
			};

			order.DeliveryDispatchCenter = "B";
			order.DeliveryDispatchCenter = "C";
			
				
			Console.ReadLine();
		}
	}
}
```

Code above has following output:
> Current dispatch center: B; Previous dispatch center: A;  <br>
Current dispatch center: C; Previous dispatch center: B;  <br>

Note that changes of *PreviousValue* property is trackable by  [PropertyChanged event](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) so you can include that property in your observable computations.

Note that instance of *PreviousTracking&lt;TResult&gt;* has strong reference to previous *TResult* value (*PreviousValue* property) (in case *TResult* is reference type). Account it when you think will about garbage collecting and memory leaks. *WeakPreviousTracking&lt;TResult&gt;* can help you. Instead of *PreviousValue* property *WeakPreviousTracking&lt;TResult&gt;* includes *TryGetPreviousValue* method. Changes of return value of that method isn't trackable, so you cannot include it in your observable computations.

## Accessing a property via reflection
Following code will not work correctly:
```csharp
using System;
using System.ComponentModel;
using System.Reflection;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		private decimal _price;
		public decimal Price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Order order = new Order()
			{
				Price = 1
			};

			PropertyInfo pricePropertyInfo = typeof(Order).GetProperty(nameof(Order.Price));

			Computing<decimal> priceReflectedComputing 
				= new Computing<decimal>(() => (decimal)pricePropertyInfo.GetValue(order));

			priceReflectedComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(PropertyAccessing<decimal>.Value))
				{
					Console.WriteLine(priceReflectedComputing.Value);
				}
			};  

			order.Price = 2;
			order.Price = 3;
		
			Console.ReadLine();
		}
	}
}
```
Code above has no output, as changes of return value of *GetValue* method cannot be tracked. Here is the fixed code:



```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		private decimal _price;
		public decimal Price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Order order = new Order()
			{
				Price = 1
			};

			PropertyAccessing<decimal> priceReflectedComputing 
				= order.PropertyAccessing<decimal>(nameof(Order.Price));

			priceReflectedComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(PropertyAccessing<decimal>.Value))
				{
					Console.WriteLine(priceReflectedComputing.Value);
				}
			};  

			order.Price = 2;
			order.Price = 3;
			
			Console.ReadLine();
		}
	}
}
```

In the code above we use *PropertyAccessing* extension method. Be sure you are aware of [Passing arguments as non-observables and observables](#passing-arguments-as-non-observables-and-observables): in the code above first argument (*order*) of *PropertyAccessing* extension method is passed as **non-observable**. In the following code that argument is passed as **observable**.
```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
{
	public class Order : INotifyPropertyChanged
	{
		private decimal _price;
		public decimal Price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class Manager : INotifyPropertyChanged
	{
		private Order _processingOrder;
		public Order ProcessingOrder
		{
			get
			{
				return _processingOrder;
			}
			set
			{
				_processingOrder = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcessingOrder)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Order order = new Order()
			{
				Price = 1
			};

			Manager manager = new Manager(){ProcessingOrder = order};

			PropertyAccessing<decimal> priceReflectedComputing 
				= new Computing<Order>(() => manager.ProcessingOrder)
					.PropertyAccessing<decimal>(nameof(Order.Price));

			priceReflectedComputing.PropertyChanged += (sender, eventArgs) =>
			{
				if (eventArgs.PropertyName == nameof(PropertyAccessing<decimal>.Value))
				{
					Console.WriteLine(priceReflectedComputing.Value);
				}
			};  

			order.Price = 2;
			order.Price = 3;
			manager.ProcessingOrder = 
				new Order()			
				{
					Price = 4
				};
			
			Console.ReadLine();
		}
	}
}
```
Following code will not work correctly as changes in *manager.ProcessingOrder* is not reflected in *priceReflectedComputing* as first argument of *PropertyAccessing* extension method is passed as **non-observable**:
```csharp
PropertyAccessing<decimal> priceReflectedComputing 
   = manager.ProcessingOrder.PropertyAccessing<decimal>(nameof(Order.Price));
```

If object reference for which a property value is being accessed is null *PropertyAccessing&lt;TResult&gt;.Value* returns default value of *TResult*. You can modify that value by passing the *defaultValue* parameter.

## Binding 
  
Binding class and extention method allows you to bind two arbitrary expressions. First expression is a source. Second expression is a target. The complexity of the expressions is not limited. The first expression is passed as an [expression tree](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/). The second expression is squashed as a [delegate](https://docs.microsoft.com/en-us/dotnet/api/system.delegate?view=netframework-4.8). If source expression value is changed, the new value is assigned to target expression:  
  
```csharp
using System;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputationsExamples
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

	class Program
	{
		static void Main(string[] args)
		{
			Order order = new Order(){DeliveryAddress = ""};
			Car assignedDeliveryCar = new Car(){DestinationAddress = ""};

			Binding<string> deliveryAddressBinding = new Binding<string>(
				() => order.DeliveryAddress,
				da => assignedDeliveryCar.DestinationAddress = da);

			Console.WriteLine(assignedDeliveryCar.DestinationAddress);

			order.DeliveryAddress = "A";
			Console.WriteLine(assignedDeliveryCar.DestinationAddress);

			order.DeliveryAddress = "B";
			Console.WriteLine(assignedDeliveryCar.DestinationAddress);

			Console.ReadLine();
		}
	}
}
```

In the code above we bind *order.DeliveryAddress* and *assignedDeliveryCar.DestinationAddress*. *order.DeliveryAddress* is a binding source. *assignedDeliveryCar.DestinationAddress* is a binding target.

*Binding* extention method extends *IReadScalar&lt;TValue&gt;*, instance oа which is a binding source. 

To avoid unloading the instance of *Binding* class from the memory by garbage collector, save reference to the one in the object that has appropriate lifetime.

## Can I use [IList&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=netframework-4.8) with ObservableComputations?
If you have [IList&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=netframework-4.8) collection of a class that does not implement [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) (for example [List&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=netframework-4.8)), you can use it with ObservableComputations. See 

https://github.com/gsonnenf/Gstc.Collections.ObservableLists

Nuget: https://www.nuget.org/packages/Gstc.Collections.ObservableLists
