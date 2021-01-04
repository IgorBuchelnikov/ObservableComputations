# ObservableComputations

## Что нужно знать для чтения этой статьи?

Для того чтобы понимать написанное здесь, Вы должны знать: базовые сведения о программировании и ООП, синтаксис C# (включая события, [методы расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), лямбда-выражения), [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/), интерфейсы: [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8).  
Желательно знать отличия [делегатов](https://docs.microsoft.com/en-us/dotnet/api/system.delegate?view=netframework-4.8) от [деревьев выражений](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/).

Для того чтобы представить себе какие [преимущества можно получить при использовании ObservableComputations](#области-применения-и-преимущества), Вы должны знать: о [привязке данных (binding) в WPF](https://docs.microsoft.com/en-us/dotnet/desktop-wpf/data/data-binding-overview) (или в другой UI платформе: [Xamarin](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/data-binding/basic-bindings), [Blazor](https://demos.telerik.com/blazor-ui/grid/observable-data)), особенно её связь с интерфейсами [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8), свойство [DbSet.Local](https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbset.local?view=entity-framework-6.2.0) ([local data](https://docs.microsoft.com/en-us/ef/ef6/querying/local-data)) из Entity framework, [асинхронные запросы Entity framewok](https://www.entityframeworktutorial.net/entityframework6/async-query-and-save.aspx).

## Что такое ObservableComputations?

Это кросс-платформенная .NET библиотека для вычислений, аргументами и результатами которых являются объекты реализующие интерфейсы [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) ([ObservableCollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8)). Вычисления включают в себя те же вычисления которые есть в [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/), вычисление произвольного выражения и некоторые дополнительные возможности. Вычисления реализованы как [методы расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), подобно [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) методам. Вы можете комбинировать вызовы [методов расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservavleComputations (цепочка вызовов и вложенные вызовы), как Вы это делаете в [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/). Поддерживаются вычисления в фоновых потоках, в том числе параллельные, а также обработка событий [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) связанная со временем. ObservableComputations это простая в использовании и мощная реализация [парадигмы реактивного программирования](https://en.wikipedia.org/wiki/Reactive_programming). С ObservableComputations, Ваш код будет более соответствовать функциональному стилю, чем при использовании стандартного [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).

## Аналоги

Ближайщими аналогами ObservableComputations являются следующие библиотеки: [Obtics](https://archive.codeplex.com/?p=obtics), [OLinq](https://github.com/wasabii/OLinq), [NFM.Expressions](https://github.com/NMFCode/NMF), [BindableLinq](https://github.com/svn2github/bindablelinq), [ContinuousLinq](https://github.com/ismell/Continuous-LINQ).

### [Reactive Extensions](https://github.com/dotnet/reactive)
ObservableComputations не является аналогом [Reactive Extensions](https://github.com/dotnet/reactive). Вот главные отличия ObservableComputations от  [Reactive Extensions](https://github.com/dotnet/reactive):

* [Reactive Extensions](https://github.com/dotnet/reactive) абстрагирован от конкретного события и от семантики событий: это библиотека для обработки всех возможных событий. Reactive Extensions обрабатывает все события одинаковым образом, а вся специфика только в пользовательском коде. ObservableComputations сфокусирован только на двух событиях:  [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) и приносит большую пользу обрабатывая их. 
* Библиотека [Reactive Extensions](https://github.com/dotnet/reactive) предоставляет поток событий. ObservableComputations предоставляет не только поток событий изменения данных, но вычисленные в данный момент данные. 

Часть задач, которые Вы решали с помощью [Reactive Extensions](https://github.com/dotnet/reactive), теперь проще и эффективней решить с помощью ObservableComputations. Вы можете использовать ObservableComputations отдельно или вместе с [Reactive Extensions](https://github.com/dotnet/reactive). ObservableComputations не заменит [Reactive Extensions](https://github.com/dotnet/reactive):

* при обработке событий связанной со временем (Throttle, Buffer). ObservableComputation позволяет реализовать связанную со временем обработку событий [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) путем взаимодествия с [Reactive Extensions](https://github.com/dotnet/reactive) (смотрите пример [здесь](#варианты-реализации-интерфейса-iocDispatcher-и-других-аналогичных-интерфейсов));
* при обработке событий не связанных с данными (например, нажатие клавиш), особенно при необходимости комбинировать эти события. Пример взаимодействия ObservableComputations с оператороми комбинирования [Reactive Extensions](https://github.com/dotnet/reactive) смотрите [здесь](#исключение-при-нарушении-целостности);
* при работе с асихронными операциями ([метод Observable.FromAsyncPattern](https://docs.microsoft.com/en-us/previous-versions/dotnet/reactive-extensions/hh229052(v%3Dvs.103))).

### [ReactiveUI](https://github.com/reactiveui/ReactiveUI) и [DynamicData](https://github.com/reactiveui/DynamicData)

Библиотека [ReactiveUI](https://github.com/reactiveui/ReactiveUI) (и её подбиблиотека [DynamicData](https://github.com/reactiveui/DynamicData)) не абстрагированы от интерфейсов [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) и при работе с этими интерфейсами позволяет делать примерно тоже самое что и ObservableComputations, но ObservableComputations менее многословна, проще в использовании, более декларативна, меньше дергает исходные данные. Почему?

* Реактивность ObservableComputations основана только на двух событиях: [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8). Такая реактивность является "родной" для ObservableComputations. Реактивность [ReactiveUI](https://github.com/reactiveui/ReactiveUI) основана на интерфейсах унаследованных от [Reactive Extensions](https://github.com/dotnet/reactive): [IObserver&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iobserver-1?view=netframework-4.8), [IObservable&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iobservable-1?view=netframework-4.8,), а также дополнительных интерфейсах для работы с коллекциями (содержащиеся в [DynamicData](https://github.com/reactiveui/DynamicData)): [IChangeSet](https://github.com/reactiveui/DynamicData/blob/master/src/DynamicData/IChangeSet.cs) и [IChangeSet&lt;TObject&gt;](https://github.com/reactiveui/DynamicData/blob/master/src/DynamicData/List/IChangeSet.cs). [ReactiveUI](https://github.com/reactiveui/ReactiveUI) осуществляет двунаправленное преобразование между этими интерфейсами и интерфейсами [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8). Даже с учётом этого преобразования интерфейсы [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) выглядят чужеродными для [ReactiveUI](https://github.com/reactiveui/ReactiveUI). (Отступление: Если Вам нужны функции [ReactiveUI](https://github.com/reactiveui/ReactiveUI), которые отсутсвуют в ObservableComputations, Вы можете получить их используя это преобразовние. Также Вы можете использовать [ReactiveList](https://reactiveui.net/docs/handbook/obsolete/collections/reactive-list) как коллекцию-источник для ObservableComputations.
* ObservableComputations не требует уникальности коллекций-источников и наличия в них всойства Id. Вместо этого ObservableComputations сохраниет порядок элементов коллекции источника в вычисленной коллекции. 
* ObservableComputations больше похожа на обычный [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).
* Интерфейсы [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) и [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) тесно интегрированы в UI платформы от Microsoft ([WPF](https://docs.microsoft.com/en-us/dotnet/desktop-wpf/data/data-binding-overview), [Xamarin](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/data-binding/basic-bindings), [Blazor](https://demos.telerik.com/blazor-ui/grid/observable-data)).

 Вы можете сравнить эти библиотеки и ObservableComputations в действии, см. [ObservableComputations.Samples](https://github.com/IgorBuchelnikov/ObservableComputations.Samples).

## Покрытие Unit-тестами

Код покрыт unit-тестами на ?% по данным JetBrains DotCover. Выполенение всех тестов занимает ? часов.

## Как установить?

Библиотека ObservableComputations доступна на [NuGet](https://www.nuget.org/packages/ObservableComputations/).

## Как получить помощь?

Вы можете [создать issue](https://github.com/IgorBuchelnikov/ObservableComputations/issues/new) or [связаться со мной по электронной почте](mailto:igor_buchelnikov_github@mail.ru).

## Как я могу помочь проекту?

Приветствуются комментарии и замечания к документации. Нужны демо проекты, посты в блогах и руководства. Нужна красивая иконка.

##  Быстрый старт
### Пример приложения 
* [Samples](https://github.com/IgorBuchelnikov/ObservableComputations.Samples)
* [Dynamic Trader](https://github.com/IgorBuchelnikov/Dynamic.Trader)

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

			// We start using ObservableComputations here!
			OcConsumer consumer = new OcConsumer();

			Filtering<Order> expensiveOrders = 
				orders
					.Filtering(o => o.Price > 25)
					.For(consumer); 
			
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

			consumer.Dispose();
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

В коде выше, во время выполнения [метода расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *For*, проиходит подписка на следующие события: событие [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) коллекции *orders* и событие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) каждго экземпляра класса *Order*. Во время выполнения метода *consumer.Dispose()* происходит отписка от событий.

Сложность выражения предиката переданного в [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Filtering* (*o => o.Price > 25*) не ограничена. Выражение может включать в себя результаты вызовов методов ObservavleComputations, включая аналоги [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).

### Отслеживание произвольного выражения
```csharp
using System;
using System.ComponentModel;
using System.Diagnostics;
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
			OcConsumer consumer = new OcConsumer();

			Computing<decimal> discountedPriceComputing = new Computing<decimal>(
				() => order.Price - order.Price * order.Discount / 100).For(consumer);
				
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

			consumer.Dispose();
		}

		static void printDiscountedPrice(Computing<decimal> discountedPriceComputing)
		{
			Console.WriteLine($"Discounted price is ₽{discountedPriceComputing.Value}");
		}
	}
}
```
В этом примере кода мы следим за значением выражения цены со скидкой. Класс *Computing&lt;TResult&gt;* реализует интерфейс [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8). Сложность отслеживаемого выражения не ограничена. Выражение может включать в себя результаты вызовов методов ObservavleComputations, включая аналоги [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/).

Так же как в предыдущем примере во время выполнения [метода расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *For* происходит подписка на событие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) экземпляра класса *Order*. Во время выполнения метода *consumer.Dispose()* происходит отписка от событий.

Если Вы хотите чтобы выражение *() => order.Price - order.Price * order.Discount / 100*  было чистой функцией, нет проблем:  

```csharp
Expression<Func<Order, decimal>> discountedPriceExpression = 
	o => o.Price - o.Price * o.Discount / 100;
	
// Здесь мы начинаем использовать ObservableComputations!
Computing<decimal> discountedPriceComputing = 
	order.Using(discountedPriceExpression).For(consumer);
```
Теперь выражение *discountedPriceExpression* может быть использовано для других экземпляров класса *Order*.


## Области применения и преимущества

### Привязка к элементам пользовательского интерфейса (binding)
WPF, Xamarin, Blazor. Вы можете привязывать (binding) элементы пользовательского интерфейса (controls) к экземплярам классов ObservableComputations (*Filtering*, *Computing* etc.). Если Вы так делаете, Вам не нужно беспокоиться о том, что Вы забыли вызвать событие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8) для вычисляемых свойств или вручную обработать изменение в какой-либо коллекции. С ObservableComputations Вы определяете как значение должно вычисляться, всё остальное ObservableComputations сделает за Вас. 

### Асинхронное программирование
Такой подход облегчает **асинхронное программирование**. Вы можете показать пользователю форму и начать загружать исходные данные (из БД или web-сервиса) в фоне. По мере того как исходные данные загружаются, форма наполняется вычисленными данными. Пользователь увидит форму быстрее (пока исходные данные загружаются в фоне, Вы можете начать рендеринг). Если форма уже показана пользователю, Вы можете обновить исходные данные в фоне, вычисляемые данные отображенные на форме обновятся благодаря ObservableComputations. Так же ObservableComputations включают специальные средства для многопочных вычислений. Подробности см. [здесь](#многопоточность).

### Повышенная производительность
Если у Вас есть сложные вычисления, часто меняющиеся исходные данные и\или данных много, вы можете получить выигрыш в производительности с ObservableComputations, так как Вам не надо перевычислять данные с нуля каждый раз когда меняются исходные данные. Каждое маленькое изменение в исходных данных вызывает маленькое изменение в данных вычисленных средствами ObservableComputations.
Производительность пользовательского интерфейса возрастает, так как необходимость в ререндеренге уменьшается (только изменённые данные рендерятся) и данные из внешних источников (DB, web-сервис) загружаются в фоне (см. [предыдущий раздел](#асинхронное-программирование)).

### Чистый и надежный код
* Меньше шаблонного императивного кода. Больше чистого декларативного (в функциональном стиле) кода. Общий объём кода уменьшается.
* Меньшая вероятность ошибки программиста: вычисляемые данные показанные пользователю пользователю будут всегда соответствовать пользовательскому вводу и данным загруженным из внешних источников (DB, web-сервис).
* Код загрузки исходных данные и код для вычисления данных отображаемых в пользовательском интерфейсе могут быть чётко разделены.
* Вы можете не беспокоиться о том, что забыли обновить вычисляемые данные. Все вычисляемые данные будут обновляться автоматически.

### Дружелюбный пользовательский интерфейс
ObservableComputations облегчают создание дружелюбного пользовательского интерфейса.
* Пользователю не нужно вручную обновлять вычисляемые данные.
* Пользователь видит вычисляемые данные всегда, а не только по запросу.
* Вам не нужно обновлять вычисляемые данные по таймеру.
* Не нужно блокировать пользовательский интерфейс во время вычисления и отображения большого объема данных (показывая при этом индикатор загрузки). Данные могут обновляться небольшими порциями, при этом пользователь может продолжать работать.

## Полный список операторов
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

<table cellspacing="0" border="0">
	<colgroup width="189"></colgroup>
	<colgroup width="147"></colgroup>
	<colgroup width="144"></colgroup>
	<colgroup width="395"></colgroup>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=4 height="45" align="center" valign=bottom><b><font size=4 color="#000000">Аналоги MS LINQ</font></b></td>
		</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Группа перегруженных <br>методов ObservableComputations</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Группа перегруженных<br>методов MS LINQ</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Возвращаемые объект<br>является</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=middle><b><font face="Segoe UI" color="#24292E">Примечание</font></b></td>
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
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">AllComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">All</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">AnyComputing</font></td>
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
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="48" align="left" valign=bottom><font color="#000000">Concatenating</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Concat</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Элементами коллекции-источника могут быть <br>INotifyCollectionChanged <br>или IReadScalar&lt;INotifyCollectionChanged&gt;</font></td>
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
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="92" align="left" valign=bottom><font color="#000000">ItemComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ElementAtOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если запрошенный индекс <br>выходит за границы коллекции-источника<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value <br>возвращает значение по умолчанию <br>для типа TSourceItem или значение <br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Excepting</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Except</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="78" align="left" valign=bottom><font color="#000000">FirstComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">FirstOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Grouping</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Group</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Может содержать группу с ключём null</font></td>
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
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">IndicesComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=middle><font color="#000000">IndexOf</font></td>
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
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Joining</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Join</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000"><br></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="78" align="left" valign=bottom><font color="#000000">LastComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">LastOrDefault</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="78" align="left" valign=bottom><font color="#000000">Maximazing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Max</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="78" align="left" valign=bottom><font color="#000000">Minimazing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Min</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">Если размер коллекции-источника нулевой<br>свойство ScalarComputing&lt;TSourceItem&gt;.Value<br>возвращает значение по умолчанию<br>для типа TSourceItem или значение<br>переданное в параметр defaultValue</font></td>
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
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=4 height="59" align="center" valign=bottom><b><font size=4 color="#000000">Другие функции</font></b></td>
		</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="62" align="center" valign=top><b><font face="Segoe UI" color="#24292E">Группа перегруженных <br>методов ObservableComputations</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="center" valign=middle><b><font face="Segoe UI" color="#24292E">Возвращаемые объект является</font></b></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="center" valign=middle><b><font face="Segoe UI" color="#24292E">Примечание</font></b></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Binding class</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000"><br></font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#binding">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">CollectionDispatching</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше<a href="#многопоточность"> здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">CollectionDisposing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше<a href="#disposing"> здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">CollectionPausing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#временная-остановка-и-возобновление-вычислений">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">CollectionProcessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#обработка-измениний-в-observablecollectiont">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Computing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#отслеживание-произвольного-выражения">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Differing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#метод-расширения-differingtresult">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="48" align="left" valign=bottom><font color="#000000">Paging</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=bottom><font color="#000000">CollectionComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">содержит подмножество элементов коллекции<br>соответствующее странице<br>с определённым номером и размером</font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PreviousTracking</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#отслеживание-предыдущего-значения-ireadscalartvalue">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PropertyAccessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#доступ-к-свойствам-через-рефлексию">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">PropertyDispatching</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#многопоточность">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ScalarDispatching</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#многопоточность">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ScalarDisposing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше<a href="#disposing"> здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ScalarPausing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=bottom><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#временная-остановка-и-возобновление-вычислений">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">ScalarProcessing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#обработка-изменений-в-ireadscalartvalue">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">Using</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#отслеживание-произвольного-выражения">здесь</a> и <a href="#области-применения-метода-расширения-usingtresult">здесь</a></font></td>
	</tr>
	<tr>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" height="18" align="left" valign=bottom><font color="#000000">WeakPreviousTracking</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" colspan=2 align="left" valign=middle><font color="#000000">ScalarComputing</font></td>
		<td style="border-top: 1px solid #000000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" align="left" valign=bottom><font color="#000000">см. больше <a href="#отслеживание-предыдущего-значения-ireadScalartvalue">здесь</a></font></td>
	</tr>
</table>


Для всех вычислений имеющих параметры типа [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8): null значение параметра обрабатывается как пустая коллекция.

Для всех вычислений имеющих параметры типа *IReadScalar*&lt;[INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8)&gt;: null значение свойства *IReadScalar*&lt;[INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8)&gt;.*Value* обрабатывается как пустая коллекция.



## Два состояния вычисления: активное и неактивное

Для того чтобы вычисление обрабатывало изменения в своих источниках, оно должно быть подписано на события PropertyChanged и CollectionChanged своих источников. В этом случае вычисление находится в активном состояние (IsActive == true). При подписке на событие возникает ссылка от источника события (источника вычисления) к делегату обработчика события. Сам делегат в свою очередь ссылается на объект в контексте которого он выполняется (вычисление). Таким образом в активном состоянии источники вычисления ссылаются на вычисление. Вычисление также ссылается на источники.  Это означается, при сборке мусора в активном состоянии вычисление может выгрузиться из памяти только вместе со своими источниками. Иными словами в активном состоянии вычислении может выгрузиться из памяти только если нет ссылок ни на вычисление, ни на источники. Иногда возникает ситуации, когда источники нужны (на них есть ссылки), а вычисление уже не нужно и его необходимо выгрузить из памяти. Это возможно только, если вычисление отпишется от событий PropertyChanged и CollectionChanged своих источников. В этом случае вычисление находится в неактивном состоянии. В неактивном состоянии вычисления-коллекции пусты, а вычисления-скаляры имеют значение по умолчанию.

ObservableComputations имеет API для управления активностью вычислений. Основная идея этого состоит в том, что когда вычисление кому-то нужно, оно активно. Если вычисление никому не нужно, оно становится неактивным. Объектами, которые могут наждаться в вычислениях, являются экземпляры класса *OcConsumer*:

```c#
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
			OcConsumer consumer = new OcConsumer();

			Selecting<Order, decimal> highPrices = 
				orders
					.Filtering(o => o.Price > 25)
                	.Selecting(o => o.Price);
            
            // Computations is not active
            Debug.Assert(!highPrices.IsActive);
            Debug.Assert(!((Filtering<Order>)highPrices.Source).IsActive);
            
            check(orders, highPrices); // Prints "False"
                  
            // Now we make computations active
			highPrices.For(consumer); // Selecting and Filtering computations is needed for consumer
            
            // Computations is active
            Debug.Assert(highPrices.IsActive);
            Debug.Assert(((Filtering<Order>)highPrices.Source).IsActive);
            
            check(orders, highPrices); // Prints "True"            
			
			Debug.Assert(highPrices is ObservableCollection<decimal>);
			
			check(orders, highPrices); // Prints "True"

			highPrices.CollectionChanged += (sender, eventArgs) =>
			{
				// see the changes (add, remove, replace, move, reset) here			
				check(orders, highPrices); // Prints "True"
			};

			// Start the changing...
			orders.Add(new Order{Num = 8, Price = 30});
			orders.Add(new Order{Num = 9, Price = 10});
			orders[0].Price = 60;
			orders[4].Price = 10;
			orders.Move(5, 1);
			orders[1] = new Order{Num = 10, Price = 17};

			check(orders, highPrices); // Prints "True"
            
			consumer.Dispose(); // the consumer no longer needs its computations
            
            check(orders, highPrices); // Prints "False"
            
            // Computations is not active
            Debug.Assert(!highPrices.IsActive);
            Debug.Assert(!((Filtering<Order>)highPrices.Source).IsActive);            
            
 			Console.ReadLine();           
		}

		static void check(
			ObservableCollection<Order> orders, 
			Selecting<Order, decimal> expensiveOrders)
		{
			Console.WriteLine(expensiveOrders.SequenceEqual(
				orders.Where(o => o.Price > 25).Select(o => o.Price)));
		}
	}
}
```

Обратите внимание на вызов метода расширения *For*. Этот метод расширения можно вызвать для всех экземпляров вычислений. Если источником вычисления является другое вычисление, оно также становится необходимым для потребителя. 

Класс *OcConsumer* реализует интерфейс [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=net-5.0). При вызове *consumer.Dispose()* *consumer* отказывается от всех своих вычислений. 
Один экземпляр *OcConsumer* может нуждаться в нескольких вычисления. Вычисление может быть необходимым для нескольких  экземпляров *OcConsumer*. Только когда от вычисления откажутся все экземпляры *OcConsumer* оно становится неактивным. 
Сказанное выше можно проиллюстрировать диаграммой состояний:
 ![Ж](https://raw.githubusercontent.com/IgorBuchelnikov/ObservableComputations/master/doc/%D0%96%D0%B8%D0%B7%D0%BD%D0%B5%D0%BD%D0%BD%D1%8B%D0%B9%20%D1%86%D0%B8%D0%BA%D0%BB%20%D0%BE%D0%B1%D0%BE%D0%B7%D1%80%D0%B5%D0%B2%D0%B0%D0%B5%D0%BC%D0%BE%D0%B3%D0%BE%20%D0%B2%D1%8B%D1%87%D0%B8%D1%81%D0%BB%D0%B5%D0%BD%D0%B8%D1%8F.png)

## Передача аргументов как обозреваемых и не обозреваемых

Аргументы [методов расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservableComputations могут быть переданы двумя путями: как обозреваемые и как не обозреваемые.

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
			hockeyTeam.RemoveAt(0);		   // 🙂
			hockeyTeam.Add(allPersons[0]);	// 🙂
			loginManager.LoggedInPerson = allPersons[4];  // 🙁!
			
			Console.ReadLine();
		}
	}
}
```
В приведенном коде мы вычисляем является ли залогиненный пользователь хоккейным игроком. Выражение "*loginManager.LoggedInPerson*" переданное в метод *ContainsComputing*  вычисляется (оценивается) алгоритмами ObservableComputations только один раз: когда класс *ContainsComputing&lt;Person&gt;* инстанцируется (когда вызывается [метод расшерения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *ContainsComputing*). Если свойство *LoggedInPerson* меняется, это изменение **не** отражается в *isLoggedInPersonHockeyPlayer*. 

Конечно, Вы можете использовать более сложное выражение, чем "*loginManager.LoggedInPerson* для передачи как аргумента в любой [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservableComputations. Как видите передача аргумента типа T как не обозреваемого это обычная передача аргумента типа T.

### Передача аргументов как обозреваемых

В предыдущем примере, мы предполагали, что наше приложение не поддерживает выход пользователя (logout) (и последующий вход (login)). Другими словами приложение не обрабатывает изменения свойства *LoginManager.LoggedInPerson*. Давайте добавим функциональность logout в наше приложение:  
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
			hockeyTeam.RemoveAt(0);		   // 🙂
			hockeyTeam.Add(allPersons[0]);	// 🙂
			loginManager.LoggedInPerson = allPersons[4];  // 🙂!!!

			Console.ReadLine();
		}
	}
}
```

В коде выше мы передаём аргумент в [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *ContainsComputing* как *IReadScalar&lt;Person&gt;* (а не как *Person* как в предыдущем разделе). *Computing&lt;Person&gt;* реализует *IReadScalar&lt;Person&gt;*. *IReadScalar&lt;TValue&gt;* первоначально был упомянут в разделе ["Полный список операторов"](#полный-список-операторов). Как видите, если Вы хотите передать аргумент типа T как обозреваемый, Вы должны выполнить обычную передачу аргумента типа *IReadScalar&lt;T&gt;*. В этом случае используется другая перегруженная вервия метода *ContainsComputing*, в отличии от версии, которая использовалась в  [предыдущем разделе](#передача-аргументов-как-не-обозреваемых). Это даёт нам возможность следить за изменениями свойства *LoginManager.LoggedInPerson*. Теперь изменения свойства *LoginManager.LoggedInPerson* отражаются в *isLoggedInPersonHockeyPlayer*. Обратите внимание на то, что теперь класс *LoginManager* реализует [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8).  

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

### Передача коллекции источника как обозреваемого аргумента
Как Вы видите все вызовы [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/) подобных [методов расширения ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) ObservableComputations в общем виде могут быть представлены как
```csharp
sourceCollection.ExtensionMethodName(arg1, arg2, ...);
```
*sourceCollection* это первый аргумент в объявлении [метода расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). Поэтому подобно другим аргументам он тоже может быть передан как  [не обозреваемый](#передача-аргументов-как-не-обозреваемых) и как [обозреваемый](#передача-аргументов-как-обозреваемых). До сих пор мы передавали коллекцию источник как не обозреваемый аргумент (это было простое выражение состоящее из одной переменной, конечно вы можете использовать более сложные выражения, суть остаётся та же). Теперь давайте попробуем передать коллекцию источник как обозреваемый аргумент:  

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
Мы продолжаем рассматривать пример из [предыдущего раздела](#передача-аргументов-как-обозреваемых). Мы использовали следующий код для того чтобы отследить изменения в *hockeyTeamManager.HockeyTeamInterested*:
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

В этом коде *"hockeyTeamManager.HockeyTeamInterested"* передан в [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *ContainsComputing* как [не обозреваемый](#передача-аргументов-как-не-обозреваемых), и не имеет значения, что  *"hockeyTeamManager.HockeyTeamInterested"* это часть выражения переданного в конструктор класса *Computing&lt;bool&gt;*, изменения в *"hockeyTeamManager.HockeyTeamInterested"* не будут отражаться в *isLoggedInPersonHockeyPlayer*. Правило обозреваемых и не обозреваемых аргументов применяется только в одном направлении: от вложенных (обёрнутых) к внешним (оборачивающим) вызовам. Другими словами, правило обозреваемых и не обозреваемых аргументов всегда справедливо, независимо от того является ли вычисление корневым или вложенным.

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
В коде выше мы создаём вычисление *"filteredByTypeOrders"* которое отражает изменения в коллекция *orders* и *selectedOrderTypes* и в свойстве *Order.Type*. Обратите внимание на аргумент переданный в *ContainsComputing*. Следующий код не будет отражать изменения в свойстве *Order.Type*:

```csharp
ObservableCollection<Order> filteredByTypeOrders =  orders.Filtering(o => 
   selectedOrderTypes.ContainsComputing(o.Type).Value);
```

## Обработчики запросов на изменение результатов вычислений
Единственный способ изменить результат вычисления это изменить исходные данные. Вот пример кода:

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

В коде выше мы создаём вычисление *stepansOrders* (заказы Степана). Мы устанавливаем делегат, в качестве значения свойства *stepansOrders.InsertItemRequestHandler* для того чтобы определить как изменить коллекцию *orders* и *order*, который нужно добавить, так чтобы он был включён в вычисление *stepansOrders*.

Обратите внимание на то, что [метод Add](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1.add?view=netframework-4.8#System_Collections_Generic_ICollection_1_Add__0_) это член интерфейса [ICollection&lt;T&gt; interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1?view=netframework-4.8).

Данная возможность может быть полезна если Вы передаёте *stepansOrders* в код, который абстрагирован от того, чем является *stepansOrders*: вычислением или обычной коллекцией. Этот код знает только то, что *stepansOrders* реализует [ICollection&lt;T&gt; interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1?view=netframework-4.8) и иногда хочет добавлять заказы в *stepansOrders*. Таким кодом, например, может [двунаправленная привязка данных в WPF ](https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.bindingmode?view=netframework-4.8#System_Windows_Data_BindingMode_TwoWay) или привязка к свойству ItemsSource у [DataGrid](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.datagrid?view=net-5.0).

Свойства аналогичные *InsertItemRequestHandler* существуют и для других операций (remove, set, move, clear). Все свойства имеют постфикс "*RequestHandler*".

## Пользовательская обработка изменений
### Обработка изменений в ObservableCollection&lt;T&gt;

Иногда возникает необходимость производить какие-либо действия 
* с добавляемыми в коллекцию элементами
* с удаляемыми из коллекции элементами
* элементами перемещаемыми внутри коллекции

Конечно вы можете обработать все текущие элементв колеекции, затем подписаться на событие [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8), но библеотека ObservableComputations сожержить более простое и эффективное средство.

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

Делегат переданный в параметр *newItemsProcessor* вызывается 

* при активации экземпляра класса *CollectionProcessing&lt;TSourceItem, TReturnValue&gt;* (если коллекция-источник (*onlineClients*) содержит элементы в момент активации), 
* при добавление элементов в коллекцию-источник, 
* при замене элемента в коллекции-источнике,
* при [reset](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.notifycollectionchangedaction?view=net-5.0) коллекции источника и она сожежит элемнты после [reset](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.notifycollectionchangedaction?view=net-5.0),
* в случае если [коллекция-источник передана как скаляр](#передача-коллекции-источника-как-обозреваемого-аргумента) (*IReadScalar&lt;TValue&gt;*), и у него меняется значение свойства *Value* на коллекцию, которая сожержит элементы.


Делегат переданный в параметр *oldItemProcessor* вызывается 

* при деактивации экземпляра класса *CollectionProcessing&lt;TSourceItem, TReturnValue&gt;*,
* при удалении элементов в коллекции-источнике,   
* при замене элемента в коллекции-источнике (установка элемента коллекции по индексу), 
* при [reset](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.notifycollectionchangedaction?view=net-5.0) коллекции источника ([метод Clear()](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.collection-1.clear?view=net-5.0)).
* в случае если [коллекция-источник передана как скаляр](#передача-коллекции-источника-как-обозреваемого-аргумента) (*IReadScalar&lt;TValue&gt;*), и у него меняется значение свойства *Value*.

Есть также возможность передать делегат *moveItemProcessor* для обработки события перемещения элемента в коллекции-источнике.

Существует также перегруженная версия метода *CollectionProcessing*, которая принимает делегат *newItemsProcessor*, возвращающий пустое значение (void).

### Обработка изменений в IReadScalar&lt;TValue&gt;

*IReadScalar&lt;TValue&gt;* упоминается в первый раз [здесь](#полный-список-операторов). Вы можете обрабатывать изменение значения свойства *Value*, подписавшись на событие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8), но по аналогии с обработкой изменений в ObservableCollection&lt;T&gt; ObservableComputations позволяет обрабатывать изменения в *IReadScalar&lt;TValue&gt;* проще и эффективнее:  

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
```

Существует также перегруженная версия метода *ScalarProcessing*, которая принимает делегат *newValueProcessor*, возвращающий не пустое значение. 

### Disposing

If items in your collection implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=net-5.0) you may need  to call [Dispose](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable.dispose?view=net-5.0) method for each item leaving the collection. You may use *CollectionProcessing* to achieve this as we did in the [previous section](#change-handling-in-observablecollectiont).  Another variant is to use *CollectionDisposing* method:

```c#
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
            
            OcConsumer consumer = new OcConsumer();

			Filtering<Client> onlineClients = clients.Filtering(c => c.Online);

			onlineClients
            .CollectionProcessing(
				(newClients, collectionProcessing) =>
				{
					NetworkChannel[] networkChannels = new NetworkChannel[newClients.Length];
					for (var index = 0; index < newClients.Length; index++)
					{
						Client newClient = newClients[index];
						NetworkChannel networkChannel = new NetworkChannel(newClient.Name);
						networkChannels[index] = networkChannel;
					}

					return networkChannels;
				})
            .CollectionDisposing()
            .For(consumer);
					
			clients[2].Online = true;
			clients.RemoveAt(1);

			Console.ReadLine();
            
            consumer.Dispose();
		}
	}
}
```

*ScalarDisposing* extension method allow you to dispose old values of *IReadScalar<TValue>*:

```c#
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

			OcConsumer consumer = new OcConsumer();

			networkChannelComputing.ScalarProcessing(
				(networkChannel1, scalarProcessing) =>
				{
					// networkChannel1 is new NetworkChannel
					networkChannel1.Open();
				})
            .ScalarDisposing()
			.For(consumer);

			client.NetworkChannel = new NetworkChannel(2);
			client.NetworkChannel = new NetworkChannel(3);

			Console.ReadLine();

			consumer.Dispose();
		}
	}
}
```

## Обработка накладывающихся изменений
Когда выполняется обработчик события PropetyChanged или CollectionChanged вычисления, это вычисление обрабатывает некоторое изменение источника и находится в несогласованном состоянии (IsConsistent == false). Все изменения источников, внесенные в это время (накладывающиеся изменения), будут отложены до тех пор, пока вычисление не завершит обработку исходного изменения источника.

Рассмотрим следующий код:

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

В коде у нас есть коллекция отношений: *relations*. Коллекция имеет избыточность: если коллекция имеет отношение  A к B типа "Родитель", она должна содержать соответствующее отношение: B к A типа "Ребёнок", и наоборот. Также мы имеем вычисляемую упорядоченную  коллекцию отношений *orderedRelations*. Наша задача поддержать целостность коллекции отношений: если кто-то меняет мы должны отреагировать и восстановить целостность. Представьте, что единственным способом сделать это является подписка на событие CollectionChanged коллекции *orderedRelations* (по каким-то причинам мы не можем подписаться на событие CollectionChanged коллекции *relations*). В коде выше мы предполагаем только один тип изменений: Add.

## Отладка

### Пользовательский код

К пользовательскому коды относятся:

* Селекторы это выражения, которые предаются в качестве аргумента в следующие [методы расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods): *Selecting*, *SelectingMany*, *Grouping*, *GroupJoining*, *Dictionaring*, *Hashing*, *Ordering*, *ThenOrdering*, *PredicateGroupJoining*

* Предикаты это выражения, которые предаются в качестве аргумента в [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Filtering*. 

* Функции агрегирования это делегаты, которые передаются в [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Aggregating*

* Произвольные выражения это выражения, которые предаются в качестве аргумента в [методы расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Computing* и *Using*. 

* Обработчики запросов на изменение вычислений описаны в разделе ["Изменение вычислений"](#изменение-вычислений).

* Обработчики результатов вычислений описаны в разделе ["Обработка изменений результатов вычислений"](#обработка-изменений-результатов-вычислений).

* Код вызванный с помощью методов [*OcDispatcher.Invoke* и *OcDispatcher.BeginInvoke*](#использование-класса-ocDispatcher).

Вот код иллюстрирующий отладку произвольного выражения (другие типы могут быть отлажены аналогичным образом):

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

Как Вы видите *exception.StackTrace* указывает на строку, которая вызвала исключение: *valueProvider.Value = new Random().Next(0, 1);*. Эта строка не указывает на вычисление, которое вызвало исключение: *computing1* or *computing2*. Чтобы определить исключение, которое вызвало исключение мы должны взглянуть на свойство *DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].InstantiatingStackTrace*. Это свойство содержит трассировку стека инстанцирования вычисления. 

По умолчанию ObservableComputations не сохранияет трассировки стека инстанцирования вычислений по соображениям производительности. Чтобы сохранять эти трассирвки стека используйте свойство *Configuration.SaveInstantiatingStackTrace*. 

По умолчанию ObservableComputations не следит за вычислениями выполняющими пользовательский код по соображениям производительности. Для того чтобы следить за вычислениями выполняющими пользовательский код используйте свойство *Configuration.TrackComputingsExecutingUserCode*. Если пользовательский код был вызван из пользовательского кода другого вычисления, то *DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].UserCodeIsCalledFrom* будет указывать на это вычисление.

Все необработанные исключения выброшенные в пользовательском коде фатальны, так как внутреннее состояние вычисление становится повреждённым. Обратите внимание на проверки на null.

### Пользовательский код в фоновых потоках
Работа с вычислениями в фоновых потоках описана [здесь](#многопоточность).

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
			Configuration.SaveOcDispatcherInvocationStackTrace = true;
			Configuration.TrackOcDispatcherInvocations = true;

			ValueProvider valueProvider = new ValueProvider(){Value = 2};
			
			OcDispatcher ocDispatcher = new OcDispatcher();

			System.AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				Console.WriteLine($"Exception stacktrace:\n{DebugInfo.ExecutingOcDispatcherInvocations[ocDispatcher.Thread].Peek().CallStackTrace}");
				Console.WriteLine($"\nComputing which caused the exception has been instantiated by the following stacktrace :\n{DebugInfo.ComputingsExecutingUserCode[Thread.CurrentThread].InstantiatingStackTrace}");
				Console.WriteLine($"\nDispatch computing which caused the exception has been instantiated by the following stacktrace :\n{((IComputing) DebugInfo.ExecutingOcDispatcherInvocations[ocDispatcher.Thread].Peek().Context).InstantiatingStackTrace}");

				Thread.CurrentThread.IsBackground = true;

				while (true)
					Thread.Sleep(TimeSpan.FromHours(1));
			};

			ScalarDispatching<int> valueProviderDispatching = valueProvider.ScalarDispatching(ocDispatcher);

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

Данный пример аналогичен предыдущему, за исключением
* Свойств, которые содержат информацю об исключении
* Установки параметров конфигурации *Configuration.SaveOcDispatcherInvocationStackTrace* и *Configuration.TrackOcDispatcherInvocations*

*DebugInfo.ExecutingOcDispatcherInvocations[ocDispatcher.ManagedThreadId]* имеет тип Stack&lt;Invocation&gt;. Стек будет содержать более одного элемента, если вы вызывали метод *OcDispatcher.DoOthers*.

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

Вместо *priceDiscounted.PropertyChanged* мы подписываемся на *priceDiscounted.PostValueChanged*. Это событие возникает после *PropertyChanged*, поэтому мы можем быть уверены: все зависимые вычисления обновили свои значения. *PostValueChanged* объявлено в *ScalarComputing&lt;TValue&gt;*. *Computing&lt;string&gt;* наследует *ScalarComputing&lt;TValue&gt;*. *ScalarComputing&lt;TValue&gt;* впервые упомянут [здесь](#полный-список-операторов). *ScalarComputing&lt;TValue&gt;* содержит событие *PreValueChanged*. Это событие позволяет посмотреть состояние вычислений до изменения. Если вы хотите обрабатывать событие изменения свойства Вашего объекта (не вычисления как в предыдущем примере) и обработчик читает зависимые вычисления (подобно предыдущему примеру) Вы должны определить своё событие и вызывать его.

*CollectionComputing&lt;TItem&gt;* содержит события *PreCollectionChanged* и *PostCollectionChanged*. *CollectionComputing&lt;TItem&gt;* впервые упомянут [здесь](#полный-список-операторов)). Если Вы хотите обрабатывать событие изменения Вашей коллекции, реализующей [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) (не вычисляемой коллекции, (например, [*ObservableCollection&lt;TItem&gt;*](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8)) и обработчик читает зависимые вычисления Вы можете использовать *ObservableCollectionExtended&lt;TItem&gt;* вместо Вашей коллекции. Этот класс наследует [*ObservableCollection&lt;TItem&gt;*](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8) и содержит события *PreCollectionChanged* и *PostCollectionChanged* . Также Вы можете использовать [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Extending*. Этот метод создаёт *ObservableCollectionExtended&lt;TItem&gt;* из [INotifyCollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8).

## Многопоточность
### Потокобезопасность
[*CollectionComputing&lt;TSourceItem&gt;*](#полный-список-операторов)) и [*ScalarComputing&lt;TSourceItem&gt;*](#полный-список-операторов))

* поддерживают несколько читающих потоков одновременно, если во время чтения не меняются пишущим потоком. Исключение: вычисление *ConcurrentDictionaring*, которое поддреживает одновременно несколько читающих потоков и один пишущий. 
* не поддерживают одновременные изменения несколькими пишущими потоками. 

Вычисления изменяются пишущим потоком когда обрабатывают события [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8) и [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) объектов-источников.

### Загрузка исходных данных в фоновом потоке
Код окна WPF приложения:  
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
					this.OcDispatcher.Invoke(() => Orders.Add(order), OcDispatcherPriority.Background);
				}

				this.OcDispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					OcDispatcherPriority.Background);
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
В этом примере мы показываем пользователю форму не дожидаясь пока закончится загрузка данных из БД. Пока идёт загрузка, форма рендерится и пользователь знакомится её содержимым. Заметьте, что код загрузки исходных данных абстрагирован от вычислений над ними (*PaidOrders* и *UnpaidOrders*).

### Выполнение вычислений в фоновом потоке
В предыдущем примере в фоновом потоке выполнялась только загрузка данных из БД. Сами вычисления (*PaidOrders* и *UnpaidOrders*) выполнялись в главном потоке (поток пользовательского интерфейса). Иногда необходимо выполнять вычисление в фоновом потоке, а в главном потоке получать только конечные результаты вычисления:  
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

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		// Диспетчер для выполнения вычислений в фоновом потоке
		ObservableComputations.OcDispatcher _ocOcDispatcher = new ObservableComputations.OcDispatcher();

		public MainWindow()
		{
			Orders = new ObservableCollection<Order>();

			WpfOcOcDispatcher wpfOcOcDispatcher = new WpfOcOcDispatcher(this.OcDispatcher);

			fillOrdersFromDb();

			PaidOrders = 
				Orders.CollectionDispatching(_ocOcDispatcher) // отправляем вычисление в фоновый поток
				.Filtering(o => o.Paid)
				.CollectionDispatching(wpfOcOcDispatcher, _ocOcDispatcher); // возвращаем вычисление в главный поток из фонового

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
				this.OcDispatcher.Invoke(() => Orders.Add(order), OcDispatcherPriority.Background);
			}
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocOcDispatcher.Dispose();
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

	public class WpfOcOcDispatcher : IOcDispatcher
	{
		private OcDispatcher _ocDispatcher;

		public WpfOcOcDispatcher(OcDispatcher ocDispatcher)
		{
			_ocDispatcher = ocDispatcher;
		}

		#region Implementation of IOcDispatcher

		public void Invoke(Action action, object context)
		{
			_ocDispatcher.BeginInvoke(action, OcDispatcherPriority.Background);
		}

		#endregion
	}
}
```
В этом примере мы грузим данные из БД в главном потоке, но фильтрация коллекции-источника *Orders* для получения оплаченных заказов (*PaidOrders*) производится в фоновом потоке.   
Класс *ObservableComputations.OcDispatcher* очень похож на класс [System.Windows.Threading.Dispatcher](https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.Dispatcher?view=netcore-3.1). Класс *ObservableComputations.OcDispatcher* ассоциирован с единственным потоком. В этом потоке вы можете выполнять делегаты, вызывая методы *ObservableComputations.OcDispatcher.Invoke* и *ObservableComputations.OcDispatcher.BeginInvoke*. 
Метод *CollectionDispatching* перенаправляет все изменения коллекции источника в поток целевого диспетчера (параметр *distinationDispatcher*). 
В момент вызова метода *CollectionDispatching* происходит перечисление коллекции-источника (*Orders* или *Orders.CollectionDispatching(_ocDispatcher).Filtering(o => o.Paid)*) и подписка на её событие [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netcore-3.1). При этом коллекция-источник не должна меняться. При вызове *.CollectionDispatching(_ocDispatcher)*, коллекция *Orders* не меняется. При вызове *.CollectionDispatching(wpfOcDispatcher, _ocDispatcher)* коллекция *Orders.CollectionDispatching(_ocDispatcher).Filtering(o => o.Paid)* может меняться в потоке *_ocOcDispatcher*, но так как мы передаём *_ocDispatcher* в параметр *sourceOcDispatcher*, то перечисление коллекции-источника и подписка на её событие [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netcore-3.1) происходит в потоке *_ocDispatcher*, что гарантирует отсутствие изменений коллекции-источника при перечислении. Так как при вызове *.CollectionDispatching(_ocDispatcher)*, коллекция *Orders* не меняется, то передавать *wpfOcDispatcher* в параметр *sourceOcDispatcher* смысла нет, тем более что в момент вызова *.CollectionDispatching(_ocDispatcher)* мы и так находимся в потоке *wpfOcDispatcher*. В большинстве случаев излишняя передача параметра *sourceDispatcher* не приведёт к потере работоспособности, разве что немного пострадает производительность.
Перечисление коллекции-источника происходит также в случае если [коллекция-источник передана как обозреваемый аргумент](#передача-коллекции-источника-как-обозреваемого-аргумента) и изменила своё значение.
Обратите внимание на необходимость вызова *_ocDispatcher.Dispose()*.

Обратите внимание  *DispatcherPriority.Background* iпередаётся через параметр *destinationOcDispatcherPriority* метода расшерения *CollectionDispatching* в метод *WpfOcDispatcher.Invoke*.

Приведённый выше пример не является единственным вариантом проектирования. Вот ещё один вариант (XAML такой же как в предыдущем примере):

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

		WpfOcOcDispatcher _wpfOcOcDispatcher;
		
		// Диспетчер для выполнения вычислений в фоновом потоке
		ObservableComputations.OcDispatcher _ocOcDispatcher = new ObservableComputations.OcDispatcher();

		public MainWindow()
		{
			_wpfOcOcDispatcher = new WpfOcOcDispatcher(this.OcDispatcher);
			
			Orders = new ObservableCollection<Order>();

			fillOrdersFromDb();

			PaidOrders = 
				Orders
				.Filtering(o => o.Paid)
				.CollectionDispatching(_wpfOcOcDispatcher, _ocOcDispatcher); // направляем вычисление в главный поток из фонового
			UnpaidOrders = 
				Orders
				.Filtering(o => !o.Paid)
				.CollectionDispatching(_wpfOcOcDispatcher, _ocOcDispatcher); // направляем вычисление в главный поток из фонового из фонового

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
					_ocOcDispatcher.Invoke(() => Orders.Add(order));
				}

				this.OcDispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					OcDispatcherPriority.Background);
			});

			thread.Start();
		}
		
		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocOcDispatcher.Dispose();
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

	public class WpfOcOcDispatcher : IOcDispatcher
	{
		private OcDispatcher _ocDispatcher;

		public WpfOcOcDispatcher(OcDispatcher ocDispatcher)
		{
			_ocDispatcher = ocDispatcher;
		}

		#region Implementation of IOcDispatcher

		public void Invoke(Action action, object context)
		{
			_ocDispatcher.BeginInvoke(action, OcDispatcherPriority.Background);
		}

		#endregion
	}
}
 ```

И ещё:
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

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		WpfOcOcDispatcher _wpfOcOcDispatcher;

		public MainWindow()
		{
			_wpfOcOcDispatcher = new WpfOcOcDispatcher(this.OcDispatcher);
			
			Orders = new ObservableCollection<Order>();

			PaidOrders = 
				Orders
				.Filtering(o => o.Paid)
				.CollectionDispatching(_wpfOcOcDispatcher); // направляем вычисление в главный поток

			UnpaidOrders = 
				Orders
				.Filtering(o => !o.Paid)
				.CollectionDispatching(_wpfOcOcDispatcher); // направляем вычисление в главный поток

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

				this.OcDispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					OcDispatcherPriority.Background);
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

	public class WpfOcOcDispatcher : IOcDispatcher
	{
		private OcDispatcher _ocDispatcher;

		public WpfOcOcDispatcher(OcDispatcher ocDispatcher)
		{
			_ocDispatcher = ocDispatcher;
		}

		#region Implementation of IOcDispatcher

		public void Invoke(Action action, object context)
		{
			_ocDispatcher.BeginInvoke(action, OcDispatcherPriority.Background);
		}

		#endregion
	}
}
```



### Диспетчеризация свойств
В предыдущих примерах мы видели как происходит диспетчеризация коллекций средствами метода *CollectionDispatching*. Но может также возникнуть необходимость в диспетчеризации свойств:

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
using OcDispatcher = System.Windows.Threading.OcDispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		// Диспетчер для выполнения вычислений в фоновом потоке
		ObservableComputations.OcDispatcher _ocOcDispatcher = new ObservableComputations.OcDispatcher();
		
		WpfOcOcDispatcher _wpfOcOcDispatcher;

		public MainWindow()
		{
			_wpfOcOcDispatcher = new WpfOcOcDispatcher(this.OcDispatcher);
			
			Orders = new ObservableCollection<Order>();

			fillOrdersFromDb();

			PaidOrders = 
				Orders.CollectionDispatching(_ocOcDispatcher) // отправляем вычисление в фоновый поток
				.Filtering(o => o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcOcDispatcher); // возвращаем вычисление в главный поток

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
					Order order = new Order(i, _ocOcDispatcher, _wpfOcOcDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					this.OcDispatcher.Invoke(() => Orders.Add(order), OcDispatcherPriority.Background);
				}

				this.OcDispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					OcDispatcherPriority.Background);
			});

			thread.Start();
		}

		private void unpaidOrdersList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			((Order) uc_UnpaidOrderList.SelectedItem).Paid = true;
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocOcDispatcher.Dispose();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IOcDispatcher backgroundOcDispatcher, IOcDispatcher wpfOcDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundOcDispatcher, wpfOcDispatcher);

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

	public class WpfOcOcDispatcher : IOcDispatcher
	{
		private OcDispatcher _ocDispatcher;

		public WpfOcOcDispatcher(OcDispatcher ocDispatcher)
		{
			_ocDispatcher = ocDispatcher;
		}

		#region Implementation of IOcDispatcher

		public void Invoke(Action action, object context)
		{
			_ocDispatcher.BeginInvoke(action, OcDispatcherPriority.Background);
		}

		#endregion
	}
}
```

В этом примере при двойном щелчке мышью по неоплаченному заказу мы делаем его оплаченным. Так как свойство *Paid* в этом случае меняется в главном потоке, то мы не можем читать его в фоновом потоке *_ocOcDispatcher*. Для того чтобы читать это свойство в фоновом потоке *_ocOcDispatcher*, необходимо диспетчеризировать изменения этого сойства в этот поток. Это происходит с помощью класса *PropertyDispatching&lt;THolder, TResult&gt;*. Аналогично методу *CollectionDispatching*, конструктор класса *PropertyDispatching&lt;THolder, TResult&gt;* имеет обязательный параметр *destinationOcDispatcher* и опциональный параметр *sourceOcDispatcher*. Отличие в том, что 

* вместо перчисления коллекции-источника и подписки на событие [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netcore-3.1), происходит считывание значения свойства и подписка на событие [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netcore-3.1). 
* значение переданное в параметр *sourceOcDispatcher*, используется для диспетчеризации изменения значения свойства (сеттер *PropertyDispatching&lt;THolder, TResult&gt;.Value*) в поток *sourceOcDispatcher*, в случае если это изменение далается в другом потоке. 

Обратите внимание как *DispatcherPriority.Background* передаётся через параметр *sourceOcDispatcherPriority* конструктора класса *PropertyDispatching* в метод *WpfOcDispatcher.Invoke*.

Приведённый выше пример не является единственным вариантом проектирования. Вот ещё один вариант (XAML не изменился):

```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ObservableComputations;
using OcDispatcher = System.Windows.Threading.OcDispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }

		// Диспетчер для выполнения вычислений в фоновом потоке
		ObservableComputations.OcDispatcher _ocOcDispatcher = new ObservableComputations.OcDispatcher();
		
		WpfOcOcDispatcher _wpfOcOcDispatcher;

		public MainWindow()
		{
			_wpfOcOcDispatcher = new WpfOcOcDispatcher(this.OcDispatcher);
			
			Orders = new ObservableCollection<Order>();

			fillOrdersFromDb();

			PaidOrders = 
				Orders
				.Filtering(o => o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcOcDispatcher, _ocOcDispatcher); // направляем вычисление в главный поток из фонового

			UnpaidOrders = 
				Orders
				.Filtering(o => !o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcOcDispatcher, _ocOcDispatcher); // направляем вычисление в главный поток из фонового

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
					Order order = new Order(i, _ocOcDispatcher, _wpfOcOcDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					_ocOcDispatcher.Invoke(() => Orders.Add(order));
				}

				this.OcDispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					OcDispatcherPriority.Background);
			});

			thread.Start();
		}

		private void unpaidOrdersList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			((Order) uc_UnpaidOrderList.SelectedItem).Paid = true;
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocOcDispatcher.Dispose();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IOcDispatcher backgroundOcDispatcher, IOcDispatcher wpfOcDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundOcDispatcher, wpfOcDispatcher);

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

	public class WpfOcOcDispatcher : IOcDispatcher
	{
		private OcDispatcher _ocDispatcher;

		public WpfOcOcDispatcher(OcDispatcher ocDispatcher)
		{
			_ocDispatcher = ocDispatcher;
		}

		#region Implementation of IOcDispatcher

		public void Invoke(Action action, object context)
		{
			_ocDispatcher.Invoke(action, OcDispatcherPriority.Background);
		}

		#endregion
	}
}
```

И ещё (XAML не изменился):
```csharp
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ObservableComputations;
using OcDispatcher = System.Windows.Threading.OcDispatcher;

namespace ObservableComputationsExample
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; }
		public ObservableCollection<Order> PaidOrders { get; }
		public ObservableCollection<Order> UnpaidOrders { get; }
		
		// Диспетчер для выполнения вычислений в фоновом потоке
		ObservableComputations.OcDispatcher _ocOcDispatcher = new ObservableComputations.OcDispatcher();
		
		WpfOcOcDispatcher _wpfOcOcDispatcher;

		public MainWindow()
		{
			_wpfOcOcDispatcher = new WpfOcOcDispatcher(this.OcDispatcher);
			
			Orders = new ObservableCollection<Order>();

			PaidOrders = 
				Orders
				.Filtering(o => o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcOcDispatcher); // направляем вычисление в главный поток

			UnpaidOrders = 
				Orders
				.Filtering(o => !o.PaidPropertyDispatching.Value)
				.CollectionDispatching(_wpfOcOcDispatcher); // направляем вычисление в главный поток

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
					Order order = new Order(i, _ocOcDispatcher, _wpfOcOcDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					_ocOcDispatcher.Invoke(() => Orders.Add(order));
				}

				this.OcDispatcher.Invoke(
					() => uc_LoadingIndicator.Visibility = Visibility.Hidden, 
					OcDispatcherPriority.Background);
			});

			thread.Start();
		}

		private void unpaidOrdersList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			((Order) uc_UnpaidOrderList.SelectedItem).Paid = true;
		}

		private void mainWindow_OnClosed(object sender, EventArgs e)
		{
			_ocOcDispatcher.Dispose();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IOcDispatcher backgroundOcDispatcher, IOcDispatcher wpfOcDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundOcDispatcher, wpfOcDispatcher);

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

	public class WpfOcOcDispatcher : IOcDispatcher
	{
		private OcDispatcher _ocDispatcher;

		public WpfOcOcDispatcher(OcDispatcher ocDispatcher)
		{
			_ocDispatcher = ocDispatcher;
		}

		#region Implementation of IOcDispatcher

		public void Invoke(Action action, object context)
		{
			_ocDispatcher.Invoke(action, OcDispatcherPriority.Background);
		}

		#endregion
	}
}
```

### Диспетчеризация *IReadScalar&lt;TValue&gt;*
*IReadScalar&lt;TValue&gt;* впервые был упомянут [здесь](#полный-список-операторов). Кроме метода *CollectionDispatching*, ObservableComputations содержит метод *ScalarDispatching*. Его использование полностью аналогично использованию *PropertyDispatching*, но с помощью *PropertyDispatching* Вы можете диспетчеризовать не только свойства. С помощью *ScalarDispatching* можно организовать [диспетчеризацию свойств](#диспетчеризация-свойств), но с помощью класса *PropertyDispatching&lt;THolder, TResult&gt;* она проще и быстрее.

### Параллельные вычисления в фоновых потоках
В предыдущих примерах мы увидели как происходит вычисление в одном фоновом потоке. Использую методы диспетчиризации описанные выше есть возможность организовать вычисления в нескольких фоновых потоках, результаты которых конкурентно объединяются в другом потоке (главном или фоновом).

### Использование класса *OcDispatcher*
Класса *OcDispatcher* имеет методы, которые Вы можете вызывать при необходимости
* *Invoke* и *BeginInvoke* - для синхронного и асинхронного выполнения делегата в потоке экземпляра класса *OcDispatcher*, например, для изменения исходных данных для вычислений выполняющихся в потоке экземпляра класса *OcDispatcher*. После вызова метода *Dispose* данные методы возвращают управление без выполнения переданного делегата и без выброса исключения. 
* *DoOthers* - в случае если делегат переданный в методы *Invoke* и *BeginInvoke* выполняется долго, при вызове *DoOthers* вызываются другие делегаты. Есть возможность задать максимальное количество делегатов, которые могут быть выполнены или приблизительное максимальное время их выполнения.

### Варианты реализации интерфейса IOcDispatcher и других аналогичных интерфейсов
До сих пор мы использовали очень простую реализацию интерфейса *IOcDispatcher*. Например, такую:  
```csharp
public class WpfOcOcDispatcher : IOcDispatcher
{
   private OcDispatcher _ocDispatcher;

   public WpfOcOcDispatcher(System.Windows.Threading.OcDispatcher ocDispatcher)
   {
	_ocDispatcher = ocDispatcher;
   }

   #region Implementation of IOcDispatcher

   public void Invoke(Action action, object context)
   {
	_ocDispatcher.Invoke(action, OcDispatcherPriority.Background);
   }

   #endregion
}
```
В этой реализации вызывается метод [System.Windows.Threading.Dispatcher.Invoke](https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.Dispatcher.invoke?view=netcore-3.1). В других реализациях мы вызывали [System.Windows.Threading.Dispatcher.BeginInvoke]([System.Windows.Threading.Dispatcher.Invoke](https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.Dispatcher.invoke?view=netcore-3.1)). На этом варианты реализации не ограничиваются, например, вы можете использвать реализацию, которая будет буферизировать изменеия коллекции c помощью [Reactive Extensions](https://github.com/dotnet/reactive):  
```csharp
public class WpfOcOcDispatcher : IOcDispatcher, IDisposable
{
	Subject<Action> _actions;

	private System.Windows.OcDispatcher _ocDispatcher;

	public WpfOcOcDispatcher(System.Windows.OcDispatcher ocDispatcher)
	{
		_ocDispatcher = ocDispatcher;

		_actions = new Subject<Action>();
		_actions.Buffer(TimeSpan.FromMilliseconds(300)).Subscribe(actions =>
		{
			_ocDispatcher.Invoke(() =>
			{
				for (var index = 0; index < actions.Count; index++)
				{
					actions[index]();
				}
			}, OcDispatcherPriority.Background);
		});
	}

	#region Implementation of IOcDispatcher

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

При дисптчеризации свойств (*PropertyDispatching*) и *IReadScalar&lt;TValue&gt;* (*ScalarDispatching*) может быть полезен ThrottlingOcDispatcher:

```
public class ThrottlingOcDispatcher : IOcDispatcher, IDisposable
{
	Subject<Action> _actions;

	private System.Windows.OcDispatcher _ocDispatcher;

	public WpfOcOcDispatcher(System.Windows.OcDispatcher ocDispatcher)
	{
		_ocDispatcher = ocDispatcher;

		_actions = new Subject<Action>();
		_actions.Throttle(TimeSpan.FromMilliseconds(300)).Subscribe(action =>
		{
			_ocDispatcher.Invoke(action, OcDispatcherPriority.Background);
		});
	}

	#region Implementation of IOcDispatcher

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

Может возникнуть необходимость в реализации завясящей от того, какие изменения прозводятся делегатом *action* переданным в метод *Invoke*. Для этого можно анализировать поараметр *context* передаваемый в метод *IOcDispatcher.Invoke* или использовать интерфейсы вместо интерфейса *IOcDispatcher*:

```csharp
	public interface ICollectionDestinationOcDispatcher
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

	public interface IPropertySourceOcDispatcher
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

### Приоритизация в класса *OcDispatcher* class

Класс *OcDispatcher* может выполнять приоритетную обработку переданных ему делегатов, так же как и WPFs [Dispatcher](https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher?view=net -5,0). По умолчанию *OcDispatcher* имеет только 1 приоритет, но у конструктора этого класса есть параметр количества возможных приоритетов: *prioritiesNumber*. В предыдущих примерах вы видели, как установить приоритет пользовательской реализации интерфейса *IOcDispatcher* (*WpfOcDispatcher*) в вызовах методов диспетчеризации (*CollectionDispatching*, *ScalarDispatching*, *PropertyDispatching*). Вы можете установить приоритет для экземпляра класса *OcDispatcher* таким же образом: через параметры *destinationOcDispatcherPriority* или *sourceOcDispatcherPriority* методов диспетчеризации. Приоритет по умолчанию самый низкий: 0; Количество или возможные приоритеты *OcDispatcher* должны быть минимальными, чтобы минимизировать накладные расходы.

### Запуск в консольном приложении

Предыдущие примеры были примерами WPF приложения. Аналогичные примеры можно запустить и в консольном приложении. Это может понадобиться для Unit-тестов.

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
		static ObservableComputations.OcDispatcher _backgroundOcDispatcher = new ObservableComputations.OcDispatcher();
		
		static ObservableComputations.OcDispatcher _mainOcDispatcher = new ObservableComputations.OcDispatcher();
		static ObservableCollection<Order> Orders;

		static void Main(string[] args)
		{
			_mainOcDispatcher.Invoke(() =>
			{
				ObservableCollection<Order> paidOrders;
				ObservableCollection<Order> unpaidOrders;

				Orders = new ObservableCollection<Order>();

				paidOrders =
					Orders.CollectionDispatching(_backgroundOcDispatcher)  // отправляем вычисление в фоновый поток
					.Filtering(o => o.PaidPropertyDispatching.Value)
					.CollectionDispatching(_mainOcDispatcher,
						_backgroundOcDispatcher); // возвращаем вычисление в главный поток из фонового

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
					Order order = new Order(i, _backgroundOcDispatcher, _mainOcDispatcher);
					order.Paid = Convert.ToBoolean(random.Next(0, 3));
					_mainOcDispatcher.Invoke(() => Orders.Add(order));
				}
			});

			thread.Start();
		}
	}

	public class Order : INotifyPropertyChanged
	{
		public Order(int num, IOcDispatcher backgroundOcDispatcher, IOcDispatcher mainOcDispatcher)
		{
			Num = num;
			PaidPropertyDispatching = new PropertyDispatching<Order, bool>(() => Paid, backgroundOcDispatcher, mainOcDispatcher);

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

### Отладка пользовательского кода

Описана [здесь](#пользовательский-код-в-фоновых-потоках).

## Отслеживание значений возвращаемых методом
До сих пор мы видели как ObservableComputations отслеживает изменения в значениях свойств и в коллекциях через события [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) и [CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=netframework-4.8). ObservableComputations вводит новый интерфейс и событие для отслеживание значений возвращаемых методами: интерфейс *INotifyMethodChanged* и событие *MethodChanged*. Вот пример:

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
			MethodChanged?.Invoke(this, new MethodChangedEventArgs(
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

		public event EventHandler<MethodChangedEventArgs> MethodChanged;
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
Как вы видите *MethodChangedEventArgs* содержит свойство *ArgumentsPredicate*. Следующее значение передаётся в это свойство:
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

Внимание: Пример кода в этом разделе не является образцом проектирования, это скорее антипаттерн: он содержит дублирование кода и изменения в свойствах класса RoomReservation не отслеживаются. Этот код приведён только для демонстрации отслеживание значений возвращаемых методом. См. исправленный код  [здесь](#используйте-публичные-структуры-предназначенные-только-для-чтения-вместо-приватных-членов).

### Вычисления реализующие *INotifyMethodChanged*
*INotifyMethodChanged* реализуют следующие вычисление
* *Dictionaring* (методы: ContainsKey, Indexer ([]), GetValueOrDefault).
* *ConcurrentDictionaring* (методы: ContainsKey, Indexer([]), GetValueOrDefault).
* *HashSetting* (метод: Contains).


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
В коде выше *selectedOrderTypes.ContainsComputing(() => o.Type)* является вложенным вычислением, которое зависит от внешнего параметра *o*. Эти два обстоятельства приводят к тому, что экземпляр класса *ContainsComputing* будет создан для каждого заказа в коллекции *orders* . Это может повлиять на производительность и потребление памяти, если количество заказов велико. К счастью, вычисление *filteredByTypeOrders* может быть сделано "плоским":

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

### Используйте аргумент capacity
Если после инстанцирования класса вычисления коллекции (напр. Filtering), ожидается что коллекция значительно вырастет, имеет смысл передать в конструктор аргумент *capacity*, чтобы зарезервировать память под коллекцию. 

### Используйте вычисления в фоновых потоках
См. подробности [здесь](#многопоточность).

### [Метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) Differing&lt;TResult&gt;
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

Иногда обработка каждого события [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8) занимает много времени и может подвесить пользовательский интерфейс (перерисовка, перевычисление). Используйте метод расширения Differing, чтобы уменьшить этот эффект.

### Временная остановка и возобновление вычислений

Если Вам необходимо произвести много изменений в исходных данных и Вы не хотите обрабатывать каждое изменение в Ваших вычислениях, Вы можете временно приостановить вычисление (поставить на паузу).

```c#
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
			OcConsumer consumer = new OcConsumer();

			CollectionPausing<Order> ordersPausing = orders.CollectionPausing();

			Filtering<Order> expensiveOrders = 
				ordersPausing
					.Filtering(o => o.Price > 25)
					.For(consumer); 
			
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

			// Start many changes...
			ordersPausing.IsPaused = true;
            
			for (int i = 10; i < 1000; i++)
				orders.Add(new Order{Num = i, Price = 30});

			checkFiltering(orders, expensiveOrders); // Prints "False"

			ordersPausing.IsPaused = false;

			checkFiltering(orders, expensiveOrders); // Prints "True"

			Console.ReadLine();

			consumer.Dispose();
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

Обратите внимание, что во время вызова "*ordersPausing.IsPaused = false;*" *ordersPausing* генерирует событие[CollectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged.collectionchanged?view=net-5.0) с [NotifyCollectionChangedAction.Reset](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.notifycollectionchangedaction?view=net-5.0#reset). Это поведение по умолчанию. Вы можете установить значение параметра *resumeType* метода расширения *CollectionPausing* в *CollectionPausingResumeType.ReplayChanges* и вместо [NotifyCollectionChangedAction.Reset](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.notifycollectionchangedaction?view=net-5.0#reset) *ordersPausing* воспроизведёт всю последовательность изменений, которые были сделаны за время паузы.
ObservableComputations также включает в себя метод расширения *ScalarPausing*. Его использование аналогично. Вместо *CollectionPausingResumeType* *ScalarPausing* позволяет установить сколько последних изменений будет воспроизведено при возобновлении. Значение по умолчанию 1. null соответствует всем изменениям.

## Советы по проектированию

### Ленивая инициализация вычислений
Если некоторое вычисление необходимо только для некоторых сценариев, либо Вы ходите отложить инициализацию до того момента когда вычисление потребуется, Вам подходит ленивая инициализация. Вот пример:  

```csharp
private Computing<string> _valueComputing;
public Computing<string> ValueComputing => _valueComputing = 
   _valueComputing ?? new Computing<string>(() => Value);
```

### Используйте публичные структуры предназначенные только для чтения вместо приватных членов
Пример кода приведённый в разделе ["Отслеживание значений возвращаемых методом"](#отслеживание-значений-возвращаемых-методом) не является образцом проектирования, это скорее антипаттерн: он содержит дублирование кода и изменения в свойствах класса RoomReservation не отслеживаются. Этот код приведён только для демонстрации отслеживание значений возвращаемых методом. Вот код с исправленным дизайном:

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
См. [здесь](#передача-аргументов-как-обозреваемых) и [здесь](#передача-коллекции-источника-как-обозреваемого-аргумента).

### Не создавайте лишних переменных
См. [здесь](#определение-переменных-в-цепочке-вычислений)

## Области применения [метода расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) Using&lt;TResult&gt;


### Чистые выражения
См. конец раздела [Отслеживание произвольного выражения](#отслеживание-произвольного-выражения).

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
Следующий код не корректен так как изменения в свойстве *OrderLine.Price* и коллекции *Order.Lines* не отражаются в результирующем вычислении:

```csharp
public Computing<decimal> PriceWithDiscount => _priceWithDiscount = _priceWithDiscount ?? 
   Lines.Selecting(l => l.Price).Summarizing().Value.Using(p => p - p * Discount);
```
В этом коде параметр *p* имеет тип decimal, а не *Summarizing&lt;decimal&gt;* как в корректном варианте. См. подробности [здесь](#передача-аргументов-как-обозреваемых-и-не-обозреваемых.

## Отслеживание предыдущего значения IReadScalar&lt;TValue&gt;
*IReadScalar&lt;TValue&gt;* упоминается в первый раз [здесь](#полный-список-операторов).
Не существует встроенных средств для получение предыдущего значения свойства во время обработки [PropertyChanged event](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.8). ObservableComputations приходит на помощь и предоставляет [методы расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *PreviousTracking&lt;TResult&gt;* и *WeakPreviousTracking&lt;TResult&gt;*.

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

Обратите внимание на то, что *PreviousTracking&lt;TResult&gt;* имеет сильную ссылку на значение *TResult* (свойство *PreviousValue*) (в случае если *TResult* является ссылочным типом). Учтите это когда будете думать о сборке мусора и утечках памяти. *WeakPreviousTracking&lt;TResult&gt;* может помочь Вам. Вместо свойства *PreviousValue* *WeakPreviousTracking&lt;TResult&gt;* включает в себя метод *TryGetPreviousValue*. Изменения в возвращаемом значении этого метода не могут отслеживаться, поэтому Вы не можете включить его в свои обозреваемые вычисления.

## Доступ к свойствам через рефлексию
Следующий код будет работать некорректно:
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

В коде выше мы используем [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *PropertyAccessing*. Убедитесь, что Вы ознакомились с [Передача аргументов как обозреваемых и не обозреваемых](#передача-аргументов-как-обозреваемых-и-не-обозреваемых): в коде выше первый аргумент (*order*) в [методе расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *PropertyAccessing* передан как **не обозреваемые**. В следующем коде этот аргумент передаётся как **observable**.
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
Следующий код не будет работать корректно, так как изменения в *manager.ProcessingOrder* не будут отражаться в *priceReflectedComputing*, так как первый аргумент (*manager.ProcessingOrder*)  в [методе расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *PropertyAccessing* передан как **не обозреваемый**:
```csharp
PropertyAccessing<decimal> priceReflectedComputing 
   = manager.ProcessingOrder.PropertyAccessing<decimal>(nameof(Order.Price));
```

Если ссылка на объект, у которого вычисляется значение свойства, является null, то *PropertyAccessing&lt;TResult&gt;.Value* возвращает значение по умолчанию для *TResult*. Вы можете изменить это значение передавая параметр *defaultValue*.

## Binding

Класс и [метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Binding* позволяет связать два произвольных выражения. Первое выражение это источник. Второе выражение является целевым. Сложность выражений не ограничена. Первое выражение передаётся как [дерево выражений](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/). Второе выражение передаётся как [делегат](https://docs.microsoft.com/en-us/dotnet/api/system.delegate?view=netframework-4.8). Когда значение выражения источника меняется, новое значение присваивается целевому выражению:  

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

В коде выше мы связываем *order.DeliveryAddress* и *assignedDeliveryCar.DestinationAddress*. *order.DeliveryAddress* является источником связывания. *assignedDeliveryCar.DestinationAddress* является целью связывания.

[Метод расширения](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) *Binding* расширяет *IReadScalar&lt;TValue&gt;*, экземпляр которого является источником связывания. 

## Могу ли я использовать [IList&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=netframework-4.8) с ObservableComputations?
Если у Вас есть коллекция реализующая [IList&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=netframework-4.8), но не реализующая [INotifyColectionChanged](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8) (на пример [List&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=netframework-4.8)), Вы можете использовать её с ObservableComputations. См. 

https://github.com/gsonnenf/Gstc.Collections.ObservableLists

Nuget: https://www.nuget.org/packages/Gstc.Collections.ObservableLists
