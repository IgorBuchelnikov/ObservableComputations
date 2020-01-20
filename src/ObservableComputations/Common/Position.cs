using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ObservableComputations
{
	internal readonly struct Positions<TPosition> where TPosition : Position, new()
	{
		public readonly List<TPosition> List;

		public Positions(List<TPosition> list)
		{
			List = list;
		}

		[Pure]
		public TPosition Add()
		{
			TPosition position = new TPosition();
			add(position);
			return position;
		}

		private void add(TPosition position)
		{
			position.Index = List.Count;
			List.Add(position);
		}

		[Pure]
		public TPosition Insert(int index)
		{
			TPosition newItemPosition = new TPosition();
			insert(index, newItemPosition);
			return newItemPosition;
		}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		private void insert(int index, TPosition newItemPosition)
		{
			newItemPosition.Index = index;

			int count = List.Count;
			for (int i = index; i < count; i++)
			{
				TPosition position = List[i];
				position.Index = position.Index + 1;
			}

			List.Insert(index, newItemPosition);
		}


		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void Remove(int index)
		{
			int count = List.Count;
			for (int i = index + 1; i < count; i++)
			{
				TPosition position = List[i];
				position.Index = position.Index - 1;
			}

			List.RemoveAt(index);
		}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void Move(int oldIndex, int newIndex)
		{
			if (oldIndex == newIndex) return;
			TPosition movingItemPosition = List[oldIndex];
			List.RemoveAt(oldIndex);
			List.Insert(newIndex, movingItemPosition);

			movingItemPosition.Index = newIndex;

			if (oldIndex < newIndex)
			{
				for (int i = oldIndex; i <= newIndex - 1; i++)
				{
					TPosition position = List[i];
					position.Index = position.Index - 1;
				}
			}
			else if (oldIndex > newIndex)
			{
				for (int i = newIndex + 1; i <= oldIndex; i++)
				{
					TPosition position = List[i];
					position.Index = position.Index + 1;
				}				
			}
		}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void ValidateConsistency()
		{
			for (int index = 0; index < List.Count; index++)
			{
				TPosition position = List[index];
				if (position.Index != index)
					throw new ObservableComputationsException("Consistency violation: Positions.1");
			}
		}
	}

	internal class Position
	{
		public int Index;
	}

	internal struct RangePositions<TRangePosition> where TRangePosition : RangePosition, new()
	{
		public readonly List<TRangePosition> List;

		public RangePositions(List<TRangePosition> list)
		{
			List = list;
		}

		[Pure]
		public TRangePosition Add(int length)
		{
			int count = List.Count;
			TRangePosition rangePosition = new TRangePosition{Length = length, Index = count};

			if (count > 0)
			{
				TRangePosition lastRangePosition = List[count - 1];
				rangePosition.PlainIndex = lastRangePosition.PlainIndex + lastRangePosition.Length;			
			}
			else
			{
				rangePosition.PlainIndex = 0;	
			}

			List.Add(rangePosition);
			return rangePosition;
		}

		[Pure]
		public TRangePosition Insert(int index, int length)
		{
			TRangePosition newRangePosition = new TRangePosition
			{
				Index = index,
				PlainIndex = index < List.Count
					? List[index].PlainIndex
					: index > 0
						? List[index - 1].PlainIndex + List[index - 1].Length
						: 0,
				Length = length
			};

			List.Insert(index, newRangePosition);

			int count = List.Count;
			for (int i = index + 1; i < count; i++)
			{
				TRangePosition rangePosition = List[i];
				rangePosition.PlainIndex = rangePosition.PlainIndex + length;
				rangePosition.Index = rangePosition.Index + 1;
			}

			return newRangePosition;
		}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void InsertRange(int startIndex, int[] lengths)
		{
			int lengthsSum = 0;
			int currentIndex = startIndex;
			int startPlainIndex =  currentIndex < List.Count 
				? List[currentIndex].PlainIndex 
				: currentIndex > 0 
					? List[currentIndex - 1].PlainIndex + List[currentIndex - 1].Length
					: 0;
			var lengthsLength = lengths.Length;
			for (var index = 0; index < lengthsLength; index++)
			{
				int length = lengths[index];
				TRangePosition newRangePosition = new TRangePosition
				{
					Index = currentIndex,
					PlainIndex = startPlainIndex + lengthsSum,
					Length = length
				};

				List.Insert(currentIndex, newRangePosition);
				lengthsSum = lengthsSum + length;
				currentIndex++;
			}

			int count = List.Count;
			for (int i = currentIndex; i < count; i++)
			{
				TRangePosition rangePosition = List[i];
				rangePosition.PlainIndex = rangePosition.PlainIndex + lengthsSum;
				rangePosition.Index = i;
			}

		}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void Remove(int index)
		{
			TRangePosition removingRangePosition = List[index];
			int count = List.Count;
			int length = removingRangePosition.Length;

			for (int i = index + 1; i < count; i++)
			{
				TRangePosition rangePosition = List[i];
				rangePosition.PlainIndex = rangePosition.PlainIndex - length;
				rangePosition.Index = rangePosition.Index - 1;
			}

			List.RemoveAt(index);
		}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void RemoveRange(int startIndex, int itemsCount)
		{
			int removedLength = 0;
			for(int i = startIndex; i < startIndex + itemsCount;  i++)
			{
				removedLength = removedLength + List[i].Length;
			}

			int count = List.Count;
			for (int i = startIndex + itemsCount; i < count; i++)
			{
				TRangePosition rangePosition = List[i];
				rangePosition.PlainIndex = rangePosition.PlainIndex - removedLength;
				rangePosition.Index = rangePosition.Index - itemsCount;
			}

			List.RemoveRange(startIndex, itemsCount);
		}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void Move(int oldIndex, int newIndex)
		{
			TRangePosition movingRangePosition = List[oldIndex];
			List.RemoveAt(oldIndex);
			List.Insert(newIndex, movingRangePosition);

			movingRangePosition.Index = newIndex;

			if (oldIndex < newIndex)
			{		
				TRangePosition rangePosition = null;
				int length = movingRangePosition.Length;
				for (int i = oldIndex; i <= newIndex - 1; i++)
				{
					rangePosition = List[i];
					rangePosition.PlainIndex = rangePosition.PlainIndex - length;
					rangePosition.Index = rangePosition.Index - 1;
				}

				// ReSharper disable once PossibleNullReferenceException
				movingRangePosition.PlainIndex = rangePosition.PlainIndex + rangePosition.Length;
			}
			else if (oldIndex > newIndex)
			{
				int newPlainIndex = List[newIndex + 1].PlainIndex;
				int length = movingRangePosition.Length;

				for (int i = newIndex + 1; i <= oldIndex; i++)
				{
					TRangePosition rangePosition = List[i];
					rangePosition.PlainIndex = rangePosition.PlainIndex + length;
					rangePosition.Index = rangePosition.Index + 1;
				}	
		
				movingRangePosition.PlainIndex = newPlainIndex;
			}
		}	

		//public void MoveRange(int oldIndex, int count, int newIndex)
		//{
		//	List<RangePosition> movingRangePositions = List.GetRange(oldIndex, count);
		//	List.RemoveRange(oldIndex, count);
		//	List.InsertRange(newIndex, movingRangePositions);

		//	int lengthsSum = 0;
		//	for (int i = 0; i < count; i++)
		//	{
		//		movingRangePositions[i].Index = newIndex + i;
		//		lengthsSum = lengthsSum + movingRangePositions[i].Length;
		//	}

		//	if (oldIndex < newIndex)
		//	{
		//		RangePosition rangePosition = null;
		//		for (int i = oldIndex; i <= newIndex - 1; i++)
		//		{
		//			rangePosition = List[i];
		//			rangePosition.PlainIndex = rangePosition.PlainIndex - lengthsSum;
		//			rangePosition.Index = rangePosition.Index - count;
		//		}

		//		int newPlainIndex = rangePosition.PlainIndex + rangePosition.Length;
		//		for (int i = 0; i < count; i++)
		//		{
		//			movingRangePositions[i].PlainIndex = newPlainIndex;
		//			newPlainIndex = newPlainIndex + movingRangePositions[i].Length;
		//		}
		//	}
		//	else if (oldIndex > newIndex)
		//	{
		//		int newPlainIndex = List[newIndex + count].PlainIndex;

		//		for (int i = newIndex + count; i <= oldIndex + count - 1; i++)
		//		{
		//			RangePosition rangePosition = List[i];
		//			rangePosition.PlainIndex = rangePosition.PlainIndex + lengthsSum;
		//			rangePosition.Index = rangePosition.Index + count;
		//		}	
		
		//		for (int i = 0; i < count; i++)
		//		{
		//			movingRangePositions[i].PlainIndex = newPlainIndex;
		//			newPlainIndex = newPlainIndex + movingRangePositions[i].Length;
		//		}
		//	}
		//}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void ModifyLength(int index, int lengthIncrement)
		{
			List[index].Length = List[index].Length + lengthIncrement;
			int count = List.Count;
			for (int i = index + 1; i < count; i++)
			{
				TRangePosition rangePosition = List[i];
				rangePosition.PlainIndex = rangePosition.PlainIndex + lengthIncrement;
			}		
		}

		//public void ModifyLengthBatch(Func<int, int> getIncrementByIndex)
		//{
		//	int incrementSum = 0;

		//	for (int i = 0; i < List.Count; i++)
		//	{
		//		int increment = getIncrementByIndex(i);

		//		RangePosition rangePosition = List[i];
		//		rangePosition.Length = rangePosition.Length + increment;
		//		incrementSum = incrementSum + increment;					
		//		rangePosition.PlainIndex = rangePosition.PlainIndex + incrementSum;
		//	}		
		//}

		// ReSharper disable once PureAttributeOnVoidMethod
		[Pure]
		public void ValidateConsistency()
		{
			int plainIndex = 0;
			for (int index = 0; index < List.Count; index++)
			{
				TRangePosition rangePosition = List[index];
				if (rangePosition.Index != index)
					throw new ObservableComputationsException("Consistency violation: RangePosition.1");
				if (rangePosition.PlainIndex != plainIndex)
					throw new ObservableComputationsException("Consistency violation: RangePosition.2");
				plainIndex = plainIndex + rangePosition.Length;
			}
		}
	}

	internal class RangePosition
	{
		public int PlainIndex;
		public int Length;
		public int Index;
	}
}
