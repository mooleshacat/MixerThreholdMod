using System;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x02000604 RID: 1540
	[Serializable]
	public class LawActivitySettings
	{
		// Token: 0x060025B1 RID: 9649 RVA: 0x000988CC File Offset: 0x00096ACC
		public void Evaluate()
		{
			for (int i = 0; i < this.Patrols.Length; i++)
			{
				this.Patrols[i].Evaluate();
			}
			for (int j = 0; j < this.Checkpoints.Length; j++)
			{
				this.Checkpoints[j].Evaluate();
			}
			for (int k = 0; k < this.Curfews.Length; k++)
			{
				this.Curfews[k].Evaluate(false);
			}
			for (int l = 0; l < this.VehiclePatrols.Length; l++)
			{
				this.VehiclePatrols[l].Evaluate();
			}
			for (int m = 0; m < this.Sentries.Length; m++)
			{
				this.Sentries[m].Evaluate();
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00098980 File Offset: 0x00096B80
		public void End()
		{
			for (int i = 0; i < this.Curfews.Length; i++)
			{
				if (this.Curfews[i].Enabled)
				{
					this.Curfews[i].shouldDisable = true;
				}
			}
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000989C0 File Offset: 0x00096BC0
		public void OnLoaded()
		{
			Debug.Log("Settings loaded");
			for (int i = 0; i < this.Curfews.Length; i++)
			{
				this.Curfews[i].Evaluate(true);
			}
		}

		// Token: 0x04001BC9 RID: 7113
		public PatrolInstance[] Patrols;

		// Token: 0x04001BCA RID: 7114
		public CheckpointInstance[] Checkpoints;

		// Token: 0x04001BCB RID: 7115
		public CurfewInstance[] Curfews;

		// Token: 0x04001BCC RID: 7116
		public VehiclePatrolInstance[] VehiclePatrols;

		// Token: 0x04001BCD RID: 7117
		public SentryInstance[] Sentries;
	}
}
