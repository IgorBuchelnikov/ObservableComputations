## Что нужно знать для чтения этой статьи?

Для того чтобы понимать написанное здесь, Вы должны знать: базовые сведения о программировании и ООП, синтаксис C# (включая события, extension методы, лямбда-выражения), [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/), интерфейсы: [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8).

Для того чтобы представить себе какие [преимущества можно получить при использовании ObservableComputations](#области-применеия-и-преимущества), Вы должны знать: о [привязке данных (binding) в WPF](https://docs.microsoft.com/en-us/dotnet/desktop-wpf/data/data-binding-overview) (или в другой UI платформе: [Xamarin](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/data-binding/basic-bindings), [Blazor](https://demos.telerik.com/blazor-ui/grid/observable-data)), особенно её связь с интерфейсами [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8), свойство [DbSet.Local](https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbset.local?view=entity-framework-6.2.0) ([local data](https://docs.microsoft.com/en-us/ef/ef6/querying/local-data)) из Entity framework, [ассинхронные запросы Entity framewok](https://www.entityframeworktutorial.net/entityframework6/async-query-and-save.aspx).

## Что такое ObservableComputations?

Это кросс-платформенная .NET библеотека для вычислений, аргументами и результатами которых являются объекты реализующие интерфейсы [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) ([ObservableCollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8)). Вычисления включают в себя те же вычисления которые есть в [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/), вычисление произвольного выражения и некоторые дополнительные вычисления. ObservableComputations реализованы как [методы расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), подобно [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) методам. Вы можете комбинировать вызовы [методов расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservavleComputations (цепочка вызовов и вложенные вызовы), как Вы это делаете в [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/). ObservableComputations это простая в использовании и мощная реализация [парадигмы реактивного программирования](https://en.wikipedia.org/wiki/Reactive_programming). С ObservableComputations, Ваш код будет более соответствовать функциональному стилю, чем при использовании стандартного [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).

## Аналоги

ObservableComputations не явялется аналогом [Reactive Extentions](https://github.com/dotnet/reactive). Аналогами ObservableComputations явяляются следующие библеотеки: Obtics, [OLinq](https://www.nuget.org/packages/OLinq/), BindableLinq, ContinuousLinq. Вот главные отличия ObservableComputations от  [Reactive Extentions](https://github.com/dotnet/reactive):

* [Reactive Extentions](https://github.com/dotnet/reactive) абстрагирован от конкретного события и от семантики событий: это библеотека для обработки всех возможных событий. Reactive Extentions обрабатывает все события одинаковым образом, а вся специфика только в пользовательском коде. ObservableComputations сфокусирован только на событиях [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) и приносит большую пользу обрабатывая эти события. Поэтому, Вы можете использовать ObservableComputations отдельно или вместе с [Reactive Extentions](https://github.com/dotnet/reactive). Это руководство включает в себя [пример](#isconsistent-property-and-the-inconsistency-exception) взаимодействия ObservableComputations с  [Reactive Extentions](https://github.com/dotnet/reactive).
* Библеотека [Reactive Extentions](https://github.com/dotnet/reactive)  предоставляет поток событий. ObservableComputations предоставляет не только поток событий изменения данных, но вычиленные в данный момент данные.

## Статус

ObservableComputations готова к использованию в производственном окружении. Все необходимые функции реализованы и покрыты unit-тестами. Все найденные баги исправлены.

## Как установить?

Библеотека ObservableComputations доступна на [NuGet](https://www.nuget.org/packages/ObservableComputations/).

## Как получить помощь?

Вы можете [создать issue](https://github.com/IgorBuchelnikov/ObservableComputations/issues/new) or [связаться со мной по электронной почте](mailto:igor_buchelnikov_github@mail.ru).

## Как я могу помочь проекту?

Приветсвуются комментарии и замечания к документации. Нужны демо проекты, посты в блогах и руководства. 

##  Бустрый старт
### Аналоги [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) методов
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

			// Здесь мы начинаем использовать ObservableComputations!
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
Как Вы видите [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Filtering* это аналог метода *Where* из [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/). [Метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Filtering* возвращает экземпляр класса *Filtering&lt;Order&gt;*. Класс *Filtering&lt;TSourceItem&gt;* реализует интерфейс [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) и наследуется от [ObservableCollection&lt;TSourceItem&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8). Изучая код выше Вы увидите, что *expensiveOrders* не перевычисляется заново каждый раз когда коллекция *orders* меняется или меняется свойство *Price* какого-либо заказа, в коллекции *expensiveOrders* проиходят только те изменения, которые отражают отдельное изменение в коллекции *orders* или отдельное изменение свойства *Price* какого-либо заказа. [Согласно терминологии реактивного программирования, такое поведение определяет модель распространения изменений, как "push"](https://en.wikipedia.org/wiki/Reactive_programming#Change_propagation_algorithms).

В коде выше, во время выполнения [метода расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Filtering*(во время сождания экземпляра класса *Filtering&lt;Order&gt;*), проихходит подписка на следующие события: событие [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) коллекции *orders* и событие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) каждго экземпляра класса *Order*. ObservableComputations выполняет только слабые подписка (**weak event pattern**), поэтому *expensiveOrders* может быть выгружен из памяти сборщиком мусора, в то время как *orders* останется в памяти.

Сложность выражения предиката переданного в [метод расшерения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Filtering* (*o => o.Price > 25*) не ограничена. Выражение может включать в себя результаты вызовов методов ObservavleComputations, включая аналоги [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).

### Отслеживание произвольного выражения
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

			// Здесь мы начинаем использовать ObservableComputations!
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
В этом примере кода мы следим за значением выражения цены со скидкой. Класс *Computing&lt;TResult&gt;* реализует интерфейс [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8). Сложность отслеживаемого выражения не ограничена. Выражение может включать в себя результаты вызовов методов ObservavleComputations, включая аналоги [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).

Так же как в предыдущем примере присходит слабая подписка (**weak event pattern**) на сыбытие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) экземпляра класса *Order*.

Если Вы хотите чтобы выражение *() => order.Price - order.Price * order.Discount / 100*  было чистой функцией, нет проблем:  
  
```csharp
			Expression<Func<Order, decimal>> discountedPriceExpression = 
				o => o.Price - o.Price * o.Discount / 100;
				
			// Здесь мы начинаем использовать ObservableComputations!
			Computing<decimal> discountedPriceComputing = 
				order.Using(discountedPriceExpression);
```
Теперь выражение *discountedPriceExpression* может быть использовано для других экземпляров класса *Order*.


## Области применеия и преимущества

### Привязка к элементам пользовательского интерфейса (binding)
WPF, Xamarin, Blazor. Вы можете привязывать (binding) элементы пользовательского интерфейса (controls) к экземплярам классов ObservableComputations (*Filtering*, *Computing* etc.). Если Вы так делаете, Вам не нужно беспокоиться о том, что Вы забыли вызвать событие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) для вычисляемых свойств или вручную обработать изменение в какой-либо коллекции. С ObservableComputations Вы определяете как значение должно вычисляться, всё остальное ObservableComputations сделает за Вас. 

### Ассинхронное программирование
Такой подход облегчает **ассинхронное программирование**. Вы можете показать пользователю форму и начать загружать исходные данные (из БД или web-сервиса) в фоне. По мере того как исходные данные загружаются, форма наполняется вычисленными данными. Пользователь увидит форму быстрее (пока исходные данные загружаются в фоне, Вы можете начать рендеринг). Если форма уже показана пользователю, Вы можете обновить исходные данные в фоне, вычисляемые данные отображенные на форме обновятся благодаря ObservableComputations. 

### Повышенная производиельность
Если у Вас есть сложные вычисления, часто меняющиеся исходные данные и\или данных много, вы можете получить выигрыш в производительности с ObservableComputations, так как Вам не надо перевычислять данные с нуля каждый раз когда меняются исходные данные. Каждое маленькое изменение в исходных данных вызывает маленькое изменение в данных вычисленных средствами ObservableComputations.
Производительность пользовательского интерфейса возрастает, так как необходимость в ререндеренге уменьшается (только изменённые данные рендерятся) и данные из внешних источников (DB, web-сервис) загружаются в фоне (см. [предыдущий раздел](#Ассинхронное-программирование)).

### Чистый и надёжный код
* Меньше шаблонного императивного кода. Больше чистого декларативного (в функциональном стиле) кода. Общий объём кода уменьшается.
* Меньшая вероятность ошибки программиста: вычисляемые данные показанные пользователю пользователю будут всегда соответвствовать пользовательскому вводц и данным загруженным из внешних источников (DB, web-сервис).
* Код загрузки исходных данные и код для вычисления данных отображаемых в пользовательском интерфейсе могут быть чётко разделены.
* Вы можете не беспокоиться о том, что забыли обновить вычисляемые данные. Все вычичляемые данные будут обновляться автоматически.

### Дружелюбный пользовательский интерфейс
ObservableComputations облегчают создание дружелюбного пользовательского интерфейса.
* Пользователю не нужно вручную обновлять вычисляемые данные.
* Пользователь видит вычисляемые данные всегда, а не только по запросу.
* Вам не нужно обновлять вычисляемые данные по таймеру.

## Полный список методов и классов
Перед изучение таблицы, представленной ниже, пожалуйста, обратите внимание на то, что

* *CollectionComputing&lt;TSourceItem&gt;* наследуется от [ObservableCollection&lt;TSourceItem&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8). Этот класс реализует интерфейс [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8).


* *ScalarComputing&lt;TValue&gt;* реализует интерфейс *IReadScalar&lt;TValue&gt;*;
```csharp
public interface IReadScalar<out TValue> : System.ComponentModel.INotifyPropertyChanged
{
	TValue Value { get;}
}
```
Свойство *Value* позволяет получить текущий результат вычисления. Из кода выше вы можете увидеть, что *ScalarComputation&lt;TValue&gt;* позволяет следить за значением свойства *Value* с помощью события [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) интерфейса [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8).

<html>
<body>
<table cellspacing="0" border="0">
	<colgroup width="267"></colgroup>
	<colgroup width="233"></colgroup>
	<colgroup width="221"></colgroup>
	<colgroup width="285"></colgroup>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="53" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Группа перегруженных <br>методов ObservableCalculations </font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Группа перегруженных<br>методов MS LINQ</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Возвращаемые объект<br>является</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Примечание</font></b></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Appending&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Append</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Aggregating&lt;TSourceItem, TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Aggregate</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TValue&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">AllCalcuating&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">All</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;bool&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">AnyCalcuating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Any</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;bool&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Appending&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Append</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Not applicable</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">AsEnumerable</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Averaging&lt;TSourceItem, TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Average</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Binding class</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше здесь</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Computing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше здесь</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Casting&lt;TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Cast</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="47" align="left" valign=bottom><font color="#000000">Concatenating&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Concat</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Элементами коллекции-источника могут быть <br>INotifyCollectionChanged <br>или IReadScalar&lt;INotifyCollectionChanged&gt;</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ContainsCalcuating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Contains</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;bool&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ObservableCollection.Count property</font></td>
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
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="47" align="left" valign=bottom><font color="#000000">Crossing&lt;<br>    TouterSourceItem,<br>    TinnerSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not implemented</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;<br>    JoinPair&lt;TOuterSourceItem, <br>        TInnerSourceItem&gt;&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Декартово произведение</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Differing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше здесь</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Distincting&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Distinct</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ElementAt</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="92" align="left" valign=bottom><font color="#000000">ItemComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ElementAtOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если запрошенный индекс <br>выходит за границы коллекции-источника<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value <br>возвращает значение по умолчанию <br>для типа TSourceItem или значение <br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Empty</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Excepting&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Except</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">First</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="77" align="left" valign=bottom><font color="#000000">FirstComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">FirstOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="41" align="left" valign=bottom><font color="#000000">Grouping&lt;TSourceItem, TKey&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Group</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;Group<br>    &lt;TSourceItem, TKey&gt;&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Может содержать группу с ключём null</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="47" align="left" valign=bottom><font color="#000000">GroupJoining&lt;<br>    TOuterSourceItem, <br>    TinnerSourceItem, Tkey&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" rowspan=2 align="left" valign=middle><font color="#000000">GroupJoin</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;JoinGroup&lt;<br>    TOuterSourceItem, <br>        TInnerSourceItem, TKey&gt;&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="left" valign=bottom><font color="#000000">PredicateGroupJoining&lt;<br>    TOuterSourceItem, <br>    TinnerSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;<br>    PredicateJoinGroup&lt;<br>        TOuterSourceItem, <br>        TinnerSourceItem&gt;&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Intersecting&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Intersect</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="47" align="left" valign=bottom><font color="#000000">Joing&lt;TOuterSourceItem, TInnerSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Join</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;<br>    JoinPair&lt;TOuterSourceItem,<br>         TInnerSourceItem&gt;&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Last</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="77" align="left" valign=bottom><font color="#000000">LastCalcuating&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">LastOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">LongCount</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="77" align="left" valign=bottom><font color="#000000">Maximazing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Max</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="77" align="left" valign=bottom><font color="#000000">Minimazing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Min</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">OfTypeComputing&lt;TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">OfType</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Ordering&lt;TSourceItem, TOrderingValue&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Order</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Ordering&lt;TSourceItem, TOrderingValue&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">OrderByDescending</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Prepending&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Prepend</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PropertyAccessing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше здесь</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">SequenceComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Range</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;int&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not implemented</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Repeate</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PreviousTracking&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше здесь</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Reversing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Reverse</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Selecting&lt;TSourceItem, TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Select</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">SelectingMany&lt;TSourceItem, TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">SelectMany</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TResultItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not implemented</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">SequenceEqual</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Single</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">SingleOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Skiping&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Skip</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">SkipingWhile&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">SkipWhile</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">StringsConcatenating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">string.Join</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;string&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Summarizing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Sum</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Taking&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Take</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">TakingWhile&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">TakeWhile</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ThenOrdering&lt;TSourceItem, TOrderingValue&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ThenBy</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ThenOrdering&lt;TSourceItem, TOrderingValue&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ThenByDescending</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ToArray</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Dictionaring&lt;TSourceItem, TKey, TValue&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ToDictionary</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Dictionary&lt;TKey, TValue&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Hashing&lt;TSourceItem, TKey&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ToHashSet</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">HashSet&lt;TKey&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ToList</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><i><font color="#000000">Not implemented</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ToLookup</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Uniting&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Union</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Using&lt;TArgument, TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше здесь и здесь</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Filtering&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Where</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;TSourceItem&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">WeakPreviousTracking&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><i><font color="#000000">Not applicable</font></i></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing&lt;TResult&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше здесь</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="47" align="left" valign=bottom><font color="#000000">Zipping&lt;TSourceItemLeft, TSourceItemRight&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Zip</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing&lt;ZipPair&lt;<br>    TSourceItemLeft, <br>    TSourceItemRight&gt;&gt;</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
</table>
<!-- ************************************************************************** -->
</body>

</html>

Для всех вычислений имеющих параметры типа [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8): null значение параметра обрабатывается как пустая коллекция.

Для всех вычислений имеющих параметры типа *IReadScalar*&lt;[INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8)&gt;: null значение свойства *IReadScalar*&lt;[INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8)&gt;.*Value* обрабатывается как пустая коллекция.



## Передача аргументов как обозреваемых и не обозреваемых
Аргументы [методов расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservableComputations могут быть переданы двуми путями: как обозреваемые и как не обозреваемые.

### Передача аргументов как не обозреваемых
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

			// Здесь мы начинаем использовать ObservableComputations!
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
В приведённом коде мы вычисляем является ли залогиненный пользователь хоккейным игроком. Выражение "*loginManager.LoggedInPerson*" переданное в метод *ContainsComputing*  вычисляется (оценивается) алгоритмами ObservableComputations только один раз: когда класс *ContainsComputing&lt;Person&gt;* инстанцируется (когда вызывается [метод расшерения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *ContainsComputing*). Если свойство *LoggedInPerson* меняется, это изменение **не** отражается в *isLoggedInPersonHockeyPlayer*. 

Конечно, Вы можете использовать более сложное выражение, чем "*loginManager.LoggedInPerson* для передачи как аргумента в любой [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservableComputations. Как видите передача аргумента типа T как не обозреваемого это обычная передача аргумента типа T.

### Передача аргументов как обозреваемых

В предыдущем примере, мы предпологали, что наше приложение не поддерживает выход пользователя (logout) (и последующий вход (login)). Другими словами приложение не обрабатывает изменения свойства *LoginManager.LoggedInPerson*. Давайте добавим функционалость logout в наше приложение:  
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

В коде выше мы передаём аргумент в [метод расшерения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *ContainsComputing* как *IReadScalar&lt;Person&gt;* (а не как *Person* как в предудущем разделе). *Computing&lt;Person&gt;* реализует *IReadScalar&lt;Person&gt;*. *IReadScalar&lt;TValue&gt;* первоначально был упомянут в разделе ["Full list of methods and classes" section](#full-list-of-methods-and-classes). Как видите, если Вы хотите передать аргумент типа T как обозреваемый, Вы должны выполнить обычную передачу аргумента типа *IReadScalar&lt;T&gt;*. В этом случае используется другая перегруженная вервия метода *ContainsComputing*, в отличии от версии, которая использовалась в  [предыдущем разделе](#Passing-arguments-as-non-observables). Это даёт нам возможность следить за изменениями свойства *LoginManager.LoggedInPerson*. Теперь изменения свойства *LoginManager.LoggedInPerson* отражаются в *isLoggedInPersonHockeyPlayer*. Обратите внимание на то, что теперь класс *LoginManager* реализует [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8).  

Код выше может быть укорочен:  
  ```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
    hockeyTeam.ContainsComputing(() => loginManager.LoggedInPerson);
```
При использовании этой перегруженной версии метода *ContainsComputing*, переменные *loggedInPersonExpression* и *isLoggedInPersonHockeyPlayer* больше не нужны. Эта перегруженная версии метода *ContainsComputing* создаёт *Computing&lt;Person&gt;* "за ценой".

Другой укороченный вариант:

```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
    hockeyTeam.ContainsComputing<Person>(
        Expr.Is(() => loginManager.LoggedInPerson).Computing());
```

Первоначальный вариант может быть полезен, если Вы хотите переиспользовать *new Computing(() => loginManager.LoggedInPerson)* для других вычислений помимо *isLoggedInPersonHockeyPlayer*. Первый укороченный вариант не позволяет этого. Укороченные варианты могут быть полезны для [expression-bodied properties and methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members).

Конечно, вы можете использовать более сложное выражение чем "*() => loginManager.LoggedInPerson*" для передачи в качестве аргумента в любой [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservableComputations.

### Пердача коллекции источника как обозваемого аргумента
Как Вы видите все вызовы [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) пообных [методов расширения ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods)ObservableComputations в общем виде могут быть представлены как
```csharp
sourceCollection.ExtentionMethodName(arg1, arg2, ...);
```
*sourceCollection* это первый аргумент в объявлении [метода расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). Поэтому подобно другим аргументам он тоже может быть передан как  [не обозреваемый](#Passing-arguments-as-non-observables) и как [обозреваемый](#Passing-arguments-as-observables). До сих пор мы передавали коллекцию источник как не обозреваемый аргумент (это было простое выражение состоящее из одной переменной, конечно вы можете использовать более сложные выражения, суть остаётся та же). Теперь давайте попробуем передать коллекцию источник как обозреваемый аргумент:  

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

Как и в предыдущем разделе код выше может быть укорочен:
```csharp
Expression<Func<ObservableCollection<Person>>> hockeyTeamInterestedExpression =
    () => hockeyTeamManager.HockeyTeamInterested;

ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
   hockeyTeamInterestedExpression
      .ContainsComputing(() => loginManager.LoggedInPerson);
```

или:
```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
   Expr.Is(() => hockeyTeamManager.HockeyTeamInterested)
      .ContainsComputing(() => loginManager.LoggedInPerson);
```

или:  
```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
	new Computing<ObservableCollection<Person>>(
	    () => hockeyTeamManager.HockeyTeamInterested)
	.ContainsComputing<Person>(
		() => loginManager.LoggedInPerson);
```

или:
```csharp
ContainsComputing<Person> isLoggedInPersonHockeyPlayer =
	Expr.Is(() => hockeyTeamManager.HockeyTeamInterested).Computing()
	.ContainsComputing(
	    () => loginManager.LoggedInPerson);
```

Конечно, Вы можете использовать более сложное выражение чем "*() => hockeyTeamManager.HockeyTeamInterested* для передачи в качестве аргумента в любой [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservableComputations.

### Не обозреваемые и обозреваемые аргументы во вложенных вызовах
Мы продолжаем рассматривать пример из [предыдущего раздела](#Passing-source-collection-argument-as-obserable). Мы использовали следующий код для того чтобы отследить измененич в *hockeyTeamManager.HockeyTeamInterested*:
```csharp
new Computing<ObservableCollection<Person>>(
    () => hockeyTeamManager.HockeyTeamInterested)
```

Может показаться на первый взгляд, что следующий код будет работать и *isLoggedInPersonHockeyPlayer* будет отражать изменения в *hockeyTeamManager.HockeyTeamInterested*:

```csharp
Computing<bool> isLoggedInPersonHockeyPlayer = new Computing<bool>(() => 
   hockeyTeamManager.HockeyTeamInterested.ContainsComputing(
      () => loginManager.LoggedInPerson).Value);
```
 
В этом коде *"hockeyTeamManager.HockeyTeamInterested"* передан в [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *ContainsComputing* как [не обозркваемый](#Passing-arguments-as-non-observables), и не имеет значения, что  *"hockeyTeamManager.HockeyTeamInterested"* это часть выражения переданного в конструктор класса *Computing&lt;bool&gt;*, изменения в *"hockeyTeamManager.HockeyTeamInterested"* не будут отражатся в *isLoggedInPersonHockeyPlayer*. Правило обозреваемых и не обозреваемых аргументов применяется только в одном направлении: от вложенных (обёрнутых) в внешним (одорачивающим) вызовам. Другими словами, правило обозреваемых и не обозреваемых аргументов всегда справедливо, независимо от того является ли вычисление корневым или вложенным.

Вот другой пример:

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
В коде выше мы создаём вычисление *"filteredByTypeOrders"* которое отражает изменения в коллекция *orders* и *selectedOrderTypes* и в свойстве *Order.Type*. Обратите внимание на аргумент переданный в *ContainsComputing*. Следующий код не юудет отражать изменеия в свойстве *Order.Type*:

```csharp
ObservableCollection<Order> filteredByTypeOrders =  orders.Filtering(o => 
   selectedOrderTypes.ContainsComputing(o.Type).Value);
```

## Изменение вычислений
Единственный способ измение вычисление это изменить исходные данные. Вот пример кода:

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

В коде выше мы создаём вычисление *stepansOrders* (заказы Степана). Мы устанавливаем значение свойства *stepansOrders.InsertItemAction* для того чтобы определить как изменить коллекцию *orders* и *order*, который нужно добавить, так чтобы он был включён в выисление *stepansOrders*.

Обратите внимание на то, что [метод Add](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.add?view=netframework-4.8#System_Collections_Generic_ICollection_1_Add__0_) это член интерфейса [ICollection&lt;T&gt; interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1?view=netframework-4.8).

Данная возможность может быть полезна если Вы передаёте *stepansOrders* который абстрагрован от того, чем является *stepansOrders*: вычислением или обычной коллекцией. Этот код знает только то, что *stepansOrders* реализует [ICollection&lt;T&gt; interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1?view=netframework-4.8) и иногда хочет добавлять заказы в *stepansOrders*. Таким кодом, например, может [привязка данных в WPF](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.bindingmode?view=netframework-4.8#System_Windows_Data_BindingMode_TwoWay).

Свойства аналогичные *InsertItemAction* существуют и для других операций ([remove](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.remove?view=netframework-4.8), [set (replace)](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.collection-1.item?view=netframework-4.8#System_Collections_ObjectModel_Collection_1_Item_System_Int32_), [move](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1.move?view=netframework-4.8), [clear](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.clear?view=netframework-4.8)) и для установки значения свойства *ScalarComputing&lt;TValue&gt;.Value* (see  ["Full list of methods and classes" section](#full-list-of-methods-and-classes)).

## Свойство IsConsistent и исключение при нарушении целостности
Сценарий описанный в этом разделе очень специфичен. Возможно Вы никогда его не встретите. Однако если Вы хотите быть полностью готовыми прочтите его. Рассмотрим следующий код:
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

В коде у нас есть коллекция отношений: *relations*. Коллекция имеет избыточность: если коллекция имеет отношение  A к B типа "Родитель", она должна содержать соответствующее отношение: B к A типа "Ребёнок", и наобарот. Также мы имеем вычисляемую упорядоченную  коллекцию отношений *orderedRelations*. Наша задачу поддержать целостность коллекции тношений: если кто-то меняет мы должны отреагировать и восстановить целостность. Представьте, что единственным способом сделать является подписка на событие CollectionChanged коллекци *orderedRelations* (по каким-то причинам мы не можем подписаться на событие CollectionChanged коллекции *relations*). В коде выше мы предпологаем только один тип изменеий: Replace.
Код выше не работает: строка "*relations.Remove(new Relation{From = oldItem.To, To = oldItem.From, Type = invertRelationType(oldItem.Type)});*" выбрасывает:
> ObservableComputations.Common.ObservableComputationsException: 'The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after Consistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.'

> ObservableComputations.Common.ObservableComputationsException: 'Коллекция источник была изменена. Невозможно обработать это измение, так как обработка предыдущего изменения не завершена. Сделайте это изменение после возниковения события ConsistencyRestored (после того как Consistent станет равным true). Это исключение фотально и не может быть обработано, так как внутреннее состояние повреждено.'

Почему? Когда в коллекции *relations* заменяется элемент, коллекция *orderedRelations* производит не только замену, но и, дополнительно, последующе перемещение элемента, для того, чтобы поддержать порядок. После замены и перед перемещением,  коллекция *orderedRelations* не находится вцелостном состоянии и поэтому не может обрабатываеть любые другие изменения в коллекции *relations*. Вот исправленный код:  
  
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
В исправленном коде, мы откладываем восстановление целостности коллекции *relations* пока не произойжёт событие ConsistencyRestored коллекции *orderedRelations*. 
Для упрощения мы не отписываемся от события ConsistencyRestored, поэтому мы будем накапливать обработчики события ConsistencyRestoreds. Для того чтобы исравить это мы должны вручную отписываться от события ConsistencyRestored или использовать [Reactive Extensions](https://github.com/dotnet/reactive):

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

Отладка исключения при нарушении целостности описана [здесь](#inconsistency-exception).

## Отладка

### Пользовательский код: селекторы, предикаты, произвольные выраженения, обработчики изменений, обработчики событий [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8)
Селекторы это выражения, которые предаются в качестве аргумента в следуюшие методы расширения: Selecting, SelectingMany, Grouping, GroupJoining, Dictionaring, Hashing, Ordering, ThenOrdering, PredicateGroupJoining. Предикаты это выражения, которые предаются в качестве аргумента в метод расширения Filtering. Произвольные выражения это выражения, которые предаются в качестве аргумента в методы расширения Computing и Using. Обработчики изменений описаны в ["Modifing computations"](#Modifing-computations).

Вот код иллюстрирующий отладку произвольного выражения (другие типы могут быть отлажены аналочным образом):

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

Как Вы видите *exception.StackTrace* указывает на строку, которая вызвала исключение: *valueProvider.Value = new Random().Next(0, 1);*. Эта строка не указывает на вычисление, которое вызвало исключение: *computing1* or *computing2*. Чтобы определить исключение, которое вызвало исключение мы должны взглянуть на свойство *DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].InstantiatingStackTrace*. Это свойство содержит трассировку стека инстанцирования вычисления. По умолчанию ObservableComputations не сохранияет трассировки стека инстанцирования вычислений по соображениям производительности. Чтобы сохранять эти трассирвки стека используйте свойство *Configuration.SaveInstantiatingStackTrace*. По умолчанию ObservableComputations не следит за вычислениями выполняющими пользовательский код по соображениям производительности. Для того чтобы следить за вычислениями выполняющими пользовательский код используйте свойство *Configuration.TrackComputingsExecutingUserCode*.

Все необработанные исключения выброшенные в пользовательском коде фотальны, так как внутреннее состояние вычисление становится повреждённым. Обратите внимание на проверки на null.

### Исключение при нарушении целостности

Исключение при нарушении целостности было описано в разделе ["IsConsistent property and inconsistency exception"](#IsConsistent-property-and-inconsistency-exception).
В следующем примере, мы пытаемся сделать скидку для дорогих заказов: мы уменьшаем цену заказа до минимального значения, кратного сотне:

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
			catch (ObservableComputationsException exception)
			{
				Console.WriteLine($"Exception stacktrace:\n{exception.StackTrace}");
				Console.WriteLine($"\nComputing which caused the exception has been instantiated by the following stacktrace :\n{exception.Computing.InstantiatingStackTrace}");
			}

			Console.ReadLine();
		}
	}
}
```

Как Вы видите *exception.StackTrace* указывает на строку, которая вызвала исключение: *orders.Add(new Order(){Price = 35397});*. Эта строка не указывает на вычисление, которое вызвало исключение: *ordinaryOrders* or *expensiveOrders*. Чтобы определить исключение, которое вызвало исключение мы должны взглянуть на свойство *exception.Computing.InstantiatingStackTrace*. Это свойство содержит трассировку стека инстанцирования вычисления. По умолчанию ObservableComputations не сохранияет трассировки стека инстанцирования вычислений по соображениям производительности. Чтобы сохранять эти трассирвки стека используйте свойство *Configuration.SaveInstantiatingStackTrace*.

## Дополнительные события для обработки изменений: PreCollectionChanged, PreValueChanged, PostCollectionChanged, PostValueChanged
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

Код sdit имеет следущий вывод:
> Your order price is ₽100

Хотя мы могли ожидать:  
> Your order price is ₽100. You have a discount! Therefore your price is ₽90!

Почему? Мы подписались на событие *priceDiscounted.PropertyChanged* перед тем как *messageForUser* сделал это. Обработчики событий вызываются в порядке подписки (это деталь реализации .NET). Поэтому мы считываем *messageForUser.Value* перед тем как *messageForUser* обрабатывает изменение *order.Discount*.

Вот исправленный код:
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

Вместо *priceDiscounted.PropertyChanged* мы подписываемся на *priceDiscounted.PostValueChanged*. Это событие возниккает после *PropertyChanged*, поэтому мы можем быть уверены: все зависимые вычисления обновили свои значения. *PostValueChanged* объявлено в *ScalarComputing&lt;TValue&gt;*. *Computing&lt;string&gt;* наследует *ScalarComputing&lt;TValue&gt;*. *ScalarComputing&lt;TValue&gt;* впервые упомянут [здесь](#full-list-of-methods-and-classes). *ScalarComputing&lt;TValue&gt;* сожержит событие *PreValueChanged*. Это событие позволяет посмотреть состояние вычислений до изменения. Если вы хотите обрабатывать событие изменения свойства Вашего объекта (не вычисления как в предыдущем примере) и обработчик читает зависимые вычисления (подобно предыдущему примеру) Вы должны определить своё событие и вызывать его.

*CollectionComputing&lt;TItem&gt;* содержит события *PreCollectionChanged* и *PostCollectionChanged*. *CollectionComputing&lt;TItem&gt;* впервые упомянут [здесь](#full-list-of-methods-and-classes). Если Вы хотите обрабатываеть событие изменения Вашей коллекции, реализующей [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) (не вычисляемой коллекции, например, [*ObservableCollection&lt;TItem&gt;*](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8) и обработчик читает зависимые вычисления Вы можете использовать *ObservableCollectionExtended&lt;TItem&gt;* вместо Вашей коллекции. Этот класс наследует [*ObservableCollection&lt;TItem&gt;*](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8) и содержит события *PreCollectionChanged* и *PostCollectionChanged* . Также Вы можете оспользовать метод расширения *Extending*. Этот метод создаёт *ObservableCollectionExtended&lt;TItem&gt;* из [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8).

## Потокобезопасность
[*CollectionComputing&lt;TSourceItem&gt;*](#full-list-of-methods-and-classes) и [*ScalarComputing&lt;TSourceItem&gt;*](#full-list-of-methods-and-classes)

* поддерживают несколько читателей одновременно, пока коллекция не изменяется. 
* не подерживают несколько одновренных писателей. 
* изменяются когда обрабатывают события [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) объектов источников.

## Отслеживание значений возвращаемых методом
До сих пор мы видели как ObservableComputations отслеживает изменения в значениях свойств и в коллекциях через события [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) and [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8). ObservableComputations вводит новый интерфейс и событие для отслеживание значений возвращаемых методами: интерфейс *INotifyMethodChanged* и событие *MethodChanged*. Вот пример:

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
Как вы видите *NotifyMethodChangedEventArgs* сожержит свойство *ArgumentsPredicate*. Following value is passed to that property:
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
Данное свойство определяет какие значения должны иметь аргументы в вызове метода, так чтобы возвращаемое значение этого вызова изменилось.

Внимание: Пример кода в этом разделе не является образцом проектирования, это скорее антипаттерн: он содержит дублирование кода и изменения в свойствах класса RoomReservation не отслеживаются. Этот код приведён только для демонстрации отслеживание значений возвращаемых методом. См. исправленный код  [здесь](#use-readonly-structures-to-expose-incapsulated-private-members).

## Советы по улучшению производительности
### Избегайте вложенных параметрозависимых вычислений на больших данных
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
В коде выше *selectedOrderTypes.ContainsComputing(() => o.Type)* является вложенным вычислением, которое завивсит от внешненго парметра *o*. Эти два обстоятельства приводят к тому, что экземпляр класса *ContainsComputing* будет создан для каждого заказа в коллекции *orders* . Это может повлиять на производительность и потребление памяти, если количество заказов велико. К счастью, вычисление *filteredByTypeOrders* может быть сделано "плоским":

```csharp
ObservableCollection<Order> filteredByTypeOrders =  orders
    .Joining(selectedOrderTypes, (o, ot) => o.Type == ot)
    .Selecting(oot => oot.OuterItem);
```

Это вычисление имеет преимущество в производительности и потреблении памяти. 

### Кэшируйте значения свойств (методов)
Предположим мы имеем долго вычисляемой свойство и хотим увеличить производительность при получении его значения:

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

Код выше имеет следующий вывод:

> Direct access to property: 2155<br>
Access to property via computing: 626

### метод расширения Differing&lt;TResult&gt;
Этот метод позволяет Вам подавить лишние вызовы события [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) (когда значение свойства не изменилось). 

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

Код выше имеет следующий вывод:
> sinComputing: 0 <br>
differingSinComputing: 0 <br>
sinComputing: 0,5 <br>
differingSinComputing: 0,5 <br>
sinComputing: 0,5 <br>
sinComputing: 0,5 <br>
sinComputing: -0,5 <br>
differingSinComputing: -0,5 <br>

Иногда обработка каждого события [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) занимает много времени и может подвесить пользовательский интерфейс (перерисовка, перевычисление). Используйте метод расшиения Differing, чтобы уменьшить этот эффект.

## Советы по пректированию

### Ленивая инициализация вычмслений
Если некоторое вычмсоение необходимо только для некоторых сценариев, либо Вы ходите отложить инициализацию до того момента когда вычисление потребуется, Вам подходит ленивая инициализация. Вот пример:  
  
```csharp
private Computing<string> _valueComputing;
public Computing<string> ValueComputing => _valueComputing = 
   _valueComputing ?? new Computing<string>(() => Value);
```

### Используйте публичные структуры предназначенные только для чтения вместо приватных членов
Пример кода приведённый в разделе ["Tracking сhanges in a method return value" section ](#tracking-changes-in-a-method-return-value) не является образцом проектирования, это скорее антипаттерн: он содержит дублирование кода и изменения в свойствах класса RoomReservation не отслеживаются. Этот код приведён только для демонстрации отслеживание значений возвращаемых методом. Вот код с исправленным дизайном:

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

Обратите внимание на то, что тип *RoomReservationManager._roomReservations* изменён на *ObservableCollection&lt;RoomReservation&gt;* и было добавлено свойство *RoomReservationManager.RoomReservations* типа *System.Collections.ObjectModel.ReadOnlyObservableCollectionn&lt;RoomReservation&gt;*.

### Укоротите Ваш код
См. [здесь](#passing-argument-as-observable) and [здесь](passing-source-collection-argument-as-obserable).

### Не создавайте лишних переменных
См. [здесь](variable-declaration-in-a-computations-chain)

## Области применения метода расширения Using&lt;TResult&gt;
### Чистые выражения
См. конец раздела [Arbitrary expression observing](#arbitrary-expression-observing).

### Определение переменных в цепочке вычислений
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
Обратите внимание на свойство *PriceWithDiscount*. В теле этого свойства мы конструируем вычисление *_priceWithDiscount* в два шага. Можем ли мы переписать свойство *PriceWithDiscount*, чтобы оно стало [expression body](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members) членом?  Да:

```csharp
public Computing<decimal> PriceWithDiscount => _priceWithDiscount = _priceWithDiscount ?? 
   Lines.Selecting(l => l.Price).Summarizing().Using(p => p.Value - p.Value * Discount);
```

В коде выше *p* это результат *Lines.Selecting(l => l.Price).Summarizing()*. Поэтому параметр *p* похож на переменную. 
Следующий код не корректен так как изменения в свойстве *OrderLine.Price* и коллекцие *Order.Lines* не отражаются в результирующем вычислении:

```csharp
public Computing<decimal> PriceWithDiscount => _priceWithDiscount = _priceWithDiscount ?? 
   Lines.Selecting(l => l.Price).Summarizing().Value.Using(p => p - p * Discount);
```
В этом коде параметр *p* имеет тип decimal, а не *Summarizing&lt;decimal&gt;* как в корректном варианте. См. подробности [здесь](sdfsd).

## Отслеживание предудущего значения IReadScalar&lt;TValue&gt;
*IReadScalar&lt;TValue&gt;* упоминается в первый раз [здесь](#full-list-of-methods-and-classes).
Не существует встроеныз средств для получение предыдущего значения свойства во время обработки [PropertyChanged event](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8). ObservableComputations приходит на помощь и предоставляет методы расширения *PreviousTracking&lt;TResult&gt;* и *WeakPreviousTracking&lt;TResult&gt;*.

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

Код выше имеет следующий вывод:
> Current dispatch center: B; Previous dispatch center: A;  <br>
Current dispatch center: C; Previous dispatch center: B;  <br>

Обратите внимание на то, что свойство *PreviousValue* можно отслеживать через событие [PropertyChanged event](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8), поэтому Вы можете включить его в Ваши обозреваемые вычисления.

Обратите внимание на то, что *PreviousTracking&lt;TResult&gt;* имеет сильную ссылку на значение *TResult* (свойство *PreviousValue*) (в случае если *TResult* является ссылочным типом). Учтите это когда будете думать о сборке мусора и утечках памяти. *WeakPreviousTracking&lt;TResult&gt;* может помоч Вам. Вместо свойства *PreviousValue* *WeakPreviousTracking&lt;TResult&gt;* включает в себя метод *TryGetPreviousValue*. Изменения в возвращаемом значении этого метода не могут отслеживаться, поэтому Вы не можете включить его в свои обозреваеваемые вычисления.

## Доступ к свойствам через рефлексию
Следующий код юудет работать некоректно:
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
Код выше не имеет вывода, так как изменения в значения возвращаемого методом *GetValue* не могут быть отслежены. Вот исправленный код:

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

В коде выше мы используем метод расширения *PropertyAccessing*. Убедитесь, что Вы ознакомились с [Passing arguments as non-observales and observables](#passing-arguments-as-non-observales-and-observables): в коде выше первый аргумент (*order*) в методе расширения *PropertyAccessing* передан как **не обозреваемые**. В следующем коде этот аргумент передаётся как **observable**.
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
Следующий код не будет работать корректно, так как исзменения в *manager.ProcessingOrder* не будут отражаться в *priceReflectedComputing*, так как первый аргумент (*manager.ProcessingOrder*)  в методе расширения *PropertyAccessing* передан как **не обозреваемый**:
```csharp
PropertyAccessing<decimal> priceReflectedComputing 
   = manager.ProcessingOrder.PropertyAccessing<decimal>(nameof(Order.Price));
```

Если ссылка на объект, у которого вычисляется значение свойства, является null, то *PropertyAccessing&lt;TResult&gt;.Value* вызвращает значение по умолчанию для *TResult*. Вы можете изменить это значение передавая параметр *defaultValue*.

## Binding
  
Класс и метод расширения Binding позволяет связать два произвольных выражения. Первое выражение это источник. Второе выражение является целевым. Когда значение выражения источника меняется, новое значение присваивается целевому выражению:  
  
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

В коде выше мы связываем *order.DeliveryAddress* и *assignedDeliveryCar.DestinationAddress*. *order.DeliveryAddress* явяляется источником связывания. *assignedDeliveryCar.DestinationAddress* явялется целью связывания.

Метод расширения *Binding* расширяет *IReadScalar&lt;TValue&gt;*, экземпляр которого является источником связывания. 

Для того чтобы избежать выгрузки из памяти экземпляра класса *Binding* сборщиком мусора, сохраните ссылку на него в объекте, который имеет подходящее время жизни.

## Могу ли я использовать [IList&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=netframework-4.8) с ObservableComputations?
Если у Вас есть коллекция реализующая [IList&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=netframework-4.8), но не реализующая [INotifyColectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) (на пример [List&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=netframework-4.8)), Вы можете использовать её с ObservableComputations. См. 

https://github.com/gsonnenf/Gstc.Collections.ObservableLists

Nuget: https://www.nuget.org/packages/Gstc.Collections.ObservableLists
