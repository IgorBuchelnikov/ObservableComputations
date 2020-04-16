using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class ExpressionWatcherTests
	{
		public class Item : INotifyPropertyChanged, INotifyMethodChanged
		{
			private string _num;
			public string Num
			{
				get { return _num; }
				set
				{
					updatePropertyValue(ref _num, value);
					onPropertyChanged("NumObject");
					onPropertyChanged("AltNum");
				}
			}

			public string AltNum
			{
				get { return _num; }
			}

			public object NumObject => (object) Num;

			Dictionary<string, Item> _children = new Dictionary<string, Item>();
			private Item _child;

			public Item GetChild(string num)
			{
				if (!_children.TryGetValue(num, out Item child))
				{
					child = new Item(){Num = num};
					_children.Add(num, child);
				}

				return child;
			}

			public void SetChild(string num, Item item)
			{
				_children[num] = item;
				MethodChanged?.Invoke(this, new NotifyMethodChangedEventArgs(nameof(GetChild), args => args[0].Equals(num)));
			}

			public Item Child
			{
				get => _child;
				set
				{
					_child = value;
					onPropertyChanged("Child");
				}
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

			public event EventHandler<NotifyMethodChangedEventArgs> MethodChanged;
		}

		[Test]
		public void TestRaiseValueChanged1()
		{
			bool raised = false;
			Item item = new Item();
			Expression<Func<bool>> expression = () => item.Num == "1";
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression));
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.Num = "1";
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged2()
		{
			bool raised = false;
			object item = new Item();
			Expression<Func<bool>> expression = () => ((Item)item).Num == "1";
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression));
			expressionWatcher.ValueChanged = ew => { raised = true; };
			((Item)item).Num = "1";
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged3()
		{
			bool raised = false;
			Item item = new Item();
			Expression<Func<string>> expression = () => (string)item.NumObject;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression));
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.Num = "1";
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged4()
		{
			bool raised = false;
			Item item = new Item();
			item.Num = "777";
			Expression<Func<string>> expression = () => item.GetChild(item.AltNum).Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression));
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.Num = "1";
			Assert.IsTrue(raised);
			raised = false;
			item.GetChild(item.AltNum).Num = "888";
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged5()
		{
			bool raised = false;
			Item item = new Item();
			item.Num = "777";
			Expression<Func<string, string>> expression = n => item.GetChild(item.AltNum + n).Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression), "777");
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.Num = "1";
			Assert.IsTrue(raised);
			raised = false;
			item.GetChild(item.AltNum + "777").Num = "888";
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged6()
		{
			bool raised = false;
			Item item = new Item();
			Expression<Func<string>> expression = () => (item.Num == "777" ? item.GetChild("888") : item.GetChild("000")).Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression));
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.Num = "777";
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged7()
		{
			bool raised = false;
			Item item = new Item();
			Expression<Func<string>> expression = () => item.GetChild("777").Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression));
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.SetChild("777", new Item(){Num = "888"});
			Assert.IsTrue(raised);
		}


		[Test]
		public void TestRaiseValueChanged8()
		{
			bool raised = false;
			Item item = new Item();
			Expression<Func<Item, string>> expression = i => i.GetChild(i.Num == "777" ? "888" : "999").Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression), item);
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.SetChild("000", new Item(){Num = "000"});
			Assert.IsFalse(raised);
			item.SetChild("999", new Item(){Num = "000"});
			Assert.IsTrue(raised);

			item.Num = "777";
			raised = false;
			item.SetChild("111", new Item(){Num = "000"});
			Assert.IsFalse(raised);
			item.SetChild("888", new Item(){Num = "000"});
			Assert.IsTrue(raised);
		}


		[Test]
		public void TestRaiseValueChanged9()
		{
			bool raised = false;
			Item item = new Item();
			Expression<Func<Item, string>> expression = i => i.GetChild("777").Num + i.GetChild("999").Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression), item);
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item.SetChild("777", new Item(){Num = "888"});
			Assert.IsTrue(raised);

			raised = false;
			item.SetChild("999", new Item(){Num = "000"});
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged10()
		{
			bool raised = false;
			Item item1 = new Item();
			Item item2 = new Item();
			Expression<Func<Item, Item, string>> expression = (i1, i2) => i1.GetChild("777").Num + i2.GetChild("999").Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression), item1, item2);
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item1.SetChild("777", new Item(){Num = "888"});
			Assert.IsTrue(raised);

			raised = false;
			item2.SetChild("999", new Item(){Num = "000"});
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged11()
		{
			bool raised = false;
			object item = new Item();
			Expression<Func<object, string>> expression = i => ((Item) i).GetChild("777").Num + ((Item) i).GetChild("999").Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression), item);
			expressionWatcher.ValueChanged = ew => { raised = true; };
			((Item)item).SetChild("777", new Item(){Num = "888"});
			Assert.IsTrue(raised);

			raised = false;
			((Item)item).SetChild("999", new Item(){Num = "000"});
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged12()
		{
			bool raised = false;
			Item item1 = new Item();
			Item item2 = new Item();
			Expression<Func<string>> expression = () => item1.GetChild("777").Num + item2.GetChild("999").Num;
			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression));
			expressionWatcher.ValueChanged = ew => { raised = true; };
			item1.SetChild("777", new Item(){Num = "888"});
			Assert.IsTrue(raised);

			raised = false;
			item2.SetChild("999", new Item(){Num = "000"});
			Assert.IsTrue(raised);
		}

		[Test]
		public void TestRaiseValueChanged13()
		{
			bool raised = false;
			Item item = new Item();
			item.Num = "777";
			Expression<Func<Item, string>> expression = i => i.Child.Num;

			ExpressionWatcher expressionWatcher = new ExpressionWatcher(
				ExpressionWatcher.GetExpressionInfo(expression), null);
		}

		[Test]
		public void TestWeakEventHandler()
		{
			bool raised = false;
			Item item = new Item();
			PropertyChangedEventHandler handler = (sender, args) => raised = true;
			WeakPropertyChangedEventHandler weakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(handler);
			item.PropertyChanged += weakPropertyChangedEventHandler.Handle;
			item.Num = "1";
			Assert.IsTrue(raised);
			raised = false;
			item.PropertyChanged -= weakPropertyChangedEventHandler.Handle;
			item.Num = "3";
			Assert.IsFalse(raised);
		}
	}
}