using System;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x0200030E RID: 782
	[Serializable]
	public class SystemTrigger
	{
		// Token: 0x06001165 RID: 4453 RVA: 0x0004D174 File Offset: 0x0004B374
		public bool Trigger()
		{
			if (this.Conditions.Evaluate())
			{
				for (int i = 0; i < this.onEvaluateTrueQuestSetters.Length; i++)
				{
					this.onEvaluateTrueQuestSetters[i].Execute();
				}
				for (int j = 0; j < this.onEvaluateTrueVariableSetters.Length; j++)
				{
					this.onEvaluateTrueVariableSetters[j].Execute();
				}
				if (this.onEvaluateTrue != null)
				{
					this.onEvaluateTrue.Invoke();
				}
				return true;
			}
			for (int k = 0; k < this.onEvaluateFalseQuestSetters.Length; k++)
			{
				this.onEvaluateFalseQuestSetters[k].Execute();
			}
			for (int l = 0; l < this.onEvaluateFalseVariableSetters.Length; l++)
			{
				this.onEvaluateFalseVariableSetters[l].Execute();
			}
			if (this.onEvaluateFalse != null)
			{
				this.onEvaluateFalse.Invoke();
			}
			return false;
		}

		// Token: 0x04001152 RID: 4434
		public Conditions Conditions;

		// Token: 0x04001153 RID: 4435
		[Header("True")]
		public VariableSetter[] onEvaluateTrueVariableSetters;

		// Token: 0x04001154 RID: 4436
		public QuestStateSetter[] onEvaluateTrueQuestSetters;

		// Token: 0x04001155 RID: 4437
		public UnityEvent onEvaluateTrue;

		// Token: 0x04001156 RID: 4438
		[Header("False")]
		public VariableSetter[] onEvaluateFalseVariableSetters;

		// Token: 0x04001157 RID: 4439
		public QuestStateSetter[] onEvaluateFalseQuestSetters;

		// Token: 0x04001158 RID: 4440
		public UnityEvent onEvaluateFalse;
	}
}
