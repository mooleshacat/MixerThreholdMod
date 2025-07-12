using System;

namespace ScheduleOne.Variables
{
	// Token: 0x0200029B RID: 667
	[Serializable]
	public class Conditions
	{
		// Token: 0x06000DDE RID: 3550 RVA: 0x0003D614 File Offset: 0x0003B814
		public bool Evaluate()
		{
			bool flag = false;
			for (int i = 0; i < this.ConditionList.Length; i++)
			{
				if (this.ConditionList[i].Evaluate())
				{
					flag = true;
					if (this.EvaluationType != Conditions.EEvaluationType.And)
					{
						return true;
					}
				}
				else if (this.EvaluationType == Conditions.EEvaluationType.And)
				{
					return false;
				}
			}
			for (int j = 0; j < this.QuestConditionList.Length; j++)
			{
				if (this.QuestConditionList[j].Evaluate())
				{
					flag = true;
					if (this.EvaluationType != Conditions.EEvaluationType.And)
					{
						return true;
					}
				}
				else if (this.EvaluationType == Conditions.EEvaluationType.And)
				{
					return false;
				}
			}
			return flag || this.ConditionList.Length + this.QuestConditionList.Length == 0;
		}

		// Token: 0x04000E4E RID: 3662
		public Conditions.EEvaluationType EvaluationType;

		// Token: 0x04000E4F RID: 3663
		public Condition[] ConditionList;

		// Token: 0x04000E50 RID: 3664
		public QuestCondition[] QuestConditionList;

		// Token: 0x0200029C RID: 668
		public enum EEvaluationType
		{
			// Token: 0x04000E52 RID: 3666
			And,
			// Token: 0x04000E53 RID: 3667
			Or
		}
	}
}
