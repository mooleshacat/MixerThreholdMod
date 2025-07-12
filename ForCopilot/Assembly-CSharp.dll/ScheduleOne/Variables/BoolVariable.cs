using System;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Variables
{
	// Token: 0x02000298 RID: 664
	public class BoolVariable : Variable<bool>
	{
		// Token: 0x06000DD9 RID: 3545 RVA: 0x0003D4F3 File Offset: 0x0003B6F3
		public BoolVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner, bool value) : base(name, replicationMode, persistent, mode, owner, value)
		{
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0003D504 File Offset: 0x0003B704
		public override bool TryDeserialize(string valueString, out bool value)
		{
			if (valueString.ToLower() == "true")
			{
				value = true;
				return true;
			}
			if (valueString.ToLower() == "false")
			{
				value = false;
				return true;
			}
			value = false;
			return false;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0003D538 File Offset: 0x0003B738
		public override bool EvaluateCondition(Condition.EConditionType operation, string value)
		{
			bool flag;
			if (!this.TryDeserialize(value, out flag))
			{
				return false;
			}
			if (operation == Condition.EConditionType.EqualTo)
			{
				return this.Value == flag;
			}
			if (operation == Condition.EConditionType.NotEqualTo)
			{
				return this.Value != flag;
			}
			Console.LogError("Invalid operation " + operation.ToString() + " for bool variable", null);
			return false;
		}
	}
}
