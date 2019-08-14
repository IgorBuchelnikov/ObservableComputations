# ObservableCalculations
## What is it? 
This is .NET library for a calculations over INotifyPropertyChanged and INotifyColectionChanged (ObservableCollection) objects. Results of the calculations are INotifyPropertyChanged and INotifyColectionChanged (ObservableCollection) objects. The calculations includes ones similar to LINQ and the calculation of arbitrary expression. ObservableCalculations is implementation of [reactive programming paradigm](https://en.wikipedia.org/wiki/Reactive_programming).
## Status
ObservableCalculations library is ready to use in production. All essential functions is implemeted. All the bugs found is fixed. Now I work on the documentation and nuget package.
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

ObservavleCalculations library contains analogs of the almost all LINQ methods. You can combine calls of ObservavleCalculations extention methods including chaining and nesting, as you do for LINQ methods.

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
In this code sample we observe value of discounted price expression. Calculating&lt;TResult&gt; class implements INotifyPropertyChanged interface. Complicity of expression to observe is not limited. The expression can contain results of any ObservavleCalculations methods, including LINQ analogs.
### Use cases and benefits
WPF, Xamarin, Blazor. You can bind UI controls to the instances of ObservableCalculations classes (Filtering, Calculating etc.). If you do it, you do not have to worry about forgetting to call INotifyPropertyChanged and INotifyColectionChanged for the calculated properties when the source properties changes. With ObservableCalculations, you define how the value should be calculated, everything else ObservableCalculations will do. 

This approach facilitates asynchronous programming. You can show the user the UI form and in the background load the source data (from DB or web service). As the source data loads, the UI form will be filled with the calculated data. If the UI form is already shown to the user, you can also refresh the source data in the background, the calculated data on the UI form will be refreshed thanks to ObservableCalculations. You get the following benefits:
* Source data loading code and UI refresh code can be clearly separated.
* The end user will see the UI form faster, since the beginning of the rendering is not tied to the availability of source data.

General benefits:
* Less human error: all calculated data is always fresh. Calculated data shown to the user will always correspond to the user input and the data loaded from an external source.
* Less boilerplate imperative code. More clear declarative (functional style) code.
* User has no need manually refresh calculated data.
* You do not need refresh calculated data by the timer.
## Full list of methods and classes
Before examine the table bellow, please take into account

* CollectionCalculating&lt;TSourceItem&gt; is derived from [ObservableCollection&lt;TSourceItem&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8). That class implments INotifyCollectionChanged interface.
* ScalarCalculating&lt;TResult&gt; implements IReadScalar&lt;TResult&gt;
```csharp
public interface IReadScalar<out ValueType> : System.ComponentModel.INotifyPropertyChanged
{
	ValueType Value { get;}
}
```
From code above you can see: ScalarCalculation&lt;TResult&gt; allows you to observe the changes of the Value property through the PropertyChanged event of the INotifyPropertyChanged interface.

<html xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:x="urn:schemas-microsoft-com:office:excel"
xmlns="http://www.w3.org/TR/REC-html40">

<head>
<meta http-equiv=Content-Type content="text/html; charset=windows-1251">
<meta name=ProgId content=Excel.Sheet>
<meta name=Generator content="Microsoft Excel 15">
<link rel=File-List href="методы.files/filelist.xml">
<!--table
	{mso-displayed-decimal-separator:"\,";
	mso-displayed-thousand-separator:" ";}
.font021225
	{color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;}
.font121225
	{color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;}
.xl1521225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:general;
	vertical-align:bottom;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl6521225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:#24292E;
	font-size:11.0pt;
	font-weight:700;
	font-style:normal;
	text-decoration:none;
	font-family:"Segoe UI", sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:center;
	vertical-align:top;
	border:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:normal;}
.xl6621225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:#24292E;
	font-size:11.0pt;
	font-weight:700;
	font-style:normal;
	text-decoration:none;
	font-family:"Segoe UI", sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:center;
	vertical-align:top;
	border-top:.5pt solid windowtext;
	border-right:.5pt solid windowtext;
	border-bottom:.5pt solid windowtext;
	border-left:none;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:normal;}
.xl6721225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:general;
	vertical-align:bottom;
	border-top:none;
	border-right:.5pt solid windowtext;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl6821225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:general;
	vertical-align:bottom;
	border-top:none;
	border-right:.5pt solid windowtext;
	border-bottom:.5pt solid windowtext;
	border-left:none;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl6921225
	{color:#24292E;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:"Segoe UI", sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:general;
	vertical-align:top;
	border-top:none;
	border-right:.5pt solid windowtext;
	border-bottom:.5pt solid windowtext;
	border-left:none;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:normal;
	padding-left:9px;
	mso-char-indent-count:1;}
.xl7021225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:general;
	vertical-align:bottom;
	border:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7121225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:right;
	vertical-align:bottom;
	border-top:none;
	border-right:none;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7221225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:italic;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:right;
	vertical-align:bottom;
	border-top:.5pt solid windowtext;
	border-right:none;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7321225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:right;
	vertical-align:bottom;
	border-top:.5pt solid windowtext;
	border-right:none;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7421225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:italic;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:left;
	vertical-align:bottom;
	border-top:.5pt solid windowtext;
	border-right:none;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7521225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:general;
	vertical-align:bottom;
	border:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:normal;}
.xl7621225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:left;
	vertical-align:bottom;
	border-top:.5pt solid windowtext;
	border-right:none;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7721225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:left;
	vertical-align:bottom;
	border-top:none;
	border-right:none;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7821225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:left;
	vertical-align:middle;
	border-top:.5pt solid windowtext;
	border-right:none;
	border-bottom:none;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl7921225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:left;
	vertical-align:middle;
	border-top:none;
	border-right:none;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:nowrap;}
.xl8021225
	{padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	color:black;
	font-size:11.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	mso-font-charset:204;
	mso-number-format:General;
	text-align:general;
	vertical-align:bottom;
	border-top:none;
	border-right:.5pt solid windowtext;
	border-bottom:.5pt solid windowtext;
	border-left:.5pt solid windowtext;
	mso-background-source:auto;
	mso-pattern:auto;
	white-space:normal;}
--></style>
</head>

<body>
<!--[if !excel]>&nbsp;&nbsp;<![endif]-->
<!--Следующие сведения были подготовлены мастером публикации веб-страниц
Microsoft Excel.-->
<!--При повторной публикации этого документа из Excel все сведения между тегами
DIV будут заменены.-->
<!----------------------------->
<!--НАЧАЛО ФРАГМЕНТА ПУБЛИКАЦИИ МАСТЕРА ВЕБ-СТРАНИЦ EXCEL -->
<!----------------------------->

<div id="методы_21225" align=center x:publishsource="Excel">

<table border=0 cellpadding=0 cellspacing=0 width=1145 style='border-collapse:
 collapse;table-layout:fixed;width:859pt'>
 <col width=402 style='mso-width-source:userset;mso-width-alt:14701;width:302pt'>
 <col width=220 style='mso-width-source:userset;mso-width-alt:8045;width:165pt'>
 <col width=272 style='mso-width-source:userset;mso-width-alt:9947;width:204pt'>
 <col width=251 style='mso-width-source:userset;mso-width-alt:9179;width:188pt'>
 <tr height=53 style='mso-height-source:userset;height:39.75pt'>
  <td height=53 class=xl6521225 width=402 style='height:39.75pt;width:302pt'><b>ObservableCalculations
  overloaded methods group</b><span style='mso-spacerun:yes'> </span></td>
  <td class=xl6621225 width=220 style='width:165pt'><b>MS LINQ overloaded methods
  group</b></td>
  <td class=xl6621225 width=272 style='width:204pt'><b>Returned instance class
  is derived from</b></td>
  <td class=xl6621225 width=251 style='width:188pt'><b>Note</b></td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7321225 style='height:15.0pt;border-top:none'><font
  class="font121225">Appending</font><font class="font021225">&lt;TSourceItem&gt;</font></td>
  <td class=xl7621225 style='border-top:none'>Append</td>
  <td class=xl7021225 style='border-top:none'>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'><font
  class="font121225">Aggregating</font><font class="font021225">&lt;TSourceItem,
  TResult&gt;</font></td>
  <td class=xl7721225>Aggregate</td>
  <td class=xl6721225>ScalarCalculating&lt;TResult&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'><font
  class="font121225">AllCalcuating</font><font class="font021225">&lt;TSourceItem&gt;</font></td>
  <td class=xl7721225>All</td>
  <td class=xl6721225>ScalarCalculating&lt;bool&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'>AnyCalcuating</td>
  <td class=xl7721225>Any</td>
  <td class=xl6721225>ScalarCalculating&lt;bool&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'><font
  class="font121225">Appending</font><font class="font021225">&lt;TSourceItem&gt;</font></td>
  <td class=xl7721225>Append</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'>Not
  applicable</td>
  <td class=xl7721225>AsEnumerable</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'><font
  class="font121225">Averaging</font><font class="font021225">&lt;TSourceItem,
  TResult&gt;</font></td>
  <td class=xl7721225>Average</td>
  <td class=xl6721225>ScalarCalculating&lt;TResult&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'><font
  class="font121225">Binding class</font></td>
  <td class=xl7421225 style='border-top:none'>Not applicable</td>
  <td class=xl6721225></td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Calculating&lt;TResult&gt;</td>
  <td class=xl7421225 style='border-top:none'>Not applicable</td>
  <td class=xl6721225>ScalarCalculating&lt;TResult&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Casting&lt;TResultItem&gt;</td>
  <td class=xl7721225>Cast</td>
  <td class=xl6721225>CollectionCalculating&lt;TResultItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Concatenating&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Concat</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>ContainsCalcuating</td>
  <td class=xl7721225>Contains</td>
  <td class=xl6721225>ScalarCalculating&lt;bool&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7321225 style='height:16.5pt;border-top:none'>ObservableCollection.Count
  property</td>
  <td class=xl7721225>Count</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7221225 style='height:16.5pt;border-top:none'>Not
  implemented</td>
  <td class=xl7721225>DefaultIfEmpty</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=60 style='height:45.0pt'>
  <td height=60 class=xl7121225 style='height:45.0pt'>Crossing&lt;TOuterSourceItem,
  TInnerSourceItem&gt;</td>
  <td class=xl7421225 style='border-top:none'>Not implemented</td>
  <td class=xl7521225 width=272 style='border-top:none;width:204pt'>CollectionCalculating&lt;<br>
    <span style='mso-spacerun:yes'>    </span>JoinPair&lt;TOuterSourceItem,
  <br>
    <span style='mso-spacerun:yes'>        </span>TInnerSourceItem&gt;&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>Cartesian product</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Distincting&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Distinct</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7221225 style='height:15.0pt;border-top:none'>Not
  applicable</td>
  <td class=xl7721225>ElementAt</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>ItemCalculating</td>
  <td class=xl7721225>ElementAtOrDefault</td>
  <td class=xl6721225>ScalarCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7221225 style='height:16.5pt;border-top:none'>Not
  applicable</td>
  <td class=xl7721225>Empty</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Excepting&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Except</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7221225 style='height:16.5pt;border-top:none'>Not
  applicable</td>
  <td class=xl7621225 style='border-top:none'>First</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>FirstCalculating&lt;TSourceItem&gt;</td>
  <td class=xl7721225>FirstOrDefault</td>
  <td class=xl6721225>ScalarCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=41 style='mso-height-source:userset;height:30.75pt'>
  <td height=41 class=xl7121225 style='height:30.75pt'>Grouping&lt;TSourceItem,
  TKey&gt;</td>
  <td class=xl7721225>Group</td>
  <td class=xl8021225 width=272 style='width:204pt'>CollectionCalculating&lt;Group<br>
    <span style='mso-spacerun:yes'>    </span>&lt;TSourceItem, TKey&gt;&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>Can contain the group with
  null key</td>
 </tr>
 <tr height=60 style='height:45.0pt'>
  <td height=60 class=xl7121225 style='height:45.0pt'>GroupJoining&lt;TOuterSourceItem,
  TInnerSourceItem, TKey&gt;,<span style='mso-spacerun:yes'> </span></td>
  <td rowspan=2 class=xl7821225 style='border-bottom:.5pt solid black;
  border-top:none'>GroupJoin</td>
  <td class=xl8021225 width=272 style='width:204pt'>CollectionCalculating&lt;JoinGroup&lt;<br>
    <span style='mso-spacerun:yes'>    </span>TOuterSourceItem, <br>
    <span style='mso-spacerun:yes'>        </span>TInnerSourceItem,
  TKey&gt;&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=60 style='height:45.0pt'>
  <td height=60 class=xl7121225 style='height:45.0pt'>PredicateGroupJoining&lt;TOuterSourceItem,
  TInnerSourceItem&gt;</td>
  <td class=xl8021225 width=272 style='width:204pt'>CollectionCalculating&lt;<br>
    <span style='mso-spacerun:yes'>    </span>PredicateJoinGroup&lt;<br>
    <span style='mso-spacerun:yes'>        </span>TOuterSourceItem,
  TInnerSourceItem&gt;&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Intersecting&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Intersect</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=60 style='height:45.0pt'>
  <td height=60 class=xl7121225 style='height:45.0pt'>Joing&lt;TOuterSourceItem,
  TInnerSourceItem&gt;</td>
  <td class=xl7721225>Join</td>
  <td class=xl8021225 width=272 style='width:204pt'>CollectionCalculating&lt;<br>
    <span style='mso-spacerun:yes'>   
  </span>JoinPair&lt;TOuterSourceItem,<br>
    <span style='mso-spacerun:yes'>         </span>TInnerSourceItem&gt;&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7221225 style='height:16.5pt;border-top:none'>Not
  applicable</td>
  <td class=xl7621225 style='border-top:none'>Last</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>LastCalcuating&lt;TSourceItem&gt;</td>
  <td class=xl7721225>LastOrDefault</td>
  <td class=xl6721225>ScalarCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7221225 style='height:16.5pt;border-top:none'>Not
  applicable</td>
  <td class=xl7621225 style='border-top:none'>LongCount</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Maximazing&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Max</td>
  <td class=xl6721225>ScalarCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Minimazing&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Min</td>
  <td class=xl6721225>ScalarCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>OfTypeCalculating&lt;TResultItem&gt;</td>
  <td class=xl7721225>OfType</td>
  <td class=xl6721225>CollectionCalculating&lt;TResultItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Ordering&lt;TSourceItem,
  TOrderingValue&gt;</td>
  <td class=xl7721225>Order</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Ordering&lt;TSourceItem,
  TOrderingValue&gt;</td>
  <td class=xl7721225>OrderByDescending</td>
  <td class=xl6721225>CollectionCalculating&lt;TResultItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>Prepending&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Prepend</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7121225 style='height:16.5pt'>SequenceCalculating</td>
  <td class=xl7721225>Range</td>
  <td class=xl6721225>CollectionCalculating&lt;int&gt;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class=xl7221225 style='height:16.5pt;border-top:none'>Not
  implemented</td>
  <td class=xl7621225 style='border-top:none'>Repeate</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6921225 width=251 style='width:188pt'>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Reversing&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Reverse</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Selecting&lt;TSourceItem,
  TResultItem&gt;</td>
  <td class=xl7721225>Select</td>
  <td class=xl6721225>CollectionCalculating&lt;TResultItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>SelectingMany&lt;TSourceItem,
  TResultItem&gt;</td>
  <td class=xl7721225>SelectMany</td>
  <td class=xl6721225>CollectionCalculating&lt;TResultItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7221225 style='height:15.0pt;border-top:none'>Not
  implemented</td>
  <td class=xl7621225 style='border-top:none'>SequenceEqual</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7221225 style='height:15.0pt;border-top:none'>Not
  applicable</td>
  <td class=xl7721225>Single</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7221225 style='height:15.0pt;border-top:none'>Not
  applicable</td>
  <td class=xl7721225>SingleOrDefault</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Skiping&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Skip</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>SkipingWhile&lt;TSourceItem&gt;</td>
  <td class=xl7721225>SkipWhile</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>StringsConcatenating</td>
  <td class=xl7721225>string.Join</td>
  <td class=xl6721225>ScalarCalculating&lt;string&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Summarizing&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Sum</td>
  <td class=xl6721225>ScalarCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Taking&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Take</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>TakingWhile&lt;TSourceItem&gt;</td>
  <td class=xl7721225>TakeWhile</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>ThenOrdering&lt;TSourceItem,
  TOrderingValue&gt;</td>
  <td class=xl7721225>ThenBy</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>ThenOrdering&lt;TSourceItem,
  TOrderingValue&gt;</td>
  <td class=xl7721225>ThenByDescending</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7221225 style='height:15.0pt;border-top:none'>Not
  applicable</td>
  <td class=xl7621225 style='border-top:none'>ToArray</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Dictionaring&lt;TSourceItem,
  TKey, TValue&gt;</td>
  <td class=xl7721225>ToDictionary</td>
  <td class=xl6721225>Dictionary&lt;TKey, TValue&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Hashing&lt;TSourceItem,
  TKey&gt;</td>
  <td class=xl7721225>ToHashSet</td>
  <td class=xl6721225>HashSet&lt;TKey&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7221225 style='height:15.0pt;border-top:none'>Not
  applicable</td>
  <td class=xl7621225 style='border-top:none'>ToList</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7221225 style='height:15.0pt;border-top:none'>Not
  implemented</td>
  <td class=xl7621225 style='border-top:none'>ToLookup</td>
  <td class=xl6721225>&nbsp;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Uniting&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Union</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Using&lt;TArgument,
  TResult&gt;</td>
  <td class=xl7421225 style='border-top:none'>Not applicable</td>
  <td class=xl6721225>ScalarCalculating&lt;TResult&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=20 style='height:15.0pt'>
  <td height=20 class=xl7121225 style='height:15.0pt'>Filtering&lt;TSourceItem&gt;</td>
  <td class=xl7721225>Where</td>
  <td class=xl6721225>CollectionCalculating&lt;TSourceItem&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <tr height=60 style='height:45.0pt'>
  <td height=60 class=xl7121225 style='height:45.0pt'>Zipping&lt;TSourceItemLeft,
  TSourceItemRight&gt;</td>
  <td class=xl7721225>Zip</td>
  <td class=xl8021225 width=272 style='width:204pt'>CollectionCalculating&lt;ZipPair&lt;<br>
    <span style='mso-spacerun:yes'>    </span>TSourceItemLeft, <br>
    <span style='mso-spacerun:yes'>    </span>TSourceItemRight&gt;&gt;</td>
  <td class=xl6821225>&nbsp;</td>
 </tr>
 <![if supportMisalignedColumns]>
 <tr height=0 style='display:none'>
  <td width=402 style='width:302pt'></td>
  <td width=220 style='width:165pt'></td>
  <td width=272 style='width:204pt'></td>
  <td width=251 style='width:188pt'></td>
 </tr>
 <![endif]>
</table>

</div>


<!----------------------------->
<!--КОНЕЦ ФРАГМЕНТА ПУБЛИКАЦИИ МАСТЕРА ВЕБ-СТРАНИЦ EXCEL-->
<!----------------------------->
</body>

</html>

## IReadScalar&lt;TValue&gt; arguments

Try to put argument of type IReadScalar&lt;TValue&gt; instead of TValue.
Feature is implemented. Documentation will be added.

## Modifing CollectionCalculating<TItem> and ScalarCalculating<string>

What about:

```csharp
Filtering<Order> myOrders = orders.Filtering(o => o.Cumtomer == me); 
myOrders.Add(new Order());

Expression<Func<decimal>> discountedPriceExpression =
	() => order.Price - order.Price * order.Discount / 100;
Calculating<decimal> discountedPriceCalculating = 
	discountedPriceExpression.Calculating();
discountedPriceCalculating.Value = 99;	
	
```
?

Feature is implemented. Documentation will be added.

## Consistent property and the inconsistency exception
Feature is implemented. Documentation will be added.

## PostCollectionChanged and PostValueChanged events
Feature is implemented. Documentation will be added.

## Debuging

### InstantiatingStackTrace property
Feature is implemented. Documentation will be added.

### DebugTag property
Feature is implemented. Documentation will be added.

## Perfomance tips






















