using System;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Variables
{
	// Token: 0x0200029D RID: 669
	public class NumberVariable : Variable<float>
	{
		// Token: 0x06000DE0 RID: 3552 RVA: 0x0003D6AC File Offset: 0x0003B8AC
		public NumberVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner, float value) : base(name, replicationMode, persistent, mode, owner, value)
		{
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0003D6C0 File Offset: 0x0003B8C0
		public override bool TryDeserialize(string valueString, out float value)
		{
			float num;
			if (float.TryParse(valueString, out num))
			{
				value = num;
				return true;
			}
			value = 0f;
			return false;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0003D6E4 File Offset: 0x0003B8E4
		public override bool EvaluateCondition(Condition.EConditionType operation, string value)
		{
			float num;
			if (!this.TryDeserialize(value, out num))
			{
				return false;
			}
			if (operation == Condition.EConditionType.EqualTo)
			{
				return this.Value == num;
			}
			if (operation == Condition.EConditionType.NotEqualTo)
			{
				return this.Value != num;
			}
			if (operation == Condition.EConditionType.GreaterThan)
			{
				return this.Value > num;
			}
			if (operation == Condition.EConditionType.LessThan)
			{
				return this.Value < num;
			}
			if (operation == Condition.EConditionType.GreaterThanOrEqualTo)
			{
				return this.Value >= num;
			}
			if (operation == Condition.EConditionType.LessThanOrEqualTo)
			{
				return this.Value <= num;
			}
			Console.LogError("Invalid operation " + operation.ToString() + " for number variable", null);
			return false;
		}
	}
}
