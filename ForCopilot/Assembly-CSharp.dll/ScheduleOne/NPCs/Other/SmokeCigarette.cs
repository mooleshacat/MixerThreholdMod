using System;
using ScheduleOne.AvatarFramework.Animation;
using UnityEngine;

namespace ScheduleOne.NPCs.Other
{
	// Token: 0x020004D0 RID: 1232
	public class SmokeCigarette : MonoBehaviour
	{
		// Token: 0x06001B08 RID: 6920 RVA: 0x00074FFD File Offset: 0x000731FD
		private void Awake()
		{
			if (this.Npc == null)
			{
				this.Npc = base.GetComponentInParent<NPC>();
			}
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0007501C File Offset: 0x0007321C
		public void Begin()
		{
			this.Anim.SetBool("Smoking", true);
			this.cigarette = UnityEngine.Object.Instantiate<GameObject>(this.CigarettePrefab, this.Anim.RightHandContainer);
			this.Npc.Avatar.LookController.OverrideIKWeight(0.3f);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x00075070 File Offset: 0x00073270
		public void End()
		{
			this.Anim.SetBool("Smoking", false);
			if (this.cigarette != null)
			{
				UnityEngine.Object.Destroy(this.cigarette.gameObject);
				this.cigarette = null;
			}
			this.Npc.Avatar.LookController.OverrideIKWeight(0.2f);
		}

		// Token: 0x040016E4 RID: 5860
		public NPC Npc;

		// Token: 0x040016E5 RID: 5861
		public GameObject CigarettePrefab;

		// Token: 0x040016E6 RID: 5862
		public AvatarAnimation Anim;

		// Token: 0x040016E7 RID: 5863
		private GameObject cigarette;
	}
}
