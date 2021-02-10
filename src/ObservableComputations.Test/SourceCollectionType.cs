using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations.Test
{
	public enum SourceCollectionType
	{
		INotifyPropertyChanged,
		ObservableCollection,
		ScalarINotifyPropertyChanged,
		ScalarObservableCollection,
		ExpressionINotifyPropertyChanged,
		ExpressionObservableCollection
	}
}
