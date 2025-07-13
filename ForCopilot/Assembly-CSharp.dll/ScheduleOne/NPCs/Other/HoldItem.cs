using System;
using ScheduleOne.AvatarFramework.Equipping;
using UnityEngine;

namespace ScheduleOne.NPCs.Other
{
	// Token: 0x020004CF RID: 1231
	public class HoldItem : MonoBehaviour
	{
		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001B02 RID: 6914 RVA: 0x00074FA9 File Offset: 0x000731A9
		// (set) Token: 0x06001B03 RID: 6915 RVA: 0x00074FB1 File Offset: 0x000731B1
		public bool active { get; protected set; }

		// Token: 0x06001B04 RID: 6916 RVA: 0x00074FBA File Offset: 0x000731BA
		public void Begin()
		{
			this.active = true;
			this.Npc.SetEquippable_Return(this.Equippable.AssetPath);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x00074FDA File Offset: 0x000731DA
		private void Update()
		{
			bool active = this.active;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x00074FE3 File Offset: 0x000731E3
		public void End()
		{
			this.active = false;
			this.Npc.SetEquippable_Return(string.Empty);
		}

		// Token: 0x040016E1 RID: 5857
		public NPC Npc;

		// Token: 0x040016E2 RID: 5858
		public AvatarEquippable Equippable;
	}
}
