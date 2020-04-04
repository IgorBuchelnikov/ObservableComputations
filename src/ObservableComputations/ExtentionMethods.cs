using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObservableComputations
{
	public static partial class ExtensionMethods
	{
		[ObservableComputationsCall]
		public static ObservableComputations.Binding<TValue> Binding<TValue>(this
			ObservableComputations.IReadScalar<TValue> sourceScalar,
			Action<TValue> modifyTargetAction,
			bool applyNow)
			
		{
			return new ObservableComputations.Binding<TValue>(
				sourceScalar: sourceScalar,
				modifyTargetAction: modifyTargetAction,
				applyNow : applyNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.Binding<TValue> Binding<TValue>(this
			ObservableComputations.IReadScalar<TValue> sourceScalar,
			Action<TValue> modifyTargetAction)
			
		{
			return new ObservableComputations.Binding<TValue>(
				sourceScalar: sourceScalar,
				modifyTargetAction: modifyTargetAction,
				applyNow : true);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.Binding<TValue> Binding<TValue>(this
			Expression<Func<TValue>> getSourceExpression,
			Action<TValue> modifyTargetAction,
			bool applyNow)
			
		{
			return new ObservableComputations.Binding<TValue>(
				getSourceExpression: getSourceExpression,
				modifyTargetAction: modifyTargetAction,
				applyNow : applyNow);
		}

		[ObservableComputationsCall]
		public static ObservableComputations.Binding<TValue> Binding<TValue>(this
			Expression<Func<TValue>> getSourceExpression,
			Action<TValue> modifyTargetAction)
			
		{
			return new ObservableComputations.Binding<TValue>(
				getSourceExpression: getSourceExpression,
				modifyTargetAction: modifyTargetAction,
				applyNow : true);
		}
	}
}
