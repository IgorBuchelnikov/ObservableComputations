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
		#region Binding

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				IReadScalar<TValue> sourceScalar,
			Action<TValue> modifyTargetAction,
			bool applyNow)
			
		{
			return new Binding<TValue>(
				sourceScalar: sourceScalar,
				modifyTargetAction: modifyTargetAction,
				applyNow : applyNow);
		}

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				IReadScalar<TValue> sourceScalar,
			Action<TValue> modifyTargetAction)
			
		{
			return new Binding<TValue>(
				sourceScalar: sourceScalar,
				modifyTargetAction: modifyTargetAction,
				applyNow : true);
		}

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				Expression<Func<TValue>> getSourceExpression,
			Action<TValue> modifyTargetAction,
			bool applyNow)
			
		{
			return new Binding<TValue>(
				getSourceExpression: getSourceExpression,
				modifyTargetAction: modifyTargetAction,
				applyNow : applyNow);
		}

		[ObservableComputationsCall]
		public static Binding<TValue> Binding<TValue>(this
				Expression<Func<TValue>> getSourceExpression,
			Action<TValue> modifyTargetAction)
			
		{
			return new Binding<TValue>(
				getSourceExpression: getSourceExpression,
				modifyTargetAction: modifyTargetAction,
				applyNow : true);
		}

		#endregion

		public static TObject Do<TObject>(
			this TObject @object, Action<TObject> action)
		{
			action(@object);
			return @object;
		}
	}
}
