using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Variables
{
	// Token: 0x02000299 RID: 665
	[Serializable]
	public class Condition
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x0003D594 File Offset: 0x0003B794
		public bool Evaluate()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return false;
			}
			BaseVariable variable = NetworkSingleton<VariableDatabase>.Instance.GetVariable(this.VariableName);
			if (variable == null)
			{
				Debug.LogError("Variable " + this.VariableName + " not found");
				return false;
			}
			return variable.EvaluateCondition(this.Operator, this.Value);
		}

		// Token: 0x04000E44 RID: 3652
		public string VariableName = "Variable Name";

		// Token: 0x04000E45 RID: 3653
		public Condition.EConditionType Operator = Condition.EConditionType.EqualTo;

		// Token: 0x04000E46 RID: 3654
		public string Value = "true";

		// Token: 0x0200029A RID: 666
		public enum EConditionType
		{
			// Token: 0x04000E48 RID: 3656
			GreaterThan,
			// Token: 0x04000E49 RID: 3657
			LessThan,
			// Token: 0x04000E4A RID: 3658
			EqualTo,
			// Token: 0x04000E4B RID: 3659
			NotEqualTo,
			// Token: 0x04000E4C RID: 3660
			GreaterThanOrEqualTo,
			// Token: 0x04000E4D RID: 3661
			LessThanOrEqualTo
		}
	}
}
