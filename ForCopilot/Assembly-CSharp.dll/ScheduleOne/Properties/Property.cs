using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000336 RID: 822
	public abstract class Property : ScriptableObject
	{
		// Token: 0x06001224 RID: 4644
		public abstract void ApplyToNPC(NPC npc);

		// Token: 0x06001225 RID: 4645
		public abstract void ClearFromNPC(NPC npc);

		// Token: 0x06001226 RID: 4646
		public abstract void ApplyToPlayer(Player player);

		// Token: 0x06001227 RID: 4647
		public abstract void ClearFromPlayer(Player player);

		// Token: 0x06001228 RID: 4648 RVA: 0x0004EA94 File Offset: 0x0004CC94
		public void OnValidate()
		{
			if (this.Name == string.Empty)
			{
				this.Name = base.name;
			}
			if (this.ID == string.Empty)
			{
				this.ID = base.name.ToLower();
			}
		}

		// Token: 0x04001179 RID: 4473
		public string Name = string.Empty;

		// Token: 0x0400117A RID: 4474
		public string Description = string.Empty;

		// Token: 0x0400117B RID: 4475
		public string ID = string.Empty;

		// Token: 0x0400117C RID: 4476
		[Range(1f, 5f)]
		public int Tier = 1;

		// Token: 0x0400117D RID: 4477
		[Range(0f, 1f)]
		public float Addictiveness = 0.1f;

		// Token: 0x0400117E RID: 4478
		public Color ProductColor = Color.white;

		// Token: 0x0400117F RID: 4479
		public Color LabelColor = Color.white;

		// Token: 0x04001180 RID: 4480
		public bool ImplementedPriorMixingRework;

		// Token: 0x04001181 RID: 4481
		[Header("Value")]
		[Range(-100f, 100f)]
		public int ValueChange;

		// Token: 0x04001182 RID: 4482
		[Range(0f, 2f)]
		public float ValueMultiplier = 1f;

		// Token: 0x04001183 RID: 4483
		[Range(-1f, 1f)]
		public float AddBaseValueMultiple;

		// Token: 0x04001184 RID: 4484
		public Vector2 MixDirection = Vector2.zero;

		// Token: 0x04001185 RID: 4485
		public float MixMagnitude = 1f;
	}
}
