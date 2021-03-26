using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class CollectionPausingTests
	{
		[Test]
		public void TestCollectionPausing()
		{
			OcConsumer consumer = new OcConsumer();
			Scalar<ObservableCollection<int>> sourceScalar = 
				new Scalar<ObservableCollection<int>>(new ObservableCollection<int>(new int[]{1,2,3}).Selecting(i => i).For(consumer));

	
			CollectionPausing<int> collectionPausing = sourceScalar.CollectionPausing(true, CollectionPausingResumeType.ReplayChanges).For(consumer);

			collectionPausing.CollectionChanged += (sender, args) =>
			{
				Assert.IsTrue(collectionPausing.IsResuming);
			};

			sourceScalar.Change(new ObservableCollection<int>(new int[]{1,2,3,5,6}).Selecting(i => i).For(consumer));
			collectionPausing.IsPaused = false;
			collectionPausing.ValidateInternalConsistency();

			collectionPausing.IsPaused = true;
			sourceScalar.Change(null);
			collectionPausing.IsPaused =false;
			collectionPausing.ValidateInternalConsistency();
		}


	}
}
