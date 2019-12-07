﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObservableComputations.Common.Interface;

namespace ObservableComputations.Common.Base
{
	public class ObservableCollectionWithChangeMarker<TItem> : ObservableCollection<TItem>, IHasChangeMarker
	{
		public bool ChangeMarker;

		public ObservableCollectionWithChangeMarker(List<TItem> list) : base(list)
		{

		}

		protected ObservableCollectionWithChangeMarker()
		{
		}

		public bool GetChangeMarker()
		{
			return ChangeMarker;
		}
	}
}