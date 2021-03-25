using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class MiscTests
	{
		[Test]
		public void DoSetDebugTagTest()
		{
			Computing<string> computing = new Computing<string>(() => "").Do(c => c.SetDebugTag("DebugTag"));
			Assert.AreEqual(computing.DebugTag, "DebugTag");
		}

		private Computing<int> _computing;

		[Test]
		public void TestComputingsExecutingUserCode()
		{
			OcConfiguration.TrackComputingsExecutingUserCode = true;
			_computing = new Computing<int>(() => test());
			OcConsumer consumer = new OcConsumer();
			_computing.For(consumer);
			consumer.Dispose();
		}

		int  test()
		{
			Assert.AreEqual(StaticInfo.ComputingsExecutingUserCode[Thread.CurrentThread.ManagedThreadId], _computing);
			return 0;
		}

		[Test]
		public void TestSameAs()
		{
			object nullO = null; 
			object o1 = new object();
			object o2 = new object();

			Assert.IsTrue(o1.IsSameAs(o1));
			Assert.IsFalse(o1.IsSameAs(o2));
			Assert.IsTrue(nullO.IsSameAs(nullO));
			Assert.IsFalse(nullO.IsSameAs(o1));
			Assert.IsFalse(o1.IsSameAs(nullO));

		}

		[Test]
		public void TestUsing()
		{
			Expression<Func<int, int>> expression = v => 1;
			var @using = 0.Using(expression);
			Assert.AreEqual(@using.Argument, 0);
			Assert.AreEqual(@using.GetValueExpressionUsing, expression);
		}

		[Test]
		public void TestScalarPausing()
		{
			OcConsumer consumer = new OcConsumer("Tag");
			Assert.AreEqual(consumer.Tag, "Tag");
			Scalar<int> scalar = new Scalar<int>(0);
			ScalarPausing<int> scalarPausing = scalar.ScalarPausing(3).For(consumer);
			Assert.AreEqual(scalarPausing.IsPaused, false);
			scalarPausing.IsPaused  = true;
			scalar.Change(1);
			scalar.Change(2);
			scalar.Change(3);
			scalar.Change(4);
			scalarPausing.LastChangesToApplyOnResumeCount = 2;

			int[] values = new []{3, 4};
			int index = 0;
			scalarPausing.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "Value" && !scalarPausing.InactivationInProgress)
				{
					Assert.AreEqual(scalarPausing.Value, values[index++]);

					Assert.Throws<ObservableComputationsInconsistencyException>(() => scalarPausing.IsPaused = true);
				}
			};

			scalarPausing.IsPaused  = false;
			Assert.AreEqual(scalarPausing.Value, 4);
			consumer.Dispose();
		}

		[Test]
		public void TestScalarPausing2()
		{
			OcConsumer consumer = new OcConsumer("Tag");
			Scalar<bool> isPausedScalar = new Scalar<bool>(true);
			Assert.AreEqual(consumer.Tag, "Tag");
			Scalar<int> scalar = new Scalar<int>(0);
			ScalarPausing<int> scalarPausing = scalar.ScalarPausing(isPausedScalar).For(consumer);
			Assert.AreEqual(scalarPausing.IsPaused, false);
			scalar.Change(1);
			scalar.Change(2);
			scalar.Change(3);
			scalar.Change(4);
			scalarPausing.LastChangesToApplyOnResumeCount = 2;

			int[] values = new []{3, 4};
			int index = 0;
			scalarPausing.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "Value" && !scalarPausing.InactivationInProgress)
				{
					Assert.AreEqual(scalarPausing.Value, values[index++]);

					Assert.Throws<ObservableComputationsInconsistencyException>(() => isPausedScalar.Change(true));
				}
			};

			scalarPausing.IsPaused  = false;
			Assert.AreEqual(scalarPausing.Value, 4);
			consumer.Dispose();
		}

		[Test]
		public void TestResetRootSourceWrapper()
		{
			OcConsumer consumer = new OcConsumer();
			MyObservableCollection<int> items1 = new MyObservableCollection<int>(new []
			{
				0,
				1,
				2
			});


			var selecting = items1.Selecting(i => i).For(consumer);
			items1.Reset(new []
			{
				0,
				1,
				2,
				3,
				4,
				5
			});

			selecting.ValidateInternalConsistency();
			consumer.Dispose();
		}

		public class MyObservableCollection<TItem> : ObservableCollection<TItem>
		{
			public MyObservableCollection(IEnumerable<TItem> collection) : base(collection)
			{
			}

			public void Reset(TItem[] newItems)
			{
				int originalCount = Items.Count;
				int count = newItems.Length;
				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
					if (originalCount > sourceIndex)
						Items[sourceIndex] = newItems[sourceIndex];
					else
						Items.Insert(sourceIndex, newItems[sourceIndex]);

				for (int index = originalCount - 1; index >= sourceIndex; index--)
					Items.RemoveAt(index);

				CheckReentrancy();
				OnPropertyChanged(Utils.CountPropertyChangedEventArgs);
				OnPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
				OnCollectionChanged(Utils.ResetNotifyCollectionChangedEventArgs);
			}
		}

		[Test]
		public void DoTest()
		{
			ObservableCollection<int> observableCollection = new ObservableCollection<int>();
			Assert.IsTrue(observableCollection.Do(oc => oc.Add(1)).Count == 1);
		}
	}
}
