using System;
using ScheduleOne.NPCs;
using UnityEngine.UI;

namespace ScheduleOne.Map
{
	// Token: 0x02000C7E RID: 3198
	public class NPCPoI : POI
	{
		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x060059D0 RID: 22992 RVA: 0x0017B166 File Offset: 0x00179366
		// (set) Token: 0x060059D1 RID: 22993 RVA: 0x0017B16E File Offset: 0x0017936E
		public NPC NPC { get; private set; }

		// Token: 0x060059D2 RID: 22994 RVA: 0x0017B178 File Offset: 0x00179378
		public override void InitializeUI()
		{
			base.InitializeUI();
			if (base.IconContainer != null && this.NPC != null)
			{
				base.IconContainer.Find("Outline/Icon").GetComponent<Image>().sprite = this.NPC.MugshotSprite;
			}
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x0017B1CC File Offset: 0x001793CC
		public void SetNPC(NPC npc)
		{
			this.NPC = npc;
			if (base.IconContainer != null && this.NPC != null)
			{
				base.IconContainer.Find("Outline/Icon").GetComponent<Image>().sprite = this.NPC.MugshotSprite;
			}
		}
	}
}
