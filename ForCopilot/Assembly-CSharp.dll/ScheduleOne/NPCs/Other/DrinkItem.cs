using System;
using ScheduleOne.AvatarFramework.Equipping;
using UnityEngine;

namespace ScheduleOne.NPCs.Other
{
	// Token: 0x020004CE RID: 1230
	public class DrinkItem : MonoBehaviour
	{
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x00074EC7 File Offset: 0x000730C7
		// (set) Token: 0x06001AFD RID: 6909 RVA: 0x00074ECF File Offset: 0x000730CF
		public bool active { get; protected set; }

		// Token: 0x06001AFE RID: 6910 RVA: 0x00074ED8 File Offset: 0x000730D8
		private void Awake()
		{
			if (this.Npc == null)
			{
				this.Npc = base.GetComponentInParent<NPC>();
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x00074EF4 File Offset: 0x000730F4
		public void Begin()
		{
			this.active = true;
			this.Npc.SetEquippable_Return(this.DrinkPrefab.AssetPath);
			this.Npc.Avatar.Anim.SetBool("Drinking", true);
			this.Npc.Avatar.LookController.OverrideIKWeight(0.3f);
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x00074F54 File Offset: 0x00073154
		public void End()
		{
			this.active = false;
			this.Npc.Avatar.Anim.SetBool("Drinking", false);
			this.Npc.Avatar.LookController.ResetIKWeight();
			this.Npc.SetEquippable_Return(string.Empty);
		}

		// Token: 0x040016DE RID: 5854
		public NPC Npc;

		// Token: 0x040016DF RID: 5855
		public AvatarEquippable DrinkPrefab;
	}
}
